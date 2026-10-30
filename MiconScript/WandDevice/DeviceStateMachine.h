#ifndef _DEVICESTATEMACHINE_H_
#define _DEVICESTATEMACHINE_H_

#include "CommonDeviceStates.h"
#include "InvokeDeviceStates.h"
#include "CastDeviceStates.h"
#include "Color.h"
#include "LED12Ring.h"
#include "StateLEDProperties.h"

class DeviceStateMachine {
private:
    NormalState _normalState;//通常時のステート

    LEDFadeState _ledFadeFromNormalToSunState;//通常時ー＞太陽ステートに移る時にLEDをフェードさせるステート
    SunCastState _sunCastState;//太陽詠唱ステート
    SunInvokeState _sunInvokeState;//太陽発動ステート

    LEDFadeState _ledFadeFromNormalToStarState;//通常時ー＞星ステートに移る時にLEDをフェードさせるステート
    StarCastState _starCastState;//星詠唱ステート
    StarInvokeState _starInvokeState;//星発動ステート

    LEDFadeState _ledFadeFromNormalToThunderState;//通常時ー＞雷ステートに移る時にLEDをフェードさせるステート
    ThunderCastState _thunderCastState;//雷詠唱ステート
    ThunderInvokeState _thunderInvokeState;//雷発動ステート

    LEDFadeState _ledFadeFromNormalToWaterState;//通常時ー＞水ステートに移る時にLEDをフェードさせるステート
    WaterCastState _waterCastState;//水詠唱ステート
    WaterInvokeState _waterInvokeState;//水発動ステート

    LEDFadeState _ledFadeFromNormalToTimeState;//通常時ー＞時ステートに移る時にLEDをフェードさせるステート
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

    LEDFadeState GetLEDFadeFromNormalToSunState() {return _ledFadeFromNormalToSunState;}
    SunCastState& GetSunCastState() {return _sunCastState;}
    SunInvokeState& GetSunInvokeState() {return _sunInvokeState;}

    LEDFadeState GetLEDFadeFromNormalToStarState() {return _ledFadeFromNormalToStarState;}
    StarCastState& GetStarCastState() {return _starCastState;}
    StarInvokeState& GetStarInvokeState() {return _starInvokeState;}

    LEDFadeState GetLEDFadeFromNormalToThunderState() {return _ledFadeFromNormalToThunderState;}
    ThunderCastState& GetThunderCastState() {return _thunderCastState;}
    ThunderInvokeState& GetThunderInvokeState() {return _thunderInvokeState;}

    LEDFadeState GetLEDFadeFromNormalToWaterState() {return _ledFadeFromNormalToWaterState;}
    WaterCastState& GetWaterCastState() {return _waterCastState;}
    WaterInvokeState& GetWaterInvokeState() {return _waterInvokeState;}

    LEDFadeState GetLEDFadeFromNormalToTimeState() {return _ledFadeFromNormalToTimeState;}
    TimeCastState& GetTimeCastState() {return _timeCastState;}
    TimeInvokeState& GetTimeInvokeState() {return _timeInvokeState;}
};

#endif
