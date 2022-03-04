using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// Assetbundle 菜单管理
/// made for maple @2022.03.01
/// TODO：
/// 1、提供可视化界面的Assetbundle管理，包括依赖、公共包查看，ab添加、移除功能
/// 2、重构这部分代码，全部功能整合到打包窗口，不用菜单选项了
/// 3、提供各个渠道历史资源版本AB变化对比工具，让增量更新透明化
/// </summary>
namespace AssetBundles 
{
    // unity editor启动和运行时调用静态构造函数
    [InitializeOnLoad]
    public class AssetBundleMenuItems
    {
        const string kSimulateMode = "AssetBundles/选择模式/模拟模式";
        const string kEditorMode = "AssetBundles/选择模式/编辑器模式";
        const string kToolRunAllCheckers = "AssetBundles/检测所有的AB资源";
        const string kToolBuildForCurrentSetting = "AssetBundles/BuildAssetBundle 当前平台";
        const string kToolsCopyAssetbundles = "AssetBundles/拷贝资源到 StreamingAssets";
        const string kToolsOpenOutput = "AssetBundles/打开当前的AB输出目录";
        const string kToolsOpenPerisitentData = "AssetBundles/打开当前的缓存目录";
        const string kToolsClearOutput = "AssetBundles/清空当前的输出目录";
        const string kToolsClearStreamingAssets = "AssetBundles/清空 StreamingAssets";
        const string kToolsClearPersistentAssets = "AssetBundles/清空 缓存目录";

        const string kCreateAssetbundleForCurrent = "Assets/AssetBundles/创建AB资源对于当前选中 &#z";
        const string kCreateAssetbundleForChildren = "Assets/AssetBundles/创建AB资源对于当前选中的子物体 &#x";
        const string kAssetDependencis = "Assets/AssetBundles/资源所有的依赖 &#h";
        const string kAssetbundleAllDependencis = "Assets/AssetBundles/资源所有的依赖的Bundle &#j";
        const string kAssetbundleDirectDependencis = "Assets/AssetBundles/资源直接的依赖的Bundle &#k";
        //构造函数
        static AssetBundleMenuItems()
        {
            CheckSimulateModelEnv();
        }
        //检测是否为模拟模式
        static void CheckSimulateModelEnv()
        {
            if (!AssetBundleConfig.IsSimulateMode)
            {
                return;
            }
            //var buildTargetName = PackageUtils.GetCurPlatformName();
        }

        [MenuItem(kEditorMode, false)]
        public static void ToggleEditorMode()
        {
            if (AssetBundleConfig.IsSimulateMode)
            {
                AssetBundleConfig.IsEditorMode = true;
            }
        }

        [MenuItem(kEditorMode, true)]
        public static bool ToggleEditorModeValidate()
        {
            Menu.SetChecked(kEditorMode, AssetBundleConfig.IsEditorMode);
            return true;
        }

        [MenuItem(kSimulateMode, false)]
        public static void ToggleSimulateMode()
        {
            if (AssetBundleConfig.IsEditorMode)
            {
                AssetBundleConfig.IsSimulateMode = true;
                CheckSimulateModelEnv();
            }
        }

        [MenuItem(kSimulateMode, true)]
        public static bool ToggleSimulateModeValidate()
        {
            Menu.SetChecked(kSimulateMode, AssetBundleConfig.IsSimulateMode);
            return true;
        }

