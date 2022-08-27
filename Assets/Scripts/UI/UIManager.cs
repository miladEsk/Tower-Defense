using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    #region Singleton
    public static UIManager Instance;
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    #endregion

    #region Static Fields
    public static int towerPlaceIndex = 0;
    #endregion

    #region Serialized Fields
    [Header("Texts")]
    [SerializeField]
    private TMP_Text healthText;
    [SerializeField]
    private TMP_Text coinText;
    [SerializeField]
    private TMP_Text currentWaveText;
    [SerializeField]
    private TMP_Text maxWaveText;
    [SerializeField]
    private TMP_Text showWaveBannerText;
    [Header("Panels")]
    [SerializeField]
    private GameObject createTowerPanel;
    [SerializeField]
    private GameObject winPanel;
    [SerializeField]
    private GameObject pausePanel;
    
    [Header("Lists")]
    [SerializeField]
    private List<TowerScriptableObject> towers;
    [SerializeField]
    private List<SpriteRenderer> towerPlaces;
    #endregion

    #region Private Fields
    private int currentHealth = 100;
    private int totalCoin = 200;
    private int currentWave = 0;

    private int buildPrice = 0;
    private int towerIndex = 0;
    #endregion

    #region Public Methods
    public void ChangeHealth(int damageAmount)
    {
        currentHealth -= damageAmount;
        healthText.text = currentHealth + "";
        if(currentHealth <= 0)
        {
            Time.timeScale = 0;
        }
    }

    public void AddCoin(int coin)
    {
        totalCoin += coin;
        coinText.text = totalCoin + "";
    }

    public void LoseCoin()
    {
        totalCoin -= buildPrice;
        coinText.text = totalCoin + "";
    }

    public void ChangeWave()
    {
        currentWave += 1;
        currentWaveText.text = currentWave + "";
        showWaveBannerText.text = "Wave " + currentWave;
    }

    public void Win()
    {
        winPanel.GetComponent<Animator>().SetTrigger("Show");
    }
    #endregion

    #region Listeners
    public void OnClickCreateTower(int index)
    {
        buildPrice = towers[index].buildPrice;
        if (totalCoin < buildPrice) return;
        GameObject tower = Instantiate(towers[index].towerPrefab, towerPlaces[towerPlaceIndex].transform.position,Quaternion.identity);
        towerPlaces[towerPlaceIndex].enabled = false;
        tower.transform.parent = towerPlaces[towerPlaceIndex].transform;
        
        LoseCoin();
        towerPlaces[towerPlaceIndex].gameObject.GetComponent<TowerPlace>().CreateTowerPanel();
    }

    public void OnClickSellTower()
    {
        towerPlaces[towerPlaceIndex].enabled = true;
        for (int i = 0; i < towerPlaces[towerPlaceIndex].transform.childCount; i++)
        {
            Destroy(towerPlaces[towerPlaceIndex].transform.GetChild(1).gameObject);
        }

        towerPlaces[towerPlaceIndex].gameObject.GetComponent<TowerPlace>().SellTowerPanel();

        towerIndex = towerPlaces[towerPlaceIndex].gameObject.GetComponentInChildren<Tower>().towerIndex;
        AddCoin(towers[towerIndex].buildPrice / 2);
    }

    public void OnClickPauseButton()
    {
        Time.timeScale = 0;
        pausePanel.SetActive(true);
    }

    public void OnClickResumeButton()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }

    public void OnClickRestartButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnClickQuitButton()
    {
        Time.timeScale = 1;
        Application.Quit();
    }
    #endregion
}
