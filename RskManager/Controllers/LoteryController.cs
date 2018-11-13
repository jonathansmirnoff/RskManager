using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using RskManager.Models;

namespace RskManager.Controllers
{
    public class LoteryController : Controller
    {
        readonly static string NodeUrl = "https://public-node.testnet.rsk.co";
        readonly Web3 Web3Client = new Web3(NodeUrl);

        static string LoteryContractAddress = "0x731e7c3ab5594c0ce7ca04e870e3e086d750218d";
        readonly static string LoteryAbi = "[{\"constant\":false,\"inputs\":[],\"name\":\"register\",\"outputs\":[],\"payable\":false,\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"constant\":true,\"inputs\":[],\"name\":\"determineWinner\",\"outputs\":[{\"name\":\"\",\"type\":\"address\"}],\"payable\":false,\"stateMutability\":\"view\",\"type\":\"function\"},{\"constant\":false,\"inputs\":[],\"name\":\"restartLotery\",\"outputs\":[],\"payable\":false,\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"constant\":true,\"inputs\":[],\"name\":\"random\",\"outputs\":[{\"name\":\"\",\"type\":\"uint256\"}],\"payable\":false,\"stateMutability\":\"view\",\"type\":\"function\"},{\"constant\":true,\"inputs\":[{\"name\":\"\",\"type\":\"uint256\"}],\"name\":\"playersByNumber\",\"outputs\":[{\"name\":\"\",\"type\":\"address\"}],\"payable\":false,\"stateMutability\":\"view\",\"type\":\"function\"},{\"constant\":true,\"inputs\":[],\"name\":\"owner\",\"outputs\":[{\"name\":\"\",\"type\":\"address\"}],\"payable\":false,\"stateMutability\":\"view\",\"type\":\"function\"},{\"constant\":true,\"inputs\":[{\"name\":\"\",\"type\":\"address\"}],\"name\":\"isPlayerRegister\",\"outputs\":[{\"name\":\"\",\"type\":\"bool\"}],\"payable\":false,\"stateMutability\":\"view\",\"type\":\"function\"},{\"constant\":true,\"inputs\":[],\"name\":\"totalPlayers\",\"outputs\":[{\"name\":\"\",\"type\":\"uint8\"}],\"payable\":false,\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"payable\":false,\"stateMutability\":\"nonpayable\",\"type\":\"constructor\"}]";
        readonly static string LoteryByteCode = "0x608060405234801561001057600080fd5b5060038054600160a060020a031916331790556000805460ff191690556104748061003c6000396000f30060806040526004361061008d5763ffffffff7c01000000000000000000000000000000000000000000000000000000006000350416631aa3a008811461009257806333b16d93146100a9578063522c669c146100da5780635ec01e4d146100ef578063680c89c2146101165780638da5cb5b1461012e5780639dc36fb314610143578063f60cdcf614610178575b600080fd5b34801561009e57600080fd5b506100a76101a3565b005b3480156100b557600080fd5b506100be61029f565b60408051600160a060020a039092168252519081900360200190f35b3480156100e657600080fd5b506100a76102d6565b3480156100fb57600080fd5b506101046103c8565b60408051918252519081900360200190f35b34801561012257600080fd5b506100be6004356103f3565b34801561013a57600080fd5b506100be61041b565b34801561014f57600080fd5b50610164600160a060020a036004351661042a565b604080519115158252519081900360200190f35b34801561018457600080fd5b5061018d61043f565b6040805160ff9092168252519081900360200190f35b3360009081526002602052604090205460ff161561022257604080517f08c379a000000000000000000000000000000000000000000000000000000000815260206004820152601a60248201527f53656e64657220697320616c7265616479207265676973746572000000000000604482015290519081900360640190fd5b6001805480820182557fb10e2d527612073b26eecdfd717e6a320cf44b4afac2b0732d9fcbe2b7fa0cf601805473ffffffffffffffffffffffffffffffffffffffff1916339081179091556000908152600260205260408120805460ff199081168417909155815460ff8181169094019093169216919091179055565b6000806102aa6103c8565b90506001818154811015156102bb57fe5b600091825260209091200154600160a060020a031691505090565b600354600090600160a060020a0316331461035257604080517f08c379a000000000000000000000000000000000000000000000000000000000815260206004820152601860248201527f53656e646572206973206e6f74206175746f72697a65642e0000000000000000604482015290519081900360640190fd5b5060005b60005460ff90811690821610156103bb5760006002600060018460ff1681548110151561037f57fe5b600091825260208083209190910154600160a060020a031683528201929092526040019020805460ff1916911515919091179055600101610356565b506000805460ff19169055565b6000805460408051428152905190819003602001902060ff909116908115156103ed57fe5b06905090565b600180548290811061040157fe5b600091825260209091200154600160a060020a0316905081565b600354600160a060020a031681565b60026020526000908152604090205460ff1681565b60005460ff16815600a165627a7a7230582076458a47ed10f7cc74d567bdde2740c5fefada0599a3d4d5841cb20e1ed6344b0029";

