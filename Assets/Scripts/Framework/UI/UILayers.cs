using System.Collections.Generic;

public enum LayerEnum 
{
	SceneLayer = 0,
	BackgroudLayer = 1,
	NormalLayer = 2,
	InfoLayer = 3,
	TipLayer = 4,
	TopLayer = 5,
}


public class layer 
{
	public LayerEnum LayerEnum;
	public string Name = string.Empty;
	public int PlaneDistance = 0;
	public int OrderInLayer = 0;

	public layer(LayerEnum layer_enum, string name, int plane_distance, int order_layer) 
	{
		this.LayerEnum = layer_enum;
		this.Name = name;
		this.PlaneDistance = plane_distance;
		this.OrderInLayer = order_layer;
	}
}
public class SortingLayerNames
{
	public static string Default = "Default";
	public static string Map = "Map";
	public static string Scene = "Scene";
	public static string Charactor = "Charactor";
	public static string UI = "UI";
}


public class UILayers  
{
	private Dictionary<LayerEnum, layer> AllLayers = null;
	public UILayers() 
	{
		AllLayers = new Dictionary<LayerEnum, layer>();
		this.Init();
	}

	public void Init() 
	{
		AllLayers.Add(LayerEnum.SceneLayer, new layer(LayerEnum.SceneLayer,"SceneLayer", 1000, 0));
		AllLayers.Add(LayerEnum.BackgroudLayer, new layer(LayerEnum.BackgroudLayer, "BackgroudLayer", 900, 1000));
		AllLayers.Add(LayerEnum.NormalLayer, new layer(LayerEnum.NormalLayer, "NormalLayer", 800, 2000));
		AllLayers.Add(LayerEnum.InfoLayer, new layer(LayerEnum.InfoLayer, "InfoLayer", 700, 3000));
		AllLayers.Add(LayerEnum.TipLayer, new layer(LayerEnum.TipLayer, "TipLayer", 600, 4000));
		AllLayers.Add(LayerEnum.TopLayer, new layer(LayerEnum.TopLayer, "TopLayer", 500, 5000));
	}
	public Dictionary<LayerEnum, layer> GetAllLayer 
	{
		get { return this.AllLayers; }
	}
	public layer GetLayerByType(LayerEnum layer_enum) 
	{
		layer temp_layer = null;
		this.AllLayers.TryGetValue(layer_enum, out temp_layer);
		return temp_layer;
	}

	public void Dispose() 
	{
		this.AllLayers.Clear();
		this.AllLayers = null;
	}
}

