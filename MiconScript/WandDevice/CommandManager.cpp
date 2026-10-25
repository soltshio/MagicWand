
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
    { ECommandKey::ChangeToSunState,     "SN0" },
    { ECommandKey::PlaySunSE,            "SN1" },
    { ECommandKey::FinishSunState,       "SN2" },
    
    //星魔法
    { ECommandKey::ChangeToStarState,     "ST0" },
    { ECommandKey::PlayStarSE,            "ST1" },
    { ECommandKey::FinishStarState,       "ST2" },

    //雷魔法
    { ECommandKey::ChangeToThunderState,  "TN0" },
    { ECommandKey::PlayThunderSE,         "TN1" },
    { ECommandKey::FinishThunderState,    "TN2" },

    //水魔法
    { ECommandKey::ChangeToWaterState,    "WT0" },
    { ECommandKey::PlayWaterSE,           "WT1" },
    { ECommandKey::FinishWaterState,      "WT2" },

    //時魔法
    { ECommandKey::ChangeToTimeState,     "TM0" },
    { ECommandKey::PlayTimeSE,            "TM1" },
    { ECommandKey::FinishTimeState,       "TM2" }
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
