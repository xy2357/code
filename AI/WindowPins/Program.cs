namespace WindowPins;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        using var instanceLock = new Mutex(true, @"Local\WindowPins", out var isFirstInstance);
        if (!isFirstInstance)
        {
            MessageBox.Show("WindowPins 已在运行，请查看任务栏通知区域。", "WindowPins",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new TrayApplicationContext());
    }
}
