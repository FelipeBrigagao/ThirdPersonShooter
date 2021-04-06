using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    Vector2 inputs;

    private void Update()
    {
        MoveInput();


        AimInput();



    }


    void MoveInput()
    {
        inputs = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        GetComponent<PlayerManager>().actualMode.SetInput(inputs);
    }


    void AimInput()
    {
        if (Input.GetMouseButtonDown(1))
        {
            PlayerManager.Instance.CallChangeEquipState(true);

        }else if (Input.GetMouseButtonUp(1))
        {
            PlayerManager.Instance.CallChangeEquipState(false);

        }


    }


}
