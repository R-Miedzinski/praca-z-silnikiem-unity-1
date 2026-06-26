using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    [HideInInspector] public bool IsPaused { get; private set; }
    [SerializeField] private Canvas PauseMenu;
    [SerializeField] private Canvas Backpack;
    [SerializeField] private HuDManager HuD;

    private InputActionMap InterfaceInput;
    private int pauseInstanceCount = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        InterfaceInput = InputSystem.actions.FindActionMap("Interface");

        Unpause();
        pauseInstanceCount = 0;
    }

    private void Start()
    {
        HuD.SetMaxHpSliderValue(PlayerCharacter.Instance.MaxHealth);
        HuD.SetCurrentHpSliderValue(PlayerCharacter.Instance.CurrentHealth);
        HuD.SetMaxHeatSliderValue(PlayerCharacter.Instance.MaxHeat);
        HuD.SetHeatSliderValue(PlayerCharacter.Instance.Heat);
    }

    private void OnEnable()
    {
        InterfaceInput.FindAction("MenuToggle").performed += OnMenuToggleClick;
        InterfaceInput.FindAction("EquipmentToggle").performed += OnBackpackToggleClick;
    }
    private void OnDisable()
    {
        InterfaceInput.FindAction("MenuToggle").performed -= OnMenuToggleClick;
        InterfaceInput.FindAction("EquipmentToggle").performed -= OnBackpackToggleClick;
    }

    private void Update()
    {
        HuD.SetMaxHpSliderValue(PlayerCharacter.Instance.MaxHealth);
        HuD.SetCurrentHpSliderValue(PlayerCharacter.Instance.CurrentHealth);
        HuD.SetMaxHeatSliderValue(PlayerCharacter.Instance.MaxHeat);
        HuD.SetHeatSliderValue(PlayerCharacter.Instance.Heat);
        // TODO: Create a way to set the level name that doesn't have to refresh constantly
        HuD.SetLevelName(RoomSelectionSystem.Instance.RoomOrder[RoomTransitionSystem.Instance.CurrentRoomIndex].RoomName);
    }

    public void OnMenuToggleClick(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (!PauseMenu.enabled)
        {
            OnMenuClick();
        }
        else
        {
            OnResumeClick();
        }
    }

    public void OnBackpackToggleClick(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (!Backpack.enabled)
        {
            OnBackpackClick();
        }
        else
        {
            OnBackpackCloseClick();
        }
    }

    public void OnMenuClick()
    {
        if (PauseMenu.enabled)
        {
            return;
        }

        PauseMenu.enabled = true;
        Pause();
    }

    public void OnResumeClick()
    {
        if (!PauseMenu.enabled)
        {
            return;
        }

        PauseMenu.enabled = false;
        Unpause();
    }

    public void OnBackpackClick()
    {
        if (Backpack.enabled)
        {
            return;
        }

        Backpack.enabled = true;
        Pause();
    }

    public void OnBackpackCloseClick()
    {
        if (!Backpack.enabled)
        {
            return;
        }

        Backpack.enabled = false;
        Unpause();
    }

    public void OnAbandonRunClick()
    {
        // TODO: Create specific abandon run flow
        OnMainMenuClick();
    }

    public void OnMainMenuClick()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void OnExitClick()
    {
        Application.Quit();
    }

    private void Pause()
    {
        pauseInstanceCount++;
        if (pauseInstanceCount > 1)
        {
            return;
        }

        Time.timeScale = 0f;
        IsPaused = true;
    }

    private void Unpause()
    {
        pauseInstanceCount--;
        if (pauseInstanceCount > 0)
        {
            return;
        }

        Time.timeScale = 1f;
        IsPaused = false;
    }
}
