using UnityEngine;
using System.Collections.Generic;

public class PlayerCharacter : Unit
{
    public float Heat { get { return heat; } set { heat = Mathf.Clamp(value, 0, maxHeat); } }
    public TargetingWidget TargetingWidget { get { return targetingWidget; } }
    [SerializeField] private float maxHeat;
    private float heat;
    [SerializeField] private TargetingWidget targetingWidget;
    // ******
    // Player Components
    // ******
    private PlayerControls playerControls;
    private Equipment equipment;
    private Collider2D playerCollider;
    private InteractionCollider interactionCollider;

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

        equipment = GetComponent<Equipment>();
        playerControls = GetComponent<PlayerControls>();
        playerCollider = GetComponent<Collider2D>();
        interactionCollider = transform.Find("InteractionCollider").GetComponent<InteractionCollider>();

        playerControls.OnMove += HandleMove;
        playerControls.OnUseItem += HandleUseItem;
        playerControls.OnInteract += HandleInteract;
        playerControls.OnSwapLoadout += HandleSwapLoadout;
    }

    private void Start()
    {
        CurrentHealth = maxHealth;
        EquipDebugItem();
    }

    private void Update()
    {
        frontDirection = (targetingWidget.transform.position - transform.position).normalized;
    }

    private void OnDestroy()
    {
        playerControls.OnMove -= HandleMove;
        playerControls.OnUseItem -= HandleUseItem;
        playerControls.OnInteract -= HandleInteract;
        playerControls.OnSwapLoadout -= HandleSwapLoadout;
    }

    public override void TakeDamage(float amount)
    {
        ItemTriggerEventSystem.Instance.SendTriggerEvent(ETriggerType.OnDamageTaken, new TakeDamageEventContext(targetedPosition: targetingWidget.transform.position, changeValue: amount));
        base.TakeDamage(amount);
    }

    public override void Heal(float amount)
    {
        // ItemTriggerEventSystem.Instance.SendTriggerEvent(ETriggerType.OnHeal, new HealEventContext(targetedPosition: targetingWidget.transform.position, changeValue: amount));
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
            // ItemTriggerEventSystem.Instance.SendTriggerEvent(ETriggerType.OnHeatGain, new HeatEventContext(targetedPosition: targetingWidget.transform.position, changeValue: heatDelta));
        }

        Heat += heatDelta;
    }

    private void HandleMove(Vector2 movementInput)
    {
        Vector2 movementInput2D = new(movementInput.x, movementInput.y);
        float movementMagnitude = movementInput2D.magnitude * MovementSpeed * Time.deltaTime;

        if (movementInput2D == Vector2.zero)
            return;

        movementInput2D = CorrectMovementInputForCollisions(movementInput2D, movementMagnitude);

        gameObject.transform.Translate(movementInput2D.normalized * movementMagnitude);
        ItemTriggerEventSystem.Instance.SendTriggerEvent(
            ETriggerType.OnMove,
            new MoveEventContext(targetedPosition: targetingWidget.transform.position, changeValue: movementMagnitude)
        );
    }
    private void HandleUseItem(ESlotsInEquipment itemSlot, EItemUsageType usageType)
    {
        Vector3 targetedPosition = targetingWidget.transform.position;
        equipment.UseItem(itemSlot, usageType, targetedPosition);
    }

    private void HandleInteract()
    {
        interactionCollider.Interact(this);
    }

    private void HandleSwapLoadout()
    {
        equipment.SwapLoadout();
    }

    private Vector2 CorrectMovementInputForCollisions(Vector2 movementInput2D, float movementMagnitude)
    {
        float radius = playerCollider.bounds.extents.x;
        float castDistance = movementMagnitude * 1.2f; // slight buffer added to ensure collision is detected

        RaycastHit2D hit = Physics2D.CircleCast(
            transform.position,
            radius,
            movementInput2D.normalized,
            castDistance,
            LayerMask.GetMask(new string[] { "Environment" })
        );

        Debug.DrawRay(transform.position, movementInput2D.normalized * (radius + castDistance), Color.red);

        if (hit.collider != null)
        {
            Debug.DrawRay(hit.point, hit.normal, Color.blue);

            float dot = Vector2.Dot(movementInput2D.normalized, hit.normal);
            if (dot < 0)
            {
                Vector2 slideDirection = (movementInput2D.normalized - dot * hit.normal).normalized;
                movementInput2D = slideDirection * movementInput2D.magnitude;

                Debug.DrawRay(transform.position, slideDirection * movementMagnitude, Color.green);
            }
        }

        return movementInput2D;
    }

    private void EquipDebugItem()
    {
        Item debugItem = ItemsDatabase.Instance.GetItemById("debug_item");
        Item debugItem2 = ItemsDatabase.Instance.GetItemById("debug_item_object");

        equipment.EquipItem(ESlotsInEquipment.RightHand, debugItem);
        equipment.SwapLoadout();
        equipment.EquipItem(ESlotsInEquipment.RightHand, debugItem2);
    }
}
