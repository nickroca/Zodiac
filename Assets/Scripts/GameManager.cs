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
    public int mapSeed;
    public int currentNodeRow;
    public int currentNodeColumn;

    private float currentTime = 0f;
    private const float cheatTime = 5f;

    [System.Serializable]
    public class MapNodeSaveData
    {
        public string nodeName;
        public MapNode.NodeState state;
        public int row;
        public int column;
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
            playerDeckInitial.Add(2); //1
            playerDeckInitial.Add(2); //2
            playerDeckInitial.Add(3); //3
            playerDeckInitial.Add(3); //4
            playerDeckInitial.Add(10); //5
            playerDeckInitial.Add(16); //6
            playerDeckInitial.Add(16); //7
            playerDeckInitial.Add(22); //8
            playerDeckInitial.Add(22); //9
            playerDeckInitial.Add(23); //10
            playerDeckInitial.Add(32); //11
            playerDeckInitial.Add(32); //12
            playerDeckInitial.Add(35); //13
            playerDeckInitial.Add(35); //14
            playerDeckInitial.Add(38); //15
            playerDeckInitial.Add(48); //16
            playerDeckInitial.Add(48); //17
            playerDeckInitial.Add(48); //18
            playerDeckInitial.Add(56); //19
            playerDeckInitial.Add(57); //20
            playerDeckInitial.Add(61); //21
            playerDeckInitial.Add(61); //22
            playerDeckInitial.Add(63); //23
            playerDeckInitial.Add(63); //24
            playerDeckInitial.Add(67); //25
            playerDeckInitial.Add(78); //26
            playerDeckInitial.Add(78); //27
            playerDeckInitial.Add(80); //28
            playerDeckInitial.Add(80); //29
            playerDeckInitial.Add(82); //30
            playerDeckInitial.Add(99); //31
            playerDeckInitial.Add(99); //32
            playerDeckInitial.Add(99); //33
            playerDeckInitial.Add(149); //34
            playerDeckInitial.Add(149); //35
            playerDeckInitial.Add(149); //36
            playerDeckInitial.Add(119); //37
            playerDeckInitial.Add(119); //38
            playerDeckInitial.Add(119); //39
            playerDeckInitial.Add(121); //40
            playerDeck = playerDeckInitial;
            SetupEnemyDecks();
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (!battleTime)
        {
            currentTime = 0f;
            return;
        }

        bool M = Input.GetKey(KeyCode.M);
        bool K = Input.GetKey(KeyCode.K);
        bool O = Input.GetKey(KeyCode.O);

        if (M && K && O)
        {
            currentTime += Time.deltaTime;
            Debug.Log("Cheating...");

            if (currentTime >= cheatTime)
            {
                WinTheGame();
                currentTime = 0f;
            }
        } 
        else
        {
            currentTime = 0f;
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

        List<int> slime = new List<int>(); //0
        slime.Add(16);
        slime.Add(22);
        slime.Add(26);
        slime.Add(35);
        slime.Add(61);
        List<int> skeleton = new List<int>(); //1
        skeleton.Add(61);
        skeleton.Add(63);
        skeleton.Add(78);
        skeleton.Add(3);
        skeleton.Add(64);
        List<int> rotten = new List<int>(); //2
        rotten.Add(61);
        rotten.Add(63);
        rotten.Add(78);
        rotten.Add(22);
        rotten.Add(5);
        List<int> minotaur = new List<int>(); //3
        minotaur.Add(41);
        minotaur.Add(16);
        minotaur.Add(5);
        minotaur.Add(23);
        minotaur.Add(7);
        minotaur.Add(17);
        minotaur.Add(49);
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

    private void WinTheGame()
    {
        Debug.Log("Cheated to win...");

        var victim = FindObjectOfType<OpponentLIFE>();

        victim.currentHP = -1;
    }
}