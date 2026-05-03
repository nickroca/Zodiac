using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class HandHolder : MonoBehaviour
{
    public static HandHolder Instance;
    public TMP_Text hhText;
    public float messageDuration = 0.5f;
    private Queue<string> messageQueue = new Queue<string>();
    private bool isDisplaying = false;


    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        hhText.text = "";
    }

    public void ShowMessage(string message)
    {
        messageQueue.Enqueue(message);

        if (!isDisplaying)
        {
            StartCoroutine(DisplayMessages());
        }
    }

    private IEnumerator DisplayMessages()
    {
        isDisplaying = true;

        while (messageQueue.Count > 0)
        {
            string msg = messageQueue.Dequeue();
            hhText.text = msg;

            yield return new WaitForSeconds(messageDuration);
        }

        //hhText.text = "";
        isDisplaying = false;
    }

    public void ShowImmediate(string message)
    {
        StopAllCoroutines();
        messageQueue.Clear();
        StartCoroutine(ShowImmediateRoutine(message));
    }

    private IEnumerator ShowImmediateRoutine(string message)
    {
        isDisplaying = true;
        hhText.text = message;

        yield return new WaitForSeconds(messageDuration);

        //hhText.text = "";
        isDisplaying = false;
    }
}