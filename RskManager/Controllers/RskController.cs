using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nethereum.Web3;
using RskManager.Models;

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

        [HttpGet("GetBlockNumber")]
        public async Task<IActionResult> GetBlockNumber()
        {
            try
            {
                //TODO: Implement Logic!
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
                //TODO: Implement Logic!
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
                var balance = await Web3Client.Eth.DeployContract.EstimateGasAsync(model.Abi,
                                                                                   model.Bytecode,
                                                                                   model.From,
                                                                                   null);
                return Json(balance);
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
