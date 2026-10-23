#include "DeviceStates.h"

//---通常時のステート---

NormalState::NormalState(MPU_AccGyro &mpu,DFPlayerMini &dfPlayerMini,LED12Ring &led12Ring):
_mpu(&mpu),
_dfPlayerMini(&dfPlayerMini),
_led12Ring(&led12Ring)
{

}

void NormalState::Enter(DeviceStateMachine& machine)
{

}

void NormalState::Update(DeviceStateMachine& machine)
{

}

void NormalState::Exit(DeviceStateMachine& machine)
{

}



//---太陽魔法のステート---

SunState::SunState()
{

}

void SunState::Enter(DeviceStateMachine& machine)
{

}

void SunState::Update(DeviceStateMachine& machine)
{

}

void SunState::Exit(DeviceStateMachine& machine)
{

}



//---星魔法のステート---

StarState::StarState()
{

}

void StarState::Enter(DeviceStateMachine& machine)
{

}

void StarState::Update(DeviceStateMachine& machine)
{

}

void StarState::Exit(DeviceStateMachine& machine)
{

}



//---雷魔法のステート---

ThunderState::ThunderState()
{

}

void ThunderState::Enter(DeviceStateMachine& machine)
{

}

void ThunderState::Update(DeviceStateMachine& machine)
{

}

void ThunderState::Exit(DeviceStateMachine& machine)
{

}



//---水魔法のステート---

WaterState::WaterState()
{

}

void WaterState::Enter(DeviceStateMachine& machine)
{

}

void WaterState::Update(DeviceStateMachine& machine)
{

}

void WaterState::Exit(DeviceStateMachine& machine)
{

}



//---時魔法のステート---

TimeState::TimeState()
{

}

void TimeState::Enter(DeviceStateMachine& machine)
{

}

void TimeState::Update(DeviceStateMachine& machine)
{

}

void TimeState::Exit(DeviceStateMachine& machine)
{

}