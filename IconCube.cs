using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using static Cubicon.Helpers;

namespace Cubicon;

public class IconCube : Window
{
    private const int HEADER_HEIGHT = 30;

    private static Helpers.IconMetrics IconMetrics = new();

    private Border headerPanel;
    private System.Windows.Controls.Label titleLabel;
    private PictureBox iconPictureBox;
    private System.Windows.Controls.ListView iconListView;

    public IconCube(WindowData window) :
        this( new Rectangle(window.X, window.Y, window.Width, window.Height), 
            window.Title, 
            window.IconPaths) { }

    public IconCube(Rectangle? bounds = null, string title = null, List<string> iconPaths = null)
    {
        // Create Transparent Color
        var original = System.Windows.SystemColors.WindowColor;
        var semiTransparent = System.Windows.Media.Color.FromArgb(64, original.R, original.G, original.B);

        SetWindowAppearance(semiTransparent, bounds);

        // Create a DockPanel as the main container
        var dockPanel = new DockPanel();

        headerPanel = GetHeaderPanel(title);
        iconListView = GetIconListView(Height, Width, iconPaths);

        dockPanel.Children.Add(headerPanel);
        dockPanel.Children.Add(iconListView);

        // Set the content of the window to be the border
        Content = new Border
        {
            BorderThickness = new Thickness(2),
            Child = dockPanel,
        };
    }

    private System.Windows.Controls.ListView GetIconListView(double height, double width, List<string> iconPaths)
    {
        // Create the ListView for content
        var listView = new System.Windows.Controls.ListView
        {
            Background = System.Windows.SystemColors.WindowBrush, // System-defined background color for windows
            Foreground = System.Windows.SystemColors.ControlTextBrush // System-defined control text color
        };

        // Add sample items to ListView
        listView.Items.Add(new System.Windows.Controls.ListViewItem { Content = "Item 1" });
        listView.Items.Add(new System.Windows.Controls.ListViewItem { Content = "Item 2" });

        return listView;
    }

    private Border GetHeaderPanel(string title)
    {
        // Header Panel
        var headerPanel = new Border();

        headerPanel.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0x00, 0x00, 0x8B));
        headerPanel.Height = 30;

        iconPictureBox = GetIconPictureBox();
        titleLabel = GetTitleLabel(title);

        headerPanel.Child = titleLabel;

        // Enable window drag by clicking on the top panel
        headerPanel.MouseLeftButtonDown += (s, e) => { this.DragMove(); };

        DockPanel.SetDock(headerPanel, Dock.Top);

        return headerPanel;
    }

    private static System.Windows.Controls.Label GetTitleLabel(string title)
    {
        title ??= AppInfo.NewCubeTitle;

        return new System.Windows.Controls.Label
        {
            Content = title,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
            Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0xFF, 0xFF, 0xFF)),
            Height = HEADER_HEIGHT,
            Cursor = System.Windows.Input.Cursors.Arrow,
            FontFamily = new System.Windows.Media.FontFamily("Tahoma"),
            FontSize = 16,

            Padding = new Thickness(5, 5, 2, 2)
        };
    }

    private static PictureBox GetIconPictureBox()
    {
        var iconPictureBox = new PictureBox
        {
            Size = new System.Drawing.Size(16, 16),
            Location = new System.Drawing.Point(7, 5),
            SizeMode = PictureBoxSizeMode.StretchImage
        };

        iconPictureBox.Image = new Icon(Path.Combine(App.IconsFolder!, "IcoBox III (16x16).ico"), 16, 16).ToBitmap();

        return iconPictureBox;
    }

    private void SetWindowAppearance(System.Windows.Media.Color semiTransparent, Rectangle? bounds = null)
    {
        bounds ??= GetWindowBounds(); // Get bounds for the window

        // Window Bounds
        Left = bounds.Value.X;
        Top = bounds.Value.Y;
        Width = bounds.Value.Width;
        Height = bounds.Value.Height;

        ShowInTaskbar = false;
        ResizeMode = ResizeMode.CanResizeWithGrip;
        AllowsTransparency = true;
        WindowStyle = WindowStyle.None;
        Background = new SolidColorBrush(semiTransparent);
        Opacity = 0.5;
    }

    private static Rectangle GetWindowBounds()
    {
        // Calculate and set window size based on icon size and spacing
        IconMetrics = GetDesktopIconMetrics();

        // Calculate window size for 3 rows and 5 columns of icons
        int windowWidth = 5 * IconMetrics.SpacingHorizontal;
        int windowHeight = 3 * IconMetrics.SpacingVertical + HEADER_HEIGHT; // Add height for title bar

        // Get screen dimensions
        var screenWidth = Screen.PrimaryScreen!.Bounds.Width;
        var screenHeight = Screen.PrimaryScreen!.Bounds.Height;

        // Calculate position (right side aligned, bottom 100px above the screen)
        int x = screenWidth - windowWidth - 10;  // Position x such that right side touches viewport (with 10px padding)
        int y = screenHeight - windowHeight - 100;  // Position y 100px above bottom of screen

        return new Rectangle(x, y, windowWidth, windowHeight);
    }
}
