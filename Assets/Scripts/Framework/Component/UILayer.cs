using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILayer  
{
    public GameObject UILayerObj;
    public Canvas UnityCanvas;
    public CanvasScaler UnityCanvasScaler;
    public GraphicRaycaster UnityGraphicRaycaster;
    public int TopWindowOrder;
    public int MinWindowOrder;

    public void OnCreate(GameObject layer_go, layer layer) 
    {
        this.UILayerObj = layer_go;

        this.UnityCanvas = UIUtil.FindComponent<Canvas>(layer_go);
        if (this.UnityCanvas == null) 
        {
            this.UnityCanvas = layer_go.AddComponent<Canvas>();
        }
        this.UnityCanvas.renderMode = RenderMode.ScreenSpaceCamera;
        this.UnityCanvas.worldCamera = UIManager.instance.UICamera;
        this.UnityCanvas.planeDistance = layer.PlaneDistance;
        this.UnityCanvas.sortingLayerName = SortingLayerNames.UI;
        this.UnityCanvas.sortingOrder = layer.OrderInLayer;

        this.UnityCanvasScaler = UIUtil.FindComponent<CanvasScaler>(layer_go);
        if (this.UnityCanvasScaler == null)
        {
            this.UnityCanvasScaler = layer_go.AddComponent<CanvasScaler>();
        }
        this.UnityCanvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        this.UnityCanvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        this.UnityCanvasScaler.referenceResolution = UIManager.instance.Resolution;


        //raycaster

        this.UnityGraphicRaycaster = UIUtil.FindComponent<GraphicRaycaster>(layer_go);

        if (this.UnityGraphicRaycaster == null) 
        {
            this.UnityGraphicRaycaster = layer_go.AddComponent<GraphicRaycaster>();
        }

        //window order
        this.TopWindowOrder = layer.OrderInLayer;
        this.MinWindowOrder = layer.OrderInLayer;
    }

    public int PopWindowOrder() 
    {
        int cur = this.TopWindowOrder;
        this.TopWindowOrder = this.TopWindowOrder + UIManager.instance.MaxOrderPerWindow;
        return cur;
    }

    public void PushWindowOrder()
    {
        if (this.TopWindowOrder < this.MinWindowOrder) 
        {
            Debug.LogError("this TopWindowOrder letter MinWindowOrder");
        }
        this.TopWindowOrder = this.TopWindowOrder - UIManager.instance.MaxOrderPerWindow;
    }

    public void Destroy() 
    {
        this.UnityCanvas = null;
        this.UnityCanvasScaler = null;
        this.UnityGraphicRaycaster = null;
    }
}
