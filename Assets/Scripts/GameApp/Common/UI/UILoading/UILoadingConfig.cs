using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILoadingConfig : Singleton<UILoadingConfig>
{
    public void InitConfig(Dictionary<string, WindowConfig> ui_loading_config_list)
    {
        WindowConfig ui_loading_config = new WindowConfig();
        ui_loading_config.Ctrl = null;
        ui_loading_config.Layer = UIManager.instance.UILayersInstance.GetLayerByType(LayerEnum.TopLayer);
        ui_loading_config.model = new UILoadingModel();
        UILayer ui_layer = UIManager.instance.GetUILayerByType(ui_loading_config.Layer.Name);
        ui_loading_config.View = new UILoading(ui_layer, ui_loading_config.model, ui_loading_config.Ctrl);
        ui_loading_config.Name = UIWindowType.UILoading.ToString();
        ui_loading_config.PrefabPath = "Common/UI/Prefabs/View/UILoading.prefab";
        ui_loading_config_list.Add(ui_loading_config.Name, ui_loading_config);
    }

    public override void Dispose()
    {
        
    }
}
