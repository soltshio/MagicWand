
#include "CommandManager.h"
#include <string.h>

namespace
{
  struct CommandEntry
  {
    ECommandKey key;
    const char command[4];
  };

  const CommandEntry commandTable[] =
  {
    //太陽魔法
    { ECommandKey::ChangeToSunState,     "SN0" },//太陽ステートに移行するコマンド
    { ECommandKey::PlaySunSE,            "SN1" },//太陽の効果音を流すコマンド
    { ECommandKey::FinishSunState,       "SN2" },//太陽ステートを終了するコマンド
    
    //星魔法
    { ECommandKey::ChangeToStarState,     "ST0" },//星ステートに移行するコマンド
    { ECommandKey::PlayStarSE,            "ST1" },//星の効果音を流すコマンド
    { ECommandKey::FinishStarState,       "ST2" },//星ステートを終了するコマンド

    //雷魔法
    { ECommandKey::ChangeToThunderState,  "TN0" },//雷ステートに移行するコマンド
    { ECommandKey::PlayThunderSE,         "TN1" },//雷の効果音を流すコマンド
    { ECommandKey::FinishThunderState,    "TN2" },//雷ステートを終了するコマンド

    //水魔法
    { ECommandKey::ChangeToWaterState,    "WT0" },//水ステートに移行するコマンド
    { ECommandKey::PlayWaterSE,           "WT1" },//水の効果音を流すコマンド
    { ECommandKey::FinishWaterState,      "WT2" },//水ステートを終了するコマンド

    //時魔法
    { ECommandKey::ChangeToTimeState,     "TM0" },//時間ステートに移行するコマンド
    { ECommandKey::PlayTimeSE,            "TM1" },//時間の効果音を流すコマンド
    { ECommandKey::FinishTimeState,       "TM2" }//時間ステートを終了するコマンド
  };

  constexpr int commandCount = sizeof(commandTable) / sizeof(commandTable[0]);
}

CommandManager::CommandManager()
{
}

const char* CommandManager::GetCommand(ECommandKey key) const
{
  for (int i = 0; i < commandCount; i++)
  {
    if (commandTable[i].key == key)
    {
      return commandTable[i].command;
    }
  }

  return nullptr;
}

void CommandManager::SetCommand(const char* command)
{
  if (command == nullptr) return;
  
  // 最大3文字と終端文字をコピー
  strncpy(_currentCommand, command, _commandLength - 1);
  _currentCommand[_commandLength - 1] = '\0';

  _isUpdatedNewCommand = true;
}

bool CommandManager::IsCommand(ECommandKey key)
{
  if (!_isUpdatedNewCommand) return false;

  const char* command = GetCommand(key);

  if (command == nullptr || strcmp(command, _currentCommand) != 0) return false;
  
  //現在格納されているコマンドと合っていた場合
  _isUpdatedNewCommand = false;
  return true;
}
