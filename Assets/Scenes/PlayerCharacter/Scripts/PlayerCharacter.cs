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

    private void HandleUseItem(ESlotsInEquipment itemSlot, EItemUsageType usageType)
    {
        equipment.UseItem(itemSlot, usageType);
    }
}
