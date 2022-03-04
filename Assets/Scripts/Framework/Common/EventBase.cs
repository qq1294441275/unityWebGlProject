
public class EventBase
{
    private string m_event_type = string.Empty;

    public EventBase(string event_type) 
    {
        this.m_event_type = event_type;
    }
    public string GetEventType()
    {
        return this.m_event_type;
    }
}

public class EventMessageEX<T> : EventBase
{
    private T m_data;

    public EventMessageEX(string event_type, T data) : base(event_type)
    {
        m_data = data;
    }

    public T GetData()
    {
        return m_data;
    }
}
public class EventMessageEX<T, U> : EventBase
{
    private T m_data_one;
    private U m_data_two;

    public EventMessageEX(string event_type, T data_one, U data_two) : base(event_type)
    {
        m_data_one = data_one;
        m_data_two = data_two;
    }
    public T GetData()
    {
        return m_data_one;
    }

    public T GetDataOne()
    {
        return m_data_one;
    }
    public U GetDataTwo()
    {
        return m_data_two;
    }
}
public class EventMessageEX<T, U, V> : EventBase
{
    private T m_data_one;
    private U m_data_two;
    private V m_data_three;

    public EventMessageEX(string event_type, T data_one, U data_two, V data_three) : base(event_type)
    {
        m_data_one = data_one;
        m_data_two = data_two;
        m_data_three = data_three;
    }
    public T GetData()
    {
        return m_data_one;
    }

    public T GetDataOne()
    {
        return m_data_one;
    }
    public U GetDataTwo()
    {
        return m_data_two;
    }
    public V GetDataThree()
    {
        return m_data_three;
    }
}
public class EventMessageEX<T, U, V, X> : EventBase
{
    private T m_data_one;
    private U m_data_two;
    private V m_data_three;
    private X m_data_four;

    public EventMessageEX(string event_type, T data_one, U data_two, V data_three, X data_four) : base(event_type)
    {
        m_data_one = data_one;
        m_data_two = data_two;
        m_data_three = data_three;
        m_data_four = data_four;
    }
    public T GetData()
    {
        return m_data_one;
    }

    public T GetDataOne()
    {
        return m_data_one;
    }
    public U GetDataTwo()
    {
        return m_data_two;
    }
    public V GetDataThree()
    {
        return m_data_three;
    }
    public X GetDataFour()
    {
        return m_data_four;
    }
}
