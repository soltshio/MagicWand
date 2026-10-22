#include "Color.h"
#include "Mathf.h"

Color::Color(int r,int g,int b):
_r(Mathf::Clamp(r,_min,_max)),
_g(Mathf::Clamp(g,_min,_max)),
_b(Mathf::Clamp(b,_min,_max))
{
  
}

void Color::SetRGB(int r,int g,int b)
{
  //値が範囲外にならないようにする
  _r=Mathf::Clamp(r,_min,_max);
  _g=Mathf::Clamp(g,_min,_max);
  _b=Mathf::Clamp(b,_min,_max);
}