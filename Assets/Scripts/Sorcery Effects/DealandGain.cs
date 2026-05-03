using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Deal and Gain")]
public class DealandGain : SorceryEffect
{
    private void OnEnable()
    {
        requiresTarget = false;
    }

    public int deal;
    public int gain;

    public override void Activate(GridManager gridManager, GridCell target = null)
    {
        PlayerLIFE plrLIFE = FindObjectOfType<PlayerLIFE>();
        OpponentLIFE oppLife = FindObjectOfType<OpponentLIFE>();
        oppLife.currentHP -= deal;
        plrLIFE.currentHP += gain;
    }
}