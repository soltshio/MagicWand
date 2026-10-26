#include "MPU_AccGyro.h"
#include <Arduino.h>

MPU_AccGyro::MPU_AccGyro(int sdaPin,int sclPin):
_maAccX(_maN),
_maAccY(_maN),
_maAccZ(_maN),
_maAccMagSq(_maN_Mag),
_maGyroX(_maN),
_maGyroY(_maN),
_maGyroZ(_maN),
_maGyroMagSq(_maN_Mag),
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
  RecordPrevValue();

  //取得した値を移動平均計算
  CalcMovingAverageValue();
}

void MPU_AccGyro::Tick()
{
  // 前の値を保存
  RecordPrevValue();

  //センサーデータを取得
  ReadSensor();

  //取得した値を移動平均計算
  CalcMovingAverageValue();

  DebugPrintln(); // 値のデバッグ表示
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

//前の値を記録する
void MPU_AccGyro::RecordPrevValue()
{
  _accXPrev_Raw = _accX_Raw;
  _accYPrev_Raw = _accY_Raw;
  _accZPrev_Raw = _accZ_Raw;
  _gyroXPrev_Raw = _gyroX_Raw;
  _gyroYPrev_Raw = _gyroY_Raw;
  _gyroZPrev_Raw = _gyroZ_Raw;
}

void MPU_AccGyro::CalcMovingAverageValue()
{
  // 取得関数で毎回計算しないよう、Tick内で計算してキャッシュする
  int accX = static_cast<int>((_accX_Raw - _accXPrev_Raw) * _valueScaleCoefficient);
  int accY = static_cast<int>((_accY_Raw - _accYPrev_Raw) * _valueScaleCoefficient);
  int accZ = static_cast<int>((_accZ_Raw - _accZPrev_Raw) * _valueScaleCoefficient);

  _accXVal = _maAccX.Add(accX);
  _accYVal = _maAccY.Add(accY);
  _accZVal = _maAccZ.Add(accZ);

  _accMagSq = _maAccMagSq.Add(Mathf::Vector3MagnitudeSq(_accXVal, _accYVal, _accZVal));


  int gyroX = static_cast<int>((_gyroX_Raw - _gyroXPrev_Raw) * _valueScaleCoefficient);
  int gyroY = static_cast<int>((_gyroY_Raw - _gyroYPrev_Raw) * _valueScaleCoefficient);
  int gyroZ = static_cast<int>((_gyroZ_Raw - _gyroZPrev_Raw) * _valueScaleCoefficient);

  _gyroXVal = _maGyroX.Add(gyroX);
  _gyroYVal = _maGyroY.Add(gyroY);
  _gyroZVal = _maGyroZ.Add(gyroZ);

  _gyroMagSq = _maGyroMagSq.Add(Mathf::Vector3MagnitudeSq(_gyroXVal, _gyroYVal, _gyroZVal));
}

bool MPU_AccGyro::ReadSensor()
{
  Wire.beginTransmission(MPU6050_ADDR);
  Wire.write(MPU6050_AX);
  if (Wire.endTransmission(false) != 0) return false;

  if (Wire.requestFrom(MPU6050_ADDR, 14) != 14) return false;

  _accX_Raw = ReadInt16();
  _accY_Raw = ReadInt16();
  _accZ_Raw = ReadInt16();
  ReadInt16(); // 温度データ（未使用）
  _gyroX_Raw = ReadInt16();
  _gyroY_Raw = ReadInt16();
  _gyroZ_Raw = ReadInt16();

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
  Serial.print("  DEBUG:"); Serial.print("3000, -3000,");
  Serial.print("  ACCX:"); Serial.print(AccXVal());
  Serial.print("  ACCY:"); Serial.print(AccYVal());
  Serial.print("  ACCZ:"); Serial.print(AccZVal());
  Serial.print("  ACCMAG:"); Serial.print(AccMagSq());
  Serial.print("  GYROX:"); Serial.print(GyroXVal());
  Serial.print("  GYROY:"); Serial.print(GyroYVal());
  Serial.print("  GYROZ:"); Serial.println(GyroZVal());
  Serial.print("  GYROMAG:"); Serial.println(GyroMagSq());

  //加速度の大きさを取得
  // Serial.print("  DEBUG:"); Serial.print("10000, -10000,");
  // int mag = AccMagSq();
  // Serial.print("  MAG:"); Serial.println(mag);
}