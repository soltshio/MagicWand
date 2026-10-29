#ifndef MATHF_H_
#define MATHF_H_

//数学用のクラス

class Mathf
{
public:
  //minがmaxより大きかった場合に入れ替える
  static void NormalizeRange(int* min, int* max);
  static void NormalizeRange(float* min, float* max);
  //Lerp
  static float Lerp(float a,float b,float t);
  //Remap: 入力範囲を出力範囲に変換する
  static float Remap(float value, float inMin, float inMax, float outMin, float outMax);
  //Clamp
  static int Clamp(int value,int min,int max);
  static float Clamp(float value,float min,float max);
  static int Clamp01(int value);
  static float Clamp01(float value);
  //min と max の間で三角波を生成する(valueがminとmaxの範囲外にあったら、自動的にClampする)
  static float TriangleWave01(float value, float min, float max);//min/max に近いほど 0、中央に近いほど 1 を返す
  static float InverseTriangleWave01(float value, float min, float max);//min/max に近いほど 1、中央に近いほど 0 を返す
  // 3Dベクトルの大きさの2乗
  static float Vector3MagnitudeSq(float x, float y, float z);
  // 3Dベクトルの大きさ
  static float Vector3Magnitude(float x, float y, float z);
};

#endif