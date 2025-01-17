﻿using System.Reflection;
using Microsoft.Win32;

namespace Cubicon;

public static class Helpers
{
    public class IconMetrics
    {
        public int IconWidth { get; set; } = 48;
        public int IconHeight { get; set; } = 48;
        public int SpacingHorizontal { get; set; } = 75;  // Size Of The Grid Holding Icon
        public int SpacingVertical { get; set; } = 75;    // Size Of The Grid Holding Icon
    }

    /// <summary>
    /// Get The Desktop Icon Metrics
    /// </summary>
    /// <returns></returns>
    public static IconMetrics GetDesktopIconMetrics()
    {
        Type t = typeof(SystemInformation);
        PropertyInfo[] pi = t.GetProperties();

        object iconSizeObject = pi.FirstOrDefault(p => p.Name == "IconSize")?.GetValue(null);
        object iconSpacingVObject = pi.FirstOrDefault(p => p.Name == "IconVerticalSpacing")?.GetValue(null);
        object iconSpacingHObject = pi.FirstOrDefault(p => p.Name == "IconHorizontalSpacing")?.GetValue(null);

        var iconSize = iconSizeObject != null ? (Size)iconSizeObject : new Size(48, 48);
        var iconSpacingV = iconSpacingVObject != null ? (int)iconSpacingVObject : 75;
        var iconSpacingH = iconSpacingHObject != null ? (int)iconSpacingHObject : 75;

        return new IconMetrics
        {
            IconWidth = iconSize.Width,
            IconHeight = iconSize.Height,
            SpacingHorizontal = iconSpacingH,
            SpacingVertical = iconSpacingV
        };
    }

    private const string RegistryPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";

    public static bool IsInStartup(string appName)
    {
        RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryPath, true);
        return key?.GetValue(appName) != null;
    }

    public static void AddToStartup(string appName, string appPath)
    {
        RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryPath, true);
        key?.SetValue(appName, appPath);
    }

    public static void RemoveFromStartup(string appName)
    {
        RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryPath, true);
        key?.DeleteValue(appName, false); // 'false' prevents an exception if the value does not exist
    }
}
