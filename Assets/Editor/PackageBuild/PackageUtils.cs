using UnityEditor;
using System.IO;
using GameChannel;
using System;
using AssetBundles;
using UnityEngine;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

public enum LocalServerType
{
	CurrentMachine = 0,
	AnyMachine = 1,
}


public class PackageUtils  
{
    public const string LocalServerPrefsKey = "AssetBundlesLocalServerType";
    public const string LocalServerIPPrefsKey = "AssetBundlesLocalServerIP";

    /// <summary>
    /// 获取安卓渠道打包的设置
    /// </summary>
    /// <returns></returns>
    public static bool GetAndroidBuildABForPerChannelSetting()
    {
        return PackagePrefs.GetBuildABForPerChannelSetting(ChannelPrefsKey.AndroidBuildABForPerChannelPrefsKey);
    }
    /// <summary>
    /// 设置安卓渠道打包的设置
    /// </summary>
    /// <returns></returns>
    public static void SaveAndroidBuildABForPerChannelSetting(bool enable)
    {
        PackagePrefs.SaveBuildABForPerChannelSetting(ChannelPrefsKey.AndroidBuildABForPerChannelPrefsKey ,enable);
    }
    /// <summary>
    /// 获取IOS渠道打包的设置
    /// </summary>
    /// <returns></returns>
    public static bool GetIOSBuildABForPerChannelSetting()
    {
        return PackagePrefs.GetBuildABForPerChannelSetting(ChannelPrefsKey.IOSBuildABForPerChannelPrefsKey);
    }
    /// <summary>
    /// 设置IOS渠道打包的设置
    /// </summary>
    /// <returns></returns>
    public static void SaveIOSBuildABForPerChannelSetting(bool enable)
    {
        PackagePrefs.SaveBuildABForPerChannelSetting(ChannelPrefsKey.IOSBuildABForPerChannelPrefsKey, enable);
    }
    /// <summary>
    /// 获取Windows渠道打包的设置
    /// </summary>
    /// <returns></returns>
    public static bool GetWindowsBuildABForPerChannelSetting()
    {
        return PackagePrefs.GetBuildABForPerChannelSetting(ChannelPrefsKey.StandaloneWindowsBuildABForPerChannelPrefsKey);
    }
    /// <summary>
    /// 设置Windows渠道打包的设置
    /// </summary>
    /// <returns></returns>
    public static void SaveWindowsBuildABForPerChannelSetting(bool enable)
    {
        PackagePrefs.SaveBuildABForPerChannelSetting(ChannelPrefsKey.StandaloneWindowsBuildABForPerChannelPrefsKey, enable);
    }
    /// <summary>
    /// 获取Windows64渠道打包的设置
    /// </summary>
    /// <returns></returns>
    public static bool GetWindows64BuildABForPerChannelSetting()
    {
        return PackagePrefs.GetBuildABForPerChannelSetting(ChannelPrefsKey.StandaloneWindows64BuildABForPerChannelPrefsKey);
    }
    /// <summary>
    /// 设置Windows64渠道打包的设置
    /// </summary>
    /// <returns></returns>
    public static void SaveWindows64BuildABForPerChannelSetting(bool enable)
    {
        PackagePrefs.SaveBuildABForPerChannelSetting(ChannelPrefsKey.StandaloneWindows64BuildABForPerChannelPrefsKey, enable);
    }
    /// <summary>
    /// 获取WebGl渠道打包的设置
    /// </summary>
    /// <returns></returns>
    public static bool GetWebGlBuildABForPerChannelSetting()
    {
        return PackagePrefs.GetBuildABForPerChannelSetting(ChannelPrefsKey.WebGlBuildABForPerChannelPrefsKey);
    }
    /// <summary>
    /// 设置WebGl渠道打包的设置
    /// </summary>
    /// <returns></returns>
    public static void SaveWebGlBuildABForPerChannelSetting(bool enable)
    {
        PackagePrefs.SaveBuildABForPerChannelSetting(ChannelPrefsKey.WebGlBuildABForPerChannelPrefsKey, enable);
    }


    /*
        ----------------------------------------------------------------------------
        ----------------------------------------------------------------------------
        ---------------------------------------------------------------------------- 
    */


    public static LocalServerType GetLocalServerType()
    {
        if (!EditorPrefs.HasKey(LocalServerPrefsKey))
        {
            SaveLocalServerType(LocalServerType.CurrentMachine);
            return LocalServerType.CurrentMachine;
        }

        int type = EditorPrefs.GetInt(LocalServerPrefsKey, (int)LocalServerType.CurrentMachine);
        return (LocalServerType)type;
    }

