using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    #region Public Fields
    public Animator animator;
    public bool isFacingRight = true;
    #endregion

    #region Private Methods
    private void Start()
    {
        animator = GetComponent<Animator>();
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
        animator.SetTrigger("Attack");
    }

    public void WalkAnimation(bool canWalk)
    {
        animator.SetBool("Walk", canWalk);
    }
    #endregion
}
