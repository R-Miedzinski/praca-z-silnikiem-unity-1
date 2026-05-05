using UnityEngine;
using System.Collections.Generic;

public static class IdToEffectMap
{
  static private Dictionary<string, System.Func<Effect>> map = new Dictionary<string, System.Func<Effect>>
  {
    { "deal_damage", () => new DealDamageEffect() },
    { "debug_effect", () => new DebugEffect() },
    { "damage_over_time", () => new DamageOverTimeEffect() },
    { "slow", () => new SlowEffect() },
    // { "heal", () => new HealEffect() },
  };

  static public Effect GetEffectById(string id)
  {
    if (!map.TryGetValue(id, out var effectFactory))
    {
      UnityEngine.Debug.LogError($"Effect with id {id} not found in IdToEffectMap.");
      return null;
    }

    return effectFactory();
  }

  static public Effect GetEffectById(string id, EffectParamsData parameters)
  {
    if (!map.TryGetValue(id, out var effectFactory))
    {
      UnityEngine.Debug.LogError($"Effect with id {id} not found in IdToEffectMap.");
      return null;
    }

    var effect = effectFactory();
    if (!(effect is IParametrizedEffect))
    {
      return effect;
    }

    IParametrizedEffect parametrizedEffect = effect as IParametrizedEffect;
    parametrizedEffect.SetParameters(parameters);
    return effect;
  }
}