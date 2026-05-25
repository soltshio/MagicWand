using UnityEngine;

//作成者:杉山
//メインゲームのフェーズを管理するステートマシン

public class GamePhaseStateMachine : MonoBehaviour
{
    [SerializeField]
    SerializableDictionary<EGamePhaseState, GamePhaseStateTypeBase> _stateDictionary;

    [Tooltip("最初のステート")] [SerializeField]
    EGamePhaseState _initialState=EGamePhaseState.None;

    GamePhaseStateTypeBase _currentState;

    void Awake()
    {

    }

    public void ChangeState(EGamePhaseState nextState)
    {
        if(_currentState != null) _currentState.OnExit(this);

        _currentState = _stateDictionary[nextState];

        if(_currentState != null) _currentState.OnEnter(this);
    }
}
