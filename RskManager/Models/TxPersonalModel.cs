using System;
using System.ComponentModel.DataAnnotations;

namespace RskManager.Models
{
    public class TxPersonalModel
    {        
        [Required]
        public string SenderAddress
        {
            get;
            set;
        }

        [Required]
        public string ToAddress
        {
            get;
            set;
        }

        [Required]
        public string Value
        {
            get;
            set;
        }
    }
}
