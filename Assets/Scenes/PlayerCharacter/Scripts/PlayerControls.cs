using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public delegate void Move(Vector2 movementInput);
    public static event Move OnMove;

    public delegate void UseItem(ESlotsInEquipment itemSlot, EItemUsageType usageType);
    public static event UseItem OnUseItem;

    private PlayerCharacterInput playerInput;
    private bool isUsingHoldItem1;
    private bool isUsingHoldItem2;
    private bool isUsingHoldItem3;
    private bool isUsingHoldHandRight;
    private bool isUsingHoldHandLeft;

    private void Awake()
    {
        playerInput = new PlayerCharacterInput();
        isUsingHoldItem1 = false;
        isUsingHoldItem2 = false;
        isUsingHoldItem3 = false;
        isUsingHoldHandRight = false;
        isUsingHoldHandLeft = false;
    }

    private void OnEnable()
    {
        playerInput.Enable();

        // ******
        // Enable Item 1 actions
        // ******
        playerInput.Player.UseHoldItem1.performed += HandleUseHoldItem1Performed;
        playerInput.Player.UseHoldItem1.canceled += HandleUseHoldItem1Cancelled;
        playerInput.Player.UseAltItem1.performed += HandleUseAltItem1;
        playerInput.Player.UseItem1.performed += HandleUseItem1;
        // ******
        // Enable Item 2 actions
        // ******
        playerInput.Player.UseHoldItem2.performed += HandleUseHoldItem2Performed;
        playerInput.Player.UseHoldItem2.canceled += HandleUseHoldItem2Cancelled;
        playerInput.Player.UseAltItem2.performed += HandleUseAltItem2;
        playerInput.Player.UseItem2.performed += HandleUseItem2;
        // ******
        // Enable Item 3 actions
        // ******
        playerInput.Player.UseHoldItem3.performed += HandleUseHoldItem3Performed;
        playerInput.Player.UseHoldItem3.canceled += HandleUseHoldItem3Cancelled;
        playerInput.Player.UseAltItem3.performed += HandleUseAltItem3;
        // ******
        // Enable Right Hand actions
        // ******
        playerInput.Player.UseHoldHandRight.performed += HandleUseHoldHandRightPerformed;
        playerInput.Player.UseHoldHandRight.canceled += HandleUseHoldHandRightCancelled;
        playerInput.Player.UseAltHandRight.performed += HandleUseAltHandRight;
        playerInput.Player.UseHandRight.performed += HandleUseHandRight;
        // ******
        // Enable Left Hand actions
        // ******
        playerInput.Player.UseHoldHandLeft.performed += HandleUseHoldHandLeftPerformed;
        playerInput.Player.UseHoldHandLeft.canceled += HandleUseHoldHandLeftCancelled;
        playerInput.Player.UseAltHandLeft.performed += HandleUseAltHandLeft;
        playerInput.Player.UseHandLeft.performed += HandleUseHandLeft;

        playerInput.Player.DebugKey.performed += HandleDebugKey;
    }

    private void OnDisable()
    {
        playerInput.Disable();
        // ******
        // Disable Item 1 actions
        // ******
        playerInput.Player.UseHoldItem1.performed -= HandleUseHoldItem1Performed;
        playerInput.Player.UseHoldItem1.canceled -= HandleUseHoldItem1Cancelled;
        playerInput.Player.UseAltItem1.performed -= HandleUseAltItem1;
        playerInput.Player.UseItem1.performed -= HandleUseItem1;
        // ******
        // Disable Item 2 actions
        // ******
        playerInput.Player.UseHoldItem2.performed -= HandleUseHoldItem2Performed;
        playerInput.Player.UseHoldItem2.canceled -= HandleUseHoldItem2Cancelled;
        playerInput.Player.UseAltItem2.performed -= HandleUseAltItem2;
        playerInput.Player.UseItem2.performed -= HandleUseItem2;
        // ******
        // Disable Item 3 actions
        // ******
        playerInput.Player.UseHoldItem3.performed -= HandleUseHoldItem3Performed;
        playerInput.Player.UseHoldItem3.canceled -= HandleUseHoldItem3Cancelled;
        playerInput.Player.UseAltItem3.performed -= HandleUseAltItem3;
        playerInput.Player.UseItem3.performed -= HandleUseItem3;
        // ******
        // Disable Right Hand actions
        // ******
        playerInput.Player.UseHoldHandRight.performed -= HandleUseHoldHandRightPerformed;
        playerInput.Player.UseHoldHandRight.canceled -= HandleUseHoldHandRightCancelled;
        playerInput.Player.UseAltHandRight.performed -= HandleUseAltHandRight;
        playerInput.Player.UseHandRight.performed -= HandleUseHandRight;
        // ******
        // Disable Left Hand actions
        // ******
        playerInput.Player.UseHoldHandLeft.performed -= HandleUseHoldHandLeftPerformed;
        playerInput.Player.UseHoldHandLeft.canceled -= HandleUseHoldHandLeftCancelled;
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

    // ******
    // Item 1
    // ******
    private void HandleUseHoldItem1Performed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        isUsingHoldItem1 = true;
        ESlotsInEquipment itemSlot = ESlotsInEquipment.Slot1;
        OnUseItem?.Invoke(itemSlot, EItemUsageType.Hold);
    }

    private void HandleUseHoldItem1Cancelled(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (!isUsingHoldItem1)
        {
            return;
        }

        isUsingHoldItem1 = false;
        ESlotsInEquipment itemSlot = ESlotsInEquipment.Slot1;
        OnUseItem?.Invoke(itemSlot, EItemUsageType.Release);
    }

    private void HandleUseAltItem1(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (isUsingHoldItem1)
        {
            return;
        }

        ESlotsInEquipment itemSlot = ESlotsInEquipment.Slot1;
        OnUseItem?.Invoke(itemSlot, EItemUsageType.Alternative);
    }

    private void HandleUseItem1(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (isUsingHoldItem1)
        {
            return;
        }

        ESlotsInEquipment itemSlot = ESlotsInEquipment.Slot1;
        OnUseItem?.Invoke(itemSlot, EItemUsageType.Default);
    }

    // ******
    // Item 2
    // ******
    private void HandleUseHoldItem2Performed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        isUsingHoldItem2 = true;
        ESlotsInEquipment itemSlot = ESlotsInEquipment.Slot2;
        OnUseItem?.Invoke(itemSlot, EItemUsageType.Hold);
    }

    private void HandleUseHoldItem2Cancelled(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (!isUsingHoldItem2)
        {
            return;
        }

        isUsingHoldItem2 = false;
        ESlotsInEquipment itemSlot = ESlotsInEquipment.Slot2;
        OnUseItem?.Invoke(itemSlot, EItemUsageType.Release);
    }

    private void HandleUseAltItem2(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (isUsingHoldItem2)
        {
            return;
        }

        ESlotsInEquipment itemSlot = ESlotsInEquipment.Slot2;
        OnUseItem?.Invoke(itemSlot, EItemUsageType.Alternative);
    }
    private void HandleUseItem2(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (isUsingHoldItem2)
        {
            return;
        }

        ESlotsInEquipment itemSlot = ESlotsInEquipment.Slot2;
        OnUseItem?.Invoke(itemSlot, EItemUsageType.Default);
    }

    // ******
    // Item 3
    // ******
    private void HandleUseHoldItem3Performed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        isUsingHoldItem3 = true;
        ESlotsInEquipment itemSlot = ESlotsInEquipment.Slot3;
        OnUseItem?.Invoke(itemSlot, EItemUsageType.Hold);
    }

    private void HandleUseHoldItem3Cancelled(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (!isUsingHoldItem3)
        {
            return;
        }

        isUsingHoldItem3 = false;
        ESlotsInEquipment itemSlot = ESlotsInEquipment.Slot3;
        OnUseItem?.Invoke(itemSlot, EItemUsageType.Release);
    }

    private void HandleUseAltItem3(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (isUsingHoldItem3)
        {
            return;
        }

        ESlotsInEquipment itemSlot = ESlotsInEquipment.Slot3;
        OnUseItem?.Invoke(itemSlot, EItemUsageType.Alternative);
    }

    private void HandleUseItem3(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (isUsingHoldItem3)
        {
            return;
        }

        ESlotsInEquipment itemSlot = ESlotsInEquipment.Slot3;
        OnUseItem?.Invoke(itemSlot, EItemUsageType.Default);
    }

    // ******
    // Right Hand
    // ******
    private void HandleUseHoldHandRightPerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        isUsingHoldHandRight = true;
        ESlotsInEquipment itemSlot = ESlotsInEquipment.RightHand;
        OnUseItem?.Invoke(itemSlot, EItemUsageType.Hold);
    }

    private void HandleUseHoldHandRightCancelled(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (!isUsingHoldHandRight)
        {
            return;
        }

        isUsingHoldHandRight = false;
        ESlotsInEquipment itemSlot = ESlotsInEquipment.RightHand;
        OnUseItem?.Invoke(itemSlot, EItemUsageType.Release);
    }

    private void HandleUseAltHandRight(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (isUsingHoldHandRight)
        {
            return;
        }

        ESlotsInEquipment itemSlot = ESlotsInEquipment.RightHand;
        OnUseItem?.Invoke(itemSlot, EItemUsageType.Alternative);
    }

    private void HandleUseHandRight(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (isUsingHoldHandRight)
        {
            return;
        }

        ESlotsInEquipment itemSlot = ESlotsInEquipment.RightHand;
        OnUseItem?.Invoke(itemSlot, EItemUsageType.Default);
    }

    // ******
    // Left Hand
    // ******
    private void HandleUseHoldHandLeftPerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        isUsingHoldHandLeft = true;
        ESlotsInEquipment itemSlot = ESlotsInEquipment.LeftHand;
        OnUseItem?.Invoke(itemSlot, EItemUsageType.Hold);
    }

    private void HandleUseHoldHandLeftCancelled(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (!isUsingHoldHandLeft)
        {
            return;
        }

        isUsingHoldHandLeft = false;
        ESlotsInEquipment itemSlot = ESlotsInEquipment.LeftHand;
        OnUseItem?.Invoke(itemSlot, EItemUsageType.Release);
    }

    private void HandleUseAltHandLeft(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (isUsingHoldHandLeft)
        {
            return;
        }

        ESlotsInEquipment itemSlot = ESlotsInEquipment.LeftHand;
        OnUseItem?.Invoke(itemSlot, EItemUsageType.Alternative);
    }

    private void HandleUseHandLeft(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (isUsingHoldHandLeft)
        {
            return;
        }

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
