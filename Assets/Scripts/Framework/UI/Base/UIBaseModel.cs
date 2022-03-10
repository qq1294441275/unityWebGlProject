using System.Collections.Generic;

public class UIBaseModel  
{
    public string UIName;

    Dictionary<string, EventFun> UICallBack;
    Dictionary<string, EventFun> DataCallBack;

    public void Init(string ui_name) 
    {
        this.UIName = ui_name;
        UICallBack = new Dictionary<string, EventFun>();
        DataCallBack = new Dictionary<string, EventFun>();
        this.OnCreate();
    }
    public virtual void OnCreate() 
    {

    }

    public virtual void OnEnable<T>(T data)
    {

    }
    public virtual void OnDisable()
    {

    }
    public virtual void OnDestroy()
    {

    }
    public virtual void OnAddListener()
    {

    }
    public virtual void OnRemoveListener()
    {

    }

    public void Activate<T>(T data) 
    {
        this.OnAddListener();
        this.OnEnable<T>(data);
    }
    public void Deactivate()
    {
        this.OnRemoveListener();
        this.OnDisable();
    }
    public void AddCallBack(Dictionary<string, EventFun> keeper,string msg_name, EventFun EventFun) 
    {
        keeper.Add(msg_name, EventFun);
    }
    public EventFun GetCallback(Dictionary<string, EventFun> keeper, string msg_name)
    {
        EventFun event_fun = null;
        keeper.TryGetValue(msg_name, out event_fun);
        return event_fun;
    }
    public void RemoveCallback(Dictionary<string, EventFun> keeper, string msg_name)
    {
        EventFun event_fun = null;
        keeper.TryGetValue(msg_name, out event_fun);
        if (event_fun != null) 
        {
            keeper.Remove(msg_name);
        }
    }


    public void AddUIListener(string event_type, EventFun event_fun)
    {
        this.AddCallBack(this.UICallBack, event_type, event_fun);
        UIManager.instance.AddListener(event_type, event_fun);
    }
    public void UIBroadcast(EventBase event_base)
    {
        UIManager.instance.Broadcast(event_base);
    }
    public void RemoveUIListener(string event_type, EventFun event_fun)
    {
        this.RemoveCallback(this.UICallBack, event_type);
        UIManager.instance.RemoveListener(event_type, event_fun);
    }

    public void AddDataListener(string event_type, EventFun event_fun)
    {
        this.AddCallBack(this.DataCallBack, event_type, event_fun);
        DataManager.instance.AddListener(event_type, event_fun);
    }
    public void RemoveDatListener(string event_type, EventFun event_fun)
    {
        this.RemoveCallback(this.DataCallBack, event_type);
        DataManager.instance.RemoveListener(event_type, event_fun);
    }

    public virtual void Dispose()
    {
        this.OnDestroy();
        foreach (KeyValuePair<string, EventFun> ui_call_back in UICallBack)
        {
            this.RemoveUIListener(ui_call_back.Key, ui_call_back.Value);
        }
        foreach (KeyValuePair<string, EventFun> data_call_back in DataCallBack)
        {
            this.RemoveUIListener(data_call_back.Key, data_call_back.Value);
        }
    }

}
