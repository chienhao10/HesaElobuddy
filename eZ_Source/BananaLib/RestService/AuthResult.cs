// Decompiled with JetBrains decompiler
// Type: BananaLib.RestService.AuthResult
// Assembly: BananaLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75213AF3-E339-4AEB-B3FE-095F85BC5F53
// Assembly location: C:\Users\Hesa\Desktop\eZ\BananaLib.dll

using System.Collections.Generic;

namespace BananaLib.RestService
{
  public class AuthResult
  {
    public string Reason { get; set; }

    public string Status { get; set; }

    public int RetryWait { get; set; }

    public Lqt Lqt { get; set; }

    public int Delay { get; set; }

    public InGameCredentials InGameCredentials { get; set; }

    public string User { get; set; }

    public string IdToken { get; set; }

    public int Vcap { get; set; }

    public int Rate { get; set; }

    public int Node { get; set; }

    public string Champ { get; set; }

    public int Backlog { get; set; }

    public List<Ticker> Tickers { get; set; }

    public double Banned { get; set; }

    public string Destination { get; set; }
  }
}
