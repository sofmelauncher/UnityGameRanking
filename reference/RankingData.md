# RankingData

## 概要
ランキングに使用するデータを格納するクラス。

## 名前空間
`Rankig`

## プロパティ
### GameID
`public static UInt64 GameID { get; }`  
設定したゲームID。
### DataID
`public UInt64 DataID { get; }`  
自身のランキングデータのID。オンラインとオフラインのデータは同期していない。データベース内で自動的に決まる。
### SaveTime
`public DateTime SaveTime { get; }`  
データを保存した時間。`yyyy-MM-dd HH:mm:ss`形式。
### DataName
`public String DataName { get; }`  
自身のランキングデータの名前。記録したプレイヤー名など。使用しなくても可。その場合空文字になる。
### ScoreValue
`public Double ScoreValue { get; }`  
ランキングデータのスコア値。64ビット浮動小数点値。

## 関数一覧
- [RankingData(Double, String)](#rankingdatadouble-string)
- [RankingData(UInt64, DateTime, String, Double)](#rankingdatauint64-datetime-string-double)
- [ToString()](#tostring)

# RankingData(Double, String)
`public RankingData(Double data, String name = "")`  
コンストラクター。スコアデータを設定してインスタンスを生成する。

### パラメーター
- `data`[Double型](https://docs.microsoft.com/ja-jp/dotnet/csharp/language-reference/keywords/double)  
指定する64ビット倍精度浮動小数点型のスコアデータ。
- `name`[String型](https://docs.microsoft.com/ja-jp/dotnet/api/system.string?redirectedfrom=MSDN&view=netframework-4.7.2)  
データの名前として指定する文字列。最大100文字。

### 使用例
```
RankingData data = new RankingData(5.2, "yahoo");

//名前を使用しない場合以下でも可
RankingData data = new RankingData(5.2);
```

# RankingData(UInt64, DateTime, String, Double)
`public RankingData(UInt64 dataid, DateTime time, String name, Double data)`  
コンストラクター。スコアデータを設定してインスタンスを生成する。主に内部処理で使用される。
　
- `dataid`[UInt64型](https://docs.microsoft.com/ja-jp/dotnet/api/system.uint64?redirectedfrom=MSDN&view=netframework-4.7.2)  
ランキングデータのデータID。
- `time`[DateTime型](https://docs.microsoft.com/ja-jp/dotnet/api/system.datetime?view=netframework-4.7.2)  
ランキングデータの保存時間。
- `name`[String型](https://docs.microsoft.com/ja-jp/dotnet/api/system.string?redirectedfrom=MSDN&view=netframework-4.7.2)  
ランキングデータの名前。最大100文字。
- `data`[Double型](https://docs.microsoft.com/ja-jp/dotnet/csharp/language-reference/keywords/double)  
ランキングデータのスコアデータ。

### 使用例
```
UInt64 id = 0;
DateTime time = DateTime.Now;
String name = "aaa";
Double score = 3.1415;

RankingData data = new RankingData(id, time, name, score);
//保存処理時にこのデータIDは使用しないため無意味。
```

# ToString()
`public override String ToString()`  
ランキングデータのデータをまとめて文字列で取得する。

### 戻り値
`String`型  
内部データをまとめ、一定の書式に従った文字列。  
`$"GameID = {GameID,2}, Time = {Time,10}, DataID = {DataID,2}, Name = {Name}, Score = {ScoreValue,5:#.###}"`

### 使用例
```
UInt64 id = 0;
DateTime time = DateTime.Now;
String name = "aaa";
Double score = 3.1415;

Ranking.RankingData datad = new Ranking.RankingData(id, time, name, score);

Console.WriteLine(datad.ToString());

//GameID =  2, Time = 2018-09-15 03:40:29, DataID =  0, Name = aaa, Score = 3.142
```