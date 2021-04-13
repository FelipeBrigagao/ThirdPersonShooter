using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSettings : MonoBehaviour
{


    [SerializeField]
    Camera mainCamera;

    [SerializeField]
    GameObject thirdPersonCam;

    CinemachineCameraOffset cineCamOffset;

    [SerializeField]
    Vector3 armedOffset;

    [SerializeField]
    Vector3 disarmedOffset;

    [SerializeField]
    float changeCamPositionSmoother = 5f;

    bool playerArmed = false;

    private void Start()
    {
        armedOffset = new Vector3(0.6f, 0.1f, 1f);
        disarmedOffset = new Vector3(0, 0, 0);

        cineCamOffset = thirdPersonCam.GetComponent<CinemachineCameraOffset>();

        cineCamOffset.m_Offset = disarmedOffset;

        PlayerManager.OnAimWeapon += ChangePerspective;

    }




    private void ChangePerspective(bool armedState)
    {

        playerArmed = armedState;

        if (armedState)
        {
            StartCoroutine(ChangeToArmedOffset());
        
        }else if (!armedState)
        {
            StartCoroutine(ChangeToDisarmedOffset());

        }


    }


    IEnumerator ChangeToArmedOffset()
    {
        while (cineCamOffset.m_Offset != armedOffset && playerArmed)
        {
            cineCamOffset.m_Offset = Vector3.Lerp(cineCamOffset.m_Offset, armedOffset, changeCamPositionSmoother * Time.deltaTime);    

            yield return null;
        
        }
    }

    IEnumerator ChangeToDisarmedOffset()
    {
        while (cineCamOffset.m_Offset != disarmedOffset && !playerArmed)
        {
            cineCamOffset.m_Offset = Vector3.Lerp(cineCamOffset.m_Offset, disarmedOffset, changeCamPositionSmoother * Time.deltaTime);

            yield return null;
        
        }
    }

    private void OnDisable()
    {

        PlayerManager.OnAimWeapon -= ChangePerspective;
    }


}
