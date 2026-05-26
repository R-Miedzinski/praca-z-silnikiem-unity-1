using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public static class IdToEffectMap
{
  private const string EffectConfigsPath = "Assets/Systems/EffectsSystem/Settings/EffectDefinitionsDataObject.asset";
  private static readonly Dictionary<string, System.Func<Effect>> map = BuildMap();

  static private Dictionary<string, System.Func<Effect>> BuildMap()
  {
    EffectConfig[] effectConfigs = AssetDatabase.LoadAssetAtPath<EffectDefinitionsDataObject>(EffectConfigsPath).EffectConfigs;

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