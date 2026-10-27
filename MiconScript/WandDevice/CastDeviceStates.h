#ifndef _CASTDEVICESTATES_H_
#define _CASTDEVICESTATES_H_

#include "MPU_AccGyro.h"
#include "DFPlayerMini.h"
#include "LED12Ring.h"
#include "CommandManager.h"
#include "CommonDeviceStates.h"

class DeviceStateMachine;



//＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
//太陽魔法の詠唱(なぞり)ステート
//＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
class SunCastState : public IDeviceState
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

  //スピーカープレイヤー
  DFPlayerMini* _dfPlayerMini;
  //LED
  LED12Ring* _led12Ring;
  //コマンドの管理クラス
  CommandManager* _commandManager;

  void ChangeToInvokeState(DeviceStateMachine& machine);//発動ステートへの遷移
public:
  SunCastState(DFPlayerMini &dfPlayerMini,LED12Ring &led12Ring,CommandManager &commandManager);

  void Enter(DeviceStateMachine& machine) override;
  void Update(DeviceStateMachine& machine) override;
  void Exit(DeviceStateMachine& machine) override;
};





//＝＝＝＝＝＝＝＝＝＝＝＝＝＝
//星魔法の詠唱(なぞり)ステート
//＝＝＝＝＝＝＝＝＝＝＝＝＝＝
class StarCastState : public IDeviceState
{
  //スピーカープレイヤー
  DFPlayerMini* _dfPlayerMini;
  //LED
  LED12Ring* _led12Ring;
  //コマンドの管理クラス
  CommandManager* _commandManager;

  void ChangeToInvokeState(DeviceStateMachine& machine);//発動ステートへの遷移
public:
  StarCastState(DFPlayerMini &dfPlayerMini,LED12Ring &led12Ring,CommandManager &commandManager);

  void Enter(DeviceStateMachine& machine) override;
  void Update(DeviceStateMachine& machine) override;
  void Exit(DeviceStateMachine& machine) override;
};





//＝＝＝＝＝＝＝＝＝＝＝＝＝＝
//雷魔法の詠唱(なぞり)ステート
//＝＝＝＝＝＝＝＝＝＝＝＝＝＝
class ThunderCastState : public IDeviceState
{
  //スピーカープレイヤー
  DFPlayerMini* _dfPlayerMini;
  //LED
  LED12Ring* _led12Ring;
  //コマンドの管理クラス
  CommandManager* _commandManager;

  void ChangeToInvokeState(DeviceStateMachine& machine);//発動ステートへの遷移
public:
  ThunderCastState(DFPlayerMini &dfPlayerMini,LED12Ring &led12Ring,CommandManager &commandManager);

  void Enter(DeviceStateMachine& machine) override;
  void Update(DeviceStateMachine& machine) override;
  void Exit(DeviceStateMachine& machine) override;
};





//＝＝＝＝＝＝＝＝＝＝＝＝＝＝
//水魔法の詠唱(なぞり)ステート
//＝＝＝＝＝＝＝＝＝＝＝＝＝＝
class WaterCastState : public IDeviceState
{
  //スピーカープレイヤー
  DFPlayerMini* _dfPlayerMini;
  //LED
  LED12Ring* _led12Ring;
  //コマンドの管理クラス
  CommandManager* _commandManager;

  void ChangeToInvokeState(DeviceStateMachine& machine);//発動ステートへの遷移
public:
  WaterCastState(DFPlayerMini &dfPlayerMini,LED12Ring &led12Ring,CommandManager &commandManager);

  void Enter(DeviceStateMachine& machine) override;
  void Update(DeviceStateMachine& machine) override;
  void Exit(DeviceStateMachine& machine) override;
};





//＝＝＝＝＝＝＝＝＝＝＝＝＝＝
//時魔法の詠唱(なぞり)ステート
//＝＝＝＝＝＝＝＝＝＝＝＝＝＝
class TimeCastState : public IDeviceState
{
  //スピーカープレイヤー
  DFPlayerMini* _dfPlayerMini;
  //LED
  LED12Ring* _led12Ring;
  //コマンドの管理クラス
  CommandManager* _commandManager;

  void ChangeToInvokeState(DeviceStateMachine& machine);//発動ステートへの遷移
public:
  TimeCastState(DFPlayerMini &dfPlayerMini,LED12Ring &led12Ring,CommandManager &commandManager);

  void Enter(DeviceStateMachine& machine) override;
  void Update(DeviceStateMachine& machine) override;
  void Exit(DeviceStateMachine& machine) override;
};

#endif