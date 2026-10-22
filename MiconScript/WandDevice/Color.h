#ifndef _COLOR_H_
#define _COLOR_H_

class Color
{
private:
  int _r;
  int _g;
  int _b;

  const int _min = 0;
  const int _max = 255;

public:
  Color(int r,int g,int b);//rgbはそれぞれ0～255まで

  void SetRGB(int r,int g,int b);

  void GetRGB(int& r,int& g,int& b) const
  {
    r=_r;
    g=_g;
    b=_b;
  }
};

#endif