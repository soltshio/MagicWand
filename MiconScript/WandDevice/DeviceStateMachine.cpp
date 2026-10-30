#include "DeviceStateMachine.h"

DeviceStateMachine::DeviceStateMachine(
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
):
//通常
_normalState(mpu,dfPlayerMini,led12Ring,commandManager,normalLEDProperty.ledColors,normalLEDProperty.lowLEDIntensity,normalLEDProperty.highLEDIntensity),
//太陽
_ledFadeFromNormalToSunState(led12Ring,normalLEDProperty.ledColors,sunLEDProperty.ledColors,normalLEDProperty.lowLEDIntensity,sunLEDProperty.lowLEDIntensity,sunLEDProperty.toFadeDuration_ms),
_sunCastState(dfPlayerMini,led12Ring,commandManager,sunLEDProperty.ledColors,sunLEDProperty.lowLEDIntensity,sunLEDProperty.highLEDIntensity),
_sunInvokeState(dfPlayerMini,led12Ring,commandManager,sunLEDProperty.ledColors,sunLEDProperty.highLEDIntensity),
//星
_ledFadeFromNormalToStarState(led12Ring,normalLEDProperty.ledColors,starLEDProperty.ledColors,normalLEDProperty.lowLEDIntensity,starLEDProperty.lowLEDIntensity,starLEDProperty.toFadeDuration_ms),
_starCastState(dfPlayerMini,led12Ring,commandManager,starLEDProperty.ledColors,starLEDProperty.lowLEDIntensity,starLEDProperty.highLEDIntensity),
_starInvokeState(dfPlayerMini,led12Ring,commandManager,starLEDProperty.ledColors,starLEDProperty.highLEDIntensity),
//雷
_ledFadeFromNormalToThunderState(led12Ring,normalLEDProperty.ledColors,thunderLEDProperty.ledColors,normalLEDProperty.lowLEDIntensity,thunderLEDProperty.lowLEDIntensity,thunderLEDProperty.toFadeDuration_ms),
_thunderCastState(dfPlayerMini,led12Ring,commandManager,thunderLEDProperty.ledColors,thunderLEDProperty.lowLEDIntensity,thunderLEDProperty.highLEDIntensity),
_thunderInvokeState(dfPlayerMini,led12Ring,commandManager,thunderLEDProperty.ledColors,thunderLEDProperty.highLEDIntensity),
//水
_ledFadeFromNormalToWaterState(led12Ring,normalLEDProperty.ledColors,waterLEDProperty.ledColors,normalLEDProperty.lowLEDIntensity,waterLEDProperty.lowLEDIntensity,waterLEDProperty.toFadeDuration_ms),
_waterCastState(dfPlayerMini,led12Ring,commandManager,waterLEDProperty.ledColors,waterLEDProperty.lowLEDIntensity,waterLEDProperty.highLEDIntensity),
_waterInvokeState(dfPlayerMini,led12Ring,commandManager,waterLEDProperty.ledColors,waterLEDProperty.highLEDIntensity),
//時
_ledFadeFromNormalToTimeState(led12Ring,normalLEDProperty.ledColors,timeLEDProperty.ledColors,normalLEDProperty.lowLEDIntensity,timeLEDProperty.lowLEDIntensity,timeLEDProperty.toFadeDuration_ms),
_timeCastState(dfPlayerMini,led12Ring,commandManager,timeLEDProperty.ledColors,timeLEDProperty.lowLEDIntensity,timeLEDProperty.highLEDIntensity),
_timeInvokeState(dfPlayerMini,led12Ring,commandManager,timeLEDProperty.ledColors,timeLEDProperty.highLEDIntensity) 
{

}

void DeviceStateMachine::Initialize() 
{
  //最初のステート
  ChangeState(_normalState);
}

void DeviceStateMachine::Update()
{
  if (_currentState == nullptr) return;

  _currentState->Update(*this);
}

void DeviceStateMachine::ChangeState(IDeviceState& nextState)
{
    if (_currentState == &nextState) return;

    if (_currentState != nullptr)
    {
      _currentState->Exit(*this);
    }

    _currentState = &nextState;

    _currentState->Enter(*this);
}