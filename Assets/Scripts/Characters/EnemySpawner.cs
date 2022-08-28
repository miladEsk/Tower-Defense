using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    #region Serialied Fields
    [SerializeField]
    private List<Transform> enemyPrefabs;
    [SerializeField]
    private Transform enemySpawnPosition;
    #endregion

    #region Public Fields
    public bool hasEnemy = false;
    #endregion

    #region Private Methods
    private void Update()
    {
        if (transform.childCount > 0) hasEnemy = true;
        else hasEnemy = false;
    }
    #endregion

    #region Public Methods
    public void SpawnEnemy() {
        Transform enemy = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Count)], enemySpawnPosition.position, Quaternion.identity);
        enemy.transform.parent = transform;
    }
    #endregion
}
