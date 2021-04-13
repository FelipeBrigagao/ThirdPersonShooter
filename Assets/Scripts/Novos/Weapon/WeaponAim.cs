using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class WeaponAim : MonoBehaviour
{

    [SerializeField]
    Rig weaponRigLayer;

    [SerializeField]
    float aimingSpeed = 0.1f;

    bool aiming;

    private void Start()
    {
        PlayerManager.OnAimWeapon += ChangeAimingState;

    }

    void ChangeAimingState(bool aimingState)
    {
        aiming = aimingState;
    }


    private void Update()
    {
        if (WeaponManager.Instance.weaponIsEquiped)
        {
            if (Input.GetMouseButtonDown(1))
            {
                PlayerManager.Instance.CallChangeAimState(true);

            }
            if (Input.GetMouseButtonUp(1))
            {
                PlayerManager.Instance.CallChangeAimState(false);

            }

        }
    }


    private void FixedUpdate()
    {
        if (aiming && weaponRigLayer.weight < 1)
        {
            weaponRigLayer.weight += Time.deltaTime / aimingSpeed;

        }else if (!aiming && weaponRigLayer.weight > 0)
        {
            weaponRigLayer.weight -= Time.deltaTime / aimingSpeed;

        }
    }

    private void OnDisable()
    {
        PlayerManager.OnAimWeapon -= ChangeAimingState;
    }


}
