using System.Collections.Generic;

public class TriggerVector
{
  public int CurrentTriggers { get { return currentTriggers; } }

  // Binary vector representing triggers config, corresponds to ETriggerType enum values in order
  private int currentTriggers;
  private Dictionary<ETriggerType, int> triggerToBitIndex;

  public TriggerVector()
  {
    currentTriggers = 0;
    triggerToBitIndex = new Dictionary<ETriggerType, int>();

    var triggerTypes = System.Enum.GetValues(typeof(ETriggerType));
    for (int i = 0; i < triggerTypes.Length; i++)
    {
      triggerToBitIndex[(ETriggerType)triggerTypes.GetValue(i)] = i;
    }
  }

  public static TriggerVector operator |(TriggerVector a, TriggerVector b)
  {
    TriggerVector result = new TriggerVector();
    result.currentTriggers = a.currentTriggers | b.currentTriggers;
    return result;
  }

  public static TriggerVector operator &(TriggerVector a, TriggerVector b)
  {
    TriggerVector result = new TriggerVector();
    result.currentTriggers = a.currentTriggers & b.currentTriggers;
    return result;
  }

  public void Activate(ETriggerType trigger)
  {
    if (triggerToBitIndex.TryGetValue(trigger, out int bitIndex))
    {
      currentTriggers |= (1 << bitIndex);
    }
  }

  public void Deactivate(ETriggerType trigger)
  {
    if (triggerToBitIndex.TryGetValue(trigger, out int bitIndex))
    {
      currentTriggers &= ~(1 << bitIndex);
    }
  }

  // Check if all triggers in 'other' are active in this vector
  public bool Check(TriggerVector other)
  {
    return (currentTriggers & other.CurrentTriggers) == other.CurrentTriggers;
  }

  public ETriggerType[] GetActiveTriggers()
  {
    List<ETriggerType> activeTriggers = new List<ETriggerType>();
    foreach (var kvp in triggerToBitIndex)
    {
      if ((currentTriggers & (1 << kvp.Value)) != 0)
      {
        activeTriggers.Add(kvp.Key);
      }
    }
    return activeTriggers.ToArray();
  }

  public bool HasActiveTrigger(ETriggerType trigger)
  {
    if (triggerToBitIndex.TryGetValue(trigger, out int bitIndex))
    {
      return (currentTriggers & (1 << bitIndex)) != 0;
    }
    return false;
  }

  public bool HasActiveTrigger()
  {
    return currentTriggers != 0;
  }

  public override string ToString()
  {
    return $"TriggerVector({currentTriggers})";
  }
}