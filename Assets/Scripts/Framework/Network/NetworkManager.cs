using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class NetworkManager : MonoSingleton<NetworkManager>
{
    [DllImport("__Internal")]
    private static extern void TryConnet();

    [DllImport("__Internal")]
    private static extern void SendTransaction(string to, string value, string gasLimit, string gasPrice);

    [DllImport("__Internal")]
    private static extern void SendContract(string method, string abi, string contract, string args, string value, string gasLimit, string gasPrice);

    [DllImport("__Internal")]
    private static extern void CallContract(string method, string abi, string contract, string args, string gasLimit, string gasPrice);

    private Dictionary<JSContractEnum, JSNet> all_jsnet = null;

    public override void Dispose()
    {
        base.Dispose();
    }

    protected override void Init()
    {
        base.Init();
        all_jsnet = new Dictionary<JSContractEnum, JSNet>();
        this.InitAllJsNet();
    }

    public void InitAllJsNet() 
    {
        
    }

    public void AddJsNet(JSContractEnum js_contract_enum, string abi_path, string adress)
    {
        TextAsset abi_text = Resources.Load(abi_path,typeof(TextAsset)) as TextAsset;
        string abi = abi_text.text;
        JSNet js_net = new JSNet(abi_path, abi, adress);
        if (!all_jsnet.ContainsKey(js_contract_enum))
            all_jsnet.Add(js_contract_enum, js_net);
        else
            Debug.LogError("已经有此类型的abi了,无需再添加");

    }

    public JSNet GetJsNet(JSContractEnum js_contract_enum) 
    {
        JSNet js_net = null;
        all_jsnet.TryGetValue(js_contract_enum, out js_net);
        return js_net;
    }

    public void CSharpTryConnet() 
    {
        TryConnet();
    }

    public void CSharpSendTransaction(string to, string value, string gasLimit, string gasPrice)
    {
        SendTransaction(to, value, gasLimit, gasPrice);
    }
    public void CSharpSendContract(JSContractEnum js_contract_enum, string method, string cur_value, string args, string gasLimit, string gasPrice)
    {
        JSNet js_net = GetJsNet(js_contract_enum);
        if (js_net != null)
        {
            SendContract(method,js_net.abi,js_net.abi_address,args,cur_value,gasLimit,gasPrice);
        }
    }
    public void CSharpCallContract(JSContractEnum js_contract_enum, string method, string args, string gasLimit, string gasPrice)
    {
        JSNet js_net = GetJsNet(js_contract_enum);
        if (js_net != null)
        {
            CallContract(method, js_net.abi, js_net.abi_address, args, gasLimit, gasPrice);
        }
    }
    //连接钱包回调
    public void TryConnetCall(string code, string response) 
    {
        Debug.LogWarningFormat("TryConnetCall 状态码->{0},回调数据->{1}", code, response);
    }
    //转账回调
    public void SendTransactionCall(string code, string response)
    {
        Debug.LogWarningFormat("SendTransactionCall 状态码->{0},回调数据->{1}", code, response);
    }
    //发送合约回调
    public void SendContractCall(string code, string response)
    {
        Debug.LogWarningFormat("SendContractCall 状态码->{0},回调数据->{1}", code, response);
    }
    //执行合约回调
    public void CallContractCall(string code, string response)
    {
        Debug.LogWarningFormat("CallContractCall 状态码->{0},回调数据->{1}", code, response);
    }
}
