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
        SceneManager.LoadSceneAsync(1);
    }

    public void  ExitGame()
    {
        Application.Quit();
    }
        
}
