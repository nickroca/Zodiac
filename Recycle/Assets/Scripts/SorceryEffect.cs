using UnityEngine;

public abstract class SorceryEffect : ScriptableObject
{
    public bool requiresTarget;
    
    public abstract void Activate(GridManager gridManager, GridCell target = null);
}
