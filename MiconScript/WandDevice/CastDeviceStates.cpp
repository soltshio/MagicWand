#include "CastDeviceStates.h"
#include <Arduino.h>
#include "DeviceStateMachine.h"
#include "Mathf.h"



//＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
//太陽魔法の詠唱(なぞり)ステート
//＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
SunCastState::SunCastState(
  DFPlayerMini &dfPlayerMini,
  LED12Ring &led12Ring,
  CommandManager &commandManager,
  const Color* ledColors,
  float minLEDIntensity,
  float maxLEDIntensity
):
_dfPlayerMini(&dfPlayerMini),
_led12Ring(&led12Ring),
_commandManager(&commandManager),
_ledColors(ledColors),
_minLEDIntensity(minLEDIntensity),
_maxLEDIntensity(maxLEDIntensity)
{

}

void SunCastState::Enter(DeviceStateMachine& machine)
{
  
}

void SunCastState::Update(DeviceStateMachine& machine)
{
  //詠唱が進むたびにLEDの光り方が変わって、効果音がなるようにする
  PlayEffectOnUpCastLevel();

  //発動ステートへの移行処理
  ChangeToInvokeState(machine);

  //初期化コマンドを受け取ったら初期化ステートに移行
  ChangeToInitializeState(machine);
}

void SunCastState::Exit(DeviceStateMachine& machine)
{
  //効果音を止めておく
  _dfPlayerMini->Stop();
}

//詠唱が進むたびにLEDの光り方が変わって、効果音がなるようにする
void SunCastState::PlayEffectOnUpCastLevel()
{
  //詠唱がどこまで進んでいるかを取得
  int castLevel = _commandManager->IsCommandAndLevel(ECommandKey::CastSun);

  if(castLevel == _commandManager->InvalidNum()) return;

  //効果音を鳴らす
  _dfPlayerMini->PlaySound(ESoundNum::Cast);

  //LEDの光り方を変える
  SetLED_CastLevel(castLevel);
}

//詠唱のレベルに合わせてLEDの光り方を変える
void SunCastState::SetLED_CastLevel(int castLevel)
{
  float intensity = Mathf::Remap((float)castLevel, 0,(float)_maxCastLevel,_minLEDIntensity,_maxLEDIntensity);

  for(int i=0; i<_led12Ring->LEDNum() ;i++)
  {
    _led12Ring->SetColor(i,_ledColors[i],intensity);
  }
}

void SunCastState::ChangeToInvokeState(DeviceStateMachine& machine)
{
  if(!_commandManager->IsCommand(ECommandKey::InvokeSun)) return;//コマンドが来ていなければ無視

  machine.ChangeState(machine.GetSunInvokeState());
}

void SunCastState::ChangeToInitializeState(DeviceStateMachine& machine)//初期化シーンへの遷移
{
  if(!_commandManager->IsCommand(ECommandKey::Initialize)) return;//初期化コマンドが来ていなければ無視
  
  machine.ChangeState(machine.GetInitializeState());
}





//＝＝＝＝＝＝＝＝＝＝＝＝＝＝
//星魔法の詠唱(なぞり)ステート
//＝＝＝＝＝＝＝＝＝＝＝＝＝＝
StarCastState::StarCastState(
  DFPlayerMini &dfPlayerMini,
  LED12Ring &led12Ring,
  CommandManager &commandManager,
  const Color* ledColors,
  float minLEDIntensity,
  float maxLEDIntensity
):
_dfPlayerMini(&dfPlayerMini),
_led12Ring(&led12Ring),
_commandManager(&commandManager),
_ledColors(ledColors),
_minLEDIntensity(minLEDIntensity),
_maxLEDIntensity(maxLEDIntensity)
{

}

void StarCastState::Enter(DeviceStateMachine& machine)
{

}

void StarCastState::Update(DeviceStateMachine& machine)
{
  //詠唱が進むたびにLEDの光り方が変わって、効果音がなるようにする
  PlayEffectOnUpCastLevel();

  //発動ステートへの移行処理
  ChangeToInvokeState(machine);

  //初期化コマンドを受け取ったら初期化ステートに移行
  ChangeToInitializeState(machine);
}

void StarCastState::Exit(DeviceStateMachine& machine)
{
  //効果音を止めておく
  _dfPlayerMini->Stop();
}

//詠唱が進むたびにLEDの光り方が変わって、効果音がなるようにする
void StarCastState::PlayEffectOnUpCastLevel()
{
  //詠唱がどこまで進んでいるかを取得
  int castLevel = _commandManager->IsCommandAndLevel(ECommandKey::CastStar);

  if(castLevel == _commandManager->InvalidNum()) return;

  //効果音を鳴らす
  _dfPlayerMini->PlaySound(ESoundNum::Cast);

  //LEDの光り方を変える
  SetLED_CastLevel(castLevel);
}

