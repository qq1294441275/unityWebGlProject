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

    public static T FindComponent<T>(GameObject trans)
    {
        T callbackType = default(T);
        GameObject targetTrans = trans;
        callbackType = targetTrans.GetComponent<T>();
        if (callbackType != null)
            return callbackType;
        callbackType = targetTrans.GetComponentInChildren<T>();
        return callbackType;
    }

    public static T FindComponent<T>(GameObject trans, string path) 
    {
        T callbackType = default(T);
        Transform targetTrans = null;
        if (path == string.Empty || path == "") 
            return callbackType;
        targetTrans = trans.transform.Find(path);
        if(targetTrans == null)
            return callbackType;
        callbackType = targetTrans.GetComponent<T>();
        if (callbackType != null)
            return callbackType;
        callbackType = targetTrans.GetComponentInChildren<T>();
        return callbackType;
    }

    public static Transform FindTrans(GameObject trans, string path) 
    {
        return trans.transform.Find(path);
    }

    public static Text FindText(GameObject trans, string path)
    {
        return UIUtil.FindComponent<Text>(trans, path);
    }
    public static Image FindImage(GameObject trans, string path)
    {
        return UIUtil.FindComponent<Image>(trans, path);
    }
    public static Button FindButton(GameObject trans, string path)
    {
        return UIUtil.FindComponent<Button>(trans, path);
    }
    public static InputField FindInput(GameObject trans, string path)
    {
        return UIUtil.FindComponent<InputField>(trans, path);
    }
    public static Slider FindSlider(GameObject trans, string path)
    {
        return UIUtil.FindComponent<Slider>(trans, path);
    }
    public static ScrollRect FindScrollRect(GameObject trans, string path)
    {
        return UIUtil.FindComponent<ScrollRect>(trans, path);
    }
    public static RawImage FindRawImage(GameObject trans, string path)
    {
        return UIUtil.FindComponent<RawImage>(trans, path);
    }
    public static Scrollbar FindScrollbar(GameObject trans, string path)
    {
        return UIUtil.FindComponent<Scrollbar>(trans, path);
    }
    public static Toggle FindToggle(GameObject trans, string path)
    {
        return UIUtil.FindComponent<Toggle>(trans, path);
    }




}
