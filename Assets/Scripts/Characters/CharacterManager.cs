using CodeMonkey.Utils;
using System;
using UnityEngine;

public class CharacterManager : MonoBehaviour, IDamageable
{
    #region Public Fields
    public Animator animator;
    public bool isFacingRight = true;
    #endregion

    #region Serialized Fields
    [SerializeField]
    private bool isEnemy = false;
    [Header("Audios")]
    [SerializeField]
    private AudioClip attackClip;
    [SerializeField]
    private AudioClip deadClip;
    #endregion

    #region Private Fields
    private HealthSystem healthSystem;
    #endregion

    #region Private Methods
    private void Start()
    {
        animator = GetComponent<Animator>();

        healthSystem = new HealthSystem(100);
        World_Bar healthBar = new World_Bar(transform, new Vector3(0.12f, 0.42f), new Vector3(7, 1.5f), Color.grey, Color.red, 1f, 1000, new World_Bar.Outline { color = Color.black, size = .5f });
        healthSystem.OnHealthChanged += (object sender, EventArgs e) => {
            healthBar.SetSize(healthSystem.GetHealthNormalized());
        };
    }
    #endregion

    #region Public Methods
    public void Look(Vector2 direction)
    {
        if (direction.x > transform.position.x && !isFacingRight) // To the right
        {
            isFacingRight = true;
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);

        }
        else if (direction.x < transform.position.x && isFacingRight) // To the left
        {
            isFacingRight = false;
            transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y, transform.localScale.z);

        }
    }
    
    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void AttackAnimation()
    {
        if (attackClip)
            AudioManager.Instance.PlaySound(attackClip);
        animator.SetTrigger("Attack");
    }

    public void WalkAnimation(bool canWalk)
    {
        animator.SetBool("Walk", canWalk);
    }

    public void Damage(int damageAmount)
    {
        Vector3 bloodDir = UtilsClass.GetRandomDir();
        Blood_Handler.SpawnBlood(GetPosition(), bloodDir);

        DamagePopup.Create(GetPosition(), damageAmount, false);

        healthSystem.Damage(damageAmount);
        if (IsDead())
        {
            FlyingBody.Create(GameAssets.i.pfEnemyFlyingBody, GetPosition(), bloodDir);
            if (isEnemy)
            {
                int collectiveMaxCoin = GetComponent<EnemyController>().collectiveMaxCoin;
                UIManager.Instance.AddCoin(UnityEngine.Random.Range(collectiveMaxCoin / 2, collectiveMaxCoin));
                EnemyController.enemies.Remove(GetComponent<EnemyController>());
            }
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
