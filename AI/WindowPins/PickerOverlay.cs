namespace WindowPins;

internal sealed class PickerOverlay : Form
{
    private readonly Action<IntPtr> _selected;
    private readonly Action _closed;
    private readonly IntPtr _previousForeground;
    private bool _finished;

    internal PickerOverlay(Action<IntPtr> selected, Action closed)
    {
        _selected = selected;
        _closed = closed;
        _previousForeground = NativeMethods.GetForegroundWindow();

        FormBorderStyle = FormBorderStyle.None;
        AutoScaleMode = AutoScaleMode.None;
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.Manual;
        Bounds = SystemInformation.VirtualScreen;
        BackColor = Color.White;
        Opacity = 0.01;
        Cursor = Cursors.Cross;
        TopMost = true;
        KeyPreview = true;
    }

    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        Bounds = SystemInformation.VirtualScreen;
        Activate();
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Escape)
        {
            e.Handled = true;
            Finish(IntPtr.Zero);
        }

        base.OnKeyDown(e);
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        base.OnMouseUp(e);
        if (e.Button == MouseButtons.Right)
        {
            Finish(IntPtr.Zero);
            return;
        }

        if (e.Button != MouseButtons.Left)
            return;

        var point = PointToScreen(e.Location);
        _finished = true;
        Hide();
        Complete(NativeMethods.WindowAt(point));
    }

    private void Finish(IntPtr window)
    {
        if (_finished)
            return;

        _finished = true;
        Hide();
        Complete(window);
    }

    private void Complete(IntPtr window)
    {
        if (window != IntPtr.Zero && NativeMethods.IsSelectableWindow(window))
        {
            NativeMethods.SetForegroundWindow(window);
            _selected(window);
        }
        else if (_previousForeground != IntPtr.Zero && NativeMethods.IsWindow(_previousForeground))
        {
            NativeMethods.SetForegroundWindow(_previousForeground);
        }

        Close();
    }

    protected override void OnDeactivate(EventArgs e)
    {
        base.OnDeactivate(e);
        if (!_finished)
            Finish(IntPtr.Zero);
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        _finished = true;
        _closed();
        base.OnFormClosed(e);
    }
}
