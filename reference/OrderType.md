# OrderType

## 概要
ランキングデータのデータの並び順。

## 名前空間
`Ranking`

### 変数
| 変数 | 説明 |
| :- | :- |
| ASC | 昇順(少ない方が上位) |
| DESC | 降順(大きい方が上位) |

### 使用例
```
OrderType type = OrderType.ASC;
RankingManager.Inst.Setting("ああああああ", 1, type, false);
```