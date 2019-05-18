# RankingManager

## 概要
ネットワーク上に置いたサーバと通信してデータベースを一元化し、統一されたランキングデータを提供する。ネットワークに接続できない場合はローカルにデータベースを作成し動作する。
シングルトンクラス。

## 名前空間
`Rankig`

## プロパティ
### Inst
`public static RankingManager Inst { get;}`  
自身のインスタンス。
### GetLogPath
`public String GetLogPath { get; }`  
ログファイルが出力されるディレクトリ。
### Versions
`public readonly String Version`  
使用しているRankingManagerのバージョン情報。
### limit
`public static UInt64 limit { get; }`  
ランキングデータの取得数。

## 関数一覧

- [Setting(String, UInt64, OrderType, Boolean)](#rankingmanagerstring-uint64-ordertype-boolean)
- [DataSetAndLoadオーバーロード](#datasetandloadオーバーロード)
- [DataSetAndLoad(Double, String)](#datasetandloaddouble-string)
- [DataSetAndLoad(RankingData)](#datasetandloadrankingdata)
- [SaveData(Doubl, String)](#savedatadoubl-string)
- [SaveData(RankingData)](#savedatarankingdata)
- [GetData()](#getdata)
- [GetAllData()](#getalldata)
- [SetLimit(UInt64)](#setlimituint64)



# Setting(String, UInt64, OrderType, Boolean)
`public Setting(String gamename, UInt64 gameid, OrderType orderType, Boolean onlie = true)`  
初期設定を指定する。

### パラメーター
- `gamename` [String型](https://docs.microsoft.com/ja-jp/dotnet/api/system.string?redirectedfrom=MSDN&view=netframework-4.7.2)  
設定するゲーム名。
- `gameid`[UInt64型](https://docs.microsoft.com/ja-jp/dotnet/api/system.uint64?redirectedfrom=MSDN&view=netframework-4.7.2)  
設定するゲームID。
- `orderType` [OrderTyple型]()  
データの並び順。
- `onlie = true`[Boolean型](https://docs.microsoft.com/ja-jp/dotnet/csharp/language-reference/keywords/bool)  
オンライン動作。デフォルトは`true`。オフライン動作のみにしたい場合は`false`を指定。

# DataSetAndLoadオーバーロード

| 名称 | 概要 |
| :- | :- |
| DataSetAndLoad(double, string) | 指定した書式情報を使用してデータベースにデータを保存して、続けて、ランキングデータを取得する。 |
| DataSetAndLoad(RankingData) | RankingData型を使用してデータベースにデータを保存して、続けて、ランキングデータを取得する。 |
　　
　　
# DataSetAndLoad(Double, String)
`public List<RankingData> DataSetAndLoad(Double data, String dataName = "")`  
データの保存を行い、続けてランキングデータを取得する。

### パラメーター
- `data`[Double型](https://docs.microsoft.com/ja-jp/dotnet/csharp/language-reference/keywords/double)  
データベースに保存する64ビット倍精度浮動小数点型のスコアデータ。
- `dataName`[String型](https://docs.microsoft.com/ja-jp/dotnet/api/system.string?redirectedfrom=MSDN&view=netframework-4.7.2)  
データベースに保存するスコアの名前。ユーザー名など。設定しなければ空文字。最大100文字。

### 戻り値型
`List<RankingData>`  
取得したランキングデータが入った`RankingData`型のリスト。データがなかった場合、空リスト。

### 使用例
```
RankingManager.Inst.Setting("めじぇど", 1, OrderType.DESC);

var get = RankingManager.Inst.DataSetAndLoad(9.41, "Apple");
```

# DataSetAndLoad(RankingData)
`public List<RankingData> DataSetAndLoad(RankingData data)`
データの保存を行い、続けてランキングデータを取得する。

### パラメーター
- `data`[RankingData型](https://github.com/sofmelauncher/UnityGameRanking/blob/develop/reference/RankingData.md)  
データベースに保存する`RankingData`型のデータ。

### 戻り値
`List<RankingData>`型
取得したランキングデータが入った`RankingData`型のリスト。データがなかった場合、空リスト。

### 使用例
```
RankingManager.Inst.("めじぇど", 1, OrderType.DESC);

RankingData data = new RankingData(5.2, "yahoo");
var get = RankingManager.Inst.DataSetAndLoad(data);
```


# SaveData(Doubl, String)
`public void SaveData(Double data, String dataName = "")`  
データを指定して保存する。
### パラメーター
- `data`[Double型](https://docs.microsoft.com/ja-jp/dotnet/csharp/language-reference/keywords/double)  
データベースに保存する64ビット倍精度浮動小数点型のスコアデータ。
- `dataName`[String型](https://docs.microsoft.com/ja-jp/dotnet/api/system.string?redirectedfrom=MSDN&view=netframework-4.7.2)  
データベースに保存するスコアの名前。ユーザー名など。設定しなければ空文字。最大100文字。

### 使用例
```
RankingManager.Inst.("めじぇど", 1, OrderType.DESC);

RankingManager.Inst.SaveData(9.41, "Apple");
```

# SaveData(RankingData)
`public void SaveData(RankingData data)`  
データを指定して保存する。
### パラメーター
-   `data`[RankingData型](https://github.com/sofmelauncher/UnityGameRanking/blob/develop/reference/RankingData.md)  
データベースに保存する`RankingData`型のデータ。

### 使用例
```
RankingManager.Inst.("めじぇど", 1, OrderType.DESC);

RankingData data = new RankingData(5.2, "yahoo");
RankingManager.Inst.SaveData(data);
```

# GetData()
`List<RankingData> GetData()`   
ランキングデータを取得する。
### 戻り値
`List<RankingData>`型
データベースに保存されている`RankingData`型のデータのリスト。データがなかった場合、空リスト。

### 使用例
```
RankingManager.Inst.("めじぇど", 1, OrderType.DESC);

var data = RankingManager.Inst.GetData();
foreach(var e in data)
{
    Console.WriteLine(e.ScoreValue);
}
//取得した全要素のスコアが出力される。
```

# GetAllData()
`public List<RankingData> GetAllData()`
保存されているすべてのランキングデータを取得する。

### 戻り値
`List<RankingData>`型
データベースに保存されている`RankingData`型の全データのリスト。データがなかった場合、空リスト。**使用非推奨**。

### 使用例
```
RankingManager.Inst.("めじぇど", 1, OrderType.DESC);

var data = RankingManager.Inst.GetAllData()
foreach(var e in data)
{
    Console.WriteLine(e.ScoreValue);
}
//取得した全要素のスコアが出力される。
```

# SetLimit(UInt64)
`public void SetLimit(UInt64 lim)`  
ランキングデータを取得する個数を変更する。

### パラメーター
- `lim`[UInt64型](https://docs.microsoft.com/ja-jp/dotnet/api/system.uint64?redirectedfrom=MSDN&view=netframework-4.7.2)  
`GetData`で取得するランキングデータの個数を指定する。デフォルトは`5`。大きくしすぎには注意。**使用非推奨**。

### 使用例
```
RankingManager.Inst.("めじぇど", 1, OrderType.DESC);

RankingManager.Inst.SetLimit(20);
//GetData()で20個のデータを取得する。
```