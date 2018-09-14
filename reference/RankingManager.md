# RankingManager

## 概要
ネットワーク上に置いたサーバと通信してデータベースを一元化し、統一されてランキングデータを提供する。ネットワークに接続できない場合はローカルにデータベースを作成し動作する。

## 名前空間
`Rankig`

## プロパティ
### GetLogPath
`public readonly String Version`  
ログファイルが出力されるディレクトリ。
### Versions
`public String GetLogPath { get; }`  
使用しているRankingManagerのバージョン情報。
### limit
`public static UInt64 limit { private set; get; }`  
ランキングデータの取得数。

## 関数一覧

<!-- TOC -->

- [RankingManager](#rankingmanager)
    - [概要](#概要)
    - [名前空間](#名前空間)
    - [プロパティ](#プロパティ)
        - [GetLogPath](#getlogpath)
        - [Versions](#versions)
        - [limit](#limit)
    - [関数一覧](#関数一覧)
- [RankingManager(String, UInt64, OrderType, Boolean)](#rankingmanagerstring-uint64-ordertype-boolean)
        - [パラメーター](#パラメーター)
- [Init()](#init)
- [DataSetAndLoadオーバーロード](#datasetandloadオーバーロード)
- [DataSetAndLoad(Double, String)](#datasetandloaddouble-string)
        - [パラメーター](#パラメーター-1)
        - [戻り値](#戻り値)
- [DataSetAndLoad(RankingData)](#datasetandloadrankingdata)
        - [パラメーター](#パラメーター-2)
        - [戻り値](#戻り値-1)
- [SaveData(Doubl, String)](#savedatadoubl-string)
        - [パラメーター](#パラメーター-3)
- [SaveData(RankingData)](#savedatarankingdata)
        - [パラメーター](#パラメーター-4)
- [GetData()](#getdata)
        - [戻り値](#戻り値-2)
- [GetAllData()](#getalldata)
        - [戻り値](#戻り値-3)
- [SetLimit(UInt64)](#setlimituint64)
        - [パラメーター](#パラメーター-5)

<!-- /TOC -->


# RankingManager(String, UInt64, OrderType, Boolean)
`public RankingManager(String gamename, UInt64 gameid, OrderType orderType, Boolean onlie = true)`
### パラメーター
- `gamename` [String型](https://docs.microsoft.com/ja-jp/dotnet/api/system.string?redirectedfrom=MSDN&view=netframework-4.7.2)  
設定するゲーム名。
- `gameid`[UInt64型](https://docs.microsoft.com/ja-jp/dotnet/api/system.uint64?redirectedfrom=MSDN&view=netframework-4.7.2)  
設定するゲームID。
- `orderType` [OrderTyple型]()  
データの並び順。
- `onlie = true`[Boolean型](https://docs.microsoft.com/ja-jp/dotnet/csharp/language-reference/keywords/bool)  
オンライン動作。ディフォルトは`true`。オフライン動作のみにしたい場合は`false`を指定。

# Init()
`public void Init()`  
クラスの初期化、データベースへの接続などを行う。

# DataSetAndLoadオーバーロード

| 名称 | 概要 |
| :- | :- |
| DataSetAndLoad(double, string) | 指定した書式情報を使用してデータベースにデータを保存して、続けて、ランキングデータを取得する。 |
| DataSetAndLoad(RankingData) | RankingData型を使用してデータベースにデータを保存して、続けて、ランキングデータを取得する。 |

# DataSetAndLoad(Double, String)
`public List<RankingData> DataSetAndLoad(Double data, String dataName = "")`

### パラメーター
- `data`[Double型](https://docs.microsoft.com/ja-jp/dotnet/csharp/language-reference/keywords/double)  
データベースに保存するスコアデータ。
- `dataName`[String型](https://docs.microsoft.com/ja-jp/dotnet/api/system.string?redirectedfrom=MSDN&view=netframework-4.7.2)  
データベースに保存するスコアの名前。ユーザー名など。設定しなければ空文字。

### 戻り値
`List<RankingData>`  
取得したランキングデータが入った`RankingData`型のリスト。データがなかった場合、空リスト。


# DataSetAndLoad(RankingData)
`public List<RankingData> DataSetAndLoad(RankingData data)`

### パラメーター
- `data`[RankingData型]()  
データベースに保存する`RankingData`型のデータ。

### 戻り値
`List<RankingData>`
取得したランキングデータが入った`RankingData`型のリスト。データがなかった場合、空リスト。

# SaveData(Doubl, String)
`public void SaveData(Double data, String dataName = "")`

### パラメーター
- `data`[Double型](https://docs.microsoft.com/ja-jp/dotnet/csharp/language-reference/keywords/double)  
データベースに保存するスコアデータ。
- `dataName`[String型](https://docs.microsoft.com/ja-jp/dotnet/api/system.string?redirectedfrom=MSDN&view=netframework-4.7.2)  
データベースに保存するスコアの名前。ユーザー名など。設定しなければ空文字。

# SaveData(RankingData)
`public void SaveData(RankingData data)`

### パラメーター
-   `data`[RankingData型]()  
データベースに保存する`RankingData`型のデータ。

# GetData()
`List<RankingData> GetData()`   

### 戻り値
`List<RankingData>`
データベースに保存されている`RankingData`型のデータのリスト。データがなかった場合、空リスト。

# GetAllData()
`public List<RankingData> GetAllData()`

### 戻り値
`List<RankingData>`
データベースに保存されている`RankingData`型の全データのリスト。データがなかった場合、空リスト。使用非推奨。

# SetLimit(UInt64)
`public void SetLimit(UInt64 lim)`

### パラメーター
- `lim`[UInt64型](https://docs.microsoft.com/ja-jp/dotnet/api/system.uint64?redirectedfrom=MSDN&view=netframework-4.7.2)  
`GetData`で取得するランキングデータの個数を指定する。ディフォルトは`5`。大きくしすぎには注意。使用非推奨。