#include "DeviceStateMachine.h"

DeviceStateMachine::DeviceStateMachine(MPU_AccGyro &mpu,DFPlayerMini &dfPlayerMini,LED12Ring &led12Ring):
_normalState(mpu,dfPlayerMini,led12Ring),
_sunState(dfPlayerMini,led12Ring),
_starState(dfPlayerMini,led12Ring),
_waterState(dfPlayerMini,led12Ring),
_thunderState(dfPlayerMini,led12Ring),
_timeState(dfPlayerMini,led12Ring) 
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