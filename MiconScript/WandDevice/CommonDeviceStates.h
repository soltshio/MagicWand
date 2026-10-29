#ifndef _COMMONDEVICESTATES_H_
#define _COMMONDEVICESTATES_H_

#include "MPU_AccGyro.h"
#include "DFPlayerMini.h"
#include "LED12Ring.h"
#include "CommandManager.h"
#include "LEDColorFadeController.h"

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
  {130,255,  0},//黄緑
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



//＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
//LEDの色をフェードでだんだん変化させるだけのステート
//＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
class LEDFadeState : public IDeviceState
{
  LEDColorFadeController _ledColorFadeController;
public:
  LEDFadeState(LED12Ring &led12Ring,const Color* fromColors,const Color* toColors,float fromIntensity,float toIntensity,int fadeDuration_ms);

  void Enter(DeviceStateMachine& machine) override;
  void Update(DeviceStateMachine& machine) override;
  void Exit(DeviceStateMachine& machine) override;
};

#endif