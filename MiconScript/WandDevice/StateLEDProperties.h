#ifndef _STATE_LED_PROPERTY_H_
#define _STATE_LED_PROPERTY_H_

#include "Color.h"

struct StateLEDProperty//状態ごとのLEDのプロパティをまとめたもの
{
  const Color ledColors[LED12Ring::LEDNum()];
  const float lowLEDIntensity;//暗い時のLEDの光
  const float highLEDIntensity;//明るい時のLEDの光
  const int toFadeDuration_ms;//この状態の色まで何ミリ秒かけてフェード変化させていくか
};

namespace StateLEDProperties
{
  //---通常状態---
  inline const StateLEDProperty normal =
  {
    .ledColors =
    {
      Color(255,   0,   0), // 赤
      Color(255,  80,   8), // 赤とオレンジの間
      Color(255, 160,  16), // オレンジ
      Color(255, 224,  32), // 黄
      Color(130, 255,   0), // 黄緑
      Color(  0, 192,   0), // 緑
      Color( 40, 200, 127), // 緑と水色の間
      Color( 80, 208, 255), // 水色
      Color(  0,  32, 255), // 青
      Color( 80,  32, 255), // 青紫
      Color(160,  32, 255), // 紫
      Color(255,  96, 208)  // ピンク
    },
    .lowLEDIntensity = 0.04f,
    .highLEDIntensity = 0.2f,
    .toFadeDuration_ms = 500//通常状態は基本この値は使わない
  };


  //---太陽魔法---
  inline const StateLEDProperty sun =
  {
    .ledColors =
    {
      Color(255,   0,   0), // 赤
      Color(255,  53,   5),
      Color(255, 106,  10),
      Color(255, 160,  16), // オレンジ
      Color(255, 181,  21),
      Color(255, 202,  26),
      Color(255, 224,  32), // 黄
      Color(255, 202,  26),
      Color(255, 181,  21),
      Color(255, 160,  16), // オレンジ
      Color(255, 106,  10),
      Color(255,  53,   5)  // 赤の一歩手前の色
    },
    .lowLEDIntensity = 0.01f,
    .highLEDIntensity = 0.2f,
    .toFadeDuration_ms = 500
  };


  //---星魔法---
  inline const StateLEDProperty star =
  {
    .ledColors =
    {
      Color(160,  32, 255), // 紫
      Color(134,  32, 255),
      Color(107,  32, 255),
      Color( 80,  32, 255), // 青紫
      Color( 54,  32, 255),
      Color( 27,  32, 255),
      Color(  0,  32, 255), // 青
      Color( 27,  32, 255),
      Color( 54,  32, 255),
      Color( 80,  32, 255), // 青紫
      Color(107,  32, 255),
      Color(134,  32, 255)  // 紫の一歩手前の色
    },
    .lowLEDIntensity = 0.01f,
    .highLEDIntensity = 0.2f,
    .toFadeDuration_ms = 500
  };


  //---雷魔法---
  inline const StateLEDProperty thunder =
  {
    .ledColors =
    {
      Color(255, 224,  32), // 黄
      Color(170, 214,  22),
      Color( 85, 203,  11),
      Color(  0, 192,   0), // 緑
      Color( 27, 197,  85),
      Color( 54, 202, 170),
      Color( 80, 208, 255), // 水色
      Color( 54, 202, 170),
      Color( 27, 197,  85),
      Color(  0, 192,   0), // 緑
      Color( 85, 203,  11),
      Color(170, 214,  22)  // 黄の一歩手前の色
    },
    .lowLEDIntensity = 0.01f,
    .highLEDIntensity = 0.2f,
    .toFadeDuration_ms = 500
  };


  //---水魔法---
  inline const StateLEDProperty water =
  {
    .ledColors =
    {
      Color( 80, 208, 255), // 水色
      Color( 54, 150, 255),
      Color( 27,  91, 255),
      Color(  0,  32, 255), // 青
      Color( 27,  32, 255),
      Color( 54,  32, 255),
      Color( 80,  32, 255), // 青紫
      Color( 54,  32, 255),
      Color( 27,  32, 255),
      Color(  0,  32, 255), // 青
      Color( 27,  91, 255),
      Color( 54, 150, 255)  // 水色の一歩手前の色
    },
    .lowLEDIntensity = 0.01f,
    .highLEDIntensity = 0.2f,
    .toFadeDuration_ms = 500
  };


  //---時魔法---
  inline const StateLEDProperty time =
  {
    .ledColors =
    {
      Color(130, 255,   0), // 黄緑
      Color( 86, 234,   0),
      Color( 43, 213,   0),
      Color(  0, 192,   0), // 緑
      Color( 13, 195,  42),
      Color( 26, 198,  84),
      Color( 40, 200, 127), // 緑と水色の間
      Color( 26, 198,  84),
      Color( 13, 195,  42),
      Color(  0, 192,   0), // 緑
      Color( 43, 213,   0),
      Color( 86, 234,   0)  // 黄緑の一歩手前の色
    },
    .lowLEDIntensity = 0.01f,
    .highLEDIntensity = 0.2f,
    .toFadeDuration_ms = 500
  };

}


#endif