//詠唱のレベルに合わせてLEDの光り方を変える
void StarCastState::SetLED_CastLevel(int castLevel)
{
  float intensity = Mathf::Remap((float)castLevel, 0,(float)_maxCastLevel,_minLEDIntensity,_maxLEDIntensity);

  for(int i=0; i<_led12Ring->LEDNum() ;i++)
  {
    _led12Ring->SetColor(i,_ledColors[i],intensity);
  }
}

void StarCastState::ChangeToInvokeState(DeviceStateMachine& machine)
{
  if(!_commandManager->IsCommand(ECommandKey::InvokeStar)) return;//コマンドが来ていなければ無視

  machine.ChangeState(machine.GetStarInvokeState());
}

void StarCastState::ChangeToInitializeState(DeviceStateMachine& machine)//初期化シーンへの遷移
{
  if(!_commandManager->IsCommand(ECommandKey::Initialize)) return;//初期化コマンドが来ていなければ無視
  
  machine.ChangeState(machine.GetInitializeState());
}





//＝＝＝＝＝＝＝＝＝＝＝＝＝＝
//雷魔法の詠唱(なぞり)ステート
//＝＝＝＝＝＝＝＝＝＝＝＝＝＝
ThunderCastState::ThunderCastState(
  DFPlayerMini &dfPlayerMini,
  LED12Ring &led12Ring,
  CommandManager &commandManager,
  const Color* ledColors,
  float minLEDIntensity,
  float maxLEDIntensity
):
_dfPlayerMini(&dfPlayerMini),
_led12Ring(&led12Ring),
_commandManager(&commandManager),
_ledColors(ledColors),
_minLEDIntensity(minLEDIntensity),
_maxLEDIntensity(maxLEDIntensity)
{

}

void ThunderCastState::Enter(DeviceStateMachine& machine)
{

}

void ThunderCastState::Update(DeviceStateMachine& machine)
{
  //詠唱が進むたびにLEDの光り方が変わって、効果音がなるようにする
  PlayEffectOnUpCastLevel();

  //発動ステートへの移行処理
  ChangeToInvokeState(machine);

  //初期化コマンドを受け取ったら初期化ステートに移行
  ChangeToInitializeState(machine);
}

void ThunderCastState::Exit(DeviceStateMachine& machine)
{
  //効果音を止めておく
  _dfPlayerMini->Stop();
}

//詠唱が進むたびにLEDの光り方が変わって、効果音がなるようにする
void ThunderCastState::PlayEffectOnUpCastLevel()
{
  //詠唱がどこまで進んでいるかを取得
  int castLevel = _commandManager->IsCommandAndLevel(ECommandKey::CastThunder);

  if(castLevel == _commandManager->InvalidNum()) return;

  //効果音を鳴らす
  _dfPlayerMini->PlaySound(ESoundNum::Cast);

  //LEDの光り方を変える
  SetLED_CastLevel(castLevel);
}

//詠唱のレベルに合わせてLEDの光り方を変える
void ThunderCastState::SetLED_CastLevel(int castLevel)
{
  float intensity = Mathf::Remap((float)castLevel, 0,(float)_maxCastLevel,_minLEDIntensity,_maxLEDIntensity);

  for(int i=0; i<_led12Ring->LEDNum() ;i++)
  {
    _led12Ring->SetColor(i,_ledColors[i],intensity);
  }
}

void ThunderCastState::ChangeToInvokeState(DeviceStateMachine& machine)
{
  if(!_commandManager->IsCommand(ECommandKey::InvokeThunder)) return;//コマンドが来ていなければ無視

  machine.ChangeState(machine.GetThunderInvokeState());
}

void ThunderCastState::ChangeToInitializeState(DeviceStateMachine& machine)//初期化シーンへの遷移
{
  if(!_commandManager->IsCommand(ECommandKey::Initialize)) return;//初期化コマンドが来ていなければ無視
  
  machine.ChangeState(machine.GetInitializeState());
}





//＝＝＝＝＝＝＝＝＝＝＝＝＝＝
//水魔法の詠唱(なぞり)ステート
//＝＝＝＝＝＝＝＝＝＝＝＝＝＝
WaterCastState::WaterCastState(
  DFPlayerMini &dfPlayerMini,
  LED12Ring &led12Ring,
  CommandManager &commandManager,
  const Color* ledColors,
  float minLEDIntensity,
  float maxLEDIntensity
):
_dfPlayerMini(&dfPlayerMini),
_led12Ring(&led12Ring),
_commandManager(&commandManager),
_ledColors(ledColors),
_minLEDIntensity(minLEDIntensity),
_maxLEDIntensity(maxLEDIntensity)
{

}

void WaterCastState::Enter(DeviceStateMachine& machine)
{

}

