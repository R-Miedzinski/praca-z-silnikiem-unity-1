using UnityEngine;

public enum Targetype
{
  Position,
  Units
}

public struct TargetInfo
{
  public Targetype Type { get; private set; }
  public Vector3 Position { get; private set; }
  public Unit[] Units { get; private set; }

  public static TargetInfo FromPosition(Vector3 position)
  {
    return new TargetInfo { Type = Targetype.Position, Position = position };
  }

  public static TargetInfo FromUnits(Unit[] units)
  {
    return new TargetInfo { Type = Targetype.Units, Units = units };
  }
}