using Cysharp.Threading.Tasks;
using UnityEngine;

//作成者:杉山
//巨大生物のリアクション(基底)

public abstract class BigCreatureReactionTypeBase : MonoBehaviour
{
    //リアクション
    //updatedBigCreatureStatus: 更新後の巨大生物のステータス
    public abstract UniTask TakeReactionAsync(BigCreatureStatus updatedBigCreatureStatus);
}
