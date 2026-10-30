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
_sunCastState(dfPlayerMini,led12Ring,commandManager),
_sunInvokeState(dfPlayerMini,led12Ring,commandManager),
_starCastState(dfPlayerMini,led12Ring,commandManager),
_starInvokeState(dfPlayerMini,led12Ring,commandManager),
_waterCastState(dfPlayerMini,led12Ring,commandManager),
_waterInvokeState(dfPlayerMini,led12Ring,commandManager),
_thunderCastState(dfPlayerMini,led12Ring,commandManager),
_thunderInvokeState(dfPlayerMini,led12Ring,commandManager),
_timeCastState(dfPlayerMini,led12Ring,commandManager),
_timeInvokeState(dfPlayerMini,led12Ring,commandManager) 
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