using CodeMonkey.Utils;
using System;
using UnityEngine;

public class PlayerController : CharacterManager, IDamageable
{
    #region Public Fields
    public Transform target;
    #endregion

    #region Serialized Fields
    [Header("Audios")]
    [SerializeField]
    private AudioClip attackClip;
    [SerializeField]
    private AudioClip deadClip;
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

    private HealthSystem healthSystem;
    #endregion

    #region Private Methods
    private void Start()
    {
        idlePosition = target;
        initialPosition = transform.position;

        range = ScriptableObjectManager.Instance.GetTower(1).range;
        damageAmount = ScriptableObjectManager.Instance.GetTower(1).damage;
        shootTimerMax = ScriptableObjectManager.Instance.GetTower(1).shootInterval;
        moveSpeed = ScriptableObjectManager.Instance.GetTower(1).playerMoveSpeed;

        healthSystem = new HealthSystem(100);
        World_Bar healthBar = new World_Bar(transform, new Vector3(0.12f, 0.42f), new Vector3(7, 1.5f), Color.grey, Color.red, 1f, 1000, new World_Bar.Outline { color = Color.black, size = .5f });
        healthSystem.OnHealthChanged += (object sender, EventArgs e) => {
            healthBar.SetSize(healthSystem.GetHealthNormalized());
        };

        Look(target.position); 
    }

    private void Update()
    {
        EnemyController enemy = GetClosestEnemy();
        Look(target.position);
        
        if (enemy != null)
        {
            target.position = enemy.GetPosition();
            
            shootTimer -= Time.deltaTime;

            if (Vector3.Distance(transform.position, target.position) <= 10f)
            {
                if (shootTimer <= 0f)
                {
                    shootTimer = shootTimerMax;

                    AttackAnimation();
                    enemy.GetComponent<IDamageable>().Damage(damageAmount);
                    if (attackClip)
                        AudioManager.Instance.PlaySound(attackClip);
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
            WalkAnimation(false);
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
        WalkAnimation(true);
        Vector3 startPosition = initialPosition;
        Vector3 endPosition = distination.position;
        float pathLength = Vector3.Distance(startPosition, endPosition);
        float totalTimeForPath = pathLength / moveSpeed;

        currentTimeOnPath += Time.deltaTime;
        gameObject.transform.position = Vector3.Lerp(startPosition, endPosition, currentTimeOnPath / totalTimeForPath);
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
            if (deadClip)
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
