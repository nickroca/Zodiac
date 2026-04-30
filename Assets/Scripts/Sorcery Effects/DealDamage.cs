using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Deal Damage")]
public class DealDamage : SorceryEffect
{
    private void OnEnable()
    {
        requiresTarget = false;
    }

    public int amount;

    public override void Activate(GridManager gridManager, GridCell target = null)
    {
        OpponentLIFE life = FindObjectOfType<OpponentLIFE>();
        life.currentHP -= amount;
    }
}