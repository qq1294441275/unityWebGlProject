using UnityEngine;

public class UIBaseView : MonoBehaviour
{
    public UILayer Holder;
    public UIBaseModel Model;
    public UIBaseCtrl Ctrl;
    public Canvas Canvas;
    public int BaseOrder = 0;

    public UIBaseView(UILayer holder, UIBaseModel model, UIBaseCtrl ctrl) 
    {
        this.Holder = holder;
        this.Model = model;
        this.Ctrl = ctrl;
        this.Canvas = null;
        this.BaseOrder = 0;
    }

    public void OnCreate() 
    {
        //this.Canvas = 
    }



    public virtual void Dispose()
    {

    }
}
