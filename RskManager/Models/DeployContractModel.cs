using System;
using System.ComponentModel.DataAnnotations;

namespace RskManager.Models
{
    public class DeployContractModel
    {
        [Required]
        public string Abi
        {
            get;
            set;
        }

        [Required]
        public string Bytecode
        {
            get;
            set;
        }

        [Required]
        public string From
        {
            get;
            set;
        }
    }
}
