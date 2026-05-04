using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TransitionToDeckBuilder : MonoBehaviour
{
    public void DeckBuilderButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(3);
    }
}
