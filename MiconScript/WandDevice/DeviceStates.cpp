#include "DeviceStates.h"
#include <Arduino.h>
#include "DeviceStateMachine.h"

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
  for(int i=0; i<_led12Ring->LEDNum();i++)
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

  if(_isOveredAccelThreshold)
  {
    _dfPlayerMini->PlaySound(ESoundNum::Swing);
  }

  //前の加速度の更新
  _accelMagnitudePrev = _mpu->AccMagSq();

  //ステートの変更
  ChangeState(machine);
}

void NormalState::Exit(DeviceStateMachine& machine)
{
  //LEDを消しておく
  _led12Ring->Clear();

  //効果音を止めておく
  _dfPlayerMini->Stop();
}

void NormalState::ChangeState(DeviceStateMachine& machine)
{
  //太陽魔法への遷移
  if(_commandManager->IsChangeToSunState())
  {
    machine.ChangeState(machine.GetSunState());
  }
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
  //各種値の初期化
  _currentShiftLEDNum=0;
  _isStartedPlaying = false;
  _previous_ms=millis();

  //LEDを光らせる
  for(int i=0; i<_led12Ring->LEDNum() ;i++)
  {
    _led12Ring->SetColor(i,_ledColors[i],_ledIntensity);
  }
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

  //ステートの変更
}

void SunState::Exit(DeviceStateMachine& machine)
{
  //LEDを消しておく
  _led12Ring->Clear();

  //効果音を止めておく
  _dfPlayerMini->Stop();
}

//LEDの色が回転するように変わっていくようにする
void SunState::SetLEDColors(int elapsed)
{
  //何個分カラーをずらして光らせるかを算出
  int shiftLEDNum = elapsed / _switchLEDInterval_ms;
  shiftLEDNum %= _led12Ring->LEDNum();

  //その個数が違っていれば色を更新(同じであれば処理を無視)
  if(shiftLEDNum == _currentShiftLEDNum) return;

  for(int i=0; i<_led12Ring->LEDNum() ;i++)
  {
    int num = (i + shiftLEDNum)/_led12Ring->LEDNum();//どのLEDの色を変更するか

    _led12Ring->SetColor(num,_ledColors[i],_ledIntensity);
  }

  _currentShiftLEDNum=shiftLEDNum;
}

void SunState::PlaySound(int elapsed)
{
  if(_isStartedPlaying) return;//既に再生していたら無視
  if(!_commandManager->IsPlaySunSE()) return;//コマンドが来ていなければ無視

  _dfPlayerMini->PlaySound(ESoundNum::Sun);
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