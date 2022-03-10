
public class UIBaseCtrl
{
    public UIBaseModel model;
    public UIBaseCtrl(UIBaseModel model)
    {
        this.model = model;
    }
    public virtual void Dispose()
    {
        this.model = null;
    }
}