        [MenuItem(kToolRunAllCheckers)]
        static public void ToolRunAllCheckers()
        {
            var buildTargetName = PackageUtils.GetCurPlatformName();
            var channelName = PackageUtils.GetCurSelectedChannel().ToString();
            bool checkCopy = EditorUtility.DisplayDialog("检测ab资源 提示",
                string.Format("准备检测AB资源 : \n\n检测平台 : {0} \n当前渠道 : {1} \n\n是否继续 ?", buildTargetName, channelName),
                "确认", "取消");
            if (!checkCopy)
            {
                return;
            }

            bool checkChannel = PackageUtils.BuildAssetBundlesForPerChannel(EditorUserBuildSettings.activeBuildTarget);
            PackageUtils.CheckAndRunAllCheckers(checkChannel, true);
        }
        [MenuItem(kToolBuildForCurrentSetting, false, 1100)]
        static public void ToolBuildForCurrentSetting()
        {
            var buildTargetName = PackageUtils.GetCurPlatformName();
            var channelName = PackageUtils.GetCurSelectedChannel().ToString();
            bool checkCopy = EditorUtility.DisplayDialog("Build AssetBundle 提示",
                string.Format("对于当前平台进行打包资源 : \n\n检测平台 : {0} \n当前渠道 : {1} \n\n是否继续 ?", buildTargetName, channelName),
                "确认", "取消");
            if (!checkCopy)
            {
                return;
            }
            PackageTool.BuildAssetBundlesForCurrentChannel();
        }
        [MenuItem(kToolsCopyAssetbundles, false, 1101)]
        static public void ToolsCopyAssetbundles()
        {
            var buildTargetName = PackageUtils.GetCurPlatformName();
            var channelName = PackageUtils.GetCurSelectedChannel().ToString();
            bool checkCopy = EditorUtility.DisplayDialog("拷贝提示！",
                string.Format("拷贝 AssetBundles 资源到 streamingAssets 文件夹下 : \n\n检测平台 : {0} \n当前渠道 : {1} \n\n是否继续 ?", buildTargetName, channelName),
                "确认", "取消");
            if (!checkCopy)
            {
                return;
            }

            // 拷贝到StreamingAssets目录时，相当于执行大版本更新，那么沙盒目录下的数据就作废了
            // 真机上会对比这两个目录下的App版本号来删除，编辑器下暴力一点，直接删除
            ToolsClearPersistentAssets();
            PackageUtils.CopyCurSettingAssetBundlesToStreamingAssets();
        }

        [MenuItem(kToolsOpenOutput, false, 1201)]
        static public void ToolsOpenOutput()
        {
            string outputPath = PackageUtils.GetCurBuildSettingAssetBundleOutputPath();
            EditorUtils.ExplorerFolder(outputPath);
        }

        [MenuItem(kToolsOpenPerisitentData, false, 1202)]
        static public void ToolsOpenPerisitentData()
        {
            EditorUtils.ExplorerFolder(Application.persistentDataPath);
        }

        [MenuItem(kToolsClearOutput, false, 1302)]
        static public void ToolsClearOutput()
        {
            var buildTargetName = PackageUtils.GetCurPlatformName();
            var channelName = PackageUtils.GetCurSelectedChannel().ToString();
            bool checkClear = EditorUtility.DisplayDialog("清除资源输出目录",
                string.Format("清除输出目录下的当前渠道所有AssetBundles资源 : \n\n检测平台 : {0} \n当前渠道 : {1} \n\n 是否继续 ?", buildTargetName, channelName),
                "确认", "取消");
            if (!checkClear)
            {
                return;
            }
            string outputPath = PackageUtils.GetCurBuildSettingAssetBundleOutputPath();
            GameUtility.SafeDeleteDir(outputPath);
            Debug.Log(string.Format("清除完成 : {0}", outputPath));
        }
        [MenuItem(kToolsClearStreamingAssets, false, 1303)]
        static public void ToolsClearStreamingAssets()
        {
            bool checkClear = EditorUtility.DisplayDialog("清除StreamingAssets目录",
                "清除StreamingAssets目录下的当前渠道所有AssetBundles资源, \n\n 是否继续 ?",
                "确认", "取消");
            if (!checkClear)
            {
                return;
            }
            string outputPath = Path.Combine(Application.streamingAssetsPath, AssetBundleConfig.AssetBundlesFolderName);
            GameUtility.SafeClearDir(outputPath);
            AssetDatabase.Refresh();
            Debug.Log(string.Format("清除 {0} assetbundle streaming assets 完成!", PackageUtils.GetCurPlatformName()));
        }

        [MenuItem(kToolsClearPersistentAssets, false, 1301)]
        static public void ToolsClearPersistentAssets()
        {
            bool checkClear = EditorUtility.DisplayDialog("清除沙盒目录",
                "清除沙盒路径assetbundles资源文件夹  \n\n是否继续 ?",
                "确认", "取消");
            if (!checkClear)
            {
                return;
            }

            string outputPath = Path.Combine(Application.persistentDataPath, AssetBundleConfig.AssetBundlesFolderName);
            GameUtility.SafeDeleteDir(outputPath);
            Debug.Log(string.Format("清除 {0} assetbundle persistent assets 完成!", PackageUtils.GetCurPlatformName()));
        }

