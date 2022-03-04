using System.Collections.Generic;

public class layer 
{
	public string Name = string.Empty;
	public int PlaneDistance = 0;
	public int OrderInLayer = 0;

	public layer(string name, int plane_distance, int order_layer) 
	{
		this.Name = name;
		this.PlaneDistance = plane_distance;
		this.OrderInLayer = order_layer;
	}
}

public class UILayers  
{
	private List<layer> AllLayers = null;
	public UILayers() 
	{
		AllLayers = new List<layer>();
		this.Init();
	}

	public void Init() 
	{
		AllLayers.Add(new layer("SceneLayer", 1000, 0));
		AllLayers.Add(new layer("BackgroudLayer", 900, 1000));
		AllLayers.Add(new layer("NormalLayer", 800, 2000));
		AllLayers.Add(new layer("InfoLayer", 700, 3000));
		AllLayers.Add(new layer("TipLayer", 600, 4000));
		AllLayers.Add(new layer("TopLayer", 500, 5000));
	}

	public void Dispose() 
	{
		this.AllLayers.Clear();
		this.AllLayers = null;
	}
}
