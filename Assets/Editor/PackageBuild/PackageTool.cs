using UnityEditor;
using UnityEngine;
using System.IO;
using GameChannel;
using AssetBundles;
using System;
using System.Collections.Generic;
using System.Text;

public class PackageTool : EditorWindow
{
    static private BuildTarget buildTarget = BuildTarget.StandaloneWindows;
    static private ChannelType channelType = ChannelType.Test;
    static private string resVersion = "1.0.000";
    static private string bundleVersion = "1.0.000";
    static private LocalServerType localServerType = LocalServerType.CurrentMachine;
    static private string localServerIP = "127.0.0.1";
    static private bool androidBuildABForPerChannel;
    static private bool iosBuildABForPerChannel;
    static private bool windosBuildABForPerChannel;
    static private bool windows64BuildABForPerChannel;
    static private bool webGLBuildABForPerChannel;


    static private bool buildABSForPerChannel;

    [MenuItem("AssetBundles/打包工具", false, 0)]
    static void Init()
    {
        EditorWindow.GetWindow(typeof(PackageTool));
    }

    void OnEnable()
    {
        buildTarget = EditorUserBuildSettings.activeBuildTarget;
        channelType = PackageUtils.GetCurSelectedChannel();

        resVersion = ReadResVersionConfig();
        bundleVersion = PlayerSettings.bundleVersion;

        localServerType = PackageUtils.GetLocalServerType();
        localServerIP = PackageUtils.GetLocalServerIP();

        androidBuildABForPerChannel = PackageUtils.GetAndroidBuildABForPerChannelSetting();
        iosBuildABForPerChannel = PackageUtils.GetIOSBuildABForPerChannelSetting();
        windosBuildABForPerChannel = PackageUtils.GetWindowsBuildABForPerChannelSetting();
        windows64BuildABForPerChannel = PackageUtils.GetWindows64BuildABForPerChannelSetting();
        webGLBuildABForPerChannel = PackageUtils.GetWebGlBuildABForPerChannelSetting();
    }
    void OnGUI() 
    {
        GUILayout.BeginVertical();
        GUILayout.Space(10);
        buildTarget = (BuildTarget)EditorGUILayout.EnumPopup("生成平台 : ", buildTarget);
        GUILayout.Space(5);
        channelType = (ChannelType)EditorGUILayout.EnumPopup("生成渠道 : ", channelType);
        GUILayout.EndVertical();
        if (GUI.changed)
        {
            PackageUtils.SaveCurSelectedChannel(channelType);
        }
        DrawAssetBundlesConfigGUI();
        DrawConfigGUI();
        DrawLocalServerGUI();
        DrawAssetBundlesGUI();
        //DrawXLuaGUI();
        //DrawBuildPlayerGUI();

    }

    #region AB相关配置
    void DrawAssetBundlesConfigGUI()
    {
        GUILayout.Space(10);
        GUILayout.Label("-------------[AB资源相关配置]-------------");
        GUILayout.Space(2);

        // 是否为每个channel打一个AB包
        GUILayout.Space(3);
        GUILayout.BeginHorizontal();
        GUILayout.Label("为每个渠道打包 : ", GUILayout.Width(150));
        switch (buildTarget)
        {
            case BuildTarget.iOS:
                buildABSForPerChannel = EditorGUILayout.Toggle(iosBuildABForPerChannel, GUILayout.Width(50));
                if (buildABSForPerChannel != iosBuildABForPerChannel)
                {
                    iosBuildABForPerChannel = buildABSForPerChannel;
                    PackageUtils.SaveIOSBuildABForPerChannelSetting(buildABSForPerChannel);
                }
                break;
            case BuildTarget.Android:
                buildABSForPerChannel = EditorGUILayout.Toggle(androidBuildABForPerChannel, GUILayout.Width(50));
                if (buildABSForPerChannel != androidBuildABForPerChannel)
                {
                    androidBuildABForPerChannel = buildABSForPerChannel;
                    PackageUtils.SaveAndroidBuildABForPerChannelSetting(buildABSForPerChannel);
                }
                break;
            case BuildTarget.StandaloneWindows:
                buildABSForPerChannel = EditorGUILayout.Toggle(windosBuildABForPerChannel, GUILayout.Width(50));
                if (buildABSForPerChannel != windosBuildABForPerChannel)
                {
                    windosBuildABForPerChannel = buildABSForPerChannel;
                    PackageUtils.SaveWindowsBuildABForPerChannelSetting(buildABSForPerChannel);
                }
                break;
            case BuildTarget.StandaloneWindows64:
                buildABSForPerChannel = EditorGUILayout.Toggle(windows64BuildABForPerChannel, GUILayout.Width(50));
                if (buildABSForPerChannel != windows64BuildABForPerChannel)
                {
                    windows64BuildABForPerChannel = buildABSForPerChannel;
                    PackageUtils.SaveWindows64BuildABForPerChannelSetting(buildABSForPerChannel);
                }
                break;
            case BuildTarget.WebGL:
                buildABSForPerChannel = EditorGUILayout.Toggle(webGLBuildABForPerChannel, GUILayout.Width(50));
                if (buildABSForPerChannel != webGLBuildABForPerChannel)
                {
                    webGLBuildABForPerChannel = buildABSForPerChannel;
                    PackageUtils.SaveWebGlBuildABForPerChannelSetting(buildABSForPerChannel);
                }
                break;
            default:
                break;
        }
        if (GUILayout.Button("检测资源", GUILayout.Width(200)))
        {
            bool checkChannel = PackageUtils.BuildAssetBundlesForPerChannel(buildTarget);
            PackageUtils.CheckAndRunAllCheckers(checkChannel, true);
        }
        GUILayout.EndHorizontal();
    }
    #endregion

