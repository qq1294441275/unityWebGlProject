using System.Collections.Generic;

public class UIBaseModel  
{
    public string m_ui_name;

    public void Init(string ui_name) 
    {
        this.m_ui_name = ui_name;
        this.OnCreate();
    }
    public virtual void OnCreate() 
    {

    }

    public virtual void OnEnable()
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

    public void Activate() 
    {
        this.OnAddListener();
        this.OnEnable();
    }
    public void Deactivate()
    {
        this.OnRemoveListener();
        this.OnDisable();
    }
}
