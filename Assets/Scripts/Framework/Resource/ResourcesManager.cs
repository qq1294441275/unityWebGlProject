using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssetBundles;
using System;

public class ResourcesManager : MonoSingleton<ResourcesManager>
{
    //是否有加载任务正在进行
    public bool IsProsessRunning() 
    {
        return AssetBundleManager.Instance.IsProsessRunning;
    }

    // 设置常驻包
    // 注意：
    // 1、公共包（被2个或者2个其它AB包所依赖的包）底层会自动设置常驻包
    // 2、任何情况下不想被卸载的非公共包（如Lua脚本）需要手动设置常驻包
    public void SetAssetBundleResident(string path, bool resident) 
    {
        string assetbundleName = AssetBundleUtility.AssetBundlePathToAssetBundleName(path);
        AssetBundleManager.Instance.SetAssetBundleResident(assetbundleName, resident);
    }
    //异步加载AssetBundle：回调形式
    public void LoadAssetBundleAsync(string path, Action callback) 
    {
        StartCoroutine(CoLoadAssetBundleAsync(path, callback));
    }
    //异步加载AssetBundle：协程形式
    public IEnumerator CoLoadAssetBundleAsync(string path, Action callback) 
    {
        string assetbundleName = AssetBundleUtility.AssetBundlePathToAssetBundleName(path);
        var loader = AssetBundleManager.Instance.LoadAssetBundleAsync(assetbundleName);
        yield return loader;
        loader.Dispose();
        if (callback != null)
            callback();
    }
    //异步加载Asset：回调形式
    public void LoadAsync<T>(string path,Action<UnityEngine.Object> callback)
    {
        StartCoroutine(CoLoadAsync<T>(path, callback));
    }
    //异步加载Asset：协程形式
    public IEnumerator CoLoadAsync<T>(string path, Action<UnityEngine.Object> callback)
    {
        var loader = AssetBundleManager.Instance.LoadAssetAsync(path, typeof(T));
        yield return loader;
        var asset = loader.asset;
        loader.Dispose();
        if (asset == null) 
            Debug.LogErrorFormat("Asset load err : {0}",path);
        if (callback != null)
            callback.Invoke(asset);
    }

    public void Cleanup() 
    {
        AssetBundleManager.Instance.ClearAssetsCache();
        AssetBundleManager.Instance.UnloadAllUnusedResidentAssetBundles();
    }

    public override void Dispose()
    {
        
    }
}
