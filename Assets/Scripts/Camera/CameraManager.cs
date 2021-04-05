using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    Camera mainCamera;

    [SerializeField]
    GameObject thirdPersonCam;

    CinemachineCameraOffset cineCamOffset;
    
    Vector3 armedOffset;
    Vector3 disarmedOffset;

    [SerializeField]
    float changeCamPositionSmoother = 5f;

    bool armed;

    private void Start()
    {
        armedOffset = new Vector3(0.45f, 0.1f, 2f);
        disarmedOffset = new Vector3(0, 0, 0);

        cineCamOffset = thirdPersonCam.GetComponent<CinemachineCameraOffset>();

        cineCamOffset.m_Offset = disarmedOffset;

        armed = false;

        PlayerManager.OnEquipWeapon += ChangePerpective;

    }




    private void ChangePerpective(bool armedState)
    {

        armed = armedState;

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
        while (cineCamOffset.m_Offset != armedOffset && armed)
        {
            cineCamOffset.m_Offset = Vector3.Lerp(cineCamOffset.m_Offset, armedOffset, changeCamPositionSmoother * Time.deltaTime);    

            yield return null;
        
        }
    }

    IEnumerator ChangeToDisarmedOffset()
    {
        while (cineCamOffset.m_Offset != disarmedOffset && !armed)
        {
            cineCamOffset.m_Offset = Vector3.Lerp(cineCamOffset.m_Offset, disarmedOffset, changeCamPositionSmoother * Time.deltaTime);

            yield return null;
        
        }
    }

    private void OnDisable()
    {

        PlayerManager.OnEquipWeapon -= ChangePerpective;
    }


}
