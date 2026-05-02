using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Zodiac;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private int playerHP;
    private int difficulty = 1;
    public bool battleTime = false;
    public OptionsManager OptionsManager { get; private set; }
    public AudioManager AudioManager { get; private set; }
    public DeckManager DeckManager { get; private set; }
    public List<Card> allCards = new List<Card>();
    public List<int> playerDeck = new List<int>();
    public List<int> playerDeckInitial = new List<int>();
    public List<int> playerInventory = new List<int>();
    public List<List<int>> enemyDecks = new List<List<int>>();

    public bool PlayingCard = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeManagers();
            Card[] cardAssets = Resources.LoadAll<Card>("CardData");
            allCards.AddRange(cardAssets);
            allCards.Sort((a, b) => a.id.CompareTo(b.id));
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void InitializeManagers()
    {
        OptionsManager = GetComponentInChildren<OptionsManager>();
        AudioManager = GetComponentInChildren<AudioManager>();
        DeckManager = GetComponentInChildren<DeckManager>();

        if (OptionsManager == null)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/OptionsManager");
            if (prefab == null)
            {
                Debug.Log($"OptionsManager not found");
            }
            else
            {
                Instantiate(prefab, transform.position, Quaternion.identity, transform);
                OptionsManager = GetComponentInChildren<OptionsManager>();
            }
        }
        if (AudioManager == null)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/AudioManager");
            if (prefab == null)
            {
                Debug.Log($"AudioManager not found");
            }
            else
            {
                Instantiate(prefab, transform.position, Quaternion.identity, transform);
                AudioManager = GetComponentInChildren<AudioManager>();
            }
        }
        if (DeckManager == null)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/DeckManager");
            if (prefab == null)
            {
                Debug.Log($"DeckManager not found");
            }
            else
            {
                Instantiate(prefab, transform.position, Quaternion.identity, transform);
                DeckManager = GetComponentInChildren<DeckManager>();
            }
        }
    }

    public Card GetCardByID(int id)
    {
        return allCards.Find(card => card.id == id);
    }

    public Card GetRandomCard()
    {
        float rand = Random.value;
        int rarity = 1;

        if (rand <= 0.8f)
        {
            rarity = 1;
        }
        else if (rand <= 0.95f)
        {
            rarity = 2;
        }
        else
        {
            rarity = 3;
        }

        List<Card> validCards = allCards.FindAll(c => c.rarity == rarity);

        return validCards[Random.Range(0, validCards.Count)];
    }

    public void AddCardtoInventory(Card card)
    {
        playerInventory.Add(card.id);
    }

    public void Reset()
    {
        playerInventory.Clear();
        playerDeck = new List<int>(playerDeckInitial);
    }

    public List<Card> GetEnemyDeck(int index)
    {
        List<Card> result = new List<Card>();

        foreach (int id in enemyDecks[index])
        {
            Card card = GetCardByID(id);
            if (card != null)
            {
                result.Add(card);
            }
        }

        return result;
    }

    public int PlayerHP
    {
        get { return playerHP; }
        set { playerHP = value; }
    }
    public int Difficulty
    {
        get { return difficulty; }
        set { difficulty = value; }
    }
}