using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class CameraControls : MonoBehaviourPun
{
    [SerializeField] int[] CameraLensSizes;
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    [SerializeField] TextMeshProUGUI ZoomLevelText;
    int cameraLensSizeNo = 0;

    // Start is called before the first frame update
    void Start()
    {
        virtualCamera.m_Lens.OrthographicSize = CameraLensSizes[0];
        if (!photonView.IsMine)
            Destroy(GetComponent<CameraControls>());
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetKeyDown("z"))
            {
                CycleCamera();
            }
        }
    }

    public void CycleCamera()
    {
        if (cameraLensSizeNo < CameraLensSizes.Length - 1)
            cameraLensSizeNo++;
        else
            cameraLensSizeNo = 0;
        virtualCamera.m_Lens.OrthographicSize = CameraLensSizes[cameraLensSizeNo];
        ZoomLevelText.text = (cameraLensSizeNo + 1).ToString();
    }
}
