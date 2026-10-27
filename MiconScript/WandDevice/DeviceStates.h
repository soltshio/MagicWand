#ifndef _DEVICESTATES_H_
#define _DEVICESTATES_H_

#include "MPU_AccGyro.h"
#include "DFPlayerMini.h"
#include "LED12Ring.h"
#include "CommandManager.h"

class DeviceStateMachine;

//ステートの基底クラス
class IDeviceState
{
public:
  virtual ~IDeviceState() = default;

  virtual void Enter(DeviceStateMachine& machine) {}
  virtual void Update(DeviceStateMachine& machine) = 0;
  virtual void Exit(DeviceStateMachine& machine) {}
};





//＝＝＝＝＝＝＝＝
//通常時のステート
//＝＝＝＝＝＝＝＝
class NormalState : public IDeviceState
{
  //音を鳴らす加速度センサーの大きさの閾値
  const int _accMagnitudeThresholdToPlaySound =7000;

  
  const float _ledIntensity_Normal = 0.04;//通常時のLEDの光量(0～1)
  const float _ledIntensity_SoundPlaying = 0.2;//効果音が鳴っている間のLEDの光量(0～1)

  const Color _ledColors[12] = {//ライトの色
  {255,  0,  0},//赤
  {255, 80,  8},//赤とオレンジの間
  {255,160, 16},//オレンジ
  {255,224, 32},//黄
  { 130,255,0},//黄緑
  {  0,192,  0},//緑
  { 40,200,127},//緑と水色の間
  { 80,208,255},//水色
  {  0, 32,255},//青
  { 80, 32,255},//青紫
  {160, 32,255},//紫
  {255, 96,208},//ピンク
  };

  //前の加速度の大きさ
  int _accelMagnitudePrev=0;

  //効果音を鳴らすクールタイム
  const int _playSoundCoolTimeDuration_ms = 1200;
  bool _isPlaySoundCoolTime = false;
  unsigned long _previousPlaySoundTime;

  //加速度センサー
  MPU_AccGyro* _mpu;
  //スピーカープレイヤー
  DFPlayerMini* _dfPlayerMini;
  //LED
  LED12Ring* _led12Ring;
  //コマンドの管理クラス
  CommandManager* _commandManager;

  void UpdateCoolTime();//クールタイムの更新
  void SetLEDLighting_SoundPlaying();//効果音が鳴っているかによってLEDの光り方を変える
  void SetLEDRainbowColors(float intensity);//LEDを虹色に光らせる(intensityで光量を変更)
  void ChangeState(DeviceStateMachine& machine);//ステートの変更
public:
  NormalState(MPU_AccGyro &mpu,DFPlayerMini &dfPlayerMini,LED12Ring &led12Ring,CommandManager &commandManager);

  void Enter(DeviceStateMachine& machine) override;
  void Update(DeviceStateMachine& machine) override;
  void Exit(DeviceStateMachine& machine) override;
};




//＝＝＝＝＝＝＝＝＝
//太陽魔法のステート
//＝＝＝＝＝＝＝＝＝

//＊太陽の詠唱(なぞり)ステート

//＊太陽の発動ステート
class SunState : public IDeviceState
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
  
  //スピーカー関係
  bool _isStartedPlaying = false;//効果音の再生を始めたか

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
  SunState(DFPlayerMini &dfPlayerMini,LED12Ring &led12Ring,CommandManager &commandManager);

  void Enter(DeviceStateMachine& machine) override;
  void Update(DeviceStateMachine& machine) override;
  void Exit(DeviceStateMachine& machine) override;
};





//＝＝＝＝＝＝＝＝
//星魔法のステート
//＝＝＝＝＝＝＝＝

//＊星の詠唱(なぞり)ステート

//＊星の発動ステート
class StarState : public IDeviceState
{
  //スピーカープレイヤー
  DFPlayerMini* _dfPlayerMini;
  //LED
  LED12Ring* _led12Ring;
  //コマンドの管理クラス
  CommandManager* _commandManager;

  void ChangeToNormalState(DeviceStateMachine& machine);//通常ステートへの遷移
public:
  StarState(DFPlayerMini &dfPlayerMini,LED12Ring &led12Ring,CommandManager &commandManager);

  void Enter(DeviceStateMachine& machine) override;
  void Update(DeviceStateMachine& machine) override;
  void Exit(DeviceStateMachine& machine) override;
};





//＝＝＝＝＝＝＝＝
//雷魔法のステート
//＝＝＝＝＝＝＝＝

//＊雷の詠唱(なぞり)ステート

//＊雷の発動ステート
class ThunderState : public IDeviceState
{
  //スピーカープレイヤー
  DFPlayerMini* _dfPlayerMini;
  //LED
  LED12Ring* _led12Ring;
  //コマンドの管理クラス
  CommandManager* _commandManager;

  void ChangeToNormalState(DeviceStateMachine& machine);//通常ステートへの遷移
public:
  ThunderState(DFPlayerMini &dfPlayerMini,LED12Ring &led12Ring,CommandManager &commandManager);

  void Enter(DeviceStateMachine& machine) override;
  void Update(DeviceStateMachine& machine) override;
  void Exit(DeviceStateMachine& machine) override;
};





//＝＝＝＝＝＝＝＝
//水魔法のステート
//＝＝＝＝＝＝＝＝

//＊水の詠唱(なぞり)ステート

//＊水の発動ステート
class WaterState : public IDeviceState
{
  //スピーカープレイヤー
  DFPlayerMini* _dfPlayerMini;
  //LED
  LED12Ring* _led12Ring;
  //コマンドの管理クラス
  CommandManager* _commandManager;

  void ChangeToNormalState(DeviceStateMachine& machine);//通常ステートへの遷移
public:
  WaterState(DFPlayerMini &dfPlayerMini,LED12Ring &led12Ring,CommandManager &commandManager);

  void Enter(DeviceStateMachine& machine) override;
  void Update(DeviceStateMachine& machine) override;
  void Exit(DeviceStateMachine& machine) override;
};





//＝＝＝＝＝＝＝＝
//時魔法のステート
//＝＝＝＝＝＝＝＝

//＊時の詠唱(なぞり)ステート

//＊時の発動ステート
class TimeState : public IDeviceState
{
  //スピーカープレイヤー
  DFPlayerMini* _dfPlayerMini;
  //LED
  LED12Ring* _led12Ring;
  //コマンドの管理クラス
  CommandManager* _commandManager;

  void ChangeToNormalState(DeviceStateMachine& machine);//通常ステートへの遷移
public:
  TimeState(DFPlayerMini &dfPlayerMini,LED12Ring &led12Ring,CommandManager &commandManager);

  void Enter(DeviceStateMachine& machine) override;
  void Update(DeviceStateMachine& machine) override;
  void Exit(DeviceStateMachine& machine) override;
};

#endif