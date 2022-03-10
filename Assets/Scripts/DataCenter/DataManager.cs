
public class DataManager : Singleton<DataManager>
{

    //消息中心
    public Messenger DataMessage = null;

    public DataManager() 
    {
        DataMessage = new Messenger();
    }

	#region Data事件处理
	public void AddListener(string event_type, EventFun event_fun)
	{
		this.DataMessage.AddListener(event_type, event_fun);
	}
	public void Broadcast(EventBase event_base)
	{
		this.DataMessage.Broadcast(event_base);
	}
	public void RemoveListener(string event_type, EventFun event_fun)
	{
		this.DataMessage.RemoveListener(event_type, event_fun);
	}
	public void RemoveListenerByType(string vent_type)
	{
		this.DataMessage.RemoveListenerByType(vent_type);
	}
	public void CleanupListener()
	{
		this.DataMessage.Cleanup();
	}
	#endregion










	public override void Dispose()
    {
        this.DataMessage = null;
    }
}
