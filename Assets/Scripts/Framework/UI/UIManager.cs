using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager> 
{
	//UIRoot路径
	public string UIRootPath = "UIRoot";
	//EventSystem路径
	public string EventSystemPath = "EventSystem";
	//UICamera路径
	public string UICameraPath = "UIRoot/UICamera";
	//当前物体
	public GameObject UIRootObj = null;
	//当前摄像机物体
	public GameObject UICameraGo = null;
	//当前摄像机
	public Camera UICamera = null;
	//当前事件监听器
	public GameObject EventSystem = null;
	//分辨率
	public Vector2 Resolution = Vector2.zero;
	//窗口最大可使用的相对order_in_layer
	public int MaxOrderPerWindow = 0;
	//Tip
	public UINoticeTip UINoticeTip = UINoticeTip.Instance;
	//消息中心
	public Messenger UIMessage = null;
	// 所有可用的层级
	public Dictionary<string, UILayer> Alllayers = null;
	// 所有窗口
	public Dictionary<string, UIWindow> AllWindow = null;
	// 保持Model
	public Dictionary<string, UIBaseModel> KeepModels = null;
	//窗口记录队列
	public List<string> WindowStack = null;
	//是否启用记录
	bool EnableRecord = true;
	//所有layer
	public UILayers UILayersInstance;

	public override void Init() 
	{
		base.Init();
		//UI消息中心
		UIMessage = new Messenger();
		//初始化所有的窗体
		AllWindow = new Dictionary<string, UIWindow>();
		//初始化所有的层级
		Alllayers = new Dictionary<string, UILayer>();
		//保持model
		KeepModels = new Dictionary<string, UIBaseModel>();

		///初始化组件
		this.UIRootObj = GameObject.Find(UIRootPath);
		this.UICameraGo = GameObject.Find(UICameraPath);
		this.UICamera = UICameraGo.GetComponent<Camera>();
		this.Resolution = new Vector2(1280, 720);
		this.MaxOrderPerWindow = 10;
		GameObject.DontDestroyOnLoad(this.UIRootObj);
		this.EventSystem = GameObject.Find(EventSystemPath);
		GameObject.DontDestroyOnLoad(this.EventSystem);

		UILayersInstance = new UILayers();
		Dictionary<LayerEnum, layer> layers = UILayersInstance.GetAllLayer;
		if (layers == null || layers.Count <= 0) 
		{
			Debug.LogError("Init UIManager failed");
			return;
		}
        foreach (var item in layers)
        {
			layer temp_layer = item.Value;
			GameObject ui_layer_obj = new GameObject(temp_layer.Name);
			ui_layer_obj.transform.SetParent(this.UIRootObj.transform, false);
			UILayer ui_layer = new UILayer();
			ui_layer.OnCreate(ui_layer_obj, temp_layer);
			Alllayers.Add(temp_layer.Name, ui_layer);
		}
	}
	//根据ui类型获取UILayer
	public UILayer GetUILayerByType(string layer_name) 
	{
		UILayer temp_layer = null;
		this.Alllayers.TryGetValue(layer_name, out temp_layer);
		return temp_layer;
	}

    #region UI事件处理
    public void AddListener(string event_type, EventFun event_fun) 
	{
		this.UIMessage.AddListener(event_type, event_fun);
	}
	public void Broadcast(EventBase event_base)
	{
		this.UIMessage.Broadcast(event_base);
	}
	public void RemoveListener(string event_type, EventFun event_fun)
	{
		this.UIMessage.RemoveListener(event_type, event_fun);
	}
	public void RemoveListenerByType(string vent_type)
	{
		this.UIMessage.RemoveListenerByType(vent_type);
	}
	public void CleanupListener()
	{
		this.UIMessage.Cleanup();
	}
	#endregion

	//得到一个窗口
	public UIWindow GetWindow(string ui_name, bool active = false, bool view_active = false) 
	{
		UIWindow ui_window = null;
		AllWindow.TryGetValue(ui_name, out ui_window);
		if (ui_window == null) 
		{
			//Debug.LogError("11111111111");
			return null;
		}
			
		if (ui_window.Active != active)
		{
			//Debug.LogError("2222222222");
			return null;
		}
			
		if (ui_window.View.ViewRoot.activeSelf != view_active) 
		{
			//Debug.LogError("333333333333");
			return null;
		}
			
		return ui_window;
	}
	//初始化窗口
	public UIWindow InitWindow(string ui_name, UIWindow window)
    {
        WindowConfig window_config = UIWindowConfig.instance.GetWindowConfig(ui_name);
		if (window_config == null) 
		{
			Debug.LogErrorFormat("The WindowConfig for {0} is Null", ui_name);
			return null;
		}
		UILayer layer = this.Alllayers[window_config.Layer.Name];
		if (layer == null)
		{
			Debug.LogErrorFormat("The layer for {0} is Null", window_config.Layer.Name);
			return null;
		}
		window.Name = ui_name;
		UIBaseModel ui_base_model;
		if (KeepModels.TryGetValue(ui_name,out ui_base_model))
			window.Model = KeepModels[ui_name];
		else
			window.Model = window_config.model;
		if (window_config.Ctrl != null)
			window.Ctrl = window_config.Ctrl;
		if (window_config.View != null)
			window.View = window_config.View;

		window.Active = false;
		window.Layer = layer;
		window.PrefabPath = window_config.PrefabPath;
		//this.Broadcast(UImes);
		this.Broadcast(new EventMessageEX<UIWindow>(UIMessageNames.UIFRAME_ON_WINDOW_CREATE, window));
		return window;
	}
	//激活窗口
	public void ActivateWindow<T>(UIWindow target, T data) 
	{
		if (target == null)
		{
			Debug.LogErrorFormat("The UIWindow for {0} is Null", target.Name);
			return;
		}
		target.Model.Activate<T>(data);
		target.View.ViewRoot.SetActive(true);
		this.Broadcast(new EventMessageEX<UIWindow>(UIMessageNames.UIFRAME_ON_WINDOW_OPEN, target));
	}
	//反激活窗口
	public void Deactivate(UIWindow target)
	{
		if (target == null)
		{
			Debug.LogErrorFormat("The UIWindow for {0} is Null", target.Name);
			return;
		}
		target.Model.Deactivate();
		target.View.ViewRoot.SetActive(false);
		this.Broadcast(new EventMessageEX<UIWindow>(UIMessageNames.UIFRAME_ON_WINDOW_CLOSE, target));
	}
	//打开窗口：私有，必要时准备资源
	private void InnerOpenWindow<T>(UIWindow target,T data, Action<bool> action) 
	{
		if (target == null)
		{
			Debug.LogErrorFormat("The UIWindow for {0} is Null", target.Name);
			return;
		}
		target.Active = true;
		bool has_view = target.View != null;
		bool has_prefab_res = target.PrefabPath != string.Empty;
		bool has_loaded = target.View.ViewRoot != null;

		bool need_load = has_view && has_prefab_res && !has_loaded;
		if (!need_load)
		{
			ActivateWindow<T>(target, data);
		}
		else if (!target.IsLoading) 
		{
			target.IsLoading = true;
			ResourcesPool.Instance.GetGameObjectAsync(target.PrefabPath, (go) =>
			 {
				 if (go == null)
				 {
					 Debug.LogError("加载窗口物体为空");
					 return;
				 }
					 
				 Transform trans = go.transform;
				 trans.SetParent(target.Layer.UILayerObj.transform);
				 trans.localPosition = Vector3.zero;
				 trans.localScale = Vector3.one;
				 trans.name = target.Name;

				 target.IsLoading = false;
				 target.View.ViewRoot = go;
				 target.View.OnCreate();
				 if (target.Active) 
				 {
					 ActivateWindow<T>(target, data);
				 }
				 if (action != null) 
				 {
					 action(true);
				 }
			 });
		} 
	}
	//关闭窗口：私有
	private void InnerCloseWindow(UIWindow target) 
	{
		if (target.Active) 
		{
			Deactivate(target);
			target.Active = false;
		}
	}

	//打开窗口：公有
	public void OpenWindow<T>(string ui_name, T data, Action<bool> action) 
	{
		UIWindow target = this.GetWindow(ui_name);
		if (target == null) 
		{
			UIWindow window = new UIWindow();
			this.AllWindow.Add(ui_name, window);
			target = InitWindow(ui_name, window);
		}
		InnerCloseWindow(target);
		InnerOpenWindow<T>(target, data, action);

		WindowConfig window_config = UIWindowConfig.instance.GetWindowConfig(ui_name);
		if (window_config.Layer.LayerEnum == LayerEnum.BackgroudLayer)
		{

		} else if (window_config.Layer.LayerEnum == LayerEnum.NormalLayer) 
		{

		}

	}
	//关闭窗口：公有
	public void CloseWindow(string ui_name)
	{
		UIWindow target = this.GetWindow(ui_name);
		if (target == null)
		{
			return;
		}
		InnerCloseWindow(target);
	}

	//获取最后添加的一个背景窗口索引
	public void GetLastBgWindowIndexInWindowStack(string ui_name)
	{
		UIWindow target = this.GetWindow(ui_name);
		if (target == null)
		{
			return;
		}
		InnerCloseWindow(target);
	}
	//关闭层级所有窗口
	public void CloseWindowByLayer(LayerEnum layer_enum, bool except_layer = false)
	{
        foreach (var temp_windows in this.AllWindow)
        {
			WindowConfig window_config = UIWindowConfig.instance.GetWindowConfig(temp_windows.Value.Name);
			if (except_layer)
			{
				if (window_config.Layer.LayerEnum != layer_enum)
				{
					InnerCloseWindow(temp_windows.Value);
				}
			}
			else 
			{
				if (window_config.Layer.LayerEnum == layer_enum)
				{
					InnerCloseWindow(temp_windows.Value);
				}
			}
		}
	}
	//关闭层级所有窗口
	public void CloseAllWindows()
	{
		foreach (var temp_windows in this.AllWindow)
		{
			InnerCloseWindow(temp_windows.Value);
		}
	}
	//展示窗口或是隐藏窗口
	public void OpenCloseView(string ui_name, bool is_close = false)
	{
		UIWindow target = this.GetWindow(ui_name);
		if (target == null)
		{
			return;
		}
		target.View.ViewRoot.SetActive(!is_close);
	}

	public void InnerDestroyWindow(bool include_keep_model, UIWindow target, string ui_name, bool is_destroy_inst = false) 
	{
		this.Broadcast(new EventMessageEX<UIWindow>(UIMessageNames.UIFRAME_ON_WINDOW_DESTROY, target));
		ResourcesPool.Instance.RecycleGameObject(target.PrefabPath, target.View.ViewRoot);
		if (is_destroy_inst) 
			ResourcesPool.Instance.DestoryGameObject(target.PrefabPath);
		if (include_keep_model)
		{
			if (KeepModels[ui_name] != null)
				KeepModels.Remove(ui_name);
			target.Model.Dispose();
		}
		else
			target.Model.Dispose();
		target.Ctrl.Dispose();
		target.View.Dispose();
		this.AllWindow.Remove(ui_name);
    }

	public void DestroyWindow(bool include_keep_model, string ui_name, bool is_destroy_inst = false) 
	{
		UIWindow target = this.GetWindow(ui_name);
		if (target == null)
		{
			return;
		}
		InnerCloseWindow(target);
		InnerDestroyWindow(include_keep_model, target, ui_name, is_destroy_inst);
	}

	public void DestroyWindowByLayer(LayerEnum layer_enum, bool include_keep_model, bool is_destroy_inst = false)
	{
		List<string> temp_layer_name = new List<string>();
		foreach (var temp_windows in this.AllWindow)
		{
			WindowConfig window_config = UIWindowConfig.instance.GetWindowConfig(temp_windows.Value.Name);
			if (window_config.Layer.LayerEnum == layer_enum) 
			{
				InnerCloseWindow(temp_windows.Value);
				temp_layer_name.Add(temp_windows.Key);
			}
		}
        for (int i = 0; i < temp_layer_name.Count; i++)
        {
			string ui_name = temp_layer_name[i];
			InnerDestroyWindow(include_keep_model, this.AllWindow[ui_name],ui_name,is_destroy_inst);
		}
	}

	public void DestroyWindowExceptLayer(LayerEnum layer_enum, bool include_keep_model, bool is_destroy_inst = false)
	{
		List<string> temp_layer_name = new List<string>();
		foreach (var temp_windows in this.AllWindow)
		{
			WindowConfig window_config = UIWindowConfig.instance.GetWindowConfig(temp_windows.Value.Name);
			if (window_config.Layer.LayerEnum != layer_enum)
			{
				InnerCloseWindow(temp_windows.Value);
				temp_layer_name.Add(temp_windows.Key);
			}
		}
		for (int i = 0; i < temp_layer_name.Count; i++)
		{
			string ui_name = temp_layer_name[i];
			InnerDestroyWindow(include_keep_model, this.AllWindow[ui_name], ui_name, is_destroy_inst);
		}
	}
	public void DestroyAllWindow(bool include_keep_model, bool is_destroy_inst = false)
	{
		List<string> temp_layer_name = new List<string>();
		foreach (var temp_windows in this.AllWindow)
		{
			InnerCloseWindow(temp_windows.Value);
			temp_layer_name.Add(temp_windows.Key);
		}
		for (int i = 0; i < temp_layer_name.Count; i++)
		{
			string ui_name = temp_layer_name[i];
			InnerDestroyWindow(include_keep_model, this.AllWindow[ui_name], ui_name, is_destroy_inst);
		}
	}

	public override void Dispose()
	{
	}
}
