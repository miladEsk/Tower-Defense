using System.Collections.Generic;
using UnityEngine;

public class ScriptableObjectManager : MonoBehaviour
{
    #region Singleton
    public static ScriptableObjectManager Instance;
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    #endregion

    #region Serialied Fields
    [SerializeField]
    private List<TowerScriptableObject> towers;
    [SerializeField]
    private List<EnemyScriptableObject> enemies;
    [SerializeField]
    private List<EnemyWaveScriptableObject> waves;
    #endregion

    #region Public Methods
    public TowerScriptableObject GetTower(int index)
    {
        return towers[index];
    }

    public EnemyScriptableObject GetEnemy(int index)
    {
        return enemies[index];
    }

    public EnemyWaveScriptableObject GetWave(int index)
    {
        return waves[index];
    }
    #endregion
}
