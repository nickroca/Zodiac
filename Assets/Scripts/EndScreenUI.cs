using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndScreenUI : MonoBehaviour
{
    public TextMeshProUGUI resultText;
    public Image fadePanel;

    void Start()
    {
        resultText.text = GameOverManager.resultMessage;
        resultText.color = GameOverManager.resultColor;

        StartCoroutine(FadeInAndPop());
    }

    public void RestartGame()
    {
        StartCoroutine(FadeAndRestart());
    }

    IEnumerator FadeInAndPop()
    {
        // Start fully black
        Color fadeColor = fadePanel.color;
        fadeColor.a = 1;
        fadePanel.color = fadeColor;

        // Start text small
        resultText.transform.localScale = Vector3.zero;

        float duration = 1f;
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;

            // Fade in
            fadeColor.a = Mathf.Lerp(1, 0, time / duration);
            fadePanel.color = fadeColor;

            // Pop text
            resultText.transform.localScale =
                Vector3.Lerp(Vector3.zero, Vector3.one, time / duration);

            yield return null;
        }
    }

    IEnumerator FadeAndRestart()
    {
        float duration = 1f;
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            Color c = fadePanel.color;
            c.a = Mathf.Lerp(0, 1, time / duration);
            fadePanel.color = c;
            yield return null;
        }

        GameManager gm = FindObjectOfType<GameManager>();
        if (gm != null)
        {
            Destroy(gm.gameObject);
        }

        SceneManager.LoadScene("Setup");
    }
}