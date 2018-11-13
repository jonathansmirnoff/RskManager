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
        readonly static string NodeUrl = "https://public-node.testnet.rsk.co";
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
                //Crypto stuff to generate new account!
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

        [HttpGet("GetBlockNumber")]
        public async Task<IActionResult> GetBlockNumber()
        {
            try
            {
                //TODO: Implement!
                return null;
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
                //TODO: Implement!
                return null;
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

        [HttpPost("SendTx")]
        public async Task<IActionResult> SendTx([FromBody]TxModel txModel)
        {
            try
            {
                //TODO: Implement!
                //Tips:
                    // - get transaction count
                    // - build and sign the tx OfflineTransactionSigner
                    // - Send the tx using SendRawTransaction

                return null;
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
                    /*
                     
                    var gas = await Web3Client.Eth.DeployContract.EstimateGasAsync(deployContractModel.Abi,
                                                                                   deployContractModel.Bytecode,
                                                                                   deployContractModel.SenderAddress,
                                                                                   null);


                    var w = new Web3(new Account(deployContractModel.SenderPrivateKey), NodeUrl);
                    ....
                    ....



                    return Json(txHash);
                    */

                    //TODO: Implement!
                    return null;
                }

                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return ReturnError(ex);
            }
        }

        [HttpGet("GetContractAddress")]
        public async Task<IActionResult> GetContractAddress(string txHash)
        {
            try
            {
                //TODO: Implement!
                return null;
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

                    var gas = new HexBigInteger(90000);

                    string tx;
                    if (callContractFunction.Parameter == null)
                    {
                        tx = await contractFunction.SendTransactionAsync(
                                              callContractFunction.SenderAddress,
                                              gas,
                                              new HexBigInteger("0x0"),
                                              new HexBigInteger("0x0"),
                                              null);
                    }
                    else
                    {                        
                        tx = await contractFunction.SendTransactionAsync(callContractFunction.SenderAddress,
                                              gas,
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
                //TODO: Implement!
                return null;

                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return ReturnError(ex);
            }
        }

        private IActionResult ReturnError(Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
