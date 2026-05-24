using UnityEngine;
using System.Collections.Generic;
using System;

public static class IdToEffectMap
{
  // ******
  // TODO: Move to external file config
  private struct EffectConfig
  {
    public string id;
    public string className;
  }

  private static readonly EffectConfig[] effectConfigs = new[]
  {
    new EffectConfig { id = "deal_damage", className = "DealDamageEffect" },
    new EffectConfig { id = "debug_effect", className = "DebugEffect" },
    new EffectConfig { id = "damage_over_time", className = "DamageOverTimeEffect" },
    new EffectConfig { id = "slow", className = "SlowEffect" },
    // new EffectConfig { id = "heal", className = "HealEffect" },
  };
  // END TODO
  // ******

  private static readonly Dictionary<string, System.Func<Effect>> map = BuildMap();

  static private Dictionary<string, System.Func<Effect>> BuildMap()
  {
    var result = new Dictionary<string, System.Func<Effect>>();
    foreach (var config in effectConfigs)
    {
      var type = System.Type.GetType(config.className);
      if (type != null)
      {
        result[config.id] = () => (Effect)System.Activator.CreateInstance(type);
      }
    }
    return result;
  }

  static public string[] GetAllEffectIds()
  {
    var keys = new string[map.Keys.Count];
    map.Keys.CopyTo(keys, 0);
    return keys;
  }
  static public Effect GetEffectById(string id)
  {
    if (!map.TryGetValue(id, out var effectFactory))
    {
      Debug.LogError($"Effect with id {id} not found in IdToEffectMap.");
      return null;
    }

    return effectFactory();
  }

  static public Effect GetEffectById(string id, EffectParamsData parameters)
  {
    if (!map.TryGetValue(id, out var effectFactory))
    {
      Debug.LogError($"Effect with id {id} not found in IdToEffectMap.");
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