        #region CreateAssetbundle
        [MenuItem(kCreateAssetbundleForCurrent)]
        static public void CreateAssetbundleForCurrent()
        {
            if (AssetBundleEditorHelper.HasValidSelection())
            {
                bool checkCreate = EditorUtility.DisplayDialog("CreateAssetbundleForCurrent Warning",
                    "Create assetbundle for cur selected objects will reset assetbundles which contains this dir, continue ?",
                    "Yes", "No");
                if (!checkCreate)
                {
                    return;
                }
                Object[] selObjs = Selection.objects;
                AssetBundleEditorHelper.CreateAssetbundleForCurrent(selObjs);
                List<string> removeList = AssetBundleEditorHelper.RemoveAssetbundleInParents(selObjs);
                removeList.AddRange(AssetBundleEditorHelper.RemoveAssetbundleInChildren(selObjs));
                string removeStr = string.Empty;
                int i = 0;
                foreach (string str in removeList)
                {
                    removeStr += string.Format("[{0}]{1}\n", ++i, str);
                }
                if (removeList.Count > 0)
                {
                    Debug.Log(string.Format("CreateAssetbundleForCurrent done!\nRemove list :" +
                        "\n-------------------------------------------\n" +
                        "{0}" +
                        "\n-------------------------------------------\n",
                        removeStr));
                }
            }
        }

        [MenuItem(kCreateAssetbundleForChildren)]
        static public void CreateAssetbundleForChildren()
        {
            if (AssetBundleEditorHelper.HasValidSelection())
            {
                bool checkCreate = EditorUtility.DisplayDialog("CreateAssetbundleForChildren Warning",
                    "Create assetbundle for all children of cur selected objects will reset assetbundles which contains this dir, continue ?",
                    "Yes", "No");
                if (!checkCreate)
                {
                    return;
                }
                Object[] selObjs = Selection.objects;
                foreach (Object str in selObjs)
                {
                    string Obj_anme = AssetDatabase.GetAssetPath(str);
                    Debug.LogError(Obj_anme);
                }
                AssetBundleEditorHelper.CreateAssetbundleForChildren(selObjs);
                List<string> removeList = AssetBundleEditorHelper.RemoveAssetbundleInParents(selObjs);
                removeList.AddRange(AssetBundleEditorHelper.RemoveAssetbundleInChildren(selObjs, true, false));
                string removeStr = string.Empty;
                int i = 0;
                foreach (string str in removeList)
                {
                    removeStr += string.Format("[{0}]{1}\n", ++i, str);
                }
                if (removeList.Count > 0)
                {
                    Debug.Log(string.Format("CreateAssetbundleForChildren done!\nRemove list :" +
                    "\n-------------------------------------------\n" +
                    "{0}" +
                    "\n-------------------------------------------\n",
                    removeStr));
                }
            }
        }

        [MenuItem(kAssetDependencis)]
        static public void ListAssetDependencis()
        {
            if (AssetBundleEditorHelper.HasValidSelection())
            {
                Object[] selObjs = Selection.objects;
                string depsStr = AssetBundleEditorHelper.GetDependencyText(selObjs, false);
                string selStr = string.Empty;
                int i = 0;
                foreach (Object obj in selObjs)
                {
                    selStr += string.Format("[{0}]{1};", ++i, AssetDatabase.GetAssetPath(obj));
                }
                Debug.Log(string.Format("Selection({0}) depends on the following assets:" +
                    "\n-------------------------------------------\n" +
                    "{1}" +
                    "\n-------------------------------------------\n",
                    selStr,
                    depsStr));
                AssetBundleEditorHelper.SelectDependency(selObjs, false);
            }
        }

        [MenuItem(kAssetbundleAllDependencis)]
        static public void ListAssetbundleAllDependencis()
        {
            ListAssetbundleDependencis(true);
        }

        [MenuItem(kAssetbundleDirectDependencis)]
        static public void ListAssetbundleDirectDependencis()
        {
            ListAssetbundleDependencis(false);
        }

        static public void ListAssetbundleDependencis(bool isAll)
        {
            if (AssetBundleEditorHelper.HasValidSelection())
            {
                string localFilePath = PackageUtils.GetCurBuildSettingAssetBundleManifestPath();

                Object[] selObjs = Selection.objects;
                var depsList = AssetBundleEditorHelper.GetDependancisFormBuildManifest(localFilePath, selObjs, isAll);
                if (depsList == null)
                {
                    return;
                }

                depsList.Sort();
                string depsStr = string.Empty;
                int i = 0;
                foreach (string str in depsList)
                {
                    depsStr += string.Format("[{0}]{1}\n", ++i, str);
                }

                string selStr = string.Empty;
                i = 0;
                foreach (Object obj in selObjs)
                {
                    selStr += string.Format("[{0}]{1};", ++i, AssetDatabase.GetAssetPath(obj));
                }
                Debug.Log(string.Format("Selection({0}) directly depends on the following assetbundles:" +
                    "\n-------------------------------------------\n" +
                    "{1}" +
                    "\n-------------------------------------------\n",
                    selStr,
                    depsStr));
            }
        }
        #endregion

    }
}



