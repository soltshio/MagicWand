#ifndef _MPU_ACCGYRO_H_
#define _MPU_ACCGYRO_H_

#include "MovingAverage.h"
#include "Mathf.h"
#include <Wire.h>

#define MPU6050_ADDR 0x68
#define MPU6050_AX  0x3B
#define MPU6050_AY  0x3D
#define MPU6050_AZ  0x3F
#define MPU6050_TP  0x41    //  data not used
#define MPU6050_GX  0x43
#define MPU6050_GY  0x45
#define MPU6050_GZ  0x47

//加速度・ジャイロセンサー

class MPU_AccGyro
{
  //ピン番号
  const int _sdaPin;
  const int _sclPin;

  const int _maN=30;
  const int _maN_Mag=5;

  const float _valueScaleCoefficient = 0.1f;//大きすぎる加速度・ジャイロの値を小さく丸めるための係数

  //現在の生の加速度値(移動平均処理済み)
  int _accX_Raw=0;
  int _accY_Raw=0;
  int _accZ_Raw=0;
  //前の生の加速度の値(移動平均処理済み)
  int _accXPrev_Raw=0;
  int _accYPrev_Raw=0;
  int _accZPrev_Raw=0;

  //現在の生のジャイロの値(移動平均処理済み)
  int _gyroX_Raw=0;
  int _gyroY_Raw=0;
  int _gyroZ_Raw=0;
  //前の生のジャイロの値(移動平均処理済み)
  int _gyroXPrev_Raw=0;
  int _gyroYPrev_Raw=0;
  int _gyroZPrev_Raw=0;

  //外部取得用の計算済みキャッシュ
  int _accXVal=0;
  int _accYVal=0;
  int _accZVal=0;
  int _accMagSq=0;
  int _gyroXVal=0;
  int _gyroYVal=0;
  int _gyroZVal=0;
  int _gyroMagSq=0;

  //移動平均クラス
  //加速度
  MovingAverage _maAccX;
  MovingAverage _maAccY;
  MovingAverage _maAccZ;
  MovingAverage _maAccMagSq;
  //ジャイロ
  MovingAverage _maGyroX;
  MovingAverage _maGyroY;
  MovingAverage _maGyroZ;
  MovingAverage _maGyroMagSq;

  bool ReadSensor();
  static int16_t ReadInt16();
  void RecordPrevValue();
  void CalcMovingAverageValue();

  void DebugPrintln();//加速度・ジャイロの値をシリアルプロッタで見えるようにするもの

public:
  MPU_AccGyro(int sdaPin,int sclPin);

  //移動平均処理を施した上で通常時の値との差を取得
  //加速度の値を取得
  int AccXVal() const{return _accXVal;}
  int AccYVal() const{return _accYVal;}
  int AccZVal() const{return _accZVal;}
  int AccMagSq() const{return _accMagSq;}//加速度の大きさ(処理負荷軽減のために2乗した値のまま渡す)

  //ジャイロの値を取得
  int GyroXVal() const{return _gyroXVal;}
  int GyroYVal() const{return _gyroYVal;}
  int GyroZVal() const{return _gyroZVal;}
  int GyroMagSq() const{return _gyroMagSq;}//ジャイロの大きさ(処理負荷軽減のために2乗した値のまま渡す)


  void Setup();
  void Tick();
};

#endif