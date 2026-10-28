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
  const float _maxLedIntensity = 0.3;//LEDの最大時の光量(0～1)
  const float _minLedIntensity = 0.02;//LEDの最小時の光量(0～1)

  const int _maxCastLevel = 4;
  
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

  void PlayEffectOnUpCastLevel();//詠唱が進むたびにLEDの光り方が変わって、効果音がなるようにする
  void SetLED_CastLevel(int castLevel);//詠唱のレベルに合わせてLEDの光り方を変える
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
  const Color _ledColors[12] = {//ライトの色
  {160, 32,255},//紫
  {134, 32,255},
  {107, 32,255},
  { 80, 32,255},//青紫
  { 54, 32,255},
  { 27, 32,255},
  {  0, 32,255},//青
  { 27, 32,255},
  { 54, 32,255},
  { 80, 32,255},//青紫
  {107, 32,255},
  {134, 32,255},//紫の一歩手前の色
  };

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
  const Color _ledColors[12] = {//ライトの色
  {255,224, 32},//黄
  {170,214, 22},
  { 85,203, 11},
  {  0,192,  0},//緑
  { 27,197, 85},
  { 54,202,170},
  { 80,208,255},//水色
  { 54,202,170},
  { 27,197, 85},
  {  0,192,  0},//緑
  { 85,203, 11},
  {170,214, 22},//黄の一歩手前の色
  };

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
  const Color _ledColors[12] = {//ライトの色
  { 80,208,255},//水色
  { 54,150,255},
  { 27, 91,255},
  {  0, 32,255},//青
  { 27, 32,255},
  { 54, 32,255},
  { 80, 32,255},//青紫
  { 54, 32,255},
  { 27, 32,255},
  {  0, 32,255},//青
  { 27, 91,255},
  { 54,150,255},//水色の一歩手前の色
  };

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
    const Color _ledColors[12] = {//ライトの色
  {130,255,  0},//黄緑
  { 86,234,  0},
  { 43,213,  0},
  {  0,192,  0},//緑
  { 13,195, 42},
  { 26,198, 84},
  { 40,200,127},//緑と水色の間
  { 26,198, 84},
  { 13,195, 42},
  {  0,192,  0},//緑
  { 43,213,  0},
  { 86,234,  0},//黄緑の一歩手前の色
  };

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