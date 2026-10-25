
#ifndef COMMAND_MANAGER_H
#define COMMAND_MANAGER_H

enum class ECommandKey
{
  //太陽魔法
  ChangeToSunState,//太陽ステートに移行
  PlaySunSE,//太陽の効果音を流す
  FinishSunState,//太陽ステートを終了する

  //星魔法
  ChangeToStarState,//星ステートに移行する
  PlayStarSE,//星の効果音を流す
  FinishStarState,//星ステートを終了する

  //雷魔法
  ChangeToThunderState,//雷ステートに移行する
  PlayThunderSE,//雷の効果音を流す
  FinishThunderState,//雷ステートを終了する

  //水魔法
  ChangeToWaterState,//水ステートに移行する
  PlayWaterSE,//水の効果音を流す
  FinishWaterState,//水ステートを終了する

  //時魔法
  ChangeToTimeState,//時間ステートに移行する
  PlayTimeSE,//時間の効果音を流す
  FinishTimeState//時間ステートを終了する
};

class CommandManager
{
private:
  static constexpr int _commandLength = 4;

  char _currentCommand[_commandLength] = {};
  bool _isUpdatedNewCommand = false;

  // キーに対応するコマンド文字列を取得
  const char* GetCommand(ECommandKey key) const;

public:
  CommandManager();

  // コマンド登録
  void SetCommand(const char* command);

  // 指定されたコマンドが入っているか
  bool IsCommand(ECommandKey key);
};

#endif
