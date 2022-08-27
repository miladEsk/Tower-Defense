using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Items/Tower"))]
public class TowerScriptableObject : ScriptableObject
{
    [Header("Values")]
    public int buildPrice;
    public int range;
    public int shootInterval;
    public int damage;
    [Tooltip("It has to be set for Barrack Tower only")]
    [Range(0,5)]
    public int playerCount;
    public int playerMoveSpeed;
    [Header("Sprites")]
    public List<Sprite> sprites;
    [Header("Prefabs")]
    public GameObject arrowPrefab;
    public GameObject playerPrefab;
    public GameObject towerPrefab;
}
