using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif
/// <summary>
/// add by maple @2022.03.01
/// TODO
/// AssetBundle的相关配置文件
/// </summary>
namespace AssetBundles 
{
	public class AssetBundleConfig
	{
        public const string AssetBundlesFolderName = "AssetBundles";
        public const string AssetBundleSuffix = ".assetbundle";
        public const string AssetsFolderName = "GameApp";
        public const string ChannelFolderName = "Channel";
        public const string AssetsPathMapFileName = "AssetsMap.bytes";
        public const string VariantsMapFileName = "VariantsMap.bytes";
        public const string AssetBundleServerUrlFileName = "AssetBundleServerUrl.txt";
        public const string VariantMapParttren = "Variant";
        public const string CommonMapPattren = ",";


#if UNITY_EDITOR
        public static string AssetBundlesBuildOutputPath
        {
            get
            {
                string outputPath = Path.Combine(System.Environment.CurrentDirectory, AssetBundlesFolderName);
                GameUtility.CheckDirAndCreateWhenNeeded(outputPath);
                return outputPath;
            }
        }

        public static string LocalSvrAppWorkPath
        {
            get
            {
                return AssetBundlesBuildOutputPath;
            }
        }

        private static int mIsEditorMode = -1;
        private const string kIsEditorMode = "IsEditorMode";
        private static int mIsSimulateMode = -1;
        private const string kIsSimulateMode = "IsSimulateMode";

        public static bool IsEditorMode
        {
            get
            {
                if (mIsEditorMode == -1)
                {
                    if (!EditorPrefs.HasKey(kIsEditorMode))
                    {
                        EditorPrefs.SetBool(kIsEditorMode, false);
                    }
                    mIsEditorMode = EditorPrefs.GetBool(kIsEditorMode, true) ? 1 : 0;
                }

                return mIsEditorMode != 0;
            }
            set
            {
                int newValue = value ? 1 : 0;
                if (newValue != mIsEditorMode)
                {
                    mIsEditorMode = newValue;
                    EditorPrefs.SetBool(kIsEditorMode, value);
                    if (value)
                    {
                        IsSimulateMode = false;
                    }
                }
            }
        }
        public static bool IsSimulateMode
        {
            get
            {
                if (mIsSimulateMode == -1)
                {
                    if (!EditorPrefs.HasKey(kIsSimulateMode))
                    {
                        EditorPrefs.SetBool(kIsSimulateMode, true);
                    }
                    mIsSimulateMode = EditorPrefs.GetBool(kIsSimulateMode, true) ? 1 : 0;
                }

                return mIsSimulateMode != 0;
            }
            set
            {
                int newValue = value ? 1 : 0;
                if (newValue != mIsSimulateMode)
                {
                    mIsSimulateMode = newValue;
                    EditorPrefs.SetBool(kIsSimulateMode, value);

                    if (value)
                    {
                        IsEditorMode = false;
                    }
                }
            }
        }
#endif
    }
}


