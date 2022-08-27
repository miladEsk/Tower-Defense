using UnityEngine;

public class UpgradeOverlay : MonoBehaviour {
    #region Singleton
    private static UpgradeOverlay Instance;
    private void Awake() {
        if(Instance == null) Instance = this;

        Hide();
    }
    #endregion

    #region Private Fields
    private Tower tower;
    #endregion

    #region Static Methods
    public static void Show_Static(Tower tower) {
        Instance.Show(tower);
    }
    #endregion

    #region Private Methods
    private void Show(Tower tower) {
        this.tower = tower;
        gameObject.SetActive(true);
        transform.position = tower.transform.position;
        RefreshRangeVisual();
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

    private void RefreshRangeVisual() {
        transform.Find("Range").localScale = Vector3.one * tower.GetRange() * 2f;
    }
    #endregion
}
