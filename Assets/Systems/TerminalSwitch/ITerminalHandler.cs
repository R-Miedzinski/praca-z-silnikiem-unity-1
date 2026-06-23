public interface ITerminalHandler
{
  // TerminalSwitchSystem calls this after matching TerminalData to a handler.
  void HandleTerminal(TerminalData terminalData, PlayerCharacter player);
}
