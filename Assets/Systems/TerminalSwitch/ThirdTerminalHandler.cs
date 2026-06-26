using UnityEngine;

public class ThirdTerminalHandler : MonoBehaviour, ITerminalHandler
{
  public void HandleTerminal(TerminalData terminalData, PlayerCharacter player)
  {
    PrintMessage();
  }

  private void PrintMessage()
  {
    Debug.Log("Third terminal activated!");
  }
}