    #region 资源配置GUI
    void DrawConfigGUI()
    {
        GUILayout.Space(10);
        GUILayout.Label("-------------[资源版本相关配置]-------------");

        GUILayout.Space(2);
        GUILayout.BeginHorizontal();
        GUILayout.Label("资源版本", GUILayout.Width(100));
        string curResVersion = GUILayout.TextField(resVersion, GUILayout.Width(100));
        if (curResVersion != resVersion)
        {
            resVersion = curResVersion;
            SaveAllCurrentVersionFile(true);
        }
        GUILayout.Label("Auto increase sub version, otherwise modify the text directly!", GUILayout.Width(500));
        GUILayout.EndHorizontal();

        GUILayout.Space(3);
        GUILayout.BeginHorizontal();
        GUILayout.Label("notice_version", GUILayout.Width(100));
        GUILayout.Label("1.0.0", GUILayout.Width(100));
        GUILayout.Label("No supported yet!", GUILayout.Width(500));
        GUILayout.EndHorizontal();

        GUILayout.Space(3);
        GUILayout.BeginHorizontal();
        GUILayout.Label("app_version", GUILayout.Width(100));
        string curBundleVersion = GUILayout.TextField(bundleVersion, GUILayout.Width(100));
        if (curBundleVersion != bundleVersion)
        {
            bundleVersion = curBundleVersion;
            PlayerSettings.bundleVersion = curBundleVersion;
        }
        GUILayout.Label("Auto increase sub version, otherwise modify the text directly or go to PlayerSetting!", GUILayout.Width(500));
        GUILayout.EndHorizontal();

        GUILayout.Space(3);
        GUILayout.BeginHorizontal();
        if (PackageUtils.BuildAssetBundlesForPerChannel(buildTarget))
        {
            if (GUILayout.Button("读取当前的渠道版本", GUILayout.Width(200)))
            {
                LoadCurrentResVersionFromFile();
            }
            if (GUILayout.Button("获取所有的渠道版本", GUILayout.Width(200)))
            {
                ValidateAllResVersionFile();
            }
            if (GUILayout.Button("保存所有渠道的版本信息", GUILayout.Width(200)))
            {
                SaveAllVersionFile();
            }
        }
        else
        {
            if (GUILayout.Button("读取当前的渠道版本", GUILayout.Width(200)))
            {
                LoadCurrentResVersionFromFile();
            }
            if (GUILayout.Button("保存所有渠道的版本信息", GUILayout.Width(200)))
            {
                SaveAllCurrentVersionFile();
            }
        }
        GUILayout.EndHorizontal();
    }
    #endregion

    #region 本地服务器配置GUI
    void DrawLocalServerGUI()
    {
        GUILayout.Space(10);
        GUILayout.Label("-------------[AssetBundles 本地服务器]-------------");
        GUILayout.Space(2);

        GUILayout.BeginHorizontal();
        var curSelected = (LocalServerType)EditorGUILayout.EnumPopup("本地服务器类型 : ", localServerType, GUILayout.Width(300));
        bool typeChanged = curSelected != localServerType;
        if (typeChanged)
        {
            PackageUtils.SaveLocalServerType(curSelected);

            localServerType = curSelected;
            localServerIP = PackageUtils.GetLocalServerIP();
        }
        if (localServerType == LocalServerType.CurrentMachine)
        {
            GUILayout.Label(localServerIP);
        }
        else
        {
            localServerIP = GUILayout.TextField(localServerIP, GUILayout.Width(100));
            if (GUILayout.Button("Save", GUILayout.Width(200)))
            {
                PackageUtils.SaveLocalServerIP(localServerIP);
            }
        }
        GUILayout.EndHorizontal();
    }
    #endregion

