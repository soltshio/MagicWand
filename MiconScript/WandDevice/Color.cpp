#include "Color.h"
#include "Mathf.h"

Color::Color(int r,int g,int b):
_r(Mathf::Clamp(static_cast<float>(r), static_cast<float>(_min), static_cast<float>(_max))),
_g(Mathf::Clamp(static_cast<float>(g), static_cast<float>(_min), static_cast<float>(_max))),
_b(Mathf::Clamp(static_cast<float>(b), static_cast<float>(_min), static_cast<float>(_max)))
{
  
}

Color Color::ColorLerp(Color colorA,Color colorB,float time)//timeは0～1まで
{
  //timeを0～1に制限
  time = Mathf::Clamp01(time);

  //Aの色を取得
  int r_a,g_a,b_a;
  colorA.GetRGB(r_a,g_a,b_a);

  //Bの色を取得
  int r_b,g_b,b_b;
  colorB.GetRGB(r_b,g_b,b_b);

  //取得した色の値をfloatに変換
  float r_a_f = static_cast<float>(r_a);
  float g_a_f = static_cast<float>(g_a);
  float b_a_f = static_cast<float>(b_a);

  float r_b_f = static_cast<float>(r_b);
  float g_b_f = static_cast<float>(g_b);
  float b_b_f = static_cast<float>(b_b);

  //RGBでそれぞれ線形補完
  int r = static_cast<int>(Mathf::Lerp(r_a_f,r_b_f,time));
  int g = static_cast<int>(Mathf::Lerp(g_a_f,g_b_f,time));
  int b = static_cast<int>(Mathf::Lerp(b_a_f,b_b_f,time));

  //線形補完した色を返す
  Color retColor(r,g,b);

  return retColor;
}

void Color::SetRGB(int r,int g,int b)
{
  //値が範囲外にならないようにする
  _r=Mathf::Clamp(r,_min,_max);
  _g=Mathf::Clamp(g,_min,_max);
  _b=Mathf::Clamp(b,_min,_max);
}