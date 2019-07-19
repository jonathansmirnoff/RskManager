using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Hex.HexTypes;
using Nethereum.Util;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using RskManager.Models;
using static Nethereum.Util.UnitConversion;

namespace RskManager.Controllers
{
    [Route("api/[controller]")]
    public class RskController : Controller
    {
        readonly static string NodeUrl = "http://localhost:4444";
        readonly Web3 Web3Client = new Web3(NodeUrl);

        public RskController()
        {
        }

        [HttpGet]
        public string Get()
        {
            return "RskController";
        }

        [HttpGet("CreateNewAccount")]
        public IActionResult CreateNewAccount()
        {
            try
            {
                var ecKey = Nethereum.Signer.EthECKey.GenerateKey();
                var privateKey = ecKey.GetPrivateKeyAsBytes().ToHex();
                var account = new Nethereum.Web3.Accounts.Account(privateKey);

                return Json(new AccountModel(account.Address.ToLower(), account.PrivateKey));
            }   
            catch (Exception ex)
            {
                return ReturnError(ex);
            }
        }

        [HttpPost("GetPublicKeyFromPrivate")]
        public async Task<IActionResult> GetPublicKeyFromPrivate(string privateKey)
        {
            try
            {
                //var ecKey = Nethereum.Signer.EthECKey.GenerateKey();
                //var privateKey = ecKey.GetPrivateKeyAsBytes().ToHex();
                var account = new Nethereum.Web3.Accounts.Account(privateKey);

                return Json(new AccountModel(account.Address.ToLower(), account.PrivateKey));
            }
            catch (Exception ex)
            {
                return ReturnError(ex);
            }
        }



        [HttpPost("SendRbtcOnlyForRegtest")]
        public async Task<IActionResult> SendRbtcOnlyForRegtest(string address)
        {
            try
            {
                UnitConversion unitConversion = new UnitConversion();

                var personalAccounts = await Web3Client.Eth.Accounts.SendRequestAsync();

                if (personalAccounts != null && personalAccounts.Length > 0)
                {

                    var transacInpunt = new Nethereum.RPC.Eth.DTOs.TransactionInput
                    {
                        From = personalAccounts[0],
                        To = address,
                        Value = new HexBigInteger(unitConversion.ToWei(0.1, EthUnit.Ether))
                    };

                    var tx = await Web3Client.Eth.Transactions.SendTransaction.SendRequestAsync(transacInpunt);

                    return Json(tx);
                }

                return Json(string.Empty);
            }
            catch (Exception ex)
            {
                return ReturnError(ex);
            }
        }

        [HttpGet("GetMinimumGasPrice")]
        public async Task<IActionResult> GetMinimumGasPrice()
        {
            try
            {
                var result = await Web3Client.Eth.Blocks.GetBlockWithTransactionsByNumber.SendRequestAsync(new Nethereum.RPC.Eth.DTOs.BlockParameter());

                return Json(result);
            }
            catch (Exception ex)
            {
                return ReturnError(ex);
            }
        }

        [HttpGet("GetBlockNumber")]
        public async Task<IActionResult> GetBlockNumber()
        {
            try
            {
                var result = await Web3Client.Eth.Blocks.GetBlockNumber.SendRequestAsync();
                return Json(result.Value);
            }
            catch (Exception ex)
            {
                return ReturnError(ex);
            }
        }

        [HttpGet("GetAccounts")]
        public async Task<IActionResult> GetAccounts()
        {
            try
            {
                var result = await Web3Client.Eth.Accounts.SendRequestAsync();
                return Json(result);
            }
            catch (Exception ex)
            {
                return ReturnError(ex);
            }
        }

        [HttpGet("Balance")]
        public async Task<IActionResult> Balance(string address)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(address))
                    BadRequest("Address is null");

                UnitConversion unitConversion = new UnitConversion();

