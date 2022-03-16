using System.Collections.Generic;
using UnityEngine;
using System;
public class SceneConfig 
{
    private Dictionary<SceneEnum, SceneInstance> SceneConfigList;
    public SceneConfig() 
    {
        SceneConfigList = new Dictionary<SceneEnum, SceneInstance>();
        this.Init();
    }
    private void Init() 
    {
        this.AddSceneInstance(new SceneInstance(SceneEnum.LaunchScene, 0, "LaunchScene"));
        this.AddSceneInstance(new SceneInstance(SceneEnum.LoadingScene, 1, "LoadingScene"));
        this.AddSceneInstance(new SceneInstance(SceneEnum.LoginScene, 2, "LoginScene"));
        this.AddSceneInstance(new SceneInstance(SceneEnum.HomeScene, 3, "HomeScene"));
        this.AddSceneInstance(new SceneInstance(SceneEnum.BattleScene, 4, "BattleScene"));
    }

    private void AddSceneInstance(SceneInstance scene_inst) 
    {
        SceneInstance temp_scene_inst = null;
        if (this.SceneConfigList.TryGetValue(scene_inst.SceneType, out temp_scene_inst))
        {
            Debug.LogErrorFormat("this {0} scene is added, not need add!");
        }
        this.SceneConfigList.Add(scene_inst.SceneType, scene_inst);
    }
    public SceneInstance GetSceneInstanceByType(SceneEnum scene_enum) 
    {
        SceneInstance temp_scene_instance;
        SceneConfigList.TryGetValue(scene_enum, out temp_scene_instance);
        return temp_scene_instance;
    }
}

public enum SceneEnum 
{
    LaunchScene = 0,
    LoadingScene = 1,
    LoginScene = 2,
    HomeScene = 3,
    BattleScene = 4,
}

public class SceneInstance 
{
    public SceneEnum SceneType;
    public int Level;
    public string Name = string.Empty;

    public SceneInstance(SceneEnum scene_type, int level, string name) 
    {
        this.SceneType = scene_type;
        this.Level = level;
        this.Name = name;
    }
}

public class ScenePreLoad
{
    public string Path;
    public int InstCount;

    public ScenePreLoad(string path, int count)
    {
        this.Path = path;
        this.InstCount = count;
    }
    public string GetPreLoadPath()
    {
        return this.Path;
    }
}

//场景基类，各场景类从这里继承：提供统一的场景加载和初始化步骤，负责资源预加载
public class SceneBase  
{
    public SceneInstance SceneInstance;
    ///目前只添加GameObject 类型的
    public List<ScenePreLoad> PreloadPrefab;


    private int PreLoadCount = 0;
    private int PreTotalCount = 0;

    //构造函数，初始化放OnInit
    public SceneBase(SceneInstance scene_inst) 
    {
        this.SceneInstance = scene_inst;
        this.PreloadPrefab = new List<ScenePreLoad>();
        this.OnCreate();
    }
    //析构函数，资源释放放OnDispose
    public virtual void Dispose() 
    {
        this.OnDestroy();
    }
    //加载前的初始化
    public virtual void OnCreate()
    {
    }
    // 添加预加载资源
    // 注意：只有prefab类型才需要填inst_count，用于指定初始实例化个数
    public void AddPreladResource<T>(string path, int inst_count) 
    {
        ScenePreLoad scene_preload = new ScenePreLoad(path, inst_count);
        if (typeof(T) == typeof(GameObject))
            this.PreloadPrefab.Add(scene_preload);
        else
            Debug.Log("目前只支持 GameObject 预加载，添加其他预加载请拓展");
    }
    //加载前的初始化
    public virtual void OnEnter()
    {
    }

    //-- 场景加载结束：后续资源准备（预加载等）
    //-- 注意：这里使用协程，需要加载的资源添加到列表就可以了
    public void CoOnPrepare(Action<bool,float> action)
    {
        this.PreTotalCount = this.PreloadPrefab.Count;
        if (this.PreTotalCount <= 0) 
        {
            Debug.LogFormat("this scene {0} preload count is  0", this.SceneInstance.Name);
            if (action != null)
            {
                action.Invoke(true, 1);
            }
            return;
        }
        this.PreLoadCount = 0;
        ScenePreLoad scene_preload = null;
        for (int i = 0; i < PreloadPrefab.Count; i++)
        {
            scene_preload = PreloadPrefab[i];
            if (scene_preload != null)
            {
                ResourcesPool.Instance.PreLoadGameObjectAsync(scene_preload.Path, () =>
                {
                    this.PreLoadCount = this.PreLoadCount++;
                    bool is_finish = this.IsPreLoadFinish();
                    float progress = (float)(this.PreLoadCount / this.PreTotalCount);
                    if (action != null) 
                    {
                        action.Invoke(is_finish, progress);
                    }
                });
            }
        }


    }
    private bool IsPreLoadFinish() 
    {
        return this.PreLoadCount == this.PreTotalCount;
    }
    public virtual void OnComplete() 
    {

    }
    public virtual void OnLeave()
    {

    }
    public virtual void OnDestroy() 
    {
        this.SceneInstance = null;
        this.PreloadPrefab.Clear();
        this.PreloadPrefab = null;
    }
}
