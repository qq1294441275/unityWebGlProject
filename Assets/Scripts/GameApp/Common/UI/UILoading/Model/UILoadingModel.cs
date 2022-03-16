using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILoadingModel : UIBaseModel
{
    public float value = 0.0f;
    public override void OnEnable<T>(T data)
    {
        base.OnEnable<T>(data);
        this.value = 0.0f;
    }
    public override void OnDisable()
    {
        base.OnDisable();
        this.value = 0.0f;
    }
    public void SetValue(float progress) 
    {
        this.value += progress;
        if (this.value >= 1)
            this.value = 1;
    }
}
