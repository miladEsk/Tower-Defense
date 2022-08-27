using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region Singleton
    public static AudioManager Instance;
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    #endregion

    #region Serialized Fields
    [SerializeField]
    private AudioSource audioSource;
    #endregion

    #region Public Methods
    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
    #endregion
}
