using UnityEngine;

abstract public class TargetingStrategy
{
  private static Material runtimeDebugLineMaterial;

  public abstract void Target(TargetingMode targetingMode, Vector3 target, Unit caster, Effect[] effects);

  protected void ApplyEffectsToUnit(Unit target, Unit caster, Effect[] effects)
  {
    foreach (var effect in effects)
    {
      effect.ApplyEffect(caster, target);
    }
  }

  // TODO: review and rework the visualizations below
  protected void DrawDebugLine(Vector3 from, Vector3 to, Color color, float duration)
  {
    var lineObject = new GameObject("TargetingDebugLine");
    var lineRenderer = lineObject.AddComponent<LineRenderer>();
    lineRenderer.positionCount = 2;
    lineRenderer.useWorldSpace = true;
    lineRenderer.startWidth = 0.05f;
    lineRenderer.endWidth = 0.05f;
    lineRenderer.numCapVertices = 2;
    var material = GetRuntimeDebugLineMaterial();
    if (material != null)
    {
      lineRenderer.material = material;
    }
    lineRenderer.startColor = color;
    lineRenderer.endColor = color;
    lineRenderer.SetPosition(0, from);
    lineRenderer.SetPosition(1, to);

    Object.Destroy(lineObject, Mathf.Max(duration, 0.02f));
  }

  private static Material GetRuntimeDebugLineMaterial()
  {
    if (runtimeDebugLineMaterial != null)
    {
      return runtimeDebugLineMaterial;
    }

    Shader shader = Shader.Find("Sprites/Default");
    if (shader == null)
    {
      shader = Shader.Find("Unlit/Color");
    }

    if (shader == null)
    {
      return null;
    }

    runtimeDebugLineMaterial = new Material(shader);
    return runtimeDebugLineMaterial;
  }
}