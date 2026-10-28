#include "InvokeDeviceStates.h"
#include <Arduino.h>
#include "DeviceStateMachine.h"



//＝＝＝＝＝＝＝＝＝
//太陽魔法の発動ステート
//＝＝＝＝＝＝＝＝＝
SunInvokeState::SunInvokeState(DFPlayerMini &dfPlayerMini,LED12Ring &led12Ring,CommandManager &commandManager):
_dfPlayerMini(&dfPlayerMini),
_led12Ring(&led12Ring),
_commandManager(&commandManager)
{

}

void SunInvokeState::Enter(DeviceStateMachine& machine)
{
  //各種値の初期化
  _currentShiftLEDNum=0;
  _previous_ms=millis();

  //LEDを光らせる
  for(int i=0; i<_led12Ring->LEDNum() ;i++)
  {
    _led12Ring->SetColor(i,_ledColors[i],_ledIntensity);
  }
}

void SunInvokeState::Update(DeviceStateMachine& machine)
{
  //経過時間の算出
  unsigned long now = millis();
  int elapsed= now - _previous_ms;

  //効果音の再生
  //PlaySound();

  //LEDの色の書き換え
  SetLEDColors(elapsed);

  //ステートの変更(通常ステートに戻る)
  ChangeToNormalState(machine);
}

void SunInvokeState::Exit(DeviceStateMachine& machine)
{
  //効果音を止めておく
  _dfPlayerMini->Stop();
}

//LEDの色が回転するように変わっていくようにする
void SunInvokeState::SetLEDColors(int elapsed)
{
  //何個分カラーをずらして光らせるかを算出
  int shiftLEDNum = elapsed / _switchLEDInterval_ms;
  shiftLEDNum %= _led12Ring->LEDNum();

  //その個数が違っていれば色を更新(同じであれば処理を無視)
  if(shiftLEDNum == _currentShiftLEDNum) return;

  for(int i=0; i<_led12Ring->LEDNum() ;i++)
  {
    int num = (i + shiftLEDNum)%_led12Ring->LEDNum();//どのLEDの色を変更するか

    _led12Ring->SetColor(num,_ledColors[i],_ledIntensity);
  }

  _currentShiftLEDNum=shiftLEDNum;
}

void SunInvokeState::PlaySound()
{
  if(!_commandManager->IsCommand(ECommandKey::InvokeSun_PlaySound)) return;//コマンドが来ていなければ無視

  _dfPlayerMini->PlaySound(ESoundNum::Sun);
}

void SunInvokeState::ChangeToNormalState(DeviceStateMachine& machine)
{
  if(!_commandManager->IsCommand(ECommandKey::FinishSun)) return;//コマンドが来ていなければ無視

  machine.ChangeState(machine.GetNormalState());
}





//＝＝＝＝＝＝＝＝＝＝
//星魔法の発動ステート
//＝＝＝＝＝＝＝＝＝＝
StarInvokeState::StarInvokeState(DFPlayerMini &dfPlayerMini,LED12Ring &led12Ring,CommandManager &commandManager):
_dfPlayerMini(&dfPlayerMini),
_led12Ring(&led12Ring),
_commandManager(&commandManager)
{

}

void StarInvokeState::Enter(DeviceStateMachine& machine)
{

}

void StarInvokeState::Update(DeviceStateMachine& machine)
{
  //効果音の再生
  //PlaySound();

  //ステートの変更(通常ステートに戻る)
  ChangeToNormalState(machine);
}

void StarInvokeState::Exit(DeviceStateMachine& machine)
{

}

void StarInvokeState::PlaySound()
{
  if(!_commandManager->IsCommand(ECommandKey::InvokeStar_PlaySound)) return;//コマンドが来ていなければ無視

  _dfPlayerMini->PlaySound(ESoundNum::Star);
}

void StarInvokeState::ChangeToNormalState(DeviceStateMachine& machine)
{
  if(!_commandManager->IsCommand(ECommandKey::FinishStar)) return;//コマンドが来ていなければ無視

  machine.ChangeState(machine.GetNormalState());
}





