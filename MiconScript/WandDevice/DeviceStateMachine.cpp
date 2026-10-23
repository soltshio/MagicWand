#include "DeviceStateMachine.h"

DeviceStateMachine::DeviceStateMachine(MPU_AccGyro &mpu,DFPlayerMini &dfPlayerMini,LED12Ring &led12Ring):
_normalState(mpu,dfPlayerMini,led12Ring),
_sunState(),
_starState(),
_waterState(),
_thunderState(),
_timeState() 
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