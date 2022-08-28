using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Awake()
    {
        EnemyController.enemies.Clear();
    }
}
