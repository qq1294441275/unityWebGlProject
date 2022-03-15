using AssetBundles;
using GameChannel;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLaunch : MonoBehaviour 
{
    [Header("此版本从热更架构修改过来的，现在是网络版本，不需要走热更，直接加载网络上AB资源就行")]
    const string launchPrefabPath = "Common/UI/Prefabs/View/UILaunch.prefab";
    const string noticeTipPrefabPath = "Common/UI/Prefabs/Common/UINoticeTip.prefab";
    GameObject launchPrefab;
    GameObject noticeTipPrefab;

    IEnumerator Start()
    {
        // 初始化App版本
        var start = DateTime.Now;
        yield return InitAppVersion();
        Debug.Log(string.Format("InitAppVersion use {0}ms", (DateTime.Now - start).Milliseconds));

        // 初始化渠道
        start = DateTime.Now;
        yield return InitChannel();
        Debug.Log(string.Format("InitChannel use {0}ms", (DateTime.Now - start).Milliseconds));

        // 启动资源管理模块
        start = DateTime.Now;
        yield return AssetBundleManager.Instance.Initialize();
        Debug.Log(string.Format("AssetBundleManager Initialize use {0}ms", (DateTime.Now - start).Milliseconds));

        // 初始化UI界面
        //yield return InitLaunchPrefab();
        //yield return null;
        //yield return InitNoticeTipPrefab();


        //开始游戏
        UIManager.instance.Startup();
        UIWindowConfig.instance.Startup();
        GameObject.Destroy(this.gameObject);
    }
    ///初始话网页版的版本
    IEnumerator InitAppVersion()
    {
        var appVersionRequest = AssetBundleManager.Instance.RequestAssetFileAsync(BuildUtils.AppVersionFileName, false);
        yield return appVersionRequest;
        var streamingAppVersion = appVersionRequest.text;
        appVersionRequest.Dispose();

        ChannelManager.instance.appVersion = streamingAppVersion;
        yield break;
    }
    IEnumerator InitChannel()
    {
#if UNITY_EDITOR
        if (AssetBundleConfig.IsEditorMode)
        {
            yield break;
        }
#endif
        var channelNameRequest = AssetBundleManager.Instance.RequestAssetFileAsync(BuildUtils.ChannelNameFileName, false);
        yield return channelNameRequest;
        var channelName = channelNameRequest.text;
        channelNameRequest.Dispose();
        ChannelManager.instance.Init(channelName);
        Debug.Log(string.Format("channelName = {0}", channelName));
        yield break;
    }


    IEnumerator InitLaunchPrefab()
    {
        var start = DateTime.Now;
        var loader = AssetBundleManager.Instance.LoadAssetAsync(launchPrefabPath, typeof(GameObject));
        yield return loader;
        launchPrefab = loader.asset as GameObject;
        Debug.Log(string.Format("Load launchPrefab use {0}ms", (DateTime.Now - start).Milliseconds));
        loader.Dispose();
        if (launchPrefab == null)
        {
            Debug.LogError("LoadAssetAsync launchPrefab err : " + launchPrefabPath);
            yield break;
        }
        InstantiateGameObject(launchPrefab);
        yield break;
    }

    GameObject InstantiateGameObject(GameObject prefab)
    {
        var start = DateTime.Now;
        GameObject go = GameObject.Instantiate(prefab);
        Debug.Log(string.Format("Instantiate use {0}ms", (DateTime.Now - start).Milliseconds));

        var luanchLayer = GameObject.Find("UIRoot/LuanchLayer");
        go.transform.SetParent(luanchLayer.transform);
        var rectTransform = go.GetComponent<RectTransform>();
        rectTransform.offsetMax = Vector2.zero;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.localScale = Vector3.one;
        rectTransform.localPosition = Vector3.zero;

        return go;
    }
    IEnumerator InitNoticeTipPrefab()
    {
        var start = DateTime.Now;
        var loader = AssetBundleManager.Instance.LoadAssetAsync(noticeTipPrefabPath, typeof(GameObject));
        yield return loader;
        noticeTipPrefab = loader.asset as GameObject;
        Debug.Log(string.Format("Load noticeTipPrefab use {0}ms", (DateTime.Now - start).Milliseconds));
        loader.Dispose();
        if (noticeTipPrefab == null)
        {
            Debug.LogError("LoadAssetAsync noticeTipPrefab err : " + noticeTipPrefabPath);
            yield break;
        }
        var go = InstantiateGameObject(noticeTipPrefab);
        UINoticeTip.Instance.UIGameObject = go;
        yield break;
    }
}
