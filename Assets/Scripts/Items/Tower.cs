using UnityEngine;

public class Tower : MonoBehaviour {
    #region Public Fields
    public int towerIndex = 0;
    #endregion

    #region Serialized Fields
    [SerializeField]
    private AudioClip shootClip;
    #endregion

    #region Private Fields
    private float range;
    private int damageAmount;
    private float shootTimerMax;
    private float shootTimer;

    private Vector3 projectileShootFromPosition;
    private Transform playersParent;
    private TowerPlace towerPlace;
    #endregion

    #region Private Methods
    private void Start()
    {  
        towerPlace = GetComponentInParent<TowerPlace>();
        if (towerIndex == 1)
        {
            playersParent = transform.Find("PlayersParent");
            Transform playerPosition = towerPlace.GetPlayerPosition().GetChild(0);
            for (int i = 0; i < ScriptableObjectManager.Instance.GetTower(towerIndex).playerCount; i++)
            {
                GameObject player = Instantiate(ScriptableObjectManager.Instance.GetTower(towerIndex).playerPrefab, transform.position, Quaternion.identity);
                player.transform.parent = playersParent;
                player.GetComponent<PlayerController>().target = playerPosition;
                if(playerPosition.GetChild(0))
                    playerPosition = playerPosition.GetChild(0).transform;
            }
        }
        else
        {
            projectileShootFromPosition = transform.Find("ProjectTileShootFromPosition").position;
            range = ScriptableObjectManager.Instance.GetTower(towerIndex).range;
            damageAmount = ScriptableObjectManager.Instance.GetTower(towerIndex).damage;
            shootTimerMax = ScriptableObjectManager.Instance.GetTower(towerIndex).shootInterval;
        }
    }

    private void Update() {
        if (towerIndex == 1) return;
        shootTimer -= Time.deltaTime;

        if (shootTimer <= 0f) {
            shootTimer = shootTimerMax;

            EnemyController enemy = GetClosestEnemy();
            
            if (enemy != null) {
                // Enemy in range!
                ProjectileArrow.Create(projectileShootFromPosition, enemy, Random.Range(damageAmount - 5, damageAmount + 5), towerIndex, shootClip);
            }
        }
    }

    private EnemyController GetClosestEnemy() {
        return EnemyController.GetClosestEnemy(transform.position, range);
    }

    private void OnMouseDown()
    {
        UpgradeOverlay.Show_Static(this);
    }
    #endregion

    #region Public Methods
    public float GetRange() {
        return range;
    }

    public void UpgradeRange() {
        range += 10f;
    }

    public void UpgradeDamageAmount() {
        damageAmount += 5;
    }
    #endregion
}
