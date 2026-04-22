using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public delegate void Move(Vector2 movementInput);
    public static event Move OnMove;

    public delegate void UseItem(ESlotsInEquipment itemSlot, EItemUsageType usageType);
    public static event UseItem OnUseItem;

    private PlayerCharacterInput playerInput;

    private void Awake()
    {
        playerInput = new PlayerCharacterInput();
    }

    private void OnEnable()
    {
        playerInput.Enable();
        playerInput.Player.UseItem1.performed += HandleUseItem1;
        playerInput.Player.UseItem2.performed += HandleUseItem2;
        playerInput.Player.UseItem3.performed += HandleUseItem3;
        playerInput.Player.UseAltHandRight.performed += HandleUseAltHandRight;
        playerInput.Player.UseHandRight.performed += HandleUseHandRight;
        playerInput.Player.UseAltHandLeft.performed += HandleUseAltHandLeft;
        playerInput.Player.UseHandLeft.performed += HandleUseHandLeft;

        playerInput.Player.DebugKey.performed += HandleDebugKey;
    }

    private void OnDisable()
    {
        playerInput.Disable();
        playerInput.Player.UseItem1.performed -= HandleUseItem1;
        playerInput.Player.UseItem2.performed -= HandleUseItem2;
        playerInput.Player.UseItem3.performed -= HandleUseItem3;
        playerInput.Player.UseAltHandRight.performed -= HandleUseAltHandRight;
        playerInput.Player.UseHandRight.performed -= HandleUseHandRight;
        playerInput.Player.UseAltHandLeft.performed -= HandleUseAltHandLeft;
        playerInput.Player.UseHandLeft.performed -= HandleUseHandLeft;

        playerInput.Player.DebugKey.performed -= HandleDebugKey;
    }

    private void Update()
    {
        HandleMove();
    }

    private void HandleMove()
    {
        Vector2 movementInput = playerInput.Player.Move.ReadValue<Vector2>();
        OnMove?.Invoke(movementInput);
    }

    private void HandleUseItem1(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        ESlotsInEquipment itemSlot = ESlotsInEquipment.Slot1;
        OnUseItem?.Invoke(itemSlot, EItemUsageType.Default);
    }

    private void HandleUseItem2(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        ESlotsInEquipment itemSlot = ESlotsInEquipment.Slot2;
        OnUseItem?.Invoke(itemSlot, EItemUsageType.Default);
    }

    private void HandleUseItem3(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        ESlotsInEquipment itemSlot = ESlotsInEquipment.Slot3;
        OnUseItem?.Invoke(itemSlot, EItemUsageType.Default);
    }

    private void HandleUseAltHandRight(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        ESlotsInEquipment itemSlot = ESlotsInEquipment.RightHand;
        OnUseItem?.Invoke(itemSlot, EItemUsageType.Alternative);
    }

    private void HandleUseHandRight(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        ESlotsInEquipment itemSlot = ESlotsInEquipment.RightHand;
        OnUseItem?.Invoke(itemSlot, EItemUsageType.Default);
    }

    private void HandleUseAltHandLeft(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        ESlotsInEquipment itemSlot = ESlotsInEquipment.LeftHand;
        OnUseItem?.Invoke(itemSlot, EItemUsageType.Alternative);
    }

    private void HandleUseHandLeft(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        ESlotsInEquipment itemSlot = ESlotsInEquipment.LeftHand;
        OnUseItem?.Invoke(itemSlot, EItemUsageType.Default);
    }

    private void HandleDebugKey(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Debug key pressed!");
            Item debugItem = ItemsDatabase.Instance.GetItemById("debug_item");
            if (debugItem != null)
            {
                Debug.Log("Debug item found in database.");
                Debug.Log($"Using debug item: {debugItem.ItemName}");
            }
            else
            {
                Debug.LogWarning("Debug item not found in the database.");
            }
        }

        Debug.Log(context);
    }
}
