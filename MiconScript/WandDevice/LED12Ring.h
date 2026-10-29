#ifndef _LED12RING_H_
#define _LED12RING_H_

#include <Adafruit_NeoPixel.h>
#include "Color.h"

//リング状のフルカラーLED
class LED12Ring
{
  //LEDの数
  static constexpr int _numLEDs =12;

  //LEDのピン番号
  const int _ledPin;

  //intensity関係
  const float _minIntensity=0;
  const float _maxIntensity=1;

  bool _isSetColor = false;

  Adafruit_NeoPixel _pixels;

public:
  LED12Ring(int ledPin);

  void Setup();
  void Tick();

  //LEDの数
  static constexpr int LEDNum() {return _numLEDs;}

  //光らせる操作
  void Clear();//全て消す
  void SetColor(int ledIndex,Color color,float intensity);//ledIndex:どのLEDを設定するか、color:色(rgb指定、それぞれ0～255)、intensity:明るさ(0～1)、colorの色に倍率で掛けて明るさを調整する、、
};

#endif