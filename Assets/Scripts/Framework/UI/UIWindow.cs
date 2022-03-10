
public class UIWindow 
{
    // 窗口名字
    public string Name;
    // Layer 层级
    public UILayer Layer;
    // Ctrl 实例
    public UIBaseModel Model;
    // View 实例
    public UIBaseView View;
    // View 实例
    public UIBaseCtrl Ctrl;
    // 是否激活
    public bool Active;
    // 预设路径
    public string PrefabPath;
    // 是否正在加载
    public bool IsLoading;
}
