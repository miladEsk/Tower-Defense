using UnityEngine;
using CodeMonkey.Utils;

public class ProjectileArrow : MonoBehaviour {

    #region Static Methods
    public static void Create(Vector3 spawnPosition, EnemyController enemy, int damageAmount, int indexArrow, AudioClip shootClip) {
        if (!ScriptableObjectManager.Instance.GetTower(indexArrow).arrowPrefab) return;
        Transform arrowTransform = Instantiate(ScriptableObjectManager.Instance.GetTower(indexArrow).arrowPrefab.transform, spawnPosition, Quaternion.identity);

        ProjectileArrow projectileArrow = arrowTransform.GetComponent<ProjectileArrow>();
        projectileArrow.Setup(enemy, damageAmount);

        AudioManager.Instance.PlaySound(shootClip);
    }
    #endregion

    #region Private Fields
    private EnemyController enemy;
    private int damageAmount;
    #endregion

    #region Private Methods
    private void Setup(EnemyController enemy, int damageAmount) {
        this.enemy = enemy;
        this.damageAmount = damageAmount;
    }

    private void Update() {
        if (enemy == null || enemy.gameObject.GetComponent<CharacterManager>().IsDead()) {
            // Enemy already dead
            Destroy(gameObject);
            return;
        }

        Vector3 targetPosition = enemy.gameObject.GetComponent<CharacterManager>().GetPosition();
        Vector3 moveDir = (targetPosition - transform.position).normalized;

        float moveSpeed = 130f;

        transform.position += moveDir * moveSpeed * Time.deltaTime;

        float angle = UtilsClass.GetAngleFromVectorFloat(moveDir);
        transform.eulerAngles = new Vector3(0, 0, angle);

        float destroySelfDistance = 1f;
        if (Vector3.Distance(transform.position, targetPosition) < destroySelfDistance) {
            enemy.GetComponent<IDamageable>().Damage(damageAmount);
            Destroy(gameObject);
        }
    }
    #endregion
}
