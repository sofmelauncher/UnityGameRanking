# RankingManager

## 概要
ネットワーク上に置いたサーバと通信してデータベースを一元化し、統一されてランキングデータを提供する。ネットワークに接続できない場合はローカルにデータベースを作成し動作する。

## 関数一覧
<!-- TOC -->

- [RankingManager](#rankingmanager)
    - [概要](#概要)
    - [関数一覧](#関数一覧)
        - [RankingManager](#rankingmanager-1)
            - [パラメーター](#パラメーター)

<!-- /TOC -->


### RankingManager
`public RankingManager(string gamename, UInt64 gameid, OrderType orderType, bool onlie = true)`
#### パラメーター
- `gamename`(string)
設定するゲーム名。
- `gameid`
設定するゲームID。
- `orderType` [OrderTyple型]()
データの並び順。
- `onlie = true`