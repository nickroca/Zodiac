using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zodiac;

#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(DeckPileManager))]
public class DeckPileManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        DeckPileManager deckPileManager = (DeckPileManager)target;
        if (GUILayout.Button("Draw Next Card"))
        {
            HandManager handManager = FindObjectOfType<HandManager>();
            if (handManager != null)
            {
                deckPileManager.DrawCard(handManager);
            }
        }
    }
}
#endif