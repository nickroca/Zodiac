using UnityEngine;
using UnityEngine.SceneManagement;

public class MapReturn : MonoBehaviour
{
    public void ReturnToMap()
    {
        SceneManager.LoadScene("MapScene");
    }
}