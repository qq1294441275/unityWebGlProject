using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowConfig
{
    public UIWindowType WindowType;
    public string Name;
    public layer Layer;
    public UIBaseModel model;
    public UIBaseCtrl Ctrl;
    public UIBaseView View;
    public string PrefabPath;
}
public enum UIWindowType
{
    //-- 登陆模块
    UILogin = 0,
    UILoginServer = 1,
	//--场景加载模块
    UILoading = 2,
	//--Tip窗口
    UINoticeTip = 3,
	//--TestMain
    UITestMain = 4,
	//--BattleMain
    UIBattleMain = 5,
}

public class UIWindowConfig : Singleton<UIWindowConfig>
{
    public Dictionary<string, WindowConfig> UIWindowConfigList;


    public override void Init()
    {
        base.Init();
        UIWindowConfigList = new Dictionary<string, WindowConfig>();
        this.InitList();
    }

    public void InitList() 
    {

    }

    public WindowConfig GetWindowConfig(string ui_name) 
    {
        WindowConfig temp_ui = null;
        UIWindowConfigList.TryGetValue(ui_name, out temp_ui);
        return temp_ui;
    }

    public override void Dispose()
    {
    }
}
