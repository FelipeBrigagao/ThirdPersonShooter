using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    public ICharacterMovement actualMode;
    CharacterMovementArmed cma;
    CharacterMovementUnnarmed cmu;

    bool armed = false;




    public static event Action<bool> OnEquipWeapon;

    public void CallChangeEquipState(bool state)
    {
        OnEquipWeapon?.Invoke(state);
    }





    private void Start()
    {
        cma = GetComponent<CharacterMovementArmed>();
        cmu = GetComponent<CharacterMovementUnnarmed>();

        actualMode = cmu;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            armed = !armed;

            CallChangeEquipState(armed);

        }


        if(armed && actualMode.Equals(cmu))
        {
            actualMode = cma;
            cma.enabled = true;
            cmu.enabled = false;
        
        }else if(!armed && actualMode.Equals(cma))
        {
            actualMode = cmu;
            cmu.enabled = true;
            cma.enabled = false;

        }


    }

}
