using UnityEngine;

abstract public class Effect
{
  public string Id { get { return id; } }
  public string EffectName { get { return effectName; } }
  public string Description { get { return description; } }

  private string id;
  private string effectName;
  private string description;

  abstract public void ApplyEffect(Unit caster, Unit target);

  public static T Clone<T, K>(K effect) where T : Effect where K : Effect
  {
    string serialized = JsonUtility.ToJson(effect);
    return JsonUtility.FromJson<T>(serialized);
  }
}