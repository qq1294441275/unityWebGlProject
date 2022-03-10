using UnityEngine;
using System.Collections.Generic;
using System;

public class GameObjectPool : Singleton<GameObjectPool> 
{
    public Transform CacheTrnasRoot = null;
    private Dictionary<string, GameObject> GoPool = null;
    private Dictionary<string, GameObject> InstCache = null;



    public override void Init()
    {

        GameObject GameObjectCacheRoot = GameObject.Find("GameObjectCacheRoot");
        if (GameObjectCacheRoot == null) 
        {
            GameObjectCacheRoot = new GameObject("GameObjectCacheRoot");
            GameObject.DontDestroyOnLoad(GameObjectCacheRoot);
        }
        this.CacheTrnasRoot = GameObjectCacheRoot.transform;
        InstCache = new Dictionary<string, GameObject>();
        GoPool = new Dictionary<string, GameObject>();
    }

    public void InitInst(GameObject Inst) 
    {
        if (Inst != null) 
        {
            Inst.SetActive(true);
        }
    }

    public bool CheckHasCached(string path) 
    {
        if (path == string.Empty || path == "") 
        {
            Debug.LogErrorFormat("The CheckHasCached for {0} is Empty", path);
            return false;
        }
        if (InstCache[path] != null) 
        {
            return true;
        }
        if (GoPool[path] != null)
        {
            return true;
        }
        Debug.LogErrorFormat("The CheckHasCached obj for {0} is Empty", path);
        return false;
    }

    public void CacheAndInstGameObject(string path, GameObject go)
    {
        if (go == null)
        {
            Debug.LogErrorFormat("The CacheGo for {0} is Empty", path);
            return;
        }
        GoPool.Add(path, go);
        GameObject inst = GameObject.Instantiate(go);
        inst.transform.SetParent(this.CacheTrnasRoot);
        inst.SetActive(false);
        InstCache.Add(path, inst);
    }


    public GameObject TryGetFromCache(string path)
    {
        GameObject inst = null;
        if (this.CheckHasCached(path))
        {
            Debug.LogErrorFormat("The CacheGo for {0} is Empty", path);
            return inst;
        }
        inst = InstCache[path];
        if (inst != null) 
        {
            InstCache.Remove(path);
            return inst;
        }
        GameObject pooled_go = GoPool[path];
        if (pooled_go != null)
        {
            inst = GameObject.Instantiate(pooled_go);
            return inst;
        }
        return inst;
    }
    //预加载：可提供初始实例化
    public void PreLoadGameObjectAsync(string path, Action callback) 
    {
        if (this.CheckHasCached(path)) 
        {
            if (callback != null)
                callback();
            return;
        }
        ResourcesManager.Instance.LoadAsync<GameObject>(path, (go) =>
        {
            if (go != null)
                this.CacheAndInstGameObject(path, go as GameObject);
            if (callback != null)
                callback();
        });
    }
    //异步获取
    public void GetGameObjectAsync(string path, Action<GameObject> callback) 
    {
        var inst = this.TryGetFromCache(path);
        if (inst != null) 
        {
            InitInst(inst);
            callback(inst);
        }
        this.PreLoadGameObjectAsync(path, () =>
        {
            inst = this.TryGetFromCache(path);
            InitInst(inst);
            callback(inst);
        });
    }
    //回收
    public void RecycleGameObject(string path, GameObject inst) 
    {
        inst.transform.SetParent(CacheTrnasRoot);
        inst.SetActive(false);
        GameObject temp_inst;
        if (!InstCache.TryGetValue(path, out temp_inst)) 
            InstCache.Add(path, inst);
    }

    public void CleanUp(bool include_pooled_go) 
    {
        foreach (var inst in InstCache)
        {
            if (inst.Value != null)
                GameObject.Destroy(inst.Value);
        }
        InstCache.Clear();
        if (include_pooled_go)
            GoPool.Clear();
    }


    public override void Dispose()
    {

    }
}
