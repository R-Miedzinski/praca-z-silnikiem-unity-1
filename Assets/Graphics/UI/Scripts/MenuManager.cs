using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Canvas MainMenu;

    private void OnPlayClick() 
    {
        SceneManager.LoadScene("SampleScene");
    }

    private void OnSettingsClick()
    {

    }

    private void OnQuitClick()
    {
        Application.Quit();
    }
}
