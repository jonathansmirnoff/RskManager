﻿using System;
using System.ComponentModel.DataAnnotations;

namespace RskManager.Models
{
    public class DeployContractModel
    {
        [Required]
        public string SenderPrivateKey
        {
            get;
            set;
        }

        [Required]
        public string SenderAddress
        {
            get;
            set;
        }

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
    }
}
