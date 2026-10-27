
#include "CommandManager.h"
#include <string.h>

namespace
{
  struct CommandEntry
  {
    ECommandKey key;
    const char commandStateAndPhase[5];//コマンドの文字の部分+終端文字分
  };

  const CommandEntry commandTable[] =//コマンドの最初の４文字(レベルを除いたもの)
  {
    //太陽魔法
    { ECommandKey::StartSun,                    "SNst" },//太陽魔法に移行するコマンド
    { ECommandKey::CastSun,                     "SNcs" },//詠唱(なぞる)コマンド
    { ECommandKey::InvokeSun,                   "SNiv" },//魔法発動コマンド
    { ECommandKey::InvokeSun_PlaySound,         "SNps" },//魔法の効果音再生コマンド
    { ECommandKey::FinishSun,                   "SNfn" },//太陽魔法を終了するコマンド
    
    //星魔法
    { ECommandKey::StartStar,                   "STst" },//星魔法に移行するコマンド
    { ECommandKey::CastStar,                    "STcs" },//詠唱(なぞる)コマンド
    { ECommandKey::InvokeStar,                  "STiv" },//魔法発動コマンド
    { ECommandKey::InvokeStar_PlaySound,        "STps" },//魔法の効果音再生コマンド
    { ECommandKey::FinishStar,                  "STfn" },//星魔法を終了するコマンド

    //雷魔法
    { ECommandKey::StartThunder,                "TNst" },//雷魔法に移行するコマンド
    { ECommandKey::CastThunder,                 "TNcs" },//詠唱(なぞる)コマンド
    { ECommandKey::InvokeThunder,               "TNiv" },//魔法発動コマンド
    { ECommandKey::InvokeThunder_PlaySound,     "TNps" },//魔法の効果音再生コマンド
    { ECommandKey::FinishThunder,               "TNfn" },//雷魔法を終了するコマンド

    //水魔法
    { ECommandKey::StartWater,                  "WTst" },//水魔法に移行するコマンド
    { ECommandKey::CastWater,                   "WTcs" },//詠唱(なぞる)コマンド
    { ECommandKey::InvokeWater,                 "WTiv" },//魔法発動コマンド
    { ECommandKey::InvokeWater_PlaySound,       "WTps" },//魔法の効果音再生コマンド
    { ECommandKey::FinishWater,                 "WTfn" },//水魔法を終了するコマンド

    //時魔法
    { ECommandKey::StartTime,                   "TMst" },//時魔法に移行するコマンド
    { ECommandKey::CastTime,                    "TMcs" },//詠唱(なぞる)コマンド
    { ECommandKey::InvokeTime,                  "TMiv" },//魔法発動コマンド
    { ECommandKey::InvokeTime_PlaySound,        "TMps" },//魔法の効果音再生コマンド
    { ECommandKey::FinishTime,                  "TMfn" },//時魔法を終了するコマンド
  };

  constexpr int commandCount = sizeof(commandTable) / sizeof(commandTable[0]);
}

CommandManager::CommandManager()
{
}

const char* CommandManager::GetCommandStateAndPhase(ECommandKey key) const
{
  for (int i = 0; i < commandCount; i++)
  {
    if (commandTable[i].key == key)
    {
      return commandTable[i].commandStateAndPhase;
    }
  }

  return nullptr;
}

void CommandManager::SetCommand(const char* command)
{
  if (command == nullptr) return;
  
  strncpy(_currentCommand, command, _commandLength - 1);//ステート、フェーズ、レベル部分をコピー
  _currentCommand[_commandLength - 1] = '\0';//最後に終端文字

  _isUpdatedNewCommand = true;
}

bool CommandManager::IsCommand(ECommandKey key)
{
  if (!_isUpdatedNewCommand) return false;

  //キーに対応するコマンドの文字部分を取得
  const char* commandStateAndPhase = GetCommandStateAndPhase(key);

  //現在入っているコマンドの文字部分を取得
  char currentCommandStateAndPhase[_commandWordLength+1];
  strncpy(currentCommandStateAndPhase, _currentCommand, _commandWordLength);
  currentCommandStateAndPhase[_commandWordLength] = '\0';

  //文字部分が合っているかを比較
  bool isValidCommand = strcmp(commandStateAndPhase,currentCommandStateAndPhase) == 0;

  if (currentCommandStateAndPhase == nullptr || !isValidCommand) return false;
  
  //現在格納されているコマンドと合っていた場合
  _isUpdatedNewCommand = false;
  return true;
}

int CommandManager::IsCommandAndLevel(ECommandKey key)
{
  if (!_isUpdatedNewCommand) return _invalidNum;
  if (!IsCommand(key)) return _invalidNum;//そもそも文字部分が違えば不正な値を返す

  //現在のコマンドからレベル部分を取り出す
  char level_char[_commandLevelLength];
  strncpy(level_char, _currentCommand + _commandWordLength, _commandLevelLength);

  //取り出した文字列を数字に変換して返す
  int level = atoi(level_char);
  return level;
}
