using UnityEngine;

[CreateAssetMenu(menuName = ("Items/Enemy"))]
public class EnemyScriptableObject : ScriptableObject
{
    [Header("Values")]
    public int healthAmount;
    public int movingSpeed;
    public int damage;
    public int collectiveMaxCoin;
    [Header("Prefabs")]
    public GameObject enemyPrefab;
}
