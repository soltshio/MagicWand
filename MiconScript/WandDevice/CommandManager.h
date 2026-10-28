
#ifndef COMMAND_MANAGER_H
#define COMMAND_MANAGER_H

enum class ECommandKey
{
  //太陽魔法
  StartSun,//太陽魔法に移行
  CastSun,//詠唱(なぞる)、段階付き
  InvokeSun,//魔法発動
  InvokeSun_PlaySound,//魔法の効果音再生
  FinishSun,//太陽魔法を終了する

  //星魔法
  StartStar,//星魔法に移行する
  CastStar,//詠唱(なぞる)、段階付き
  InvokeStar,//魔法発動
  InvokeStar_PlaySound,//魔法の効果音再生
  FinishStar,//星魔法を終了する

  //雷魔法
  StartThunder,//雷魔法に移行する
  CastThunder,//詠唱(なぞる)、段階付き
  InvokeThunder,//魔法発動
  InvokeThunder_PlaySound,//魔法の効果音再生
  FinishThunder,//雷魔法を終了する

  //水魔法
  StartWater,//水魔法に移行する
  CastWater,//詠唱(なぞる)、段階付き
  InvokeWater,//魔法発動
  InvokeWater_PlaySound,//魔法の効果音再生
  FinishWater,//水魔法を終了する

  //時魔法
  StartTime,//時魔法に移行する
  CastTime,//詠唱(なぞる)、段階付き
  InvokeTime,//魔法発動
  InvokeTime_PlaySound,//魔法の効果音再生
  FinishTime//時魔法を終了する
};

class CommandManager
{
private:
  //☆コマンド文字列のルール
  //基本的に
  //ステート(状態)を大文字２文字、
  //フェーズ(段階)を小文字２文字、
  //レベルを数字２文字
  //の合計6文字+終端文字＝７文字で表現
  //例：STct03 星の詠唱、レベル３の段階
  static constexpr int _commandLength = 7;
  static constexpr int _commandWordLength = 4;//コマンドの文字の部分(ステート＋フェーズ)の長さ
  static constexpr int _commandLevelLength = 2;//コマンドのレベル(数字部分)の長さ

  char _currentCommand[_commandLength] = {};
  bool _isUpdatedNewCommand = false;

  // キーに対応するコマンドの文字部分を取得
  const char* GetCommandStateAndPhase(ECommandKey key) const;

  const int _invalidNum =-1;//不正な値
public:
  CommandManager();

  // コマンド登録
  void SetCommand(const char* command);

  // 指定されたコマンドが入っているか
  bool IsCommand(ECommandKey key);

  //指定されたコマンドが入っていて、なおかつレベルはどのくらいか(そもそも指定されていないコマンドが入っていればInvalidNumが返ってくる)
  int IsCommandAndLevel(ECommandKey key);

  //不正な値
  int InvalidNum() const{return _invalidNum;}
};

#endif
