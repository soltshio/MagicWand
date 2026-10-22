#include "MPU_AccGyro.h"

MPU_AccGyro::MPU_AccGyro(int sdaPin,int sclPin):
_maAccX(_maN),
_maAccY(_maN),
_maAccZ(_maN),
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
}

void MPU_AccGyro::Tick()
{
  //  send start address
  Wire.beginTransmission(MPU6050_ADDR);
  Wire.write(MPU6050_AX);
  Wire.endTransmission();  

  //  request 14bytes (int16 x 7)
  Wire.requestFrom(MPU6050_ADDR, 14);

  //  get 14bytes
  _accX = Wire.read() << 8;  _accX |= Wire.read();
  _accY = Wire.read() << 8;  _accY |= Wire.read();
  _accZ = Wire.read() << 8;  _accZ |= Wire.read();
  _temp = Wire.read() << 8;  _temp |= Wire.read();  //  (Temp-12421)/340.0 [degC]
  _gyroX = Wire.read() << 8; _gyroX |= Wire.read();
  _gyroY = Wire.read() << 8; _gyroY |= Wire.read();
  _gyroZ = Wire.read() << 8; _gyroZ |= Wire.read();

  _accX_ma=_maAccX.Add(_accX);
  _accY_ma=_maAccY.Add(_accY);
  _accZ_ma=_maAccZ.Add(_accZ);

  //DebugPrintln();//値のデバッグ表示
}

void MPU_AccGyro::DebugPrintln()
{
  Serial.print("  DEBUG:"); Serial.print("30000, -30000,");
  Serial.print("  ACCX:"); Serial.print(AccX_ma);
  Serial.print("  ACCY:"); Serial.print(AccY_ma);
  Serial.print("  ACCZ:"); Serial.print(AccZ_ma);
  Serial.print("  TEMP:"); Serial.print(Temp);
  Serial.print("  GYROX:"); Serial.print(GyroX_ma);
  Serial.print("  GYROY:"); Serial.print(GyroY_ma);
  Serial.print("  GYROZ:"); Serial.println(GyroZ_ma); 
}