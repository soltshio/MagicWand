#ifndef _INVOKEDEVICESTATES_H_
#define _INVOKEDEVICESTATES_H_

#include "MPU_AccGyro.h"
#include "DFPlayerMini.h"
#include "LED12Ring.h"
#include "CommandManager.h"
#include "CommonDeviceStates.h"

class DeviceStateMachine;


//＝＝＝＝＝＝＝＝＝＝＝
//太陽魔法の発動ステート
//＝＝＝＝＝＝＝＝＝＝＝
class SunInvokeState : public IDeviceState
{
  //LED関係
  const float _ledIntensity = 0.3;//LEDの光量(0～1)
  
  const Color _ledColors[12] = {//ライトの色
  {255,  0,  0},//赤
  {255, 53,  5},
  {255,106, 10},
  {255,160, 16},//オレンジ
  {255,181, 21},
  {255,202, 26},
  {255,224, 32},//黄
  {255,202, 26},
  {255,181, 21},
  {255,160, 16},//オレンジ
  {255,106, 10},
  {255, 53,  5},//赤の一歩手前の色
  };

  const int _switchLEDInterval_ms = 100;//LEDのライトの光り方を切り替える(1つずらす)インターバル(ms)
  int _currentShiftLEDNum;

  //その他
  unsigned long _previous_ms=0;

  //スピーカープレイヤー
  DFPlayerMini* _dfPlayerMini;
  //LED
  LED12Ring* _led12Ring;
  //コマンドの管理クラス
  CommandManager* _commandManager;

  void SetLEDColors(int elapsed);//LEDの色の書き換え
  void PlaySound(int elapsed);//効果音の再生
  void ChangeToNormalState(DeviceStateMachine& machine);//通常ステートへの遷移
public:
  SunInvokeState(DFPlayerMini &dfPlayerMini,LED12Ring &led12Ring,CommandManager &commandManager);

  void Enter(DeviceStateMachine& machine) override;
  void Update(DeviceStateMachine& machine) override;
  void Exit(DeviceStateMachine& machine) override;
};





//＝＝＝＝＝＝＝＝＝＝
//星魔法の発動ステート
//＝＝＝＝＝＝＝＝＝＝
class StarInvokeState : public IDeviceState
{
  //スピーカープレイヤー
  DFPlayerMini* _dfPlayerMini;
  //LED
  LED12Ring* _led12Ring;
  //コマンドの管理クラス
  CommandManager* _commandManager;

  void ChangeToNormalState(DeviceStateMachine& machine);//通常ステートへの遷移
public:
  StarInvokeState(DFPlayerMini &dfPlayerMini,LED12Ring &led12Ring,CommandManager &commandManager);

  void Enter(DeviceStateMachine& machine) override;
  void Update(DeviceStateMachine& machine) override;
  void Exit(DeviceStateMachine& machine) override;
};





//＝＝＝＝＝＝＝＝＝＝
//雷魔法の発動ステート
//＝＝＝＝＝＝＝＝＝＝
class ThunderInvokeState : public IDeviceState
{
  //スピーカープレイヤー
  DFPlayerMini* _dfPlayerMini;
  //LED
  LED12Ring* _led12Ring;
  //コマンドの管理クラス
  CommandManager* _commandManager;

  void ChangeToNormalState(DeviceStateMachine& machine);//通常ステートへの遷移
public:
  ThunderInvokeState(DFPlayerMini &dfPlayerMini,LED12Ring &led12Ring,CommandManager &commandManager);

  void Enter(DeviceStateMachine& machine) override;
  void Update(DeviceStateMachine& machine) override;
  void Exit(DeviceStateMachine& machine) override;
};





//＝＝＝＝＝＝＝＝＝＝
//水魔法の発動ステート
//＝＝＝＝＝＝＝＝＝＝
class WaterInvokeState : public IDeviceState
{
  //スピーカープレイヤー
  DFPlayerMini* _dfPlayerMini;
  //LED
  LED12Ring* _led12Ring;
  //コマンドの管理クラス
  CommandManager* _commandManager;

  void ChangeToNormalState(DeviceStateMachine& machine);//通常ステートへの遷移
public:
  WaterInvokeState(DFPlayerMini &dfPlayerMini,LED12Ring &led12Ring,CommandManager &commandManager);

  void Enter(DeviceStateMachine& machine) override;
  void Update(DeviceStateMachine& machine) override;
  void Exit(DeviceStateMachine& machine) override;
};





//＝＝＝＝＝＝＝＝＝＝
//時魔法の発動ステート
//＝＝＝＝＝＝＝＝＝＝
class TimeInvokeState : public IDeviceState
{
  //スピーカープレイヤー
  DFPlayerMini* _dfPlayerMini;
  //LED
  LED12Ring* _led12Ring;
  //コマンドの管理クラス
  CommandManager* _commandManager;

  void ChangeToNormalState(DeviceStateMachine& machine);//通常ステートへの遷移
public:
  TimeInvokeState(DFPlayerMini &dfPlayerMini,LED12Ring &led12Ring,CommandManager &commandManager);

  void Enter(DeviceStateMachine& machine) override;
  void Update(DeviceStateMachine& machine) override;
  void Exit(DeviceStateMachine& machine) override;
};

#endif