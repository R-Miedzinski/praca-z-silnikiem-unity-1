using UnityEngine;
using System.Collections.Generic;

public class PlayerCharacter : Unit
{
    public float Heat { get { return heat; } set { heat = Mathf.Clamp(value, 0, maxHeat); } }
    private float heat;
    [SerializeField] private float maxHeat;
    [SerializeField] private PlayerControls playerControls;
    [SerializeField] private Equipment equipment;
    [SerializeField] private TargettingWidget targettingWidget;

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
        EquipDebugItem();
    }

    private void OnDestroy()
    {
        PlayerControls.OnMove -= HandleMove;
        PlayerControls.OnUseItem -= HandleUseItem;
    }

    public override void TakeDamage(float amount)
    {
        ItemTriggerEventSystem.Instance.SendTriggerEvent(ETriggerType.OnDamageTaken, new ItemTriggerEventContext(targettedPosition: targettingWidget.transform.position, changeValue: amount));
        CurrentHealth -= amount;

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    public override void Heal(float amount)
    {
        ItemTriggerEventSystem.Instance.SendTriggerEvent(ETriggerType.OnHeal, new ItemTriggerEventContext(targettedPosition: targettingWidget.transform.position, changeValue: amount));
        CurrentHealth += amount;
    }

    public override void Die()
    {
        Debug.Log($"{UnitName} has died.");
    }

    public void ChangeHeat(float heatDelta)
    {
        if (heatDelta > 0)
        {
            ItemTriggerEventSystem.Instance.SendTriggerEvent(ETriggerType.OnHeatGain, new ItemTriggerEventContext(targettedPosition: targettingWidget.transform.position, changeValue: heatDelta));
        }

        Heat += heatDelta;
    }

    private void HandleMove(Vector2 movementInput)
    {
        gameObject.transform.Translate(movementInput * MovementSpeed * Time.deltaTime);
        if (movementInput != Vector2.zero)
        {
            float movementMagnitude = movementInput.magnitude * MovementSpeed * Time.deltaTime;
            ItemTriggerEventSystem.Instance.SendTriggerEvent(ETriggerType.OnMove, new ItemTriggerEventContext(targettedPosition: targettingWidget.transform.position, changeValue: movementMagnitude));
        }
    }

    private void HandleUseItem(ESlotsInEquipment itemSlot, EItemUsageType usageType)
    {
        Vector3 targettedPosition = targettingWidget.transform.position;
        equipment.UseItem(itemSlot, usageType, targettedPosition);
    }

    private void EquipDebugItem()
    {
        Item debugItem = ItemsDatabase.Instance.GetItemById("debug_item");

        equipment.EquipItem(ESlotsInEquipment.RightHand, debugItem);
    }
}
