#include "MPU_AccGyro.h"
#include <Arduino.h>
#include "Mathf.h"

MPU_AccGyro::MPU_AccGyro(int sdaPin,int sclPin):
_maAccX(_maN),
_maAccY(_maN),
_maAccZ(_maN),
_maGyroX(_maN),
_maGyroY(_maN),
_maGyroZ(_maN),
_sdaPin(sdaPin),
_sclPin(sclPin)
{

}

void MPU_AccGyro::Setup()
{
  Wire.begin(_sdaPin,_sclPin);

  Wire.beginTransmission(MPU6050_ADDR);
  Wire.write(0x6B);
  Wire.write(0);
  Wire.endTransmission();

  delay(1000);

  //  send start address
  Wire.beginTransmission(MPU6050_ADDR);
  Wire.write(MPU6050_AX);
  Wire.endTransmission();

  //値の初期化
  Wire.requestFrom(MPU6050_ADDR, 14);

  //加速度・ジャイロセンサーの生の値
  short int accX_raw, accY_raw, accZ_raw;
  short int temp_raw;
  short int gyroX_raw, gyroY_raw, gyroZ_raw;

  //  get 14bytes
  accX_raw = Wire.read() << 8;  accX_raw |= Wire.read();
  accY_raw = Wire.read() << 8;  accY_raw |= Wire.read();
  accZ_raw = Wire.read() << 8;  accZ_raw |= Wire.read();
  temp_raw = Wire.read() << 8;  temp_raw |= Wire.read();  //  (Temp-12421)/340.0 [degC]
  gyroX_raw = Wire.read() << 8; gyroX_raw |= Wire.read();
  gyroY_raw = Wire.read() << 8; gyroY_raw |= Wire.read();
  gyroZ_raw = Wire.read() << 8; gyroZ_raw |= Wire.read();

  //値の更新
  _accX=_maAccX.Add(accX_raw);
  _accY=_maAccY.Add(accY_raw);
  _accZ=_maAccZ.Add(accZ_raw);
  _gyroX=_maGyroX.Add(gyroX_raw);
  _gyroY=_maGyroY.Add(gyroY_raw);
  _gyroZ=_maGyroZ.Add(gyroZ_raw);

  //前の値と今の値を同じにしておく
  _accXPrev = _accX;
  _accYPrev = _accY;
  _accZPrev = _accZ;
  _gyroXPrev = _gyroX;
  _gyroYPrev = _gyroY;
  _gyroZPrev = _gyroZ;
}

void MPU_AccGyro::Tick()
{
  //  send start address
  Wire.beginTransmission(MPU6050_ADDR);
  Wire.write(MPU6050_AX);
  Wire.endTransmission();

  Wire.requestFrom(MPU6050_ADDR, 14);

  //加速度・ジャイロセンサーの生の値
  short int accX_raw, accY_raw, accZ_raw;
  short int temp_raw;
  short int gyroX_raw, gyroY_raw, gyroZ_raw;

  //  get 14bytes
  accX_raw = Wire.read() << 8;  accX_raw |= Wire.read();
  accY_raw = Wire.read() << 8;  accY_raw |= Wire.read();
  accZ_raw = Wire.read() << 8;  accZ_raw |= Wire.read();
  temp_raw = Wire.read() << 8;  temp_raw |= Wire.read();  //  (Temp-12421)/340.0 [degC]
  gyroX_raw = Wire.read() << 8; gyroX_raw |= Wire.read();
  gyroY_raw = Wire.read() << 8; gyroY_raw |= Wire.read();
  gyroZ_raw = Wire.read() << 8; gyroZ_raw |= Wire.read();

  //前の値を保存
  _accXPrev = _accX;
  _accYPrev = _accY;
  _accZPrev = _accZ;
  _gyroXPrev = _gyroX;
  _gyroYPrev = _gyroY;
  _gyroZPrev = _gyroZ;

  //値の更新
  _accX=_maAccX.Add(accX_raw);
  _accY=_maAccY.Add(accY_raw);
  _accZ=_maAccZ.Add(accZ_raw);
  _gyroX=_maGyroX.Add(gyroX_raw);
  _gyroY=_maGyroY.Add(gyroY_raw);
  _gyroZ=_maGyroZ.Add(gyroZ_raw);

  DebugPrintln();//値のデバッグ表示
}

void MPU_AccGyro::DebugPrintln()
{
  //加速度・ジャイロの生の値を表示
  // Serial.print("  DEBUG:"); Serial.print("30000, -30000,");
  // Serial.print("  ACCX:"); Serial.print(_accX_ma);
  // Serial.print("  ACCY:"); Serial.print(_accY_ma);
  // Serial.print("  ACCZ:"); Serial.print(_accZ_ma);
  // Serial.print("  TEMP:"); Serial.print(_temp);
  // Serial.print("  GYROX:"); Serial.print(_gyroX_ma);
  // Serial.print("  GYROY:"); Serial.print(_gyroY_ma);
  // Serial.print("  GYROZ:"); Serial.println(_gyroZ_ma);

  //加速度・ジャイロの差の値を表示
  // Serial.print("  DEBUG:"); Serial.print("200, -200,");
  // Serial.print("  ACCX:"); Serial.print(AccXVal());
  // Serial.print("  ACCY:"); Serial.print(AccYVal());
  // Serial.print("  ACCZ:"); Serial.print(AccZVal());
  // Serial.print("  GYROX:"); Serial.print(GyroXVal());
  // Serial.print("  GYROY:"); Serial.print(GyroYVal());
  // Serial.print("  GYROZ:"); Serial.println(GyroZVal());

  //加速度の大きさを取得
  Serial.print("  DEBUG:"); Serial.print("30000, -30000,");
  int mag = Mathf::Vector3MagnitudeSq(AccXVal(),AccYVal(),AccZVal());
  Serial.print("  MAG:"); Serial.println(mag);
}