using UnityEngine;
using Zodiac;

public class HexHandler : MonoBehaviour
{
    private GridManager gridManager;
    private Hex hexData;
    private Vector2 gridPosition;

    public void Init(Hex data, GridManager gm, Vector2 pos)
    {
        hexData = data;
        gridManager = gm;
        gridPosition = pos;

        Subscribe();
    }

    void Subscribe()
    {
        switch (hexData.trigger)
        {
            case Hex.Trigger.OnSummon:
                gridManager.OnOpponentSummon += Activate;
                break;

            case Hex.Trigger.OnAttack:
                gridManager.OnOpponentAttack += Activate;
                break;

            case Hex.Trigger.OnTurnStart:
                gridManager.OnOpponentTurnStart += ActivateNoParam;
                break;

            case Hex.Trigger.OnTurnEnd:
                gridManager.OnOpponentTurnStart += ActivateNoParam;
                break;
        }
    }

    void Unsubscribe()
    {
        gridManager.OnOpponentSummon -= Activate;
        gridManager.OnOpponentAttack -= Activate;
        gridManager.OnOpponentTurnStart -= ActivateNoParam;
        gridManager.OnOpponentTurnEnd -= ActivateNoParam;
    }

    void Activate(GameObject opponentObj)
    {
        Resolve(opponentObj);
    }

    void ActivateNoParam()
    {
        Resolve(null);
    }

    void Resolve(GameObject opponentObj)
    {
        Debug.Log($"Hex Activated: {hexData.name}");
        GridCell target = null;
        if (opponentObj != null) {
            target = AutoSelectTarget(opponentObj);
        }
        hexData.effect.Activate(gridManager, target);
        gridManager.RemoveObjectFromGrid(gridPosition);
        FindObjectOfType<DiscardManager>().AddToDiscard(hexData);

        Unsubscribe();
        Destroy(gameObject);
    }

    GridCell AutoSelectTarget(GameObject opponentObj)
    {
        if (!hexData.effect.requiresTarget)
        {
            return null;
        }

        if (opponentObj != null)
        {
            SummonSelect s = opponentObj.GetComponent<SummonSelect>();
            if (s != null && s.controller == 2)
            {
                return gridManager.gridCells[(int)s.gridPosition.x, (int)s.gridPosition.y];
            }
        }

        for (int x = 0; x < GridManager.width; x++)
        {
            GridCell cell = gridManager.gridCells[x, 2];
            if (cell.cellFull)
            {
                SummonSelect summon = cell.objectInCell.GetComponent<SummonSelect>();
                if(summon != null && summon.controller ==2)
                {
                    return cell;
                }
            }
        }

        return null;
    }
}
