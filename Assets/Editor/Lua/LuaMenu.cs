using UnityEngine;
using UnityEditor;
using System.IO;
using Debug = UnityEngine.Debug;
using AssetBundles;
public static class XLuaMenu
{
    const string luaAssetbundleAssetName = "Lua";
    const string luaScriptsFolder = "LuaScripts";

    [MenuItem("Lua工具/拷贝luaScripts到打包目录", false, 51)]
    public static void CopyLuaFilesToAssetsPackage()
    {
        string destination = Path.Combine(Application.dataPath, AssetBundleConfig.AssetsFolderName);
        destination = Path.Combine(destination, luaAssetbundleAssetName);
        string source = Path.Combine(Application.dataPath, luaScriptsFolder);
        GameUtility.SafeDeleteDir(destination);

        FileUtil.CopyFileOrDirectoryFollowSymlinks(source, destination);

        var notLuaFiles = GameUtility.GetSpecifyFilesInFolder(destination, new string[] { ".lua" }, true);
        if (notLuaFiles != null && notLuaFiles.Length > 0)
        {
            for (int i = 0; i < notLuaFiles.Length; i++)
            {
                GameUtility.SafeDeleteFile(notLuaFiles[i]);
            }
        }

        var luaFiles = GameUtility.GetSpecifyFilesInFolder(destination, new string[] { ".lua" }, false);
        if (luaFiles != null && luaFiles.Length > 0)
        {
            for (int i = 0; i < luaFiles.Length; i++)
            {
                GameUtility.SafeRenameFile(luaFiles[i], luaFiles[i] + ".bytes");
            }
        }

        AssetDatabase.Refresh();
        Debug.Log("拷贝Lua文件成功！");
    }
}
