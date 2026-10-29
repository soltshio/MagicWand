#include "CommonDeviceStates.h"
#include <Arduino.h>
#include "DeviceStateMachine.h"


//＝＝＝＝＝＝＝＝
//通常時のステート
//＝＝＝＝＝＝＝＝
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
  for(int i=0; i<_led12Ring->LEDNum();i++)
  {
    _led12Ring->SetColor(i,_ledColors[i],intensity);
  }
}

void NormalState::ChangeState(DeviceStateMachine& machine)
{
  //太陽魔法への遷移
  if(_commandManager->IsCommand(ECommandKey::StartSun))
  {
    machine.ChangeState(machine.GetSunCastState());
  }
  //星魔法への遷移
  else if(_commandManager->IsCommand(ECommandKey::StartStar))
  {
    machine.ChangeState(machine.GetStarCastState());
  }
  //雷魔法への遷移
  else if(_commandManager->IsCommand(ECommandKey::StartThunder))
  {
    machine.ChangeState(machine.GetThunderCastState());
  }
  //水魔法への遷移
  else if(_commandManager->IsCommand(ECommandKey::StartWater))
  {
    machine.ChangeState(machine.GetWaterCastState());
  }
  //時魔法への遷移
  else if(_commandManager->IsCommand(ECommandKey::StartTime))
  {
    machine.ChangeState(machine.GetTimeCastState());
  }
}



//＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
//LEDの色をフェードでだんだん変化させるだけのステート
//＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
LEDFadeState::LEDFadeState(LED12Ring &led12Ring,const Color* fromColors,const Color* toColors,float fromIntensity,float toIntensity,int fadeDuration_ms):
_ledColorFadeController(led12Ring,fromColors,toColors,fromIntensity,toIntensity,fadeDuration_ms)
{

}

void LEDFadeState::Enter(DeviceStateMachine& machine)
{
  _ledColorFadeController.Initialize();
}

void LEDFadeState::Update(DeviceStateMachine& machine)
{
  _ledColorFadeController.Tick();
}

void LEDFadeState::Exit(DeviceStateMachine& machine)
{

}
