#include "CastDeviceStates.h"
#include <Arduino.h>
#include "DeviceStateMachine.h"
#include "Mathf.h"



//＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
//太陽魔法の詠唱(なぞり)ステート
//＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
SunCastState::SunCastState(DFPlayerMini &dfPlayerMini,LED12Ring &led12Ring,CommandManager &commandManager):
_dfPlayerMini(&dfPlayerMini),
_led12Ring(&led12Ring),
_commandManager(&commandManager)
{

}

void SunCastState::Enter(DeviceStateMachine& machine)
{
  
}

void SunCastState::Update(DeviceStateMachine& machine)
{
  //詠唱が進むたびにLEDの光り方が変わって、効果音がなるようにする
  PlayEffectOnUpCastLevel();

  //ステートの変更処理
  ChangeToInvokeState(machine);
}

void SunCastState::Exit(DeviceStateMachine& machine)
{
  //LEDを消しておく
  _led12Ring->Clear();

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
  //_dfPlayerMini->PlaySound(ESoundNum::Cast);

  //LEDの光り方を変える
  SetLED_CastLevel(castLevel);
}

//詠唱のレベルに合わせてLEDの光り方を変える
void SunCastState::SetLED_CastLevel(int castLevel)
{
  float intensity = Mathf::Remap((float)castLevel, 0,(float)_maxCastLevel,_minLedIntensity,_maxLedIntensity);

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





//＝＝＝＝＝＝＝＝＝＝＝＝＝＝
//星魔法の詠唱(なぞり)ステート
//＝＝＝＝＝＝＝＝＝＝＝＝＝＝
StarCastState::StarCastState(DFPlayerMini &dfPlayerMini,LED12Ring &led12Ring,CommandManager &commandManager):
_dfPlayerMini(&dfPlayerMini),
_led12Ring(&led12Ring),
_commandManager(&commandManager)
{

}

void StarCastState::Enter(DeviceStateMachine& machine)
{

}

void StarCastState::Update(DeviceStateMachine& machine)
{
  //ステートの変更処理
  ChangeToInvokeState(machine);
}

void StarCastState::Exit(DeviceStateMachine& machine)
{

}

void StarCastState::ChangeToInvokeState(DeviceStateMachine& machine)
{
  if(!_commandManager->IsCommand(ECommandKey::InvokeStar)) return;//コマンドが来ていなければ無視

  machine.ChangeState(machine.GetStarInvokeState());
}





//＝＝＝＝＝＝＝＝＝＝＝＝＝＝
//雷魔法の詠唱(なぞり)ステート
//＝＝＝＝＝＝＝＝＝＝＝＝＝＝
ThunderCastState::ThunderCastState(DFPlayerMini &dfPlayerMini,LED12Ring &led12Ring,CommandManager &commandManager):
_dfPlayerMini(&dfPlayerMini),
_led12Ring(&led12Ring),
_commandManager(&commandManager)
{

}

void ThunderCastState::Enter(DeviceStateMachine& machine)
{

}

void ThunderCastState::Update(DeviceStateMachine& machine)
{
  //ステートの変更処理
  ChangeToInvokeState(machine);
}

void ThunderCastState::Exit(DeviceStateMachine& machine)
{

}

void ThunderCastState::ChangeToInvokeState(DeviceStateMachine& machine)
{
  if(!_commandManager->IsCommand(ECommandKey::InvokeThunder)) return;//コマンドが来ていなければ無視

  machine.ChangeState(machine.GetThunderInvokeState());
}





//＝＝＝＝＝＝＝＝＝＝＝＝＝＝
//水魔法の詠唱(なぞり)ステート
//＝＝＝＝＝＝＝＝＝＝＝＝＝＝
WaterCastState::WaterCastState(DFPlayerMini &dfPlayerMini,LED12Ring &led12Ring,CommandManager &commandManager):
_dfPlayerMini(&dfPlayerMini),
_led12Ring(&led12Ring),
_commandManager(&commandManager)
{

}

void WaterCastState::Enter(DeviceStateMachine& machine)
{

}

void WaterCastState::Update(DeviceStateMachine& machine)
{
  //ステートの変更処理
  ChangeToInvokeState(machine);
}

void WaterCastState::Exit(DeviceStateMachine& machine)
{

}

void WaterCastState::ChangeToInvokeState(DeviceStateMachine& machine)
{
  if(!_commandManager->IsCommand(ECommandKey::InvokeWater)) return;//コマンドが来ていなければ無視

  machine.ChangeState(machine.GetWaterInvokeState());
}



//＝＝＝＝＝＝＝＝＝＝＝＝＝＝
//時魔法の詠唱(なぞり)ステート
//＝＝＝＝＝＝＝＝＝＝＝＝＝＝
TimeCastState::TimeCastState(DFPlayerMini &dfPlayerMini,LED12Ring &led12Ring,CommandManager &commandManager):
_dfPlayerMini(&dfPlayerMini),
_led12Ring(&led12Ring),
_commandManager(&commandManager)
{

}

void TimeCastState::Enter(DeviceStateMachine& machine)
{
  
}

void TimeCastState::Update(DeviceStateMachine& machine)
{
  //ステートの変更処理
  ChangeToInvokeState(machine);
}

void TimeCastState::Exit(DeviceStateMachine& machine)
{

}

void TimeCastState::ChangeToInvokeState(DeviceStateMachine& machine)
{
  if(!_commandManager->IsCommand(ECommandKey::InvokeTime)) return;//コマンドが来ていなければ無視

  machine.ChangeState(machine.GetTimeInvokeState());
}