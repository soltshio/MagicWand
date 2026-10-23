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

  // 初回のセンサーデータを取得
  ReadSensor();

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
  // 前の値を保存
  _accXPrev = _accX;
  _accYPrev = _accY;
  _accZPrev = _accZ;
  _gyroXPrev = _gyroX;
  _gyroYPrev = _gyroY;
  _gyroZPrev = _gyroZ;

  if (ReadSensor()) 
  {
    DebugPrintln(); // 値のデバッグ表示
  }
}

int16_t MPU_AccGyro::ReadInt16()
{
  // 上位8ビットのデータを読み取る
  int16_t high = Wire.read();

  // 下位8ビットのデータを読み取る
  int16_t low = Wire.read();

  // 上位8ビットを左に8ビットずらし、
  // 下位8ビットと結合して16ビットの値にする
  return (high << 8) | low;
}

bool MPU_AccGyro::ReadSensor()
{
  Wire.beginTransmission(MPU6050_ADDR);
  Wire.write(MPU6050_AX);
  if (Wire.endTransmission(false) != 0) return false;

  if (Wire.requestFrom(MPU6050_ADDR, 14) != 14) return false;

  int16_t accX_raw = ReadInt16();
  int16_t accY_raw = ReadInt16();
  int16_t accZ_raw = ReadInt16();
  ReadInt16(); // 温度データ（未使用）
  int16_t gyroX_raw = ReadInt16();
  int16_t gyroY_raw = ReadInt16();
  int16_t gyroZ_raw = ReadInt16();

  _accX = _maAccX.Add(accX_raw);
  _accY = _maAccY.Add(accY_raw);
  _accZ = _maAccZ.Add(accZ_raw);
  _gyroX = _maGyroX.Add(gyroX_raw);
  _gyroY = _maGyroY.Add(gyroY_raw);
  _gyroZ = _maGyroZ.Add(gyroZ_raw);
  return true;
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
  Serial.print("  DEBUG:"); Serial.print("10000, -10000,");
  int mag = Mathf::Vector3MagnitudeSq(AccXVal(),AccYVal(),AccZVal());
  Serial.print("  MAG:"); Serial.println(mag);
}