                var balance = await Web3Client.Eth.GetBalance.SendRequestAsync(address);                
                return Json(unitConversion.FromWei(balance.Value, EthUnit.Ether));
            }
            catch (Exception ex)
            {
                return ReturnError(ex);
            }
        }

        [HttpPost("EstimateContractGas")]
        public async Task<IActionResult> EstimateContractGas([FromBody] DeployContractModel model)
        {
            try
            {
                var gas = await Web3Client.Eth.DeployContract.EstimateGasAsync(model.Abi,
                                                                                   model.Bytecode,
                                                                                   model.SenderAddress,
                                                                                   null);
                return Json(gas);
            }
            catch (Exception ex)
            {
                return ReturnError(ex);
            }
        }

        [HttpPost("PersonalSendTx")]
        public async Task<IActionResult> PersonalSendTx([FromBody]TxPersonalModel txModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UnitConversion unitConversion = new UnitConversion();

                    var transacInpunt = new Nethereum.RPC.Eth.DTOs.TransactionInput
                    {
                        From = txModel.SenderAddress,
                        To = txModel.ToAddress,
                        Value = new HexBigInteger(unitConversion.ToWei(txModel.Value, EthUnit.Ether))
                    };

                    var tx = await Web3Client.Eth.Transactions.SendTransaction.SendRequestAsync(transacInpunt);
                    return Json(tx);
                }

                return BadRequest(ModelState);
                
            }
            catch(Exception ex)
            {
                return ReturnError(ex);
            }
            
        }

        [HttpPost("SendTx")]
        public async Task<IActionResult> SendTx([FromBody]TxModel txModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var txCount = Web3Client
                        .Eth.Transactions
                        .GetTransactionCount
                        .SendRequestAsync(txModel.SenderAddress).Result;


                    var encoded = Web3.OfflineTransactionSigner
                                      .SignTransaction(txModel.SenderPrivateKey,
                                                       txModel.ToAddress,
                                                       new HexBigInteger(txModel.Value),
                                                       txCount.Value,
                                                       new HexBigInteger("0x0"),
                                                       new HexBigInteger(21000));

                    var send = await Web3Client.Eth
                                               .Transactions
                                               .SendRawTransaction
                                               .SendRequestAsync("0x" + encoded);

                    return Json(send);
                }

                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return ReturnError(ex);
            }
        }

        [HttpPost("DeployContract")]
        public async Task<IActionResult> DeployContract([FromBody]DeployContractModel deployContractModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //new HexBigInteger(0x300000)
                    //gasPrice = 0x0
                    //value = 0x0
                    var gas = await Web3Client.Eth.DeployContract.EstimateGasAsync(deployContractModel.Abi,
                                                                                   deployContractModel.Bytecode,
                                                                                   deployContractModel.SenderAddress,
                                                                                   null);
                    

                    var w = new Web3(new Account(deployContractModel.SenderPrivateKey), NodeUrl);
                    var txHash = await w.Eth.DeployContract
                                        .SendRequestAsync(deployContractModel.Abi,
                                                          deployContractModel.Bytecode,
                                                          deployContractModel.SenderAddress,
                                                          gas,
                                                          new HexBigInteger("0x0"),
                                                          new HexBigInteger("0x0"));
                    return Json(txHash);
                }

                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return ReturnError(ex);
            }
        }

        [HttpGet("GetTransactionReceipt")]
        public async Task<IActionResult> GetTransactionReceipt(string txHash)
        {
            try
            {
                return Json(await Web3Client.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(txHash));
            }
            catch(Exception ex)
            {
                return ReturnError(ex);
            }

        }

        [HttpGet("GetContractAddress")]
        public async Task<IActionResult> GetContractAddress(string txHash)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txHash))
                    return BadRequest("txHash is null");

                var receipt = await Web3Client.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(txHash);

                if (receipt != null)
                    return Json(receipt.ContractAddress);
                else
                    return Json(string.Empty);
            }
            catch (Exception ex)
            {
                return ReturnError(ex);
            }        
        }

        [HttpPost("CallContractFunctionTxParameter")]
        public async Task<IActionResult> CallContractFunctionTxParameter([FromBody]CallContractFunctionParameterModel callContractFunction)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var w = new Web3(new Account(callContractFunction.SenderPrivateKey), NodeUrl);

                    var contract = w.Eth.GetContract(callContractFunction.Abi, callContractFunction.ContractAddress);
                    var contractFunction = contract.GetFunction(callContractFunction.FunctionName);

                    string tx;

                    if (callContractFunction.Parameter == null)
                    {                        
                        var gas = await contractFunction.EstimateGasAsync(null);

                        tx = await contractFunction.SendTransactionAsync(
                                              callContractFunction.SenderAddress,
                                              gas,
                                              new HexBigInteger("0x0"),
                                              new HexBigInteger("0x0"),
                                              null);
                    }
                    else
                    {
                        //var gas = await contractFunction.EstimateGasAsync(callContractFunction.Parameter);
                        tx = await contractFunction.SendTransactionAsync(callContractFunction.SenderAddress,
                                              new HexBigInteger("0x34000"),
                                              new HexBigInteger("0x0"),
                                              new HexBigInteger("0x0"),
                                              callContractFunction.Parameter);
                    }


                    return Json(tx);
                }

                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return ReturnError(ex);
            }
        }

        [HttpPost("CallContractFunction")]
        public async Task<IActionResult> CallContractFunction([FromBody]CallContractFunctionModel callContractFunction)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var contract = Web3Client.Eth.GetContract(callContractFunction.Abi, callContractFunction.ContractAddress);
                    var result = await contract.GetFunction(callContractFunction.FunctionName).CallAsync<object>();

                    return Json(result);
                }

                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return ReturnError(ex);
            }
        }


        /// <summary>
        /// Returns the error.
        /// </summary>
        /// <returns>The error.</returns>
        /// <param name="ex">Ex.</param>
        private IActionResult ReturnError(Exception ex)
        {            
            return StatusCode(500, ex.Message);
        }
    }
}