        public LoteryController()
        {
        }


        [HttpPost("GetNumberOfPlayers")]
        public async Task<IActionResult> GetNumberOfPlayers()
        {
            try
            {
                var contract = Web3Client.Eth.GetContract(LoteryAbi, LoteryContractAddress);
                var result = await contract.GetFunction("totalPlayers").CallAsync<object>();

                return Json(result);
            }
            catch (Exception ex)
            {
                return ReturnError(ex);
            }
        }

        [HttpPost("GetOwner")]
        public async Task<IActionResult> GetOwner()
        {
            try
            {
                var contract = Web3Client.Eth.GetContract(LoteryAbi, LoteryContractAddress);
                var result = await contract.GetFunction("owner").CallAsync<object>();

                return Json(result);
            }
            catch (Exception ex)
            {
                return ReturnError(ex);
            }
        }

        [HttpPost("GetWinner")]
        public async Task<IActionResult> GetWinner()
        {
            try
            {
                var contract = Web3Client.Eth.GetContract(LoteryAbi, LoteryContractAddress);
                var result = await contract.GetFunction("determineWinner").CallAsync<object>();

                return Json(result);
            }
            catch (Exception ex)
            {
                return ReturnError(ex);
            }
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody]AccountModel accountModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var w = new Web3(new Account(accountModel.PrivateKey), NodeUrl);

                    var contract = w.Eth.GetContract(LoteryAbi, LoteryContractAddress);
                    var contractFunction = contract.GetFunction("register");

                    var gas = await contractFunction.EstimateGasAsync(null);

                    var accountBalance = await w.Eth.GetBalance.SendRequestAsync(accountModel.Address);

                    if (accountBalance < gas.Value * 2)
                        return ReturnError("Insufficient gas");

                    var tx = await contractFunction.SendTransactionAsync(
                                              accountModel.Address,
                                              new HexBigInteger(gas.Value * 2),
                                              new HexBigInteger("0x0"),
                                              new HexBigInteger("0x0"),
                                              null);

                    return Json(tx);
                }

                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return ReturnError(ex);
            }
        }

        [HttpPost("DeployLoteryContract")]
        public async Task<IActionResult> DeployLoteryContract([FromBody]AccountModel accountModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //new HexBigInteger(0x300000)
                    //gasPrice = 0x0
                    //value = 0x0
                    var gas = await Web3Client.Eth.DeployContract.EstimateGasAsync(LoteryAbi,
                                                                                   LoteryByteCode,
                                                                                   accountModel.Address,
                                                                                   null);


                    var w = new Web3(new Account(accountModel.PrivateKey), NodeUrl);
                    var txHash = await w.Eth.DeployContract
                                        .SendRequestAsync(LoteryAbi,
                                                          LoteryByteCode,
                                                          accountModel.Address,
                                                          gas,
                                                          new HexBigInteger("0x0"),
                                                          new HexBigInteger("0x0"));

                    LoteryContractAddress = await GetContractAddress(txHash);

                    return Json(LoteryContractAddress);
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


        /// <summary>
        /// Returns the error.
        /// </summary>
        /// <returns>The error.</returns>
        /// <param name="message">Error.</param>
        private IActionResult ReturnError(string message)
        {
            return StatusCode(500, message);
        }

        private async Task<string> GetContractAddress(string txHash)
        {
            while (true)
            {
                var receipt = await Web3Client.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(txHash);

                if (receipt != null)
                    return receipt.ContractAddress;
            }
        }
    }
}
