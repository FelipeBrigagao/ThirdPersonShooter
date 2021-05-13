using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class WeaponAim : MonoBehaviour
{

    [SerializeField]
    Rig weaponAimingRigLayer;

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


    private void FixedUpdate()
    {
          if (aiming && weaponAimingRigLayer.weight < 1)
          {
              weaponAimingRigLayer.weight += Time.deltaTime / aimingSpeed;
              CharacterAnimation.Instance.ChangeWeaponAimingAnimationsWeight(weaponAimingRigLayer.weight);                                      //Altera entre as animações de pose com a arma mirando ou não

          }else if (!aiming && weaponAimingRigLayer.weight > 0)
          {
              weaponAimingRigLayer.weight -= Time.deltaTime / aimingSpeed;
              CharacterAnimation.Instance.ChangeWeaponAimingAnimationsWeight(weaponAimingRigLayer.weight);

          }

    }

    private void OnDisable()
    {
        PlayerManager.OnAimWeapon -= ChangeAimingState;
    }


}
