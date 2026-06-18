using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu_Manager : MonoBehaviour
{
    [SerializeField] private GameObject M_Menu;

    public void OnPlayClick() 
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void OnSettingsClick()
    {

    }
    public void OnQuitClick()
    {
        Application.Quit();
    }
}