void WaterCastState::Update(DeviceStateMachine& machine)
{
  //詠唱が進むたびにLEDの光り方が変わって、効果音がなるようにする
  PlayEffectOnUpCastLevel();

  //発動ステートへの移行処理
  ChangeToInvokeState(machine);

  //初期化コマンドを受け取ったら初期化ステートに移行
  ChangeToInitializeState(machine);
}

void WaterCastState::Exit(DeviceStateMachine& machine)
{
  //効果音を止めておく
  _dfPlayerMini->Stop();
}

//詠唱が進むたびにLEDの光り方が変わって、効果音がなるようにする
void WaterCastState::PlayEffectOnUpCastLevel()
{
  //詠唱がどこまで進んでいるかを取得
  int castLevel = _commandManager->IsCommandAndLevel(ECommandKey::CastWater);

  if(castLevel == _commandManager->InvalidNum()) return;

  //効果音を鳴らす
  _dfPlayerMini->PlaySound(ESoundNum::Cast);

  //LEDの光り方を変える
  SetLED_CastLevel(castLevel);
}

//詠唱のレベルに合わせてLEDの光り方を変える
void WaterCastState::SetLED_CastLevel(int castLevel)
{
  float intensity = Mathf::Remap((float)castLevel, 0,(float)_maxCastLevel,_minLEDIntensity,_maxLEDIntensity);

  for(int i=0; i<_led12Ring->LEDNum() ;i++)
  {
    _led12Ring->SetColor(i,_ledColors[i],intensity);
  }
}

void WaterCastState::ChangeToInvokeState(DeviceStateMachine& machine)
{
  if(!_commandManager->IsCommand(ECommandKey::InvokeWater)) return;//コマンドが来ていなければ無視

  machine.ChangeState(machine.GetWaterInvokeState());
}

void WaterCastState::ChangeToInitializeState(DeviceStateMachine& machine)//初期化シーンへの遷移
{
  if(!_commandManager->IsCommand(ECommandKey::Initialize)) return;//初期化コマンドが来ていなければ無視
  
  machine.ChangeState(machine.GetInitializeState());
}





//＝＝＝＝＝＝＝＝＝＝＝＝＝＝
//時魔法の詠唱(なぞり)ステート
//＝＝＝＝＝＝＝＝＝＝＝＝＝＝
TimeCastState::TimeCastState(
  DFPlayerMini &dfPlayerMini,
  LED12Ring &led12Ring,
  CommandManager &commandManager,
  const Color* ledColors,
  float minLEDIntensity,
  float maxLEDIntensity
):
_dfPlayerMini(&dfPlayerMini),
_led12Ring(&led12Ring),
_commandManager(&commandManager),
_ledColors(ledColors),
_minLEDIntensity(minLEDIntensity),
_maxLEDIntensity(maxLEDIntensity)
{

}

void TimeCastState::Enter(DeviceStateMachine& machine)
{
  
}

void TimeCastState::Update(DeviceStateMachine& machine)
{
  //詠唱が進むたびにLEDの光り方が変わって、効果音がなるようにする
  PlayEffectOnUpCastLevel();

  //発動ステートへの移行処理
  ChangeToInvokeState(machine);

  //初期化コマンドを受け取ったら初期化ステートに移行
  ChangeToInitializeState(machine);
}

void TimeCastState::Exit(DeviceStateMachine& machine)
{
  //効果音を止めておく
  _dfPlayerMini->Stop();
}

//詠唱が進むたびにLEDの光り方が変わって、効果音がなるようにする
void TimeCastState::PlayEffectOnUpCastLevel()
{
  //詠唱がどこまで進んでいるかを取得
  int castLevel = _commandManager->IsCommandAndLevel(ECommandKey::CastTime);

  if(castLevel == _commandManager->InvalidNum()) return;

  //効果音を鳴らす
  _dfPlayerMini->PlaySound(ESoundNum::Cast);

  //LEDの光り方を変える
  SetLED_CastLevel(castLevel);
}

//詠唱のレベルに合わせてLEDの光り方を変える
void TimeCastState::SetLED_CastLevel(int castLevel)
{
  float intensity = Mathf::Remap((float)castLevel, 0,(float)_maxCastLevel,_minLEDIntensity,_maxLEDIntensity);

  for(int i=0; i<_led12Ring->LEDNum() ;i++)
  {
    _led12Ring->SetColor(i,_ledColors[i],intensity);
  }
}

void TimeCastState::ChangeToInvokeState(DeviceStateMachine& machine)
{
  if(!_commandManager->IsCommand(ECommandKey::InvokeTime)) return;//コマンドが来ていなければ無視

  machine.ChangeState(machine.GetTimeInvokeState());
}

void TimeCastState::ChangeToInitializeState(DeviceStateMachine& machine)//初期化シーンへの遷移
{
  if(!_commandManager->IsCommand(ECommandKey::Initialize)) return;//初期化コマンドが来ていなければ無視
  
  machine.ChangeState(machine.GetInitializeState());
}