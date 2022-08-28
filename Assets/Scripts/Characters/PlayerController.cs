using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Public Fields
    public Transform target;
    #endregion

    #region Private Fields
    private int damageAmount;
    private float range;
    private float moveSpeed;
    private float currentTimeOnPath;
    private float shootTimerMax;
    private float shootTimer;
    private Vector2 initialPosition;
    private Transform idlePosition;

    private CharacterManager characterManager;

    #endregion

    #region Private Methods
    private void Awake()
    {
        characterManager = GetComponent<CharacterManager>();
    }

    private void Start()
    {
        idlePosition = target;
        initialPosition = transform.position;

        range = ScriptableObjectManager.Instance.GetTower(1).range;
        damageAmount = ScriptableObjectManager.Instance.GetTower(1).damage;
        shootTimerMax = ScriptableObjectManager.Instance.GetTower(1).shootInterval;
        moveSpeed = ScriptableObjectManager.Instance.GetTower(1).playerMoveSpeed;

        characterManager.Look(target.position);
    }

    private void Update()
    {
        EnemyController enemy = GetClosestEnemy();
        characterManager.Look(target.position);

        if (enemy != null)
        {
            target.position = enemy.gameObject.GetComponent<CharacterManager>().GetPosition();

            shootTimer -= Time.deltaTime;

            if (Vector3.Distance(transform.position, target.position) <= 10f)
            {
                if (shootTimer <= 0f)
                {
                    shootTimer = shootTimerMax;

                    characterManager.AttackAnimation();
                    enemy.GetComponent<IDamageable>().Damage(damageAmount);
                    initialPosition = transform.position;
                    currentTimeOnPath = 0;
                }
            }
            else
                Walk(target);
        }
        else if (Vector3.Distance(transform.position, idlePosition.position) <= 0.1f)
        {
            initialPosition = transform.position;
            currentTimeOnPath = 0;
            characterManager.WalkAnimation(false);
        }
        else
            Walk(idlePosition);

    }

    private EnemyController GetClosestEnemy()
    {
        return EnemyController.GetClosestEnemy(transform.position, range);
    }

    private void Walk(Transform distination)
    {
        characterManager.WalkAnimation(true);
        Vector3 startPosition = initialPosition;
        Vector3 endPosition = distination.position;
        float pathLength = Vector3.Distance(startPosition, endPosition);
        float totalTimeForPath = pathLength / moveSpeed;

        currentTimeOnPath += Time.deltaTime;
        gameObject.transform.position = Vector3.Lerp(startPosition, endPosition, currentTimeOnPath / totalTimeForPath);
    }
    #endregion
}
