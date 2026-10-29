#include "Color.h"
#include "Mathf.h"

Color::Color(int r,int g,int b):
_r(Mathf::Clamp(r,_min,_max)),
_g(Mathf::Clamp(g,_min,_max)),
_b(Mathf::Clamp(b,_min,_max))
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
  colorA.GetRGB(r_b,g_b,b_b);

  //RGBでそれぞれ線形補完
  int r = Mathf::Lerp(r_a,r_b,time);
  int g = Mathf::Lerp(g_a,g_b,time);
  int b = Mathf::Lerp(b_a,b_b,time);

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