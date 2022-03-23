var obj = 
{	
	TryConnet: function()
	{
		var account = '';
		if(typeof window.ethereum === "undefined")
		{
			UnityTryConnet("0","please install metamask");
		}else
		{
			ethereum.enable().catch(function(reason)
			{
				if (reason === "User rejected provider access") 
				{
					UnityTryConnet("0","User rejected provider access");
                } else 
				{
					UnityTryConnet("0","error");
                }
			}).then(function(accounts)
			{
				var currentProvider = web3.currentProvider;
				web3.setProvider(currentProvider);
				web3.eth.defaultAccount = accounts[0];
				account = accounts[0];
				UnityTryConnet("1",account);
			});
		}
	},
	SendTransaction: function(to, value, gasLimit, gasPrice)
	{
		var toStr = Pointer_stringify(to);
		var from = web3.eth.accounts[0];
		var valueStr = Pointer_stringify(value);
		var limitStr = Pointer_stringify(gasLimit);
		var PriceStr = Pointer_stringify(gasPrice);			

		web3.eth.sendTransaction({
			from,
			toStr,
			valueStr,
			limitStr: limitStr ? limitStr : undefined,
			PriceStr: PriceStr ? PriceStr : undefined,
		})
		.on("transactionHash", (transactionHash) => {
			UnitySendTransaction("1",transactionHash);
		})
		.on("error", (error) => {
			UnitySendTransaction("0",error.message);
		});
	},
	SendContract: function(method, abi, contract, args, value, gasLimit, gasPrice)
	{
		var methodStr =	Pointer_stringify(method);
		var abiStr =	Pointer_stringify(abi);
		var contractStr =	Pointer_stringify(contract);
		var argsStr =	Pointer_stringify(args);
		var valueStr =	Pointer_stringify(value);
		var gasLimitStr =	Pointer_stringify(gasLimit);
		var gasPriceStr =	Pointer_stringify(gasPrice);
		new web3.eth.Contract(JSON.parse(abiStr), contractStr).methods[methodStr](...JSON.parse(argsStr)).send({
			from,
			valueStr,
			gasLimitStr: gasLimitStr ? gasLimitStr : undefined,
			gasPriceStr: gasPriceStr ? gasPriceStr : undefined,
		})
		.on("transactionHash", (transactionHash) => {
			UnitySendContract("0",transactionHash);
		})
		.on("error", (error) => {
			UnitySendContract("0",error.message);
		});
	},	
	CallContract: function(method, abi, contract, args, gasLimit, gasPrice)
	{
		var methodStr =	Pointer_stringify(method);
		var abiStr =	Pointer_stringify(abi);
		var contractStr =	Pointer_stringify(contract);
		var argsStr =	Pointer_stringify(args);
		var gasLimitStr =	Pointer_stringify(gasLimit);
		var gasPriceStr =	Pointer_stringify(gasPrice);
		new web3.eth.Contract(JSON.parse(abiStr), contractStr).methods[methodStr](...JSON.parse(argsStr)).call({
			from,
			gasLimitStr: gasLimitStr ? gasLimitStr : undefined,
			gasPriceStr: gasPriceStr ? gasPriceStr : undefined,
		})
		.on("transactionHash", (transactionHash) => {
			UnityCallContract("0",transactionHash);
		})
		.on("error", (error) => {
			UnityCallContract("0",error.message);
		});
	},
}
mergeInto(LibraryManager.library, obj);