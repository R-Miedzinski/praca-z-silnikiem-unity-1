using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HuDManager : MonoBehaviour
{
    [SerializeField] private Slider HpBar;
    [SerializeField] private Slider HeatBar;
    [SerializeField] private TextMeshProUGUI HpCounter;
    [SerializeField] private TextMeshProUGUI LevelName;

    public void SetMaxHpSliderValue(float maxHp) 
    {
        HpBar.maxValue = maxHp;
    }

    public void SetCurrentHpSliderValue(float currentHp) 
    {
        HpBar.value = currentHp;

        HpCounter.text = $"{currentHp:0.00}/{HpBar.maxValue:0.00}";
    }

    public void SetMaxHeatSliderValue(float maxHeat)
    {
        HeatBar.maxValue = maxHeat;
    }    

    public void SetHeatSliderValue(float heat)
    {
        HeatBar.value = heat;
    }

    public void SetLevelName(string levelDisplay)
    {
        LevelName.text = levelDisplay;
    }

}