//＝＝＝＝＝＝＝＝＝＝
//雷魔法の発動ステート
//＝＝＝＝＝＝＝＝＝＝
ThunderInvokeState::ThunderInvokeState(DFPlayerMini &dfPlayerMini,LED12Ring &led12Ring,CommandManager &commandManager):
_dfPlayerMini(&dfPlayerMini),
_led12Ring(&led12Ring),
_commandManager(&commandManager)
{

}

void ThunderInvokeState::Enter(DeviceStateMachine& machine)
{

}

void ThunderInvokeState::Update(DeviceStateMachine& machine)
{
  //効果音の再生
  //PlaySound();

  //ステートの変更(通常ステートに戻る)
  ChangeToNormalState(machine);
}

void ThunderInvokeState::Exit(DeviceStateMachine& machine)
{

}

void ThunderInvokeState::PlaySound()
{
  if(!_commandManager->IsCommand(ECommandKey::InvokeThunder_PlaySound)) return;//コマンドが来ていなければ無視

  _dfPlayerMini->PlaySound(ESoundNum::Thunder);
}

void ThunderInvokeState::ChangeToNormalState(DeviceStateMachine& machine)
{
  if(!_commandManager->IsCommand(ECommandKey::FinishThunder)) return;//コマンドが来ていなければ無視

  machine.ChangeState(machine.GetNormalState());
}





//＝＝＝＝＝＝＝＝＝＝
//水魔法の発動ステート
//＝＝＝＝＝＝＝＝＝＝
WaterInvokeState::WaterInvokeState(DFPlayerMini &dfPlayerMini,LED12Ring &led12Ring,CommandManager &commandManager):
_dfPlayerMini(&dfPlayerMini),
_led12Ring(&led12Ring),
_commandManager(&commandManager)
{

}

void WaterInvokeState::Enter(DeviceStateMachine& machine)
{

}

void WaterInvokeState::Update(DeviceStateMachine& machine)
{
  //効果音の再生
  //PlaySound();

  //ステートの変更(通常ステートに戻る)
  ChangeToNormalState(machine);
}

void WaterInvokeState::Exit(DeviceStateMachine& machine)
{

}

void WaterInvokeState::PlaySound()
{
  if(!_commandManager->IsCommand(ECommandKey::InvokeWater_PlaySound)) return;//コマンドが来ていなければ無視

  _dfPlayerMini->PlaySound(ESoundNum::Water);
}

void WaterInvokeState::ChangeToNormalState(DeviceStateMachine& machine)
{
  if(!_commandManager->IsCommand(ECommandKey::FinishWater)) return;//コマンドが来ていなければ無視

  machine.ChangeState(machine.GetNormalState());
}



//＝＝＝＝＝＝＝＝＝＝
//時魔法の発動ステート
//＝＝＝＝＝＝＝＝＝＝
TimeInvokeState::TimeInvokeState(DFPlayerMini &dfPlayerMini,LED12Ring &led12Ring,CommandManager &commandManager):
_dfPlayerMini(&dfPlayerMini),
_led12Ring(&led12Ring),
_commandManager(&commandManager)
{

}

void TimeInvokeState::Enter(DeviceStateMachine& machine)
{
  
}

void TimeInvokeState::Update(DeviceStateMachine& machine)
{
  //効果音の再生
  //PlaySound();

  //ステートの変更(通常ステートに戻る)
  ChangeToNormalState(machine);
}

void TimeInvokeState::Exit(DeviceStateMachine& machine)
{

}

void TimeInvokeState::PlaySound()
{
  if(!_commandManager->IsCommand(ECommandKey::InvokeTime_PlaySound)) return;//コマンドが来ていなければ無視

  _dfPlayerMini->PlaySound(ESoundNum::Time);
}

void TimeInvokeState::ChangeToNormalState(DeviceStateMachine& machine)
{
  if(!_commandManager->IsCommand(ECommandKey::FinishTime)) return;//コマンドが来ていなければ無視

  machine.ChangeState(machine.GetNormalState());
}