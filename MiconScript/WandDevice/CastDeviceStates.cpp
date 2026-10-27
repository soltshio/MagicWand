#include "CastDeviceStates.h"
#include <Arduino.h>
#include "DeviceStateMachine.h"



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
  //LEDを光らせる
  _led12Ring->Clear();
  for(int i=0; i<_led12Ring->LEDNum() ;i++)
  {
    _led12Ring->SetColor(i,_ledColors[i],_ledIntensity);
  }
}

void SunCastState::Update(DeviceStateMachine& machine)
{
  //ステートの変更(通常ステートに戻る)
  ChangeToNormalState(machine);
}

void SunCastState::Exit(DeviceStateMachine& machine)
{
  //LEDを消しておく
  _led12Ring->Clear();

  //効果音を止めておく
  _dfPlayerMini->Stop();
}

void SunCastState::ChangeToNormalState(DeviceStateMachine& machine)
{
  if(!_commandManager->IsCommand(ECommandKey::InvokeSun)) return;//コマンドが来ていなければ無視

  machine.ChangeState(machine.GetNormalState());
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

}

void StarCastState::Exit(DeviceStateMachine& machine)
{

}

void StarCastState::ChangeToNormalState(DeviceStateMachine& machine)
{
  if(!_commandManager->IsCommand(ECommandKey::FinishStar)) return;//コマンドが来ていなければ無視

  machine.ChangeState(machine.GetNormalState());
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

}

void ThunderCastState::Exit(DeviceStateMachine& machine)
{

}

void ThunderCastState::ChangeToNormalState(DeviceStateMachine& machine)
{
  if(!_commandManager->IsCommand(ECommandKey::FinishThunder)) return;//コマンドが来ていなければ無視

  machine.ChangeState(machine.GetNormalState());
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

}

void WaterCastState::Exit(DeviceStateMachine& machine)
{

}

void WaterCastState::ChangeToNormalState(DeviceStateMachine& machine)
{
  if(!_commandManager->IsCommand(ECommandKey::FinishWater)) return;//コマンドが来ていなければ無視

  machine.ChangeState(machine.GetNormalState());
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
  
}

void TimeCastState::Exit(DeviceStateMachine& machine)
{

}

void TimeCastState::ChangeToNormalState(DeviceStateMachine& machine)
{
  if(!_commandManager->IsCommand(ECommandKey::FinishTime)) return;//コマンドが来ていなければ無視

  machine.ChangeState(machine.GetNormalState());
}