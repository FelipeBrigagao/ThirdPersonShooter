using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerManagerOld : MonoBehaviour
{
    bool armed;

    public static event Action <bool> OnEquipWeapon;


    private void Start()
    {
        armed = false;

        CallEquipWeaponEvent();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))                                                   //Teste para trocar entre armado e desarmado
        {
            armed = !armed;

            Debug.Log(armed);

            CallEquipWeaponEvent();
        }
    }

    public void CallEquipWeaponEvent()
    {
        OnEquipWeapon?.Invoke(armed);
    }



}
