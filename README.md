# UnityGameRanking
C#上でローカルサーバーまたはオンラインサーバーとランキングデータをやり取りするためのもの。
オンラインサーバーについては別レポジトリ参照。

# 使用方法
1. SofmeRanking.zipをダウンロードして解凍する[(リンク)](https://github.com/sofmelauncher/UnityGameRanking/releases)
1. SofmeRanking.dllを参照する。
1. プログラム内から呼び出す

```
Ranking.RankingManager m = new Ranking.RankingManager(ゲーム名, ゲームID, オーダータイプ, オンライン動作);
//例
//Ranking.RankingManager m = new Ranking.RankingManager("神のゲーム", 1, Ranking.OrderType.DESC);

//一度実行
m.init();

//データ挿入
m.SaveData(スコアデータ, データ名);
//例
//m.SaveData(10.5, "ジェイソン");

//データ取得
//例
var data = m.GetData();
foreach(var e in data)
{
  Console.WriteLine(e.ScoreValue);
  Console.WriteLine(e.DataName);
}
```

## 引数の説明
- ゲーム名  
  文字列
- ゲームID  
  符号なし整数型, 0は指定不可
- オーダータイプ  
  - ASC  
  昇順
  - DESC  
  降順
- オンライン動作  
  ディフォルトはtrue  
  オフライン動作のみにしたい場合はfalse指定
  
- スコアデータ  
double型
- データ名  
文字列型  
最大100文字
---
詳しくはリファレンスへ
[reference](https://github.com/sofmelauncher/UnityGameRanking/tree/develop/reference)

---
<br><br><br><br>
リポジトリ名にUnityって書いたけど、Untiyに依存しないようにした。  
問題とかあればissue立ててくれたり連絡してくれるととてもありがたいです。  
あと、正式な名前がきまってない...
