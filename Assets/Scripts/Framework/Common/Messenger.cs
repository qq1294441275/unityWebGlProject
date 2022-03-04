using System.Collections.Generic;
using UnityEngine;

public delegate void EventFun(EventBase eventBase);
public class Messenger  
{
    private Dictionary<string, List<EventFun>> m_event_list;
    public Messenger() 
    {
        m_event_list = new Dictionary<string, List<EventFun>>();
    }
    public void AddListener(string vent_type, EventFun event_fun)
    {
        List<EventFun> event_funs = null;
        bool is_has_type = m_event_list.ContainsKey(vent_type);
        if (is_has_type)
        {
            m_event_list.TryGetValue(vent_type, out event_funs);
            if (event_fun == null)
            {
                m_event_list.Remove(vent_type);
                event_funs = new List<EventFun>();
                event_funs.Add(event_fun);
                m_event_list.Add(vent_type, event_funs);
            }
            event_funs.Add(event_fun);
        }
        else
        {
            event_funs = new List<EventFun>();
            event_funs.Add(event_fun);
            m_event_list.Add(vent_type, event_funs);
        }
    }
    public void Broadcast(EventBase event_base)
    {
        List<EventFun> event_funs = null;
        string Type = event_base.GetEventType();
        bool is_has_type = m_event_list.ContainsKey(Type);
        if (is_has_type)
        {
            m_event_list.TryGetValue(Type, out event_funs);
            if (event_funs == null)
            {
                Debug.LogWarning("This EventDictionary Is Not Have This EventList!");
                return;
            }
            for (int i = 0; i < event_funs.Count; i++)
            {
                if (event_funs[i] != null)
                {
                    event_funs[i](event_base);
                }
            }
        }
        else
        {
            Debug.LogWarning("This EventDictionary Is Not Have This EventType!");
            return;
        }
    }

    public void RemoveListener(string vent_type, EventFun event_fun)
    {
        List<EventFun> event_funs = null;
        bool is_has_type = m_event_list.ContainsKey(vent_type);
        if (is_has_type)
        {
            m_event_list.TryGetValue(vent_type, out event_funs);
            if (event_funs == null)
            {
                Debug.LogWarning("This EventDictionary Is Not Have This EventList!");
                return;
            }
            for (int i = 0; i < event_funs.Count; i++)
            {
                if (event_funs[i] == event_fun)
                {
                    event_funs.Remove(event_funs[i]);
                    return;
                }
            }
            Debug.LogWarning("This EventList Is Not Have This Event!");
        }
        else
        {
            Debug.LogWarning("This EventDictionary Is Not Have This EventType!");
            return;
        }
    }

    public void RemoveListenerByType(string vent_type)
    {
        bool is_has_type = m_event_list.ContainsKey(vent_type);
        if (is_has_type)
        {
            m_event_list.Remove(vent_type);
        }
        else
        {
            Debug.LogWarning("This EventDictionary Is Not Have This EventType!");
            return;
        }
    }

    public void Cleanup() 
    {
        m_event_list = null;
    }

}
