#ifndef _COMMAND_MANAGER_H_
#define _COMMAND_MANAGER_H_

class CommandManager
{
  static constexpr const int _commandLength =4;//コマンドの文字数、コマンド自体は3文字だが'\n'の分も含めて+1してある

  //コマンド
  //コマンドは文字３つで表現。１、２つ目はステートを表す英語大文字、３つ目は数字。
  //以下はステートに対応する文字

  //---太陽魔法---
  const char _isChangeToSunState[_commandLength] = {'S','N','0'};//太陽ステートに移行するためのコマンド
  const char _isPlaySunSE[_commandLength] = {'S','N','1'};//太陽の効果音を流すコマンド
  const char _isFinishSunState[_commandLength] = {'S','N','2'};//太陽ステートを終了するコマンド

  //---星魔法---
  //---雷魔法---
  //---水魔法---
  //---時魔法---

  char _currentCommand[_commandLength];//現在入っているコマンド
  bool _isUpdatedNewCommand=false;//新しくコマンドが更新されたか

  bool IsEqualCommand(const char* command);//引数のコマンドと現在登録されているコマンドは同じか
public:
  CommandManager();

  void SetCommand(const char* command);//コマンド登録

  //指定のコマンドが入っているかを取得(一度trueを受け取ると、再度指定のコマンド更新が行われるまでfalseしか取得できなくなる)

  //---太陽魔法---
  bool IsChangeToSunState();//太陽ステートに移行コマンドを受け取ったか
  bool IsPlaySunSE();//太陽の効果音を流すコマンドを受け取ったか
  bool IsFinishSunState();//太陽ステートを終了するコマンドを受け取ったか

};

#endif