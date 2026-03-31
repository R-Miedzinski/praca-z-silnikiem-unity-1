using UnityEngine;
using System.Collections.Generic;

public class PlayerCharacter : MonoBehaviour
{
    [SerializeField] private PlayerControls playerControls;
    [SerializeField] private Equipment equipment;

    private string playerName = "Hero";
    private int health = 100;
    private int armor = 0;
    private int baseDamage = 10;
    private float moveSpeed = 5f;
    private float cooldownReduction = 0f;

    public string PlayerName { get => playerName; set => playerName = value; }
    public int Health { get => health; set => health = value; }
    public int Armor { get => armor; set => armor = value; }
    public int BaseDamage { get => baseDamage; set => baseDamage = value; }
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public float CooldownReduction { get => cooldownReduction; set => cooldownReduction = value; }

    private void Awake()
    {
        PlayerControls.OnMove += HandleMove;
        PlayerControls.OnUseItem += HandleUseItem;
    }

    private void Start()
    {
        AddDebugItems();
    }

    private void OnDestroy()
    {
        PlayerControls.OnMove -= HandleMove;
        PlayerControls.OnUseItem -= HandleUseItem;
    }

    private void HandleMove(Vector2 movementInput)
    {
        gameObject.transform.Translate(movementInput * moveSpeed * Time.deltaTime);
        // TODO: rework trigger to work on events.
        if (movementInput != Vector2.zero)
        {
            equipment.TriggerPassiveEffects(EPassiveTrigger.OnMove);
        }
    }

    private void HandleUseItem(EEquipmentSlot itemSlot, EItemUsageType usageType)
    {
        equipment.UseItem(itemSlot, usageType);
    }

    private void AddDebugItems()
    {     // Add some debug items to the equipment for testing
        Item debugItem1 = new GameObject("Debug Item 1").AddComponent<Item>();
        debugItem1.id = 1;
        debugItem1.itemName = "Test Sword";
        debugItem1.description = "For testing purposes.";

        ItemEffect dealDamageEffect = new ItemEffect(1, "Deals damage.", EItemUsageType.Default, 0.2f);

        ItemEffect altDealDamageEffect = new ItemEffect(2, "Deals alternative damage.", EItemUsageType.Alternative, 2f);

        debugItem1.effects = new Dictionary<EItemUsageType, ItemEffect[]>
        {
            { EItemUsageType.Default, new ItemEffect[] { dealDamageEffect } },
            { EItemUsageType.Alternative, new ItemEffect[] { altDealDamageEffect } }
        };

        Item debugItem2 = new GameObject("Debug Item 2").AddComponent<Item>();
        debugItem2.id = 2;
        debugItem2.itemName = "Test Shoes";
        debugItem2.description = "For testing purposes.";
        
        ItemEffect onMoveEffect = new GameObject("OnMove effect").AddComponent<ItemEffect>();
        onMoveEffect.id = 3;
        onMoveEffect.description = "Does something when the player moves.";
        onMoveEffect.type = EItemUsageType.Passive;
        onMoveEffect.passiveTrigger = EPassiveTrigger.OnMove;
        onMoveEffect.cooldown = 5f;

        debugItem2.effects = new Dictionary<EItemUsageType, ItemEffect[]>
        {
            { EItemUsageType.Passive, new ItemEffect[] { onMoveEffect } }
        };

        equipment.EquipItem(EEquipmentSlot.RightHand, debugItem1);
        equipment.EquipItem(EEquipmentSlot.Slot1, debugItem2);
    }
}
