using System.Collections.Generic;
using UnityEngine;

public class SetBestFit : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField]
    private Camera mainCamera;

    [SerializeField]
    private List<float> cameraUnitSizes;
    #endregion

    #region Private Methods
    private void Awake()
    {
        float cameraUnitSize = 0;
        cameraUnitSize = mainCamera.transform.GetComponent<ViewportHandler>().UnitsSize;
        float ratio = Screen.width / (float)Screen.height;
        if (ratio > 2f) cameraUnitSize = cameraUnitSizes[0];
        else if (ratio <= 2f && ratio > 1.7f) cameraUnitSize = cameraUnitSizes[1];
        else if (ratio <= 1.7f && ratio > 1.5f) cameraUnitSize = cameraUnitSizes[2];
        else if (ratio <= 1.5f) cameraUnitSize = cameraUnitSizes[3];
        mainCamera.transform.GetComponent<ViewportHandler>().UnitsSize = cameraUnitSize;
    }
    #endregion
}
