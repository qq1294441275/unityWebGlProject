﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProjSceneManager : MonoSingleton<ProjSceneManager>
{
    //场景配置
    public SceneConfig SceneConfig;
    //当前场景
    public SceneBase CurrentScene = null;
    //是否繁忙
    public bool IsBusing = false;

    public Dictionary<SceneEnum,SceneBase> Scenes = null;



    protected override void Init()
    {
        base.Init();
        SceneConfig = new SceneConfig();
        Scenes = new Dictionary<SceneEnum, SceneBase>();
        this.AddScene();
    }

    private void AddScene() 
    {
        //this.Scenes.Add(SceneEnum.LoadingScene,)
    }


    public void SwitchScene<T>(SceneEnum scene_enum) 
    {
        if (this.IsBusing)  
            return;
        if (this.CurrentScene != null && this.CurrentScene.SceneInstance.SceneType == scene_enum)
            return;
        this.IsBusing = true;
        StartCoroutine(CoInnerSwitchScene<T>(scene_enum));
    }

    IEnumerator CoInnerSwitchScene<T>(SceneEnum scene_enum) 
    {
        //打开loading界面
        int number = 0;
        string ui_loading = UIWindowType.UILoading.ToString();
        UIManager.instance.OpenWindow<int>(ui_loading, number);
        UIWindow window = UIManager.instance.GetWindow(ui_loading);
        UILoadingModel model = window.Model as UILoadingModel;
        model.value = 0;
        yield return 0;
        //等待资源管理器加载任务结束，否则很多Unity版本在切场景时会有异常
        yield return new WaitUntil(()=> ResourcesManager.Instance.IsProsessRunning());
        //清理旧场景
        if (this.CurrentScene != null)
            this.CurrentScene.OnLeave();
        model.value += 0.01f;
        yield return 0;
        //清理UI
        UIManager.instance.DestroyWindowExceptLayer(LayerEnum.TopLayer, false);
        model.value += 0.01f;
        yield return 0;
        //清理资源缓存
        ResourcesPool.Instance.CleanUp(true);
        model.value += 0.01f;
        yield return 0;
        ResourcesManager.Instance.Cleanup();
        model.value += 0.01f;
        yield return 0;
        //同步加载loading场景
        SceneInstance temp_scene_instance = SceneConfig.GetSceneInstanceByType(SceneEnum.LoadingScene);
        SceneManager.LoadScene(temp_scene_instance.Level);
        model.value += 0.01f;
        yield return 0;
        //GC：清理两次
        GC.Collect();
        GC.Collect();
        float cur_progress = model.value;
        AsyncOperation async_operation = Resources.UnloadUnusedAssets();
        yield return new WaitUntil(() => { return async_operation.isDone; });
        model.value += 0.2f;
        yield return 0;
        //初始化目标场景
        var logic_scene = this.Scenes[scene_enum];
        logic_scene.OnEnter();
        model.value += 0.02f;
        yield return 0;
        temp_scene_instance = SceneConfig.GetSceneInstanceByType(scene_enum);
        async_operation = SceneManager.LoadSceneAsync(temp_scene_instance.Level);
        yield return new WaitUntil(() => { return async_operation.isDone; });
        model.value += 0.15f;
        yield return 0;
        //准备工作：预加载资源等
        //说明：现在的做法是不热更场景（都是空场景），所以主要的加载时间会放在场景资源的prefab上，这里给65%的进度时间
        bool pre_load = false;
        logic_scene.CoOnPrepare((is_loaded,progress) => {
            pre_load = is_loaded;
            model.value += 0.65f * progress;
        });
        yield return new WaitUntil(() => { return pre_load; });
        yield return 0;
        logic_scene.OnComplete();
        model.value = 1;
        yield return 3;
        //加载完成，关闭loading界面
        UIManager.instance.DestroyWindow(false, UIWindowType.UILoading.ToString());
        this.CurrentScene = logic_scene;
        this.IsBusing = false;
    }

    public override void Dispose()
    {
        foreach (var scene in this.Scenes)
        {
            scene.Value.Dispose();
        }
        this.Scenes.Clear();
    }
}
