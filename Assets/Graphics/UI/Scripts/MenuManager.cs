using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Canvas MainMenu;

    public void OnPlayClick()
    {
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }

    public void OnSettingsClick()
    {

    }

    public void OnQuitClick()
    {
        Application.Quit();
    }
}
