using UnityEngine;

[CreateAssetMenu(fileName = "TerminalData", menuName = "TerminalSwitch/Terminal Data")]
public class TerminalData : ScriptableObject
{
  [SerializeField] private string terminalId;
  [SerializeField] private string terminalName;
  [TextArea]
  [SerializeField] private string description;
  [SerializeField] private string handlerId;

  public string TerminalId { get { return terminalId; } }
  public string TerminalName { get { return terminalName; } }
  public string Description { get { return description; } }
  public string HandlerId { get { return handlerId; } }
}
