using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Progress Asset", order = 1)]
public class ProgressAsset : ScriptableObject
{
    
    [Header("Progress Variables")]
    public bool[] unlockedActs = { true, false, false, false, false, false };
}