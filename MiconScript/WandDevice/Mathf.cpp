#include "Mathf.h"
#include <math.h>

//minがmaxより大きかった場合に入れ替える
void Mathf::NormalizeRange(int* min, int* max)
{
  if (*min > *max)
  {
    int tmp = *min;
    *min = *max;
    *max = tmp;
  }
}

void Mathf::NormalizeRange(float* min, float* max)
{
  if (*min > *max)
  {
    float tmp = *min;
    *min = *max;
    *max = tmp;
  }
}


//Lerp
float Mathf::Lerp(float a,float b,float t)
{
  return a + (b - a) * t;
}


//Clamp
int Mathf::Clamp(int value,int min,int max)
{
  NormalizeRange(&min,&max);

  if (value < min) return min;
  if (value > max) return max;
  return value;
}

float Mathf::Clamp(float value,float min,float max)
{
  NormalizeRange(&min,&max);

  if (value < min) return min;
  if (value > max) return max;
  return value;
}


//min と max の間で三角波を生成する
float Mathf::TriangleWave01(float value, float min, float max)
{
  NormalizeRange(&min,&max);
  value = Clamp(value,min,max);

  float halfRange = (max - min) * 0.5f;
  float middle = min + halfRange;

  return 1 - fabs(value - middle) / halfRange;//変換式
}

float Mathf::InverseTriangleWave01(float value, float min, float max)
{
  NormalizeRange(&min,&max);
  value = Clamp(value,min,max);

  float halfRange = (max - min) * 0.5f;
  float middle = min + halfRange;

  return fabs(value - middle) / halfRange;//変換式
}