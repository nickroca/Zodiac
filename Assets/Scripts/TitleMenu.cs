using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMenu : MonoBehaviour
{
    GameManager gameManager;

    private void Start()
    {
        
    }

    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void  ExitGame()
    {
        Application.Quit();
    }
        
}
