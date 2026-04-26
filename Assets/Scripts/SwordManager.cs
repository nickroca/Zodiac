using UnityEngine;

public class SwordManager : MonoBehaviour
{
    GridManager gridManager;
    TurnSystem turnSystem;
    public GameObject sword1obj;
    public GameObject sword2obj;
    public GameObject sword3obj;
    public GameObject sword4obj;
    public GameObject sword5obj;


    void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
        turnSystem = FindObjectOfType<TurnSystem>();
        sword1obj.SetActive(false);
        sword2obj.SetActive(false);
        sword3obj.SetActive(false);
        sword4obj.SetActive(false);
        sword5obj.SetActive(false);
    }

    void Update()
    {
        UpdateSwordGraphics();
    }

    public void UpdateSwordGraphics()
    {
        if (turnSystem.isYourTurn && turnSystem.phaseCount == 2)
        {
            if (gridManager.CanSummonAttack(0, 1))
            {
                sword1obj.SetActive(true);
            }
            else
            {
                sword1obj.SetActive(false);
            }
            if (gridManager.CanSummonAttack(1, 1))
            {
                sword2obj.SetActive(true);
            }
            else
            {
                sword2obj.SetActive(false);
            }
            if (gridManager.CanSummonAttack(2, 1))
            {
                sword3obj.SetActive(true);
            }
            else
            {
                sword3obj.SetActive(false);
            }
            if (gridManager.CanSummonAttack(3, 1))
            {
                sword4obj.SetActive(true);
            }
            else
            {
                sword4obj.SetActive(false);
            }
            if (gridManager.CanSummonAttack(4, 1))
            {
                sword5obj.SetActive(true);
            }
            else
            {
                sword5obj.SetActive(false);
            }
        }
        else
        {
            sword1obj.SetActive(false);
            sword2obj.SetActive(false);
            sword3obj.SetActive(false);
            sword4obj.SetActive(false);
            sword5obj.SetActive(false);
        }
    }
}