    #region AB相关操作GUI
    void DrawAssetBundlesGUI()
    {
        GUILayout.Space(10);
        GUILayout.Label("-------------[Build AssetBundles资源]-------------");
        GUILayout.Space(2);

        GUILayout.Space(3);
        GUILayout.BeginHorizontal();
        if (buildABSForPerChannel)
        {
            if (GUILayout.Button("只打包当前渠道", GUILayout.Width(200)))
            {
                EditorApplication.delayCall += BuildAssetBundlesForCurrentChannel;
            }
            if (GUILayout.Button("打包所有渠道", GUILayout.Width(200)))
            {
                EditorApplication.delayCall += BuildAssetBundlesForAllChannels;
            }
            if (GUILayout.Button("打开当前的输出目录", GUILayout.Width(200)))
            {
                AssetBundleMenuItems.ToolsOpenOutput();
            }
            if (GUILayout.Button("拷贝到本地目录", GUILayout.Width(200)))
            {
                AssetBundleMenuItems.ToolsCopyAssetbundles();
            }
        }
        else
        {
            if (GUILayout.Button("执行打包", GUILayout.Width(200)))
            {
                EditorApplication.delayCall += BuildAssetBundlesForCurrentChannel;
            }
            if (GUILayout.Button("打开当前的输出目录", GUILayout.Width(200)))
            {
                AssetBundleMenuItems.ToolsOpenOutput();
            }
            if (GUILayout.Button("拷贝到本地目录", GUILayout.Width(200)))
            {
                AssetBundleMenuItems.ToolsCopyAssetbundles();
            }
        }
        GUILayout.EndHorizontal();
    }
    #endregion

