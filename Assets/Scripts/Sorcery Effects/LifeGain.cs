using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Life Gain")]
public class LifeGain : SorceryEffect
{
    private void OnEnable()
    {
        requiresTarget = false;
    }

    public int amount;

    public override void Activate(GridManager gridManager, GridCell target = null)
    {
        PlayerLIFE life = FindObjectOfType<PlayerLIFE>();
        life.currentHP += amount;
    }
}