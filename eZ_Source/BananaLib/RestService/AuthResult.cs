using System.Collections.Generic;

namespace BananaLib.RestService
{
    public class AuthResult
    {
        public int Delay { get; set; }

        public GasToken GasToken { get; set; }

        public string IdToken { get; set; }

        public InGameCredentials InGameCredentials { get; set; }

        public Lqt Lqt { get; set; }

        public int Rate { get; set; }

        public string Reason { get; set; }

        public string Status { get; set; }

        public string User { get; set; }

        public int RetryWait { get; set; }

        public int Vcap { get; set; }

        public int Node { get; set; }

        public string Champ { get; set; }

        public int Backlog { get; set; }

        public List<Ticker> Tickers { get; set; }

        public double Banned { get; set; }

        public string Destination { get; set; }
    }
}