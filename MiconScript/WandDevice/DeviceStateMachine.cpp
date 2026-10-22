#include "DeviceStateMachine.h"

DeviceStateMachine::DeviceStateMachine():
_normalState(),
_sunState(),
_starState(),
_waterState(),
_thunderState(),
_timeState() 
{

}

void DeviceStateMachine::Initialize() 
{
    
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