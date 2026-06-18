using System;
using System.Collections.Generic;

// Bazowa klasa dla stanów w grafie zachowania wroga.
// Każdy stan wie do jakich stanów może przejść i pod jakim warunkiem.
// Graf przejść buduje się w Enemy.BuildStateGraph() przez AddTransition().
public abstract class EnemyState
{
    private readonly List<(EnemyState target, Func<bool> condition)> transitions
        = new List<(EnemyState, Func<bool>)>();

    public void AddTransition(EnemyState target, Func<bool> condition)
    {
        transitions.Add((target, condition));
    }

    public EnemyState TryGetNextState()
    {
        foreach (var (target, condition) in transitions)
        {
            if (condition())
                return target;
        }
        return null;
    }

    public abstract void Execute(Enemy enemy);
}
