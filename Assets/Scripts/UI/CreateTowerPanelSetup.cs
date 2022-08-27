using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CreateTowerPanelSetup : MonoBehaviour
{
    #region Serialised Fields
    [Header("Texts")]
    [SerializeField]
    private TMP_Text archerBuildPriceText;
    [SerializeField]
    private TMP_Text barrackBuildPriceText;
    [SerializeField]
    private TMP_Text wizardBuildPriceText;
    [SerializeField]
    private TMP_Text queenArcherPriceText;

    [Header("Lists")]
    [SerializeField]
    private List<TowerScriptableObject> towers;
    #endregion

    #region Private Methods
    private void Start()
    {
        SetItem();
    }


    private void SetItem()
    {
        archerBuildPriceText.text = towers[0].buildPrice + "";
        barrackBuildPriceText.text = towers[1].buildPrice + "";
        wizardBuildPriceText.text = towers[2].buildPrice + "";
        queenArcherPriceText.text = towers[3].buildPrice + "";
    }
    #endregion
}
