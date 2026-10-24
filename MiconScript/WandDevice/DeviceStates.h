#ifndef _DEVICESTATES_H_
#define _DEVICESTATES_H_

#include "MPU_AccGyro.h"
#include "DFPlayerMini.h"
#include "LED12Ring.h"

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

//通常時のステート
class NormalState : public IDeviceState
{
  //音を鳴らす加速度センサーの大きさの閾値
  const int _accMagnitudeThresholdToPlaySound =7000;

  //LEDの光量(0～1)
  const float _ledIntensity = 0.3;

  //ライトの色
  const Color _ledColors[12] = {
  {255,  0,  0},//赤
  {255, 80,  8},//赤とオレンジの間
  {255,160, 16},//オレンジ
  {255,224, 32},//黄
  { 96,255,128},//黄緑
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

  //加速度センサー
  MPU_AccGyro* _mpu;
  //スピーカープレイヤー
  DFPlayerMini* _dfPlayerMini;
  //LED
  LED12Ring* _led12Ring;

public:
  NormalState(MPU_AccGyro &mpu,DFPlayerMini &dfPlayerMini,LED12Ring &led12Ring);

  void Enter(DeviceStateMachine& machine) override;
  void Update(DeviceStateMachine& machine) override;
  void Exit(DeviceStateMachine& machine) override;
};

//太陽魔法のステート
class SunState : public IDeviceState
{
  //ライトの色
  const Color _ledColors[12] = {
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

  const int _cycleLED_ms = 2000;//LEDのライトの光り方が一周するのにかける時間(ms)
  const int _playSoundTime_ms;//音を鳴らし始める時間

  unsigned long _previous_ms=0;

  //スピーカープレイヤー
  DFPlayerMini* _dfPlayerMini;
  //LED
  LED12Ring* _led12Ring;
public:
  SunState(DFPlayerMini &dfPlayerMini,LED12Ring &led12Ring);

  void Enter(DeviceStateMachine& machine) override;
  void Update(DeviceStateMachine& machine) override;
  void Exit(DeviceStateMachine& machine) override;
};

//星魔法のステート
class StarState : public IDeviceState
{
  //スピーカープレイヤー
  DFPlayerMini* _dfPlayerMini;
  //LED
  LED12Ring* _led12Ring;
public:
  StarState(DFPlayerMini &dfPlayerMini,LED12Ring &led12Ring);

  void Enter(DeviceStateMachine& machine) override;
  void Update(DeviceStateMachine& machine) override;
  void Exit(DeviceStateMachine& machine) override;
};

//雷魔法のステート
class ThunderState : public IDeviceState
{
  //スピーカープレイヤー
  DFPlayerMini* _dfPlayerMini;
  //LED
  LED12Ring* _led12Ring;
public:
  ThunderState(DFPlayerMini &dfPlayerMini,LED12Ring &led12Ring);

  void Enter(DeviceStateMachine& machine) override;
  void Update(DeviceStateMachine& machine) override;
  void Exit(DeviceStateMachine& machine) override;
};

//水魔法のステート
class WaterState : public IDeviceState
{
  //スピーカープレイヤー
  DFPlayerMini* _dfPlayerMini;
  //LED
  LED12Ring* _led12Ring;
public:
  WaterState(DFPlayerMini &dfPlayerMini,LED12Ring &led12Ring);

  void Enter(DeviceStateMachine& machine) override;
  void Update(DeviceStateMachine& machine) override;
  void Exit(DeviceStateMachine& machine) override;
};

//時魔法のステート
class TimeState : public IDeviceState
{
  //スピーカープレイヤー
  DFPlayerMini* _dfPlayerMini;
  //LED
  LED12Ring* _led12Ring;
public:
  TimeState(DFPlayerMini &dfPlayerMini,LED12Ring &led12Ring);

  void Enter(DeviceStateMachine& machine) override;
  void Update(DeviceStateMachine& machine) override;
  void Exit(DeviceStateMachine& machine) override;
};

#endif