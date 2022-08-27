using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField]
    private EnemySpawner enemySpawner;
    [SerializeField]
    private Animator showWaveBannerAnimator;
    #endregion

    #region Private Fields
    private int duration = 0;
    private int enemyCount = 0;
    private int currentWave = 0;

    private float currentTime = 0;
    private float nextTimeToSpawn = 1;

    private bool canStartWave = false;
    #endregion

    #region Private Methods
    private void Start()
    {
        StartCoroutine(IEChangeWave());
    }

    private void Update()
    {
        if (!canStartWave) return;
        currentTime += Time.deltaTime;
        if(currentTime >= duration)
        {
            currentTime = 0;
            nextTimeToSpawn = 1;
            StartCoroutine(IEChangeWave());
        }
        else if(currentTime >= nextTimeToSpawn && currentTime <= duration / 2 && enemyCount > 0)
        {
            enemyCount -= 1;
            nextTimeToSpawn = currentTime + 1;
            enemySpawner.SpawnEnemy();
        }
    }

    private IEnumerator IEChangeWave()
    {
        
        canStartWave = false;
        yield return new WaitForSeconds(0.5f);

        if(currentWave < 5)
        {
            duration = ScriptableObjectManager.Instance.GetWave(currentWave).duration;
            enemyCount = ScriptableObjectManager.Instance.GetWave(currentWave).enemyCount;
            currentWave++;
            UIManager.Instance.ChangeWave();
            yield return new WaitForSeconds(1f);
            showWaveBannerAnimator.SetTrigger("Show");
            yield return new WaitForSeconds(3f);
            canStartWave = true;
        }
        else
        {
            yield return new WaitWhile(() => enemySpawner.hasEnemy);
            UIManager.Instance.Win();
        }
    }
    #endregion
}
