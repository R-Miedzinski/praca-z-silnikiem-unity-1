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

    public static PlayerCharacter Instance { get; private set; }
    public string PlayerName { get => playerName; set => playerName = value; }
    public int Health { get => health; set => health = value; }
    public int Armor { get => armor; set => armor = value; }
    public int BaseDamage { get => baseDamage; set => baseDamage = value; }
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public float CooldownReduction { get => cooldownReduction; set => cooldownReduction = value; }

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
        if (movementInput != Vector2.zero)
        {
            ItemTriggerEventSystem.Instance.SendTriggerEvent(EPassiveTrigger.OnMove, null);
        }
    }

    private void HandleUseItem(EEquipmentSlot itemSlot, EItemUsageType usageType)
    {
        equipment.UseItem(itemSlot, usageType);
    }

    private void AddDebugItems()
    {     // Add some debug items to the equipment for testing
        Item debugItem1 = ScriptableObject.CreateInstance<Item>();
        debugItem1.id = 1;
        debugItem1.itemName = "Test Sword";
        debugItem1.description = "For testing purposes.";

        ItemEffect dealDamageEffect = ScriptableObject.CreateInstance<ItemEffect>();
        dealDamageEffect.id = 1;
        dealDamageEffect.description = "Deals damage.";
        dealDamageEffect.type = EItemUsageType.Default;
        dealDamageEffect.cooldown = 0.2f;

        ItemEffect altDealDamageEffect = ScriptableObject.CreateInstance<ItemEffect>();
        altDealDamageEffect.id = 2;
        altDealDamageEffect.description = "Deals alternative damage.";
        altDealDamageEffect.type = EItemUsageType.Alternative;
        altDealDamageEffect.cooldown = 2f;

        debugItem1.effects = new Dictionary<EItemUsageType, List<ItemEffect>>
        {
            { EItemUsageType.Default, new List<ItemEffect> { dealDamageEffect } },
            { EItemUsageType.Alternative, new List<ItemEffect> { altDealDamageEffect } }
        };

        Item debugItem2 = ScriptableObject.CreateInstance<Item>();
        debugItem2.id = 2;
        debugItem2.itemName = "Test Shoes";
        debugItem2.description = "For testing purposes.";

        ItemEffect onMoveEffect = ScriptableObject.CreateInstance<ItemEffect>();
        onMoveEffect.id = 3;
        onMoveEffect.description = "Does something when the player moves.";
        onMoveEffect.type = EItemUsageType.Passive;
        onMoveEffect.passiveTrigger = EPassiveTrigger.OnMove;
        onMoveEffect.cooldown = 5f;

        debugItem2.effects = new Dictionary<EItemUsageType, List<ItemEffect>>
        {
            { EItemUsageType.Passive, new List<ItemEffect> { onMoveEffect } }
        };

        equipment.EquipItem(EEquipmentSlot.RightHand, debugItem1);
        equipment.EquipItem(EEquipmentSlot.Slot1, debugItem2);
    }
}
