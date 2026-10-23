#ifndef _MPU_ACCGYRO_H_
#define _MPU_ACCGYRO_H_

#include "MovingAverage.h"
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

  //現在の加速度の値(移動平均処理済み)
  int _accX=0;
  int _accY=0;
  int _accZ=0;
  //前の加速度の値(移動平均処理済み)
  int _accXPrev=0;
  int _accYPrev=0;
  int _accZPrev=0;

  //現在のジャイロの値(移動平均処理済み)
  int _gyroX=0;
  int _gyroY=0;
  int _gyroZ=0;
  //前のジャイロの値(移動平均処理済み)
  int _gyroXPrev=0;
  int _gyroYPrev=0;
  int _gyroZPrev=0;

  const int _maN=50;

  const int _divValueCoefficient =10;//大きすぎる加速度・ジャイロの値を小さく丸めるための係数

  //移動平均クラス
  //加速度
  MovingAverage _maAccX;
  MovingAverage _maAccY;
  MovingAverage _maAccZ;
  //ジャイロ
  MovingAverage _maGyroX;
  MovingAverage _maGyroY;
  MovingAverage _maGyroZ;

  void DebugPrintln();//加速度・ジャイロの値をシリアルプロッタで見えるようにするもの

public:
  MPU_AccGyro(int sdaPin,int sclPin);

  //移動平均処理を施した上で通常時の値との差を取得
  //加速度の値を取得
  int AccXVal() const{return (_accX-_accXPrev)/_divValueCoefficient;}
  int AccYVal() const{return (_accY-_accYPrev)/_divValueCoefficient;}
  int AccZVal() const{return (_accZ-_accZPrev)/_divValueCoefficient;}
  //ジャイロの値を取得
  int GyroXVal() const{return (_gyroX-_gyroXPrev)/_divValueCoefficient;}
  int GyroYVal() const{return (_gyroY-_gyroYPrev)/_divValueCoefficient;}
  int GyroZVal() const{return (_gyroZ-_gyroZPrev)/_divValueCoefficient;}


  void Setup();
  void Tick();
};

#endif