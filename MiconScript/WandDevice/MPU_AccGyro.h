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

  //移動平均処理をした加速度の値
  int _accX_ma=0;
  int _accY_ma=0;
  int _accZ_ma=0;
  //移動平均処理をしたジャイロの値
  int _gyroX_ma=0;
  int _gyroY_ma=0;
  int _gyroZ_ma=0;

  const int _maN=35;

  //移動平均クラス
  //加速度
  MovingAverage _maAccX;
  MovingAverage _maAccY;
  MovingAverage _maAccZ;
  //ジャイロ
  MovingAverage _maGyroX;
  MovingAverage _maGyroY;
  MovingAverage _maGyroZ;

  //加速度・ジャイロセンサーの生の値
  short int _accX, _accY, _accZ;
  short int _temp;
  short int _gyroX, _gyroY, _gyroZ;

  void DebugPrintln();//加速度・ジャイロの値をシリアルプロッタで見えるようにするもの

public:
  MPU_AccGyro(int sdaPin,int sclPin);

  //移動平均処理を施した加速度の値を取得
  int AccX() const{return _accX_ma;}
  int AccY() const{return _accY_ma;}
  int AccZ() const{return _accZ_ma;}
  //移動平均処理を施したジャイロの値を取得
  int GyroX() const{return _gyroX_ma;}
  int GyroY() const{return _gyroY_ma;}
  int GyroZ() const{return _gyroZ_ma;}

  void Setup();
  void Tick();
};

#endif