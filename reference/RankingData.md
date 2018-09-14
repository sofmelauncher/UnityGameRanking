# RankingData

## 概要
ランキングに使用するデータを格納するクラス。

## 名前空間
`Rankig`

## プロパティ
### GameID
`public static UInt64 GameID { private set; get; }`  
設定したゲームID。
### DataID
`public UInt64 DataID { private set; get; }`  
自身のランキングデータのID。オンラインとオフラインのデータは同期していない。
### SaveTime
`public DateTime SaveTime { private set; get; }`  
データを保存した時間。`yyyy-MM-dd HH:mm:ss`形式。
### DataName
`public String DataName { private set; get; }`  
自身のランキングデータの名前。記録したプレイヤー名など。使用しなくても可。
### ScoreValue
`public Double ScoreValue { private set; get; }`
ランキングデータのスコア値。64 ビット浮動小数点値。

## 関数一覧



# RankingData(Double, String)
`public RankingData(Double data, String name = "")`
コンストラクタ。スコアデータを設定してインスタンスを生成する。

### パラメーター
- `data`[Double型](https://docs.microsoft.com/ja-jp/dotnet/csharp/language-reference/keywords/double)  
指定する64ビット倍精度浮動小数点型のスコアデータ。
- `name`[String型](https://docs.microsoft.com/ja-jp/dotnet/api/system.string?redirectedfrom=MSDN&view=netframework-4.7.2)  
データの名前として指定する文字列。

### 使用例
```
RankingData data = new RankingData(5.2, "yahoo");
```

# RankingData(UInt64, DateTime, String, Double)
`public RankingData(UInt64 dataid, DateTime time, String name, Double data)`　
コンストラクタ。スコアデータを設定してインスタンスを生成する。主に内部処理で使用される。
　
- `dataid`[UInt64型](https://docs.microsoft.com/ja-jp/dotnet/api/system.uint64?redirectedfrom=MSDN&view=netframework-4.7.2)  
ランキングデータのデータID。
- `time`[DateTime型](https://docs.microsoft.com/ja-jp/dotnet/api/system.datetime?view=netframework-4.7.2)  
ランキングデータの保存時間。
- `name`[String型](https://docs.microsoft.com/ja-jp/dotnet/api/system.string?redirectedfrom=MSDN&view=netframework-4.7.2)  
ランキングデータの名前。
- `data`[Double型](https://docs.microsoft.com/ja-jp/dotnet/csharp/language-reference/keywords/double)  
ランキングデータのスコアデータ。

### 使用例
```
UInt64 id = 0;
DateTime time = DateTime.Now;
String name = "aaa";
Double score = 3.1415;

RankingData data = new RankingData(id, time, name, score);
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