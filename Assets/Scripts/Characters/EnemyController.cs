using CodeMonkey.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : CharacterManager, IDamageable
{
    #region Static Fields
    public static List<EnemyController> enemies = new List<EnemyController>();
    #endregion

    #region Serialized Fields
    [SerializeField]
    private int enemyIndex = 0;
    [Header("Audios")]
    [SerializeField]
    private AudioClip attackClip;
    [SerializeField]
    private AudioClip deadClip;
    #endregion

    #region Private Fields
    private int damageAmount = 0;
    private float moveSpeed;
    private int collectiveMaxCoin = 0;
    private float currentTimeOnPath;
    private float timeBetweenAttack = 1;
    private float currentTime = 0;
    private bool hasTarget = false;
    private bool canAttack = true;
    private Transform waypoint;
    private Transform target;
    private Vector2 initialPosition;

    private HealthSystem healthSystem;
    #endregion

    #region Static Methods
    public static EnemyController GetClosestEnemy(Vector3 position,float maxRange)
    {
        EnemyController closest = null;
        foreach (EnemyController enemy in enemies)
        {
            if (enemy.IsDead()) continue;
            if(Vector3.Distance(position,enemy.GetPosition()) <= maxRange)
            {
                if(closest == null)
                {
                    closest = enemy;
                }
                else
                {
                    if(Vector3.Distance(position,enemy.GetPosition()) < Vector3.Distance(position, closest.GetPosition()))
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
        enemies.Add(this);
    }

    private void Start()
    {
        damageAmount = ScriptableObjectManager.Instance.GetEnemy(enemyIndex).damage;
        moveSpeed = ScriptableObjectManager.Instance.GetEnemy(enemyIndex).movingSpeed;
        collectiveMaxCoin = ScriptableObjectManager.Instance.GetEnemy(enemyIndex).collectiveMaxCoin;
        waypoint = GameObject.FindObjectOfType<WayPoint>().transform.GetChild(0);
        initialPosition = transform.position;

        healthSystem = new HealthSystem(100);
        World_Bar healthBar = new World_Bar(transform, new Vector3(0.12f, 0.42f), new Vector3(7, 1.5f), Color.grey, Color.red, 1f, 1000, new World_Bar.Outline { color = Color.black, size = .5f });
        healthSystem.OnHealthChanged += (object sender, EventArgs e) => {
            healthBar.SetSize(healthSystem.GetHealthNormalized());
        };

        Look(waypoint.position);
    }

    private void Update()
    {
        if (hasTarget) return;
        Walk();
    }

    private void Walk()
    {
        WalkAnimation(true);
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
                Look(waypoint.position);
            }
            else
            {
                enemies.Remove(this);
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
        WalkAnimation(false);
        yield return new WaitForSeconds(1f);
        AttackAnimation();
        if (player)
        {
            player.GetComponent<IDamageable>().Damage(damageAmount);
            if(attackClip)
                AudioManager.Instance.PlaySound(attackClip);
        }
        yield return new WaitForSeconds(1f);
        WalkAnimation(false);
        currentTime = 0;
        canAttack = true;
    }
    #endregion

    #region Public Methods
    public void Damage(int damageAmount)
    {
        Vector3 bloodDir = UtilsClass.GetRandomDir();
        Blood_Handler.SpawnBlood(GetPosition(), bloodDir);

        DamagePopup.Create(GetPosition(), damageAmount, false);

        healthSystem.Damage(damageAmount);
        if (IsDead())
        {
            FlyingBody.Create(GameAssets.i.pfEnemyFlyingBody, GetPosition(), bloodDir);
            enemies.Remove(this);
            UIManager.Instance.AddCoin(UnityEngine.Random.Range(1, collectiveMaxCoin));
            if(deadClip)
                AudioManager.Instance.PlaySound(deadClip);
            Destroy(gameObject);
        }
        else
        {
            transform.position += bloodDir * 2.5f;
        }
    }

    public bool IsDead()
    {
        return healthSystem.IsDead();
    }
    #endregion
}
