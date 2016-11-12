// Decompiled with JetBrains decompiler
// Type: BananaLib.RestService.InGameCredentials
// Assembly: BananaLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75213AF3-E339-4AEB-B3FE-095F85BC5F53
// Assembly location: C:\Users\Hesa\Desktop\eZ\BananaLib.dll

namespace BananaLib.RestService
{
  public class InGameCredentials
  {
    public bool InGame { get; set; }

    public double? SummonerId { get; set; }

    public string ServerIp { get; set; }

    public int? ServerPort { get; set; }

    public string EncryptionKey { get; set; }

    public string HandshakeToken { get; set; }
  }
}
