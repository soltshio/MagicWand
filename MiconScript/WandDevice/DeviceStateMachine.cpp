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
_normalState(mpu,dfPlayerMini,led12Ring,commandManager),
_sunCastState(dfPlayerMini,led12Ring,commandManager,sunLEDProperty.ledColors,sunLEDProperty.lowLEDIntensity,sunLEDProperty.highLEDIntensity),
_sunInvokeState(dfPlayerMini,led12Ring,commandManager,sunLEDProperty.ledColors,sunLEDProperty.highLEDIntensity),
_starCastState(dfPlayerMini,led12Ring,commandManager,starLEDProperty.ledColors,starLEDProperty.lowLEDIntensity,starLEDProperty.highLEDIntensity),
_starInvokeState(dfPlayerMini,led12Ring,commandManager,starLEDProperty.ledColors,starLEDProperty.highLEDIntensity),
_thunderCastState(dfPlayerMini,led12Ring,commandManager,thunderLEDProperty.ledColors,thunderLEDProperty.lowLEDIntensity,thunderLEDProperty.highLEDIntensity),
_thunderInvokeState(dfPlayerMini,led12Ring,commandManager,thunderLEDProperty.ledColors,thunderLEDProperty.highLEDIntensity),
_waterCastState(dfPlayerMini,led12Ring,commandManager,waterLEDProperty.ledColors,waterLEDProperty.lowLEDIntensity,waterLEDProperty.highLEDIntensity),
_waterInvokeState(dfPlayerMini,led12Ring,commandManager,waterLEDProperty.ledColors,waterLEDProperty.highLEDIntensity),
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