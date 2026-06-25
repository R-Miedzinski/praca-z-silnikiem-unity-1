using JetBrains.Annotations;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private HuD_Manager HuD; 
    public void OnMenuClick() 
    {
        PauseMenu.SetActive(true);
    }
    public void OnResumeClick() 
    {
        PauseMenu.SetActive(false);
    }

    public void OnAbandonRunClick() 
    {

    }
    public void OnMainMenuClick() 
    {

    }

    public void OnExitClick() 
    {
        Application.Quit();
    }

    public void Start()
    {
        HuD.SetMaxHpSliderValue(PlayerCharacter.Instance.MaxHealth);
        HuD.SetCurrentHpSliderValue(PlayerCharacter.Instance.CurrentHealth);
        HuD.SetMaxHeatSliderValue(PlayerCharacter.Instance.MaxHeat);
        HuD.SetHeatSliderValue(PlayerCharacter.Instance.Heat);
        HuD.SetLevelName("level1");
    }
    public void Update()
    {
        HuD.SetMaxHpSliderValue(PlayerCharacter.Instance.MaxHealth);
        HuD.SetCurrentHpSliderValue(PlayerCharacter.Instance.CurrentHealth); 
        HuD.SetMaxHeatSliderValue(PlayerCharacter.Instance.MaxHeat);
        HuD.SetHeatSliderValue(PlayerCharacter.Instance.Heat);
    }

}
