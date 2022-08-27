using System.Collections.Generic;
using UnityEngine;

public class TowerPlace : MonoBehaviour
{
    #region Serialied Fields
    [Header("Values")]
    [SerializeField]
    private int towerIndex = 0;
    [Header("Transforms")]
    [SerializeField]
    private Transform playerPosition;
    [Header("Panels")]
    [SerializeField]
    private GameObject createTowerPanel;
    [SerializeField]
    private GameObject sellTowerPanel;
    #endregion

    #region Static Fields
    private static List<bool> isOpenCreates = new List<bool>();
    private static List<bool> isOpenSells = new List<bool>();
    #endregion

    #region Private Methods
    private void Start()
    {
        isOpenCreates.Add(false);
        isOpenSells.Add(false);
    }

    private void OnMouseDown()
    {
        if (transform.childCount > 1) SellTowerPanel();
        else CreateTowerPanel();
    }
    #endregion

    #region Public Methods
    public void CreateTowerPanel()
    {
        UIManager.towerPlaceIndex = towerIndex;
        createTowerPanel.GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(transform.localPosition);
        isOpenCreates[towerIndex] = !isOpenCreates[towerIndex];
        createTowerPanel.GetComponent<Animator>().SetBool("IsOpen", isOpenCreates[towerIndex]);
        sellTowerPanel.GetComponent<Animator>().SetBool("IsOpen", false);
        for (int i = 0; i < isOpenSells.Count; i++)
        {
            isOpenSells[i] = false;
        }

        for (int i = 0; i < isOpenCreates.Count; i++)
        {
            if (i == towerIndex) continue;
            isOpenCreates[i] = false;
        }
    }

    public void SellTowerPanel()
    {
        UIManager.towerPlaceIndex = towerIndex;
        UIManager.Instance.ChangeSellTower();
        sellTowerPanel.GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(transform.localPosition);
        isOpenSells[towerIndex] = !isOpenSells[towerIndex];
        sellTowerPanel.GetComponent<Animator>().SetBool("IsOpen", isOpenSells[towerIndex]);
        createTowerPanel.GetComponent<Animator>().SetBool("IsOpen", false);
        for (int i = 0; i < isOpenCreates.Count; i++)
        {
            isOpenCreates[i] = false;
        }

        for (int i = 0; i < isOpenSells.Count; i++)
        {
            if (i == towerIndex) continue;
            isOpenSells[i] = false;
        }
    }

    public Transform GetPlayerPosition()
    {
        return playerPosition;
    }
    #endregion
}
