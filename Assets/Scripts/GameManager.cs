using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public bool returnToMap;
    public bool mapGenerated = false;
    public int currentEnemyID = 0;
    public bool pendingFloorAdvance = false;

    [System.Serializable]
    public class MapNodeSaveData
    {
        public string nodeName;
        public MapNode.NodeState state;
    }

    public List<MapNodeSaveData> savedMap = new List<MapNodeSaveData>();
    public string currentNodeName;

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
            playerDeckInitial.Add(1); //1
            playerDeckInitial.Add(1); //2
            playerDeckInitial.Add(2); //3
            playerDeckInitial.Add(2); //4
            playerDeckInitial.Add(9); //5
            playerDeckInitial.Add(15); //6
            playerDeckInitial.Add(15); //7
            playerDeckInitial.Add(21); //8
            playerDeckInitial.Add(21); //9
            playerDeckInitial.Add(22); //10
            playerDeckInitial.Add(31); //11
            playerDeckInitial.Add(31); //12
            playerDeckInitial.Add(34); //13
            playerDeckInitial.Add(34); //14
            playerDeckInitial.Add(37); //15
            playerDeckInitial.Add(47); //16
            playerDeckInitial.Add(47); //17
            playerDeckInitial.Add(47); //18
            playerDeckInitial.Add(55); //19
            playerDeckInitial.Add(56); //20
            playerDeckInitial.Add(60); //21
            playerDeckInitial.Add(60); //22
            playerDeckInitial.Add(62); //23
            playerDeckInitial.Add(62); //24
            playerDeckInitial.Add(66); //25
            playerDeckInitial.Add(77); //26
            playerDeckInitial.Add(77); //27
            playerDeckInitial.Add(79); //28
            playerDeckInitial.Add(79); //29
            playerDeckInitial.Add(81); //30
            playerDeckInitial.Add(1); //31
            playerDeckInitial.Add(1); //32
            playerDeckInitial.Add(1); //33
            playerDeckInitial.Add(1); //34
            playerDeckInitial.Add(1); //35
            playerDeckInitial.Add(1); //36
            playerDeckInitial.Add(1); //37
            playerDeckInitial.Add(1); //38
            playerDeckInitial.Add(1); //39
            playerDeckInitial.Add(1); //40
            playerDeck = playerDeckInitial;
            SetupEnemyDecks();
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

    public void SetupEnemyDecks()
    {
        // REMINDER: REDUCE ALL IDS BY 1, THEY START AT 0

        List<int> slime = new List<int>(); //0
        slime.Add(15);
        slime.Add(21);
        slime.Add(25);
        slime.Add(34);
        slime.Add(60);
        List<int> skeleton = new List<int>(); //1
        skeleton.Add(60);
        skeleton.Add(62);
        skeleton.Add(77);
        skeleton.Add(2);
        skeleton.Add(63);
        List<int> rotten = new List<int>(); //2
        rotten.Add(60);
        rotten.Add(62);
        rotten.Add(77);
        rotten.Add(22);
        rotten.Add(4);
        List<int> minotaur = new List<int>(); //3
        minotaur.Add(40);
        minotaur.Add(15);
        minotaur.Add(4);
        minotaur.Add(22);
        minotaur.Add(6);
        minotaur.Add(16);
        minotaur.Add(48);
        List<int> skeletonKnight = new List<int>(); //4
        //skeletonKnight.Add();
        //skeletonKnight.Add();
        //skeletonKnight.Add();
        //skeletonKnight.Add();
        //skeletonKnight.Add();
        List<int> chimera = new List<int>(); //5
        List<int> puppeteer = new List<int>(); //6
        List<int> lichKing = new List<int>(); //7
        List<int> grimReaper = new List<int>(); //8
        List<int> mechAngel = new List<int>(); //9
        List<int> cyclops = new List<int>(); //10
        List<int> warden = new List<int>(); //11
        enemyDecks.Add(slime);
        enemyDecks.Add(skeleton);
        enemyDecks.Add(rotten);
        enemyDecks.Add(minotaur);
        enemyDecks.Add(skeletonKnight);
        enemyDecks.Add(chimera);
        enemyDecks.Add(puppeteer);
        enemyDecks.Add(lichKing);
        enemyDecks.Add(grimReaper);
        enemyDecks.Add(mechAngel);
        enemyDecks.Add(cyclops);
        enemyDecks.Add(warden);
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