#ifndef _DEVICESTATEMACHINE_H_
#define _DEVICESTATEMACHINE_H_

#include "CommonDeviceStates.h"
#include "InvokeDeviceStates.h"
#include "CastDeviceStates.h"
#include "Color.h"
#include "LED12Ring.h"

struct StateLEDProperty//状態ごとのLEDのプロパティをまとめたもの
{
    const Color ledColors[LED12Ring::LEDNum()];
    const float _lowLEDIntensity;//暗い時のLEDの光
    const float _highLEDIntensity;//明るい時のLEDの光
};

class DeviceStateMachine {
private:
    NormalState _normalState;//通常時のステート

    SunCastState _sunCastState;//太陽詠唱ステート
    SunInvokeState _sunInvokeState;//太陽発動ステート

    StarCastState _starCastState;//星詠唱ステート
    StarInvokeState _starInvokeState;//星発動ステート

    WaterCastState _waterCastState;//水詠唱ステート
    WaterInvokeState _waterInvokeState;//水発動ステート

    ThunderCastState _thunderCastState;//雷詠唱ステート
    ThunderInvokeState _thunderInvokeState;//雷発動ステート

    TimeCastState _timeCastState;//時詠唱ステート
    TimeInvokeState _timeInvokeState;//時発動ステート

    IDeviceState* _currentState = nullptr;

public:
    DeviceStateMachine(
        MPU_AccGyro              &mpu,
        DFPlayerMini             &dfPlayerMini,
        LED12Ring                &led12Ring,
        CommandManager           &commandManager,
        const StateLEDProperty   &normalLEDProperty,
        const StateLEDProperty   &sunLEDProperty,
        const StateLEDProperty   &starLEDProperty,
        const StateLEDProperty   &thunderLEDProperty,
        const StateLEDProperty   &waterLEDProperty,
        const StateLEDProperty   &timeLEDProperty
    );

    void Initialize();
    void Update();

    void ChangeState(IDeviceState& nextState);

    //ステートの取得
    NormalState& GetNormalState() {return _normalState;}

    SunCastState& GetSunCastState() {return _sunCastState;}
    SunInvokeState& GetSunInvokeState() {return _sunInvokeState;}

    StarCastState& GetStarCastState() {return _starCastState;}
    StarInvokeState& GetStarInvokeState() {return _starInvokeState;}

    WaterCastState& GetWaterCastState() {return _waterCastState;}
    WaterInvokeState& GetWaterInvokeState() {return _waterInvokeState;}

    ThunderCastState& GetThunderCastState() {return _thunderCastState;}
    ThunderInvokeState& GetThunderInvokeState() {return _thunderInvokeState;}

    TimeCastState& GetTimeCastState() {return _timeCastState;}
    TimeInvokeState& GetTimeInvokeState() {return _timeInvokeState;}
};

#endif
