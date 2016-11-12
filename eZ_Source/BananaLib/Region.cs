// Decompiled with JetBrains decompiler
// Type: BananaLib.Region
// Assembly: BananaLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75213AF3-E339-4AEB-B3FE-095F85BC5F53
// Assembly location: C:\Users\Hesa\Desktop\eZ\BananaLib.dll

namespace BananaLib
{
  public enum Region
  {
    [FullName("North America"), GameServerAddress("prod.na2.lol.riotgames.com"), LoginQueue("https://lq.na2.lol.riotgames.com/"), Locale("en_US"), UseGarena(false)] NA,
    [FullName("Europe West"), GameServerAddress("prod.euw1.lol.riotgames.com"), LoginQueue("https://lq.euw1.lol.riotgames.com/"), Locale("en_GB"), UseGarena(false)] EUW,
    [FullName("Europe Nordic and East"), GameServerAddress("prod.eun1.lol.riotgames.com"), LoginQueue("https://lq.eun1.lol.riotgames.com/"), Locale("en_GB"), UseGarena(false)] EUNE,
    [FullName("South Korea"), GameServerAddress("prod.kr.lol.riotgames.com"), LoginQueue("https://lq.kr.lol.riotgames.com/"), Locale("ko_KR"), UseGarena(false)] KR,
    [FullName("Brazil"), GameServerAddress("prod.br.lol.riotgames.com"), LoginQueue("https://lq.br.lol.riotgames.com/"), Locale("pt_BR"), UseGarena(false)] BR,
    [FullName("Turkey"), GameServerAddress("prod.tr.lol.riotgames.com"), LoginQueue("https://lq.tr.lol.riotgames.com/"), Locale("pt_BR"), UseGarena(false)] TR,
    [FullName("Public Beta Environment"), GameServerAddress("prod.pbe1.lol.riotgames.com"), LoginQueue("https://lq.pbe1.lol.riotgames.com/"), Locale("en_US"), UseGarena(false)] PBE,

    //[FullName("Singapore"), GameServerAddress("prod.lol.garenanow.com"), LoginQueue("https://lq.lol.garenanow.com/"), UseGarena(true)] SG,

    [FullName("Singapore/Malaysia"), GameServerAddress("prod.lol.garenanow.com"), LoginQueue("https://lq.lol.garenanow.com/"), Locale("en_US"), UseGarena(true)] SGMY,
    [FullName("Taiwan"), GameServerAddress("prodtw.lol.garenanow.com"), LoginQueue("https://loginqueuetw.lol.garenanow.com/"), Locale("en_US"), UseGarena(true)] TW,
    [FullName("Thailand"), GameServerAddress("prodth.lol.garenanow.com"), LoginQueue("https://lqth.lol.garenanow.com/"), Locale("en_US"), UseGarena(true)] TH,
    [FullName("Phillipines"), GameServerAddress("prodph.lol.garenanow.com"), LoginQueue("https://storeph.lol.garenanow.com/"), Locale("en_US"), UseGarena(true)] PH,
    [FullName("Vietnam"), GameServerAddress("prodvn1.lol.garenanow.com"), LoginQueue("https://lqvn1.lol.garenanow.com/"), Locale("en_US"), UseGarena(true)] VN,
    [FullName("Russia"), GameServerAddress("prod.ru.lol.riotgames.com"), LoginQueue("https://lq.ru.lol.riotgames.com/"), Locale("en_US"), UseGarena(false)] RU,
    [FullName("Oceania"), GameServerAddress("prod.oc1.lol.riotgames.com"), LoginQueue("https://lq.oc1.lol.riotgames.com/"), Locale("en_US"), UseGarena(false)] OCE,
    [FullName("Latin America North"), GameServerAddress("prod.la1.lol.riotgames.com"), LoginQueue("https://lq.la1.lol.riotgames.com/"), Locale("en_US"), UseGarena(false)] LAN,
    [FullName("Latin America South"), GameServerAddress("prod.la2.lol.riotgames.com"), LoginQueue("https://lq.la2.lol.riotgames.com/"), Locale("en_US"), UseGarena(false)] LAS,
    [FullName("Japan"), GameServerAddress("prod.jp1.lol.riotgames.com"), LoginQueue("https://lq.jp1.lol.riotgames.jp/"), Locale("ja_JP"), UseGarena(false)] JP,
  }
}