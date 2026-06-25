using JetBrains.Annotations;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Canvas PauseMenu;
    [SerializeField] private Canvas Backpack;
    [SerializeField] private HuDManager HuD;
    private InputActionMap InterfaceInput;

    private void Awake()
    {
        InterfaceInput = InputSystem.actions.FindActionMap("Interface");
    }

    private void Start()
    {
        HuD.SetMaxHpSliderValue(PlayerCharacter.Instance.MaxHealth);
        HuD.SetCurrentHpSliderValue(PlayerCharacter.Instance.CurrentHealth);
        HuD.SetMaxHeatSliderValue(PlayerCharacter.Instance.MaxHeat);
        HuD.SetHeatSliderValue(PlayerCharacter.Instance.Heat);
        HuD.SetLevelName("level1");
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
        PauseMenu.enabled = (true);
        Pause();
    }

    public void OnResumeClick() 
    {
        PauseMenu.enabled = (false);
        Unpause();
    }

    public void OnBackpackClick()
    {
        Backpack.enabled = (true);
        Pause();
    }

    public void OnBackpackCloseClick()
    {
        Backpack.enabled = (false);
        Unpause();
    }

    public void OnAbandonRunClick() 
    {

    }

    public void OnMainMenuClick() 
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnExitClick() 
    {
        Application.Quit();
    }

    private void Pause()
    {
        Time.timeScale = 0f;
    }

    private void Unpause()
    {
        Time.timeScale = 1f;
    }
}
