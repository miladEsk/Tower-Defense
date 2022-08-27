using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    #region Static Fields
    public static List<EnemyController> enemies = new List<EnemyController>();
    #endregion

    #region Serialized Fields
    [SerializeField]
    private int enemyIndex = 0;
    #endregion

    #region Public Fields
    [HideInInspector]
    public int collectiveMaxCoin = 0;
    #endregion

    #region Private Fields
    private int damageAmount = 0;
    private float moveSpeed;
    private float currentTimeOnPath;
    private float timeBetweenAttack = 1;
    private float currentTime = 0;
    private bool hasTarget = false;
    private bool canAttack = true;
    private Transform waypoint;
    private Transform target;
    private Vector2 initialPosition;

    private CharacterManager characterManager;

    //private HealthSystem healthSystem;
    #endregion

    #region Static Methods
    public static EnemyController GetClosestEnemy(Vector3 position,float maxRange)
    {
        EnemyController closest = null;
        foreach (EnemyController enemy in enemies)
        {
            if (enemy.gameObject.GetComponent<CharacterManager>().IsDead()) continue;
            if(Vector3.Distance(position,enemy.gameObject.GetComponent<CharacterManager>().GetPosition()) <= maxRange)
            {
                if(closest == null)
                {
                    closest = enemy;
                }
                else
                {
                    if(Vector3.Distance(position,enemy.gameObject.GetComponent<CharacterManager>().GetPosition()) < Vector3.Distance(position, closest.gameObject.GetComponent<CharacterManager>().GetPosition()))
                    {
                        closest = enemy;
                    }
                }
            }
        }
        return closest;
    }
    #endregion

    #region Private Methods
    private void Awake()
    {
        characterManager = GetComponent<CharacterManager>();
        enemies.Add(this);
    }

    private void Start()
    {
        damageAmount = ScriptableObjectManager.Instance.GetEnemy(enemyIndex).damage;
        moveSpeed = ScriptableObjectManager.Instance.GetEnemy(enemyIndex).movingSpeed;
        collectiveMaxCoin = ScriptableObjectManager.Instance.GetEnemy(enemyIndex).collectiveMaxCoin;
        waypoint = GameObject.FindObjectOfType<WayPoint>().transform.GetChild(0);
        initialPosition = transform.position;

        characterManager.Look(waypoint.position);
    }

    private void Update()
    {
        if (hasTarget) return;
        Walk();
    }

    private void Walk()
    {
        characterManager.WalkAnimation(true);
        Vector3 startPosition = initialPosition;
        Vector3 endPosition = waypoint.transform.position;

        float pathLength = Vector3.Distance(startPosition, endPosition);
        float totalTimeForPath = pathLength / moveSpeed;
        currentTimeOnPath += 1 * Time.deltaTime;
        gameObject.transform.position = Vector3.Lerp(startPosition, endPosition, currentTimeOnPath / totalTimeForPath);

        if (gameObject.transform.position.Equals(endPosition))
        {
            initialPosition = transform.position;
            if (waypoint.childCount > 0)
            {
                waypoint = waypoint.transform.GetChild(0);
                currentTimeOnPath = 0;
                characterManager.Look(waypoint.position);
            }
            else
            {
                //enemies.Remove(this);
                UIManager.Instance.ChangeHealth(damageAmount);
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            target = collision.transform;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Vector3.Distance(collision.transform.position, transform.position) <= 10)
            {
                
                hasTarget = true;
                currentTime += 0.05f;
                if (currentTime >= timeBetweenAttack && canAttack)
                {
                    StartCoroutine(IEAttack(collision.transform.GetComponent<PlayerController>()));
                }
            }
            else
            {
                hasTarget = false;
            }
        }
        else
        {
            hasTarget = false;
        }
    }

    private IEnumerator IEAttack(PlayerController player)
    {
        canAttack = false;
        characterManager.WalkAnimation(false);
        yield return new WaitForSeconds(1f);
        characterManager.AttackAnimation();
        if (player)
        {
            player.GetComponent<IDamageable>().Damage(damageAmount);
        }
        yield return new WaitForSeconds(1f);
        characterManager.WalkAnimation(false);
        currentTime = 0;
        canAttack = true;
    }
    #endregion
}
