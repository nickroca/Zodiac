using UnityEngine;

public class InfoButton : MonoBehaviour
{
    public GameObject InfoPanel;

    // open info panel
    public void OpenInfo()
    {
        InfoPanel.SetActive(true);
    }

    // close info panel
    public void CloseInfo()
    {
        InfoPanel.SetActive(false);
    }
}