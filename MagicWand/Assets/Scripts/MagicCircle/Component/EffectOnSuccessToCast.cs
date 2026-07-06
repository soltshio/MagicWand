using Unity.VisualScripting;
using UnityEngine;

public class EffectOnSuccessToCast : MonoBehaviour
{
    [SerializeField]
    MagicCircleManagerVer3 _magicCircleManager;

    

    void Awake()
    {
        _magicCircleManager.OnSuccessToCast += TestLog;
    }

    void TestLog(EMagic castMagic,int touchedMagicSphereindex)
    {
        Debug.Log(castMagic);
    }
}
