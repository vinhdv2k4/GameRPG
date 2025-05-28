using TV;
using UnityEngine;

public class CharacterEffect : ScriptableObject
{
    [Header("Effect ID")]
    public int EffectID;
    
    public virtual void ProcessEffect(CharacterManager character)
    {

    }
}
