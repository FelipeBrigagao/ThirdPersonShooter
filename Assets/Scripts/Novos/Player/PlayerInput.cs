using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    Vector2 inputs;

    private void Update()
    {
        MoveInput();

        ChangeWeapon();

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

}
