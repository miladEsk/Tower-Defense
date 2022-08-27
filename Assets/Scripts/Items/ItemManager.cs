using System.Collections.Generic;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    #region Serialied Fields
    [SerializeField]
    private List<TowerScriptableObject> towers;
    #endregion

    #region Public Methods
    public TowerScriptableObject GetTower(int index)
    {
        return towers[index];
    }
    #endregion
}
