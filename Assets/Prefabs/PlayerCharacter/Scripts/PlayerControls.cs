using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    public delegate void Move(Vector2 movementInput);
    public event Move OnMove;

    public delegate void UseItem(ESlotsInEquipment itemSlot, EItemUsageType usageType);
    public event UseItem OnUseItem;

    public delegate void Interact();
    public event Interact OnInteract;

    public delegate void SwapLoadout();
    public event SwapLoadout OnSwapLoadout;

    private InputActionMap playerInput;
    private bool isUsingHoldItem1;
    private bool isUsingHoldItem2;
    private bool isUsingHoldItem3;
    private bool isUsingHoldHandRight;
    private bool isUsingHoldHandLeft;

    private void Awake()
    {
        playerInput = InputSystem.actions.FindActionMap("Player");
        isUsingHoldItem1 = false;
        isUsingHoldItem2 = false;
        isUsingHoldItem3 = false;
        isUsingHoldHandRight = false;
        isUsingHoldHandLeft = false;
    }

    private void OnEnable()
    {
        // ******
        // Enable Item 1 actions
        // ******
        playerInput.FindAction("UseHoldItem1").performed += HandleUseHoldItem1Performed;
        playerInput.FindAction("UseHoldItem1").canceled += HandleUseHoldItem1Cancelled;
        playerInput.FindAction("UseAltItem1").performed += HandleUseAltItem1;
        playerInput.FindAction("UseItem1").performed += HandleUseItem1;
        // ******
        // Enable Item 2 actions
        // ******
        playerInput.FindAction("UseHoldItem2").performed += HandleUseHoldItem2Performed;
        playerInput.FindAction("UseHoldItem2").canceled += HandleUseHoldItem2Cancelled;
        playerInput.FindAction("UseAltItem2").performed += HandleUseAltItem2;
        playerInput.FindAction("UseItem2").performed += HandleUseItem2;
        // ******
        // Enable Item 3 actions
        // ******
        playerInput.FindAction("UseHoldItem3").performed += HandleUseHoldItem3Performed;
        playerInput.FindAction("UseHoldItem3").canceled += HandleUseHoldItem3Cancelled;
        playerInput.FindAction("UseAltItem3").performed += HandleUseAltItem3;
        // ******
        // Enable Right Hand actions
        // ******
        playerInput.FindAction("UseHoldHandRight").performed += HandleUseHoldHandRightPerformed;
        playerInput.FindAction("UseHoldHandRight").canceled += HandleUseHoldHandRightCancelled;
        playerInput.FindAction("UseAltHandRight").performed += HandleUseAltHandRight;
        playerInput.FindAction("UseHandRight").performed += HandleUseHandRight;
        // ******
        // Enable Left Hand actions
        // ******
        playerInput.FindAction("UseHoldHandLeft").performed += HandleUseHoldHandLeftPerformed;
        playerInput.FindAction("UseHoldHandLeft").canceled += HandleUseHoldHandLeftCancelled;
        playerInput.FindAction("UseAltHandLeft").performed += HandleUseAltHandLeft;
        playerInput.FindAction("UseHandLeft").performed += HandleUseHandLeft;
        // ******
        // Enable Interact actions
        // ******
        playerInput.FindAction("Interact").performed += HandleInteract;
        // ******
        // Enable Swap Loadout actions
        // ******
        playerInput.FindAction("SwapLoadout").performed += HandleSwapLoadout;
    }

    private void OnDisable()
    {
        // playerInput.Disable();
        // ******
        // Disable Item 1 actions
        // ******
        playerInput.FindAction("UseHoldItem1").performed -= HandleUseHoldItem1Performed;
        playerInput.FindAction("UseHoldItem1").canceled -= HandleUseHoldItem1Cancelled;
        playerInput.FindAction("UseAltItem1").performed -= HandleUseAltItem1;
        playerInput.FindAction("UseItem1").performed -= HandleUseItem1;
        // ******
        // Disable Item 2 actions
        // ******
        playerInput.FindAction("UseHoldItem2").performed -= HandleUseHoldItem2Performed;
        playerInput.FindAction("UseHoldItem2").canceled -= HandleUseHoldItem2Cancelled;
        playerInput.FindAction("UseAltItem2").performed -= HandleUseAltItem2;
        playerInput.FindAction("UseItem2").performed -= HandleUseItem2;
        // ******
        // Disable Item 3 actions
        // ******
        playerInput.FindAction("UseHoldItem3").performed -= HandleUseHoldItem3Performed;
        playerInput.FindAction("UseHoldItem3").canceled -= HandleUseHoldItem3Cancelled;
        playerInput.FindAction("UseAltItem3").performed -= HandleUseAltItem3;
        playerInput.FindAction("UseItem3").performed -= HandleUseItem3;
        // ******
        // Disable Right Hand actions
        // ******
        playerInput.FindAction("UseHoldHandRight").performed -= HandleUseHoldHandRightPerformed;
        playerInput.FindAction("UseHoldHandRight").canceled -= HandleUseHoldHandRightCancelled;
        playerInput.FindAction("UseAltHandRight").performed -= HandleUseAltHandRight;
        playerInput.FindAction("UseHandRight").performed -= HandleUseHandRight;
        // ******
        // Disable Left Hand actions
        // ******
        playerInput.FindAction("UseHoldHandLeft").performed -= HandleUseHoldHandLeftPerformed;
        playerInput.FindAction("UseHoldHandLeft").canceled -= HandleUseHoldHandLeftCancelled;
        playerInput.FindAction("UseAltHandLeft").performed -= HandleUseAltHandLeft;
        playerInput.FindAction("UseHandLeft").performed -= HandleUseHandLeft;
        // ******
        // Disable Interact actions 
        // ******
        playerInput.FindAction("Interact").performed -= HandleInteract;
        // ******
        // Disable Swap Loadout actions
        // ******
        playerInput.FindAction("SwapLoadout").performed -= HandleSwapLoadout;
    }

    private void Update()
    {
        HandleMove();
    }

    private void HandleMove()
    {
        Vector2 movementInput = playerInput.FindAction("Move").ReadValue<Vector2>();
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

    // ******
    // Interact
    // ******
    private void HandleInteract(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        OnInteract?.Invoke();
    }

    // ******
    // Swap Loadout
    // ******
    private void HandleSwapLoadout(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        OnSwapLoadout?.Invoke();
    }
}
