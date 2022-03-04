using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager> 
{
	//UIRoot路径
	private string UIRootPath = "UIRoot";
	//EventSystem路径
	private string EventSystemPath = "EventSystem";
	//UICamera路径
	private string UICameraPath = "UIRoot/UICamera";
	//分辨率
	private Vector2 Resolution = Vector2.zero;
	//窗口最大可使用的相对order_in_layer
	private int MaxOrderPerWindow = 0;
	//Tip
	private UINoticeTip UINoticeTip = UINoticeTip.Instance;
	//消息中心
	private Messenger UIMessage = null;


	//当前物体
	public GameObject UIRootObj = null;
	//当前摄像机物体
	public GameObject UICameraGo = null;
	//当前摄像机
	public Camera UICamera = null;
	//当前事件监听器
	public GameObject EventSystem = null;

	public override void Init() 
	{
		//UI消息中心
		UIMessage = new Messenger();


		///初始化组件
		this.UIRootObj = GameObject.Find(UIRootPath);
		this.UICameraGo = GameObject.Find(UICameraPath);
		this.UICamera = UICameraGo.GetComponent<Camera>();
		this.Resolution = new Vector2(1280, 720);
		this.MaxOrderPerWindow = 10;
		GameObject.DontDestroyOnLoad(this.UIRootObj);
		this.EventSystem = GameObject.Find(EventSystemPath);
		GameObject.DontDestroyOnLoad(this.EventSystem);

		UILayers uILayers = new UILayers();



	}










	public override void Dispose()
	{
	}
}
