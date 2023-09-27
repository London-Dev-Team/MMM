using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Act Properties", order = 1)]
public class ActProperties : ScriptableObject
{
    
    [Header("Progress Variables")]

    public int WinThreshold = 100;

    public int LoseThreshold = 0;

    public int StartingProgress = 50;

    [Range(0, 5)]
    public int ActIndex = 0;
    
    public float TickTime = 1.0f;
}