using System.Collections.Generic;

namespace DataEncryptAndDecrypt
{
    public class Unamepass
    {
        public string Source { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class Cardinfo
    {
        public string Source { get; set; }
        public string CardNo { get; set; }
        public string IFSCCODE { get; set; }
        public string Validthrough { get; set; }
        public string ValidFrom { get; set; }
        public string NameOnCard { get; set; }
        public string ThreeDSecureCode { get; set; }
        public string CVV { get; set; }
        public string Notes { get; set; }
    }

    public class Mydata
    {
        public List<Unamepass> Unamepass { get; set; }
        public List<Cardinfo> Cardinfo { get; set; }
    }

    public class FileData
    {
        public Mydata Mydata { get; set; }
    }

    public interface IChangeViewvalues
    {
        void Changevalues();
    }

}