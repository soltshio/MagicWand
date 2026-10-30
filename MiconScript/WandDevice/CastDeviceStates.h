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
  const float _maxLEDIntensity;//LEDの最大時の光量(0～1)
  const float _minLEDIntensity;//LEDの最小時の光量(0～1)

  //最初に球に触れた時のcastLevelは0
  const int _maxCastLevel = 4;
  
  const Color* _ledColors;

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
  SunCastState(
    DFPlayerMini &dfPlayerMini,
    LED12Ring &led12Ring,
    CommandManager &commandManager,
    const Color* ledColors,
    float minLEDIntensity,
    float maxLEDIntensity
  );

  void Enter(DeviceStateMachine& machine) override;
  void Update(DeviceStateMachine& machine) override;
  void Exit(DeviceStateMachine& machine) override;
};





//＝＝＝＝＝＝＝＝＝＝＝＝＝＝
//星魔法の詠唱(なぞり)ステート
//＝＝＝＝＝＝＝＝＝＝＝＝＝＝
class StarCastState : public IDeviceState
{
  //LED関係
  const float _maxLEDIntensity;//LEDの最大時の光量(0～1)
  const float _minLEDIntensity;//LEDの最小時の光量(0～1)

  //最初に球に触れた時のcastLevelは0
  const int _maxCastLevel = 4;

  const Color* _ledColors;

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
  StarCastState(
    DFPlayerMini &dfPlayerMini,
    LED12Ring &led12Ring,
    CommandManager &commandManager,
    const Color* ledColors,
    float minLEDIntensity,
    float maxLEDIntensity
  );

  void Enter(DeviceStateMachine& machine) override;
  void Update(DeviceStateMachine& machine) override;
  void Exit(DeviceStateMachine& machine) override;
};





//＝＝＝＝＝＝＝＝＝＝＝＝＝＝
//雷魔法の詠唱(なぞり)ステート
//＝＝＝＝＝＝＝＝＝＝＝＝＝＝
class ThunderCastState : public IDeviceState
{
  //LED関係
  const float _maxLEDIntensity;//LEDの最大時の光量(0～1)
  const float _minLEDIntensity;//LEDの最小時の光量(0～1)

  //最初に球に触れた時のcastLevelは0
  const int _maxCastLevel = 3;

  const Color* _ledColors;

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
  ThunderCastState(
    DFPlayerMini &dfPlayerMini,
    LED12Ring &led12Ring,
    CommandManager &commandManager,
    const Color* ledColors,
    float minLEDIntensity,
    float maxLEDIntensity
  );

  void Enter(DeviceStateMachine& machine) override;
  void Update(DeviceStateMachine& machine) override;
  void Exit(DeviceStateMachine& machine) override;
};





//＝＝＝＝＝＝＝＝＝＝＝＝＝＝
//水魔法の詠唱(なぞり)ステート
//＝＝＝＝＝＝＝＝＝＝＝＝＝＝
class WaterCastState : public IDeviceState
{
  //LED関係
  const float _maxLEDIntensity;//LEDの最大時の光量(0～1)
  const float _minLEDIntensity;//LEDの最小時の光量(0～1)

  //最初に球に触れた時のcastLevelは0
  const int _maxCastLevel = 2;

  const Color* _ledColors;//ライトの色

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
  WaterCastState(
    DFPlayerMini &dfPlayerMini,
    LED12Ring &led12Ring,
    CommandManager &commandManager,
    const Color* ledColors,
    float minLEDIntensity,
    float maxLEDIntensity
  );

  void Enter(DeviceStateMachine& machine) override;
  void Update(DeviceStateMachine& machine) override;
  void Exit(DeviceStateMachine& machine) override;
};





//＝＝＝＝＝＝＝＝＝＝＝＝＝＝
//時魔法の詠唱(なぞり)ステート
//＝＝＝＝＝＝＝＝＝＝＝＝＝＝
class TimeCastState : public IDeviceState
{
  //LED関係
  const float _maxLEDIntensity;//LEDの最大時の光量(0～1)
  const float _minLEDIntensity;//LEDの最小時の光量(0～1)

  //最初に球に触れた時のcastLevelは0
  const int _maxCastLevel = 3;

  const Color* _ledColors;//ライトの色

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
  TimeCastState(
    DFPlayerMini &dfPlayerMini,
    LED12Ring &led12Ring,
    CommandManager &commandManager,
    const Color* ledColors,
    float minLEDIntensity,
    float maxLEDIntensity
  );

  void Enter(DeviceStateMachine& machine) override;
  void Update(DeviceStateMachine& machine) override;
  void Exit(DeviceStateMachine& machine) override;
};

#endif