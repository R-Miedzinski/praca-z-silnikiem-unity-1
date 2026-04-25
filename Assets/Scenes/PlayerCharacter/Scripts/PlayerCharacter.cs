using UnityEngine;
using System.Collections.Generic;

public class PlayerCharacter : Unit
{
    [SerializeField] private PlayerControls playerControls;
    [SerializeField] private Equipment equipment;

    public static PlayerCharacter Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        PlayerControls.OnMove += HandleMove;
        PlayerControls.OnUseItem += HandleUseItem;
    }

    private void Start()
    {
        CurrentHealth = maxHealth;
    }

    private void OnDestroy()
    {
        PlayerControls.OnMove -= HandleMove;
        PlayerControls.OnUseItem -= HandleUseItem;
    }

    public override void TakeDamage(float amount)
    {
        CurrentHealth -= amount;

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    public override void Heal(float amount)
    {
        CurrentHealth += amount;
    }

    public override void Die()
    {
        Debug.Log($"{UnitName} has died.");
    }

    private void HandleMove(Vector2 movementInput)
    {
        gameObject.transform.Translate(movementInput * movementSpeed * Time.deltaTime);
        if (movementInput != Vector2.zero)
        {
            ItemTriggerEventSystem.Instance.SendTriggerEvent(EPassiveTrigger.OnMove, null);
        }
    }

    private void HandleUseItem(ESlotsInEquipment itemSlot, EItemUsageType usageType)
    {
        Debug.Log($"Use item in slot {itemSlot} with usage type {usageType}");
        // equipment.UseItem(itemSlot, usageType);
    }
}
