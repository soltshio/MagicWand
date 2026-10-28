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
  void PlaySound();//効果音の再生
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

  void PlaySound();//効果音の再生
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

  void PlaySound();//効果音の再生
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

  void PlaySound();//効果音の再生
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

  void PlaySound();//効果音の再生
  void ChangeToNormalState(DeviceStateMachine& machine);//通常ステートへの遷移
public:
  TimeInvokeState(DFPlayerMini &dfPlayerMini,LED12Ring &led12Ring,CommandManager &commandManager);

  void Enter(DeviceStateMachine& machine) override;
  void Update(DeviceStateMachine& machine) override;
  void Exit(DeviceStateMachine& machine) override;
};

#endif