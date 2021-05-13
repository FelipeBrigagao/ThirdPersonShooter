using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerInput : MonoBehaviour
{
    Vector2 inputs;


    private void Update()
    {
        MoveInput();

        if (!WeaponManager.Instance.changingWeapons)
        {
            ChangeWeapon();

        }


        if (WeaponManager.Instance.weaponIsEquiped)
        {
            WeaponInput();
        
        }


    }


    void MoveInput()
    {
        inputs = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        GetComponent<PlayerManager>().actualMode.SetInput(inputs);
    }


    void ChangeWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            WeaponManager.Instance.ChangeEquipedWeapon(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            WeaponManager.Instance.ChangeEquipedWeapon(1);
        }

    }


    void WeaponInput()
    {
        if (Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(0))
        {
            PlayerManager.Instance.CallChangeAimState(true);

        }
        if (Input.GetMouseButtonUp(1) && !Input.GetMouseButton(0) || Input.GetMouseButtonUp(0) && !Input.GetMouseButton(1))
        {
            PlayerManager.Instance.CallChangeAimState(false);

        }



        if (Input.GetMouseButton(0) )
        {
           WeaponManager.Instance.WeaponShootInputs();
        }
    }




}
