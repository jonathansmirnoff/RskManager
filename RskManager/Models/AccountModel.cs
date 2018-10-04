using System;
namespace RskManager.Models
{
    public class AccountModel
    {
        public AccountModel(string address, string privateKey)
        {
            Address = address;
            PrivateKey = privateKey;
        }

        public string Address
        {
            get;
            set;
        }

        public string PrivateKey
        {
            get;
            set;
        }
    }
}
