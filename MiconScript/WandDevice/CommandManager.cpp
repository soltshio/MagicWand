#include "CommandManager.h"
#include <string.h>

CommandManager::CommandManager()
{

}

bool CommandManager::IsEqualCommand(const char* command)
{
  return strcmp(command, _currentCommand) == 0;
}

void CommandManager::SetCommand(const char* command)
{
  memcpy(_currentCommand, command, sizeof(_currentCommand));
  _isUpdatedNewCommand=true;
}

//---太陽魔法---
bool CommandManager::IsChangeToSunState()//太陽ステートに移行コマンドを受け取ったか
{
  if(!IsEqualCommand(_isChangeToSunState)) return false;

  _isUpdatedNewCommand=false;
  return true;
}

bool CommandManager::IsPlaySunSE()//太陽の効果音を流すコマンドを受け取ったか
{
  if(!IsEqualCommand(_isPlaySunSE)) return false;

  _isUpdatedNewCommand=false;
  return true;
}

bool CommandManager::IsFinishSunState()//太陽ステートを終了するコマンドを受け取ったか
{
  if(!IsEqualCommand(_isFinishSunState)) return false;

  _isUpdatedNewCommand=false;
  return true;
}