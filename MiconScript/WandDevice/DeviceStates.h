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
  const int _accMagnitudeThresholdToPlaySound =10;

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
public:
  SunState();

  void Enter(DeviceStateMachine& machine) override;
  void Update(DeviceStateMachine& machine) override;
  void Exit(DeviceStateMachine& machine) override;
};

//星魔法のステート
class StarState : public IDeviceState
{
public:
  StarState();

  void Enter(DeviceStateMachine& machine) override;
  void Update(DeviceStateMachine& machine) override;
  void Exit(DeviceStateMachine& machine) override;
};

//雷魔法のステート
class ThunderState : public IDeviceState
{
public:
  ThunderState();

  void Enter(DeviceStateMachine& machine) override;
  void Update(DeviceStateMachine& machine) override;
  void Exit(DeviceStateMachine& machine) override;
};

//水魔法のステート
class WaterState : public IDeviceState
{
public:
  WaterState();

  void Enter(DeviceStateMachine& machine) override;
  void Update(DeviceStateMachine& machine) override;
  void Exit(DeviceStateMachine& machine) override;
};

//時魔法のステート
class TimeState : public IDeviceState
{
public:
  TimeState();

  void Enter(DeviceStateMachine& machine) override;
  void Update(DeviceStateMachine& machine) override;
  void Exit(DeviceStateMachine& machine) override;
};

#endif