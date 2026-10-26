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
  SetLEDRainbowColors(_ledIntensity_Normal);
  //前の加速度の初期化
  _accelMagnitudePrev = _mpu->AccMagSq();
}

void NormalState::Update(DeviceStateMachine& machine)
{
  bool _isOveredAccelThreshold = (_mpu->AccMagSq() > _accMagnitudeThresholdToPlaySound) && (_accelMagnitudePrev < _accMagnitudeThresholdToPlaySound);//加速度が閾値を超えたか
  
  //前の加速度の更新
  _accelMagnitudePrev = _mpu->AccMagSq();

  //加速度が閾値を超えて、なおかつクールタイム中でなければ、効果音を鳴らす
  if(_isOveredAccelThreshold && !_isPlaySoundCoolTime)
  {
    _dfPlayerMini->PlaySound(ESoundNum::Thunder);//仮で雷の音を鳴らすようにする

    //クールタイム状態にする
    _isPlaySoundCoolTime = true;
    _previousPlaySoundTime = millis();
  }

  //クールタイムの更新
  UpdateCoolTime();

  //効果音が鳴っているかによってLEDの光り方を変える
  SetLEDLighting_SoundPlaying();

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

void NormalState::UpdateCoolTime()
{
  if(!_isPlaySoundCoolTime) return;

  unsigned long now = millis();
  int elapsed_ms = now - _previousPlaySoundTime;

  //最後に効果音を鳴らしてから一定時間が経ったらクールタイム状態をオフにする
  if(elapsed_ms < _playSoundCoolTimeDuration_ms) return;

  _isPlaySoundCoolTime = false;
}

//効果音が鳴っているかによってLEDの光り方を変える
void NormalState::SetLEDLighting_SoundPlaying()
{
  //効果音が鳴り始めたら光らせ方を変える
  if(_dfPlayerMini->IsStartPlaying())
  {
    SetLEDRainbowColors(_ledIntensity_SoundPlaying);
    return;
  }

  //効果音が鳴り終わったら元の光り方に戻しておく
  if(_dfPlayerMini->IsFinishPlaying())
  {
    SetLEDRainbowColors(_ledIntensity_Normal);
    return;
  }
}

//LEDを虹色に光らせる(intensityで光量を変更)
void NormalState::SetLEDRainbowColors(float intensity)
{
  _led12Ring->Clear();
  for(int i=0; i<_led12Ring->LEDNum();i++)
  {
    _led12Ring->SetColor(i,_ledColors[i],intensity);
  }
}

void NormalState::ChangeState(DeviceStateMachine& machine)
{
  //太陽魔法への遷移
  if(_commandManager->IsCommand(ECommandKey::ChangeToSunState))
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
  _led12Ring->Clear();
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

  //ステートの変更(通常ステートに戻る)
  ChangeToNormalState(machine);
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

  _led12Ring->Clear();
  for(int i=0; i<_led12Ring->LEDNum() ;i++)
  {
    int num = (i + shiftLEDNum)%_led12Ring->LEDNum();//どのLEDの色を変更するか

    _led12Ring->SetColor(num,_ledColors[i],_ledIntensity);
  }

  _currentShiftLEDNum=shiftLEDNum;
}

void SunState::PlaySound(int elapsed)
{
  if(_isStartedPlaying) return;//既に再生していたら無視
  if(!_commandManager->IsCommand(ECommandKey::PlaySunSE)) return;//コマンドが来ていなければ無視

  _dfPlayerMini->PlaySound(ESoundNum::Sun);
}

void SunState::ChangeToNormalState(DeviceStateMachine& machine)
{
  if(!_commandManager->IsCommand(ECommandKey::FinishSunState)) return;//コマンドが来ていなければ無視

  machine.ChangeState(machine.GetNormalState());
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

void StarState::ChangeToNormalState(DeviceStateMachine& machine)
{
  if(!_commandManager->IsCommand(ECommandKey::FinishStarState)) return;//コマンドが来ていなければ無視

  machine.ChangeState(machine.GetNormalState());
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

void ThunderState::ChangeToNormalState(DeviceStateMachine& machine)
{
  if(!_commandManager->IsCommand(ECommandKey::FinishThunderState)) return;//コマンドが来ていなければ無視

  machine.ChangeState(machine.GetNormalState());
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

void WaterState::ChangeToNormalState(DeviceStateMachine& machine)
{
  if(!_commandManager->IsCommand(ECommandKey::FinishWaterState)) return;//コマンドが来ていなければ無視

  machine.ChangeState(machine.GetNormalState());
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

void TimeState::ChangeToNormalState(DeviceStateMachine& machine)
{
  if(!_commandManager->IsCommand(ECommandKey::FinishTimeState)) return;//コマンドが来ていなければ無視

  machine.ChangeState(machine.GetNormalState());
}