using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public static class IdToEffectMap
{
  private const string EffectConfigsPath = "Assets/Systems/EffectsSystem/Settings/EffectDefinitionsDataObject.asset";
  private static readonly Dictionary<string, Func<Effect>> map = BuildMap();

  static private Dictionary<string, Func<Effect>> BuildMap()
  {
    EffectConfig[] effectConfigs = AssetDatabase.LoadAssetAtPath<EffectDefinitionsDataObject>(EffectConfigsPath).EffectConfigs;

    var result = new Dictionary<string, Func<Effect>>();
    foreach (var config in effectConfigs)
    {
      var type = Type.GetType(config.ClassName);
      if (type != null)
      {
        result[config.Id] = () => (Effect)Activator.CreateInstance(type);
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