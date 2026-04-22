using UnityEngine;

public class ItemEffectOld : ScriptableObject
{
    public int id;
    public string description;
    public EItemUsageType type;
    public float cooldown;
    public EPassiveTrigger? passiveTrigger;
    private float lastUsedTime = -Mathf.Infinity;

    public ItemEffectOld(int id, string description, EItemUsageType type, float cooldown = 0f, EPassiveTrigger? passiveTrigger = null)
    {
        this.id = id;
        this.description = description;
        this.type = type;
        this.cooldown = cooldown;
        this.passiveTrigger = passiveTrigger;
    }

    public void ActivateEffect(object context)
    {
        float cooldownRemaining = lastUsedTime + cooldown - Time.time;

        if (cooldownRemaining <= 0f)
        {
            Debug.Log($"Activating effect: {description} with context: {context}");
            lastUsedTime = Time.time;
        }
        else
        {
            Debug.Log($"Effect {description} is on cooldown. Time remaining: {cooldownRemaining:F1} seconds");
        }
    }

}