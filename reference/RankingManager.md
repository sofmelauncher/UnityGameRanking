# RankingManager

## 概要
ネットワーク上に置いたサーバと通信してデータベースを一元化し、統一されてランキングデータを提供する。ネットワークに接続できない場合はローカルにデータベースを作成し動作する。

## 関数一覧
<!-- TOC -->

- [RankingManager](#rankingmanager)
    - [概要](#概要)
    - [関数一覧](#関数一覧)
        - [RankingManager(string, UInt64, OrderType, bool)](#rankingmanagerstring-uint64-ordertype-bool)
            - [パラメーター](#パラメーター)

<!-- /TOC -->


### RankingManager(string, UInt64, OrderType, bool)
  
`public RankingManager(string gamename, UInt64 gameid, OrderType orderType, bool onlie = true)`
#### パラメーター
- `gamename` [string型](https://docs.microsoft.com/ja-jp/dotnet/api/system.string?redirectedfrom=MSDN&view=netframework-4.7.2)  
設定するゲーム名。
- `gameid`[UInt64型](https://docs.microsoft.com/ja-jp/dotnet/api/system.uint64?redirectedfrom=MSDN&view=netframework-4.7.2)  
設定するゲームID。
- `orderType` [OrderTyple型]()  
データの並び順。
- `onlie = true`[bool型](https://docs.microsoft.com/ja-jp/dotnet/csharp/language-reference/keywords/bool)  
オンライン動作。ディフォルトは`true`。オフライン動作のみにしたい場合は`false`を指定。

