using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUtil  
{
    public static Transform GetChild(Transform trans, int index) 
    {
        return trans.GetChild(index);
    }

    public static T FindComponent<T>(Transform trans)
    {
        T callbackType = default(T);
        Transform targetTrans = null;
        callbackType = targetTrans.GetComponent<T>();
        if (callbackType != null)
            return callbackType;
        callbackType = targetTrans.GetComponentInChildren<T>();
        return callbackType;
    }

    public static T FindComponent<T>(Transform trans, string path) 
    {
        T callbackType = default(T);
        Transform targetTrans = null;
        if (path == string.Empty || path == "") 
            return callbackType;
        targetTrans = trans.Find(path);
        if(targetTrans == null)
            return callbackType;
        callbackType = targetTrans.GetComponent<T>();
        if (callbackType != null)
            return callbackType;
        callbackType = targetTrans.GetComponentInChildren<T>();
        return callbackType;
    }

    public static Transform FindTrans(Transform trans, string path) 
    {
        return trans.Find(path);
    }

    public static Text FindText(Transform trans, string path)
    {
        return UIUtil.FindComponent<Text>(trans, path);
    }
    public static Image FindImage(Transform trans, string path)
    {
        return UIUtil.FindComponent<Image>(trans, path);
    }
    public static Button FindButton(Transform trans, string path)
    {
        return UIUtil.FindComponent<Button>(trans, path);
    }
    public static InputField FindInput(Transform trans, string path)
    {
        return UIUtil.FindComponent<InputField>(trans, path);
    }
    public static Slider FindSlider(Transform trans, string path)
    {
        return UIUtil.FindComponent<Slider>(trans, path);
    }
    public static ScrollRect FindScrollRect(Transform trans, string path)
    {
        return UIUtil.FindComponent<ScrollRect>(trans, path);
    }
    public static RawImage FindRawImage(Transform trans, string path)
    {
        return UIUtil.FindComponent<RawImage>(trans, path);
    }
    public static Scrollbar FindScrollbar(Transform trans, string path)
    {
        return UIUtil.FindComponent<Scrollbar>(trans, path);
    }
    public static Toggle FindToggle(Transform trans, string path)
    {
        return UIUtil.FindComponent<Toggle>(trans, path);
    }




}
