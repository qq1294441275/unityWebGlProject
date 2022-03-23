public enum JSContractEnum 
{
    test = 0,
}

public class JSNet  
{
    public string abi_path = "";
    public string abi = "";
    public string abi_address = "";
    public JSNet(string abi_path,string abi, string adress) 
    {
        this.abi_path = abi_path;
        this.abi = abi;
        this.abi_address = adress;
    }
}