    public static void SaveLocalServerType(LocalServerType type)
    {
        EditorPrefs.SetInt(LocalServerPrefsKey, (int)type);
    }


    public static string GetLocalServerIP()
    {
        string ip = string.Empty;
        var type = GetLocalServerType();
        if (type == LocalServerType.CurrentMachine)
        {
            ip = GetCurrentMachineLocalIP();
        }
        else
        {
            ip = EditorPrefs.GetString(LocalServerIPPrefsKey, "127.0.0.1");
        }
        return ip;
    }

    public static void SaveLocalServerIP(string ip)
    {
        var type = GetLocalServerType();
        if (type == LocalServerType.CurrentMachine)
        {
            return;
        }
        EditorPrefs.SetString(LocalServerIPPrefsKey, ip);
    }
    public static string GetCurrentMachineLocalIP()
    {
        try
        {
            // 注意：这里获取所有内网地址后选择一个最小的，因为可能存在虚拟机网卡
            var ips = new List<string>();
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    ips.Add(ip.ToString());
                }
            }
            ips.Sort();
            if (ips.Count <= 0)
            {
                Debug.LogError("Get inter network ip failed!");
            }
            else
            {
                return ips[0];
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Get inter network ip failed with err : " + ex.Message);
            Debug.LogError("Go Tools/Package to specify any machine as local server!!!");
        }
        return string.Empty;
    }


    public static bool BuildAssetBundlesForPerChannel(BuildTarget buildTarget)
    {
        if (buildTarget == BuildTarget.Android && GetAndroidBuildABForPerChannelSetting() ||
            buildTarget == BuildTarget.iOS && GetIOSBuildABForPerChannelSetting() ||
            buildTarget == BuildTarget.StandaloneWindows && GetWindowsBuildABForPerChannelSetting() ||
            buildTarget == BuildTarget.StandaloneWindows64 && GetWindows64BuildABForPerChannelSetting()||
            buildTarget == BuildTarget.WebGL && GetWebGlBuildABForPerChannelSetting())

        {
            return true;
        }
        return false;
    }

    public static string GetCurPlatformName()
    {
        return GetPlatformName(EditorUserBuildSettings.activeBuildTarget);
    }
    public static string GetPlatformName(BuildTarget buildTarget)
    {
        switch (buildTarget)
        {
            case BuildTarget.Android:
                return "Android";
            case BuildTarget.iOS:
                return "iOS";
            case BuildTarget.StandaloneWindows:
                return "StandaloneWindows";
            case BuildTarget.StandaloneWindows64:
                return "StandaloneWindows64";
            case BuildTarget.WebGL:
                return "WebGL";
            default:
                Debug.LogError("Error buildTarget!!!");
                return null;
        }
    }
    public static ChannelType GetCurSelectedChannel()
    {
        ChannelType channelType = ChannelType.Test;
        string channelName = EditorPrefs.GetString("ChannelName");
        if (Enum.IsDefined(typeof(ChannelType), channelName))
        {
            channelType = (ChannelType)Enum.Parse(typeof(ChannelType), channelName);
        }
        else
        {
            EditorPrefs.SetString("ChannelName", ChannelType.Test.ToString());
        }
        return channelType;
    }

    public static void SaveCurSelectedChannel(ChannelType channelType)
    {
        EditorPrefs.SetString("ChannelName", channelType.ToString());
    }

    public static string GetPlatformChannelFolderName(BuildTarget target, string channelName)
    {
        if (BuildAssetBundlesForPerChannel(target))
        {
            // 不同渠道的AB输出到不同的文件夹
            return channelName;
        }
        else
        {
            // 否则写入通用的平台文件夹
            return "Common";
            //return GetPlatformName(target);
        }
    }

    public static string GetChannelRelativePath(BuildTarget target, string channelName)
    {
        string outputPath = Path.Combine(GetPlatformName(target), GetPlatformChannelFolderName(target, channelName));
        return outputPath;
    }

    public static string GetAssetBundleRelativePath(BuildTarget target, string channelName)
    {
        string outputPath = GetChannelRelativePath(target, channelName);
        outputPath = Path.Combine(outputPath, BuildUtils.ManifestBundleName);
        return outputPath;
    }

    public static string GetChannelOutputPath(BuildTarget target, string channelName)
    {
        string outputPath = Path.Combine(AssetBundleConfig.AssetBundlesBuildOutputPath, GetChannelRelativePath(target, channelName));
        GameUtility.CheckDirAndCreateWhenNeeded(outputPath);
        return outputPath;
    }

    public static string GetAssetBundleOutputPath(BuildTarget target, string channelName)
    {
        string outputPath = Path.Combine(AssetBundleConfig.AssetBundlesBuildOutputPath, GetAssetBundleRelativePath(target, channelName));
        GameUtility.CheckDirAndCreateWhenNeeded(outputPath);
        return outputPath;
    }

    public static string GetAssetBundleFilePath(BuildTarget target, string channelName, string fileName)
    {
        string outputPath = GetAssetBundleOutputPath(target, channelName);
        return Path.Combine(outputPath, fileName);

    }

    public static string GetAssetbundleManifestPath(BuildTarget target, string channelName)
    {
        string outputPath = GetAssetBundleOutputPath(target, channelName);
        return Path.Combine(outputPath, BuildUtils.ManifestBundleName);
    }

    public static string GetCurPlatformChannelRelativePath()
    {
        var buildTarget = EditorUserBuildSettings.activeBuildTarget;
        var channelName = GetCurSelectedChannel().ToString();
        return GetChannelRelativePath(buildTarget, channelName);
    }

    public static string GetCurBuildSettingAssetBundleOutputPath()
    {
        var buildTarget = EditorUserBuildSettings.activeBuildTarget;
        var channelType = GetCurSelectedChannel();
        return GetAssetBundleOutputPath(buildTarget, channelType.ToString());
    }

    public static string GetCurBuildSettingAssetBundleManifestPath()
    {
        var buildTarget = EditorUserBuildSettings.activeBuildTarget;
        var channelType = GetCurSelectedChannel();
        return GetAssetbundleManifestPath(buildTarget, channelType.ToString());
    }

    public static string GetCurBuildSettingStreamingManifestPath()
    {
        string path = AssetBundleUtility.GetStreamingAssetsDataPath();
        path = Path.Combine(path, BuildUtils.ManifestBundleName);
        return path;
    }

    public static AssetBundleManifest GetManifestFormLocal(string manifestPath)
    {
        FileInfo fileInfo = new FileInfo(manifestPath);
        if (!fileInfo.Exists)
        {
            Debug.LogError("You need to build assetbundles first to get assetbundle dependencis info!");
            return null;
        }
        byte[] bytes = GameUtility.SafeReadAllBytes(fileInfo.FullName);
        if (bytes == null)
        {
            return null;
        }
        AssetBundle assetBundle = AssetBundle.LoadFromMemory(bytes);
        AssetBundleManifest manifest = assetBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        assetBundle.Unload(false);
        return manifest;
    }

    public static void CopyAssetBundlesToStreamingAssets(BuildTarget buildTarget, string channelName)
    {
        string source = GetAssetBundleOutputPath(buildTarget, channelName);
        string destination = AssetBundleUtility.GetStreamingAssetsDataPath();
        // 有毒，竟然在有的windows系统这个函数删除不了目录，不知道是不是Unity的Bug
        // GameUtility.SafeDeleteDir(destination);
        AssetDatabase.DeleteAsset(GameUtility.FullPathToAssetPath(destination));
        AssetDatabase.Refresh();

        try
        {
            FileUtil.CopyFileOrDirectoryFollowSymlinks(source, destination);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Something wrong, you need manual delete AssetBundles folder in StreamingAssets, err : " + ex);
            return;
        }

        var allManifest = GameUtility.GetSpecifyFilesInFolder(destination, new string[] { ".manifest" });
        if (allManifest != null && allManifest.Length > 0)
        {
            for (int i = 0; i < allManifest.Length; i++)
            {
                GameUtility.SafeDeleteFile(allManifest[i]);
            }
        }

        AssetDatabase.Refresh();
    }

    public static void CopyCurSettingAssetBundlesToStreamingAssets()
    {
        var buildTarget = EditorUserBuildSettings.activeBuildTarget;
        var channelName = GetCurSelectedChannel().ToString();
        CopyAssetBundlesToStreamingAssets(buildTarget, channelName);
        Debug.Log("Copy channel assetbundles to streaming assets done!");
    }

    public static void CheckAndRunAllCheckers(bool buildForPerChannel, bool forceRun)
    {
        // 这东西有点浪费时间，没必要的时候不跑它
        if (AssetBundleDispatcherInspector.hasAnythingModified || forceRun)
        {
            AssetBundleDispatcherInspector.hasAnythingModified = false;
            var start = DateTime.Now;
            CheckAssetBundles.Run(buildForPerChannel);
            Debug.Log("Finished CheckAssetBundles.Run! use " + (DateTime.Now - start).TotalSeconds + "s");
        }
    }

}
