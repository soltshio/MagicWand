#include "DeviceStates.h"
#include <Arduino.h>

//---通常時のステート---

NormalState::NormalState(MPU_AccGyro &mpu,DFPlayerMini &dfPlayerMini,LED12Ring &led12Ring,CommandManager &commandManager):
_mpu(&mpu),
_dfPlayerMini(&dfPlayerMini),
_led12Ring(&led12Ring),
_commandManager(&commandManager)
{

}

void NormalState::Enter(DeviceStateMachine& machine)
{
  //LEDを虹色に光らせる
  _led12Ring->Clear();
  for(int i=0; i<12;i++)
  {
    _led12Ring->SetColor(i,_ledColors[i],_ledIntensity);
  }
  _led12Ring->Show();

  //前の加速度の初期化
  _accelMagnitudePrev = _mpu->AccMagSq();
}

void NormalState::Update(DeviceStateMachine& machine)
{
  //一定の加速度を超えた瞬間にのみ効果音を流す
  bool _isOveredAccelThreshold = (_mpu->AccMagSq() > _accMagnitudeThresholdToPlaySound) && (_accelMagnitudePrev > _accMagnitudeThresholdToPlaySound);

  //前の加速度の更新
  _accelMagnitudePrev = _mpu->AccMagSq();

  if(!_isOveredAccelThreshold) return;

  _dfPlayerMini->PlaySound(ESoundNum::Swing);
}

void NormalState::Exit(DeviceStateMachine& machine)
{
  //LEDを消しておく
  _led12Ring->Clear();

  //効果音を止めておく
  _dfPlayerMini->Stop();
}



//---太陽魔法のステート---

SunState::SunState(DFPlayerMini &dfPlayerMini,LED12Ring &led12Ring,CommandManager &commandManager):
_dfPlayerMini(&dfPlayerMini),
_led12Ring(&led12Ring),
_commandManager(&commandManager)
{

}

void SunState::Enter(DeviceStateMachine& machine)
{
  _currentLEDCycleZeroNum=0;
  _isStartedPlaying = false;

  _previous_ms=millis();
}

void SunState::Update(DeviceStateMachine& machine)
{
  //経過時間の算出
  unsigned long now = millis();
  int elapsed= now - _previous_ms;

  //効果音の再生
  PlaySound(elapsed);

  //LEDの色の書き換え
  SetLEDColors(elapsed);
}

void SunState::Exit(DeviceStateMachine& machine)
{
  //LEDを消しておく
  _led12Ring->Clear();

  //効果音を止めておく
  _dfPlayerMini->Stop();
}

void SunState::SetLEDColors(int elapsed)
{

}

void SunState::PlaySound(int elapsed)
{
  if(_isStartedPlaying) return;//既に再生していたら無視
  
}


//---星魔法のステート---

StarState::StarState(DFPlayerMini &dfPlayerMini,LED12Ring &led12Ring,CommandManager &commandManager):
_dfPlayerMini(&dfPlayerMini),
_led12Ring(&led12Ring),
_commandManager(&commandManager)
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

ThunderState::ThunderState(DFPlayerMini &dfPlayerMini,LED12Ring &led12Ring,CommandManager &commandManager):
_dfPlayerMini(&dfPlayerMini),
_led12Ring(&led12Ring),
_commandManager(&commandManager)
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

WaterState::WaterState(DFPlayerMini &dfPlayerMini,LED12Ring &led12Ring,CommandManager &commandManager):
_dfPlayerMini(&dfPlayerMini),
_led12Ring(&led12Ring),
_commandManager(&commandManager)
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

TimeState::TimeState(DFPlayerMini &dfPlayerMini,LED12Ring &led12Ring,CommandManager &commandManager):
_dfPlayerMini(&dfPlayerMini),
_led12Ring(&led12Ring),
_commandManager(&commandManager)
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