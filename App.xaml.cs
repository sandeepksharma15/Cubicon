using System.IO;
using System.Windows;

using Microsoft.Extensions.DependencyInjection;

namespace Cubicon;

public partial class App : System.Windows.Application
{
    private NotifyIcon trayIcon;
    private ContextMenuStrip trayMenu;
    private IServiceProvider serviceProvider;

    public static string IconsFolder => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources");

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Register Services
        serviceProvider = AppServices.RegisterServices();

        // Restore window state
        serviceProvider
            .GetRequiredService<IAppStateService>()
            .RestoreAppState();

        DisplayTrayMenu();
    }

    private void DisplayTrayMenu()
    {
        trayMenu = new ContextMenuStrip();
        trayMenu.Items.Add("About", null, AboutIcoBox);
        trayMenu.Items.Add("New Icon Box", null, CreateIconGrpup);
        trayMenu.Items.Add("Start with Windows", null, LoadAtStartup);
        trayMenu.Items.Add("-");
        trayMenu.Items.Add("Exit", null, OnExitClick);

        // Set the checked state
        UpdateStartupMenuCheckState();

        // Create a tray icon
        trayIcon = new NotifyIcon();
        trayIcon.Text = "Cubicon";
        trayIcon.Icon = new Icon(Path.Combine(IconsFolder!, "IcoBox.ico"), 40, 40);

        // Add menu to tray icon
        trayIcon.ContextMenuStrip = trayMenu;

        // Show the tray icon
        trayIcon.Visible = true;
    }

    private void UpdateStartupMenuCheckState()
    {
        var loadAtStartupMenuItem = (ToolStripMenuItem)trayMenu!.Items[2];
        loadAtStartupMenuItem.Checked = Helpers.IsInStartup(AppInfo.AppName);
    }

    private void LoadAtStartup(object sender, EventArgs e)
    {
        //    if (Helpers.IsInStartup(AppInfo.AppName))
        //        Helpers.RemoveFromStartup(AppInfo.AppName);
        //    else
        //        Helpers.AddToStartup(AppInfo.AppName, Application.ExecutablePath);

        //    // Set the checked state
        //    UpdateStartupMenuCheckState();
    }

    private void CreateIconGrpup(object sender, EventArgs e)
    {
        new IconCube().Show();
    }

    private void AboutIcoBox(object sender, EventArgs e)
    {
        System.Windows.MessageBox.Show("Show About Box");
    }

    // Exit action
    private void OnExitClick(object sender, EventArgs e)
    {
        serviceProvider
            .GetRequiredService<IAppStateService>()
            .SaveAppState();

        trayIcon.Visible = false;
        Shutdown();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        trayIcon.Dispose();
        base.OnExit(e);
    }
}
