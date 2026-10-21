#ifndef _DEVICESTATEMACHINE_H_
#define _DEVICESTATEMACHINE_H_

#include "DeviceStates.h"

class DeviceStateMachine {
private:
    NormalState _normalState;//通常時のステート
    SunState _sunState;//太陽魔法ステート
    StarState _starState;//星魔法ステート
    WaterState _waterState;//水魔法ステート
    ThunderState _thunderState;//雷魔法ステート
    TimeState _timeState;//時魔法ステート

    IDeviceState* _currentState = nullptr;

public:
    DeviceStateMachine();

    void Initialize();
    void Update();

    void ChangeState(IDeviceState& nextState);

    //ステートの取得
    NormalState& GetNormalState();
    SunState& GetSunState();
    StarState& GetStarState();
    WaterState& GetWaterState();
    ThunderState& GetThunderState();
    TimeState& GetTimeState();
};

#endif
