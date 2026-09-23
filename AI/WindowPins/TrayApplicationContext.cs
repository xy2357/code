namespace WindowPins;

internal sealed class TrayApplicationContext : ApplicationContext
{
    private sealed record PinState(bool WasTopmost, PinBadge Badge);

    private readonly Dictionary<IntPtr, PinState> _pins = [];
    // Properties disappear when HWNDs are destroyed, so a recycled handle cannot
    // cause us to change an unrelated new window.
    private readonly string _windowTag = $"WindowPins.{Guid.NewGuid():N}";
    private static readonly IntPtr TagValue = new(1);
    private readonly ContextMenuStrip _menu = new();
    private readonly Icon _icon = PinArtwork.CreateTrayIcon();
    private readonly NotifyIcon _tray;
    private readonly System.Windows.Forms.Timer _maintenance = new() { Interval = 150 };
    private PickerOverlay? _picker;
    private bool _startPickerAfterMenu;

    internal TrayApplicationContext()
    {
        _menu.Opening += (_, _) => BuildMenu();
        _tray = new NotifyIcon
        {
            Icon = _icon,
            Text = "WindowPins - 单击选择窗口置顶",
            ContextMenuStrip = _menu,
            Visible = true
        };
        _tray.MouseClick += (_, e) =>
        {
            if (e.Button == MouseButtons.Left)
                StartPicker();
        };

        _maintenance.Tick += (_, _) =>
        {
            MaintainPins();
            // Let the menu finish releasing focus before showing the picker.
            if (_startPickerAfterMenu && !_menu.Visible)
            {
                _startPickerAfterMenu = false;
                StartPicker();
            }
        };
        _maintenance.Start();
    }

    private void BuildMenu()
    {
        MaintainPins();
        while (_menu.Items.Count > 0)
        {
            var item = _menu.Items[0];
            _menu.Items.RemoveAt(0);
            item.Dispose();
        }

        var select = new ToolStripMenuItem("选择窗口置顶");
        select.Click += (_, _) => _startPickerAfterMenu = true;
        _menu.Items.Add(select);
        _menu.Items.Add(new ToolStripSeparator());

        if (_pins.Count == 0)
        {
            _menu.Items.Add(new ToolStripMenuItem("暂无置顶窗口") { Enabled = false });
        }
        else
        {
            foreach (var window in _pins.Keys)
            {
                var title = NativeMethods.WindowTitle(window);
                if (title.Length > 48)
                    title = title[..45] + "...";
                var remove = new ToolStripMenuItem($"取消置顶：{title}");
                remove.Click += (_, _) => UnpinWindow(window);
                _menu.Items.Add(remove);
            }
        }

        _menu.Items.Add(new ToolStripSeparator());
        var exit = new ToolStripMenuItem("退出");
        exit.Click += (_, _) => ExitThread();
        _menu.Items.Add(exit);
    }

    private void StartPicker()
    {
        if (_picker is not null)
            return;

        _picker = new PickerOverlay(PinWindow, () => _picker = null);
        _picker.Show();
    }

    private void PinWindow(IntPtr window)
    {
        if (!NativeMethods.IsSelectableWindow(window))
            return;

        if (_pins.ContainsKey(window))
        {
            UnpinWindow(window);
            return;
        }

        var wasTopmost = NativeMethods.IsTopmost(window);
        if (!NativeMethods.SetProp(window, _windowTag, TagValue))
        {
            ShowError("无法操作该窗口。若目标程序以管理员身份运行，请以管理员身份运行 WindowPins。");
            return;
        }
        if (!NativeMethods.SetTopmost(window, true) || !NativeMethods.IsTopmost(window))
        {
            NativeMethods.RemoveProp(window, _windowTag);
            ShowError("无法将该窗口置顶。若目标程序以管理员身份运行，请以管理员身份运行 WindowPins。");
            return;
        }

        var badge = new PinBadge(window, () => UnpinWindow(window));
        try
        {
            badge.Attach();
            _pins.Add(window, new PinState(wasTopmost, badge));
        }
        catch (Exception error)
        {
            badge.Dispose();
            if (!wasTopmost && NativeMethods.IsWindow(window))
                NativeMethods.SetTopmost(window, false);
            NativeMethods.RemoveProp(window, _windowTag);
            ShowError($"无法显示图钉：{error.Message}");
        }
    }

    private void UnpinWindow(IntPtr window)
    {
        if (!_pins.TryGetValue(window, out var state))
            return;

        if (NativeMethods.GetProp(window, _windowTag) == TagValue)
        {
            if (!state.WasTopmost && !NativeMethods.SetTopmost(window, false))
            {
                ShowError("无法恢复窗口原来的置顶状态，请再次尝试取消置顶。");
                return;
            }
            NativeMethods.RemoveProp(window, _windowTag);
        }
        _pins.Remove(window);
        state.Badge.Dispose();
    }

    private void MaintainPins()
    {
        foreach (var (window, state) in _pins.ToArray())
        {
            if (NativeMethods.GetProp(window, _windowTag) != TagValue || state.Badge.IsDisposed)
            {
                _pins.Remove(window);
                state.Badge.Dispose();
                continue;
            }

            if (!NativeMethods.IsTopmost(window))
            {
                // Respect an explicit change made by the target or another tool.
                NativeMethods.RemoveProp(window, _windowTag);
                _pins.Remove(window);
                state.Badge.Dispose();
                continue;
            }
            state.Badge.SyncPosition();
        }
    }

    private void ShowError(string message) =>
        _tray.ShowBalloonTip(5000, "WindowPins", message, ToolTipIcon.Warning);

    protected override void ExitThreadCore()
    {
        _maintenance.Stop();
        _picker?.Dispose();
        _picker = null;
        foreach (var window in _pins.Keys.ToArray())
            UnpinWindow(window);
        if (_pins.Count > 0)
        {
            _maintenance.Start();
            return;
        }
        _tray.Visible = false;
        _tray.Dispose();
        _menu.Dispose();
        _icon.Dispose();
        _maintenance.Dispose();
        base.ExitThreadCore();
    }
}