    #region 资源配置操作
    public static string ReadResVersionConfig()
    {
        // 从数据库加载资源版本号
        AssetBundleResVersionConfig config = AssetDatabase.LoadAssetAtPath(AssetBundleResVersionConfig.RES_PATH, typeof(AssetBundleResVersionConfig)) as AssetBundleResVersionConfig;
        if (config == null)
        {
            config = CreateInstance<AssetBundleResVersionConfig>();
            AssetDatabase.CreateAsset(config, AssetBundleResVersionConfig.RES_PATH);
            AssetDatabase.Refresh();
        }

        return config.resVersion;
    }
    public static void SaveResVersionConfig(string curResVersion)
    {
        // 保存资源版本号到数据库
        AssetBundleResVersionConfig config = AssetDatabase.LoadAssetAtPath(AssetBundleResVersionConfig.RES_PATH, typeof(AssetBundleResVersionConfig)) as AssetBundleResVersionConfig;
        if (config == null)
        {
            config = CreateInstance<AssetBundleResVersionConfig>();
            AssetDatabase.CreateAsset(config, AssetBundleResVersionConfig.RES_PATH);
            AssetDatabase.Refresh();
        }

        config.resVersion = curResVersion;
        EditorUtility.SetDirty(config);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    public static string ReadResVersionFile(BuildTarget target, ChannelType channel)
    {
        // 从资源版本号文件（当前渠道AB输出目录中）加载资源版本号
        string rootPath = PackageUtils.GetAssetBundleOutputPath(target, channel.ToString());
        return GameUtility.SafeReadAllText(Path.Combine(rootPath, BuildUtils.ResVersionFileName));
    }
    public static void SaveAllVersionFile(BuildTarget target, ChannelType channel)
    {
        // 保存所有版本号信息到资源版本号文件（当前渠道AB输出目录中）
        string rootPath = PackageUtils.GetAssetBundleOutputPath(target, channel.ToString());
        GameUtility.SafeWriteAllText(Path.Combine(rootPath, BuildUtils.ResVersionFileName), resVersion);
        GameUtility.SafeWriteAllText(Path.Combine(rootPath, BuildUtils.NoticeVersionFileName), resVersion);
        GameUtility.SafeWriteAllText(Path.Combine(rootPath, BuildUtils.AppVersionFileName), bundleVersion);
    }
    public static void LoadCurrentResVersionFromFile(bool silence = false)
    {
        var buildTargetName = PackageUtils.GetPlatformName(buildTarget);
        string readVersion = ReadResVersionFile(buildTarget, channelType);
        if (string.IsNullOrEmpty(readVersion))
        {
            if (!silence)
            {
                EditorUtility.DisplayDialog("错误", string.Format("没有资源版本号文件！ : \n\n检测平台 : {0} \n当前渠道 : {1} \n\n",
                    buildTargetName, channelType.ToString()), "确定");
            }
        }
        else
        {
            resVersion = readVersion;
            if (!silence)
            {
                EditorUtility.DisplayDialog("成功", string.Format("读取资源版本号文件成功 : \n\n检测平台 : {0} \n当前渠道 : {1} \n当前渠道版本：{2} \n\n",
                    buildTargetName, channelType.ToString(), resVersion), "确定");
            }
        }
    }
    public static void SaveAllCurrentVersionFile(bool silence = false)
    {
        var buildTargetName = PackageUtils.GetPlatformName(buildTarget);
        SaveAllVersionFile(buildTarget, channelType);
        SaveResVersionConfig(resVersion);
        PlayerSettings.bundleVersion = bundleVersion;
        if (!silence)
        {
            EditorUtility.DisplayDialog("成功", string.Format("保存所有版本文件 : \n\n检测平台 : {0} \n当前渠道 : {1} \n\n",
                buildTargetName, channelType.ToString()), "确定");
        }
    }
    public static void ValidateAllResVersionFile()
    {
        // 校验所有渠道AB输出目录下资源版本号信息
        Dictionary<string, List<ChannelType>> versionMap = new Dictionary<string, List<ChannelType>>();
        List<ChannelType> channelList = null;
        foreach (var current in (ChannelType[])Enum.GetValues(typeof(ChannelType)))
        {
            string readVersion = ReadResVersionFile(buildTarget, current);
            if (readVersion == null)
            {
                readVersion = "";
            }
            versionMap.TryGetValue(readVersion, out channelList);
            if (channelList == null)
            {
                channelList = new List<ChannelType>();
            }
            channelList.Add(current);
            versionMap[readVersion] = channelList;
        }

        StringBuilder sb = new StringBuilder();
        foreach (var current in versionMap)
        {
            var version = current.Key;
            var channels = current.Value;
            sb.AppendFormat("Version : {0}\n", version);
            sb.AppendFormat("{0} Channels : ", channels.Count);
            foreach (var channel in channels)
            {
                sb.AppendFormat("{0}, ", channel.ToString());
            }
            sb.AppendLine("\n-------------------------------------\n");
        }
        EditorUtility.DisplayDialog("Result", sb.ToString(), "Confirm");
    }
    void SaveAllVersionFile()
    {
        // 保存当前版本号信息到所有渠道AB输出目录
        foreach (var current in (ChannelType[])Enum.GetValues(typeof(ChannelType)))
        {
            SaveAllVersionFile(buildTarget, current);
        }
        EditorUtility.DisplayDialog("Success", "Save all version files to all channels done!", "Confirm");
    }

    #endregion

    #region AB相关操作
    public static void IncreaseResSubVersion()
    {
        // 每一次构建资源，子版本号自增，注意：前两个字段这里不做托管，自行编辑设置
        string[] vers = resVersion.Split('.');
        if (vers.Length > 0)
        {
            int subVer = 0;
            int.TryParse(vers[vers.Length - 1], out subVer);
            vers[vers.Length - 1] = string.Format("{0:D3}", subVer + 1);
        }
        resVersion = string.Join(".", vers);
        SaveAllCurrentVersionFile(true);
    }
    public static void BuildAssetBundlesForCurrentChannel()
    {
        buildTarget = EditorUserBuildSettings.activeBuildTarget;
        IncreaseResSubVersion();

        var start = DateTime.Now;
 
        BuildPlayer.BuildAssetBundles(buildTarget, channelType.ToString());

        var buildTargetName = PackageUtils.GetPlatformName(buildTarget);
        EditorUtility.DisplayDialog("成功", string.Format("Build AssetBundles for : \n\n检测平台 : {0} \n当前渠道 : {1} \n\n结束! 用时 {2}s",
            buildTargetName, channelType, (DateTime.Now - start).TotalSeconds), "确定");
    }
    public static void BuildAssetBundlesForAllChannels()
    {
        buildTarget = EditorUserBuildSettings.activeBuildTarget;
        IncreaseResSubVersion();

        var start = DateTime.Now;
        BuildPlayer.BuildAssetBundlesForAllChannels(buildTarget);

        var buildTargetName = PackageUtils.GetPlatformName(buildTarget);
        EditorUtility.DisplayDialog("成功", string.Format("Build AssetBundles for : \n\n检测平台 : {0} \n当前渠道 : all \n\n结束! 用时 {1}s", 
            buildTargetName, (DateTime.Now - start).TotalSeconds), "确定");
    }

    #endregion


}
