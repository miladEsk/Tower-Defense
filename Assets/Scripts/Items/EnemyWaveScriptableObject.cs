using UnityEngine;

[CreateAssetMenu(menuName = ("Items/EnemyWave"))]
public class EnemyWaveScriptableObject : ScriptableObject
{
    [Header("Values")]
    public int duration;
    public int enemyCount;
}
