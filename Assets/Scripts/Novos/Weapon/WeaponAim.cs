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
        PlayerManager.OnEquipWeapon += ChangeAimingState;
    }

    void ChangeAimingState(bool aimingState)
    {
        aiming = aimingState;
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
        PlayerManager.OnEquipWeapon -= ChangeAimingState;
    }


}
