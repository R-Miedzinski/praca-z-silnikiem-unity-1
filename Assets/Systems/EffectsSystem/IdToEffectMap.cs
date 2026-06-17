using UnityEngine;
using System.Collections.Generic;
using System;

public static class IdToEffectMap
{
  private const string EffectConfigsPath = "Config/EffectDefinitionsDataObject";
  private static readonly Dictionary<string, Func<Effect>> map = BuildMap();

  private static Dictionary<string, Func<Effect>> BuildMap()
  {
    var result = new Dictionary<string, Func<Effect>>();
    EffectDefinitionsDataObject definitions = Resources.Load<EffectDefinitionsDataObject>(EffectConfigsPath);

    if (definitions == null)
    {
      Debug.LogWarning($"Effect definitions asset not found at Resources/{EffectConfigsPath}.");
      return result;
    }

    if (definitions.EffectConfigs == null || definitions.EffectConfigs.Length == 0)
    {
      Debug.LogWarning($"No effect configs found in Resources/{EffectConfigsPath}.");
      return result;
    }

    foreach (EffectConfig config in definitions.EffectConfigs)
    {
      Type type = Type.GetType(config.ClassName);
      if (type == null)
      {
        Debug.LogError($"Effect type '{config.ClassName}' not found for effect id '{config.Id}'.");
        continue;
      }

      if (!typeof(Effect).IsAssignableFrom(type))
      {
        Debug.LogError($"Type '{config.ClassName}' is not assignable to Effect for effect id '{config.Id}'.");
        continue;
      }

      if (result.ContainsKey(config.Id))
      {
        Debug.LogWarning($"Duplicate effect id '{config.Id}' found. Overwriting previous mapping.");
      }

      result[config.Id] = () => (Effect)Activator.CreateInstance(type);
    }

    return result;
  }

  public static string[] GetAllEffectIds()
  {
    var keys = new string[map.Keys.Count];
    map.Keys.CopyTo(keys, 0);
    return keys;
  }

  public static Effect GetEffectById(string id)
  {
    if (!map.TryGetValue(id, out var effectFactory))
    {
      Debug.LogError($"Effect with id {id} not found in IdToEffectMap.");
      return null;
    }

    return effectFactory();
  }

  public static Effect GetEffectById(string id, string[] parameters)
  {
    if (!map.TryGetValue(id, out var effectFactory))
    {
      Debug.LogError($"Effect with id {id} not found in IdToEffectMap.");
      return null;
    }

    var effect = effectFactory();
    if (effect is not IParametrizedEffect)
    {
      return effect;
    }

    IParametrizedEffect parametrizedEffect = effect as IParametrizedEffect;
    parametrizedEffect.SetParameters(parameters);
    return effect;
  }
}