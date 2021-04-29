using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerManager : MonoBehaviour
{
    #region Singleton

    private static PlayerManager _instance;

    public static PlayerManager Instance
    {
        get
        {
            if(_instance == null)
            {
                Debug.LogError("Player Manager não encontrado, criando um.");

                GameObject go = new GameObject("PlayerManager");

                go.AddComponent<PlayerManager>();
                
            }
            return _instance;
        }
    }


    private void Awake()
    {
        if(_instance != null)
        {
            Debug.LogWarning("Player manager já existente, uma segunda instância não será criada.");
            return;
        }

        Debug.Log("Player Manager criado.");
        _instance = this;
    }

    #endregion


    public ICharacterMovement actualMode;
    CharacterMovementArmed cma;
    CharacterMovementUnnarmed cmu;

    bool aiming = false;




    public static event Action<bool> OnAimWeapon;

    public void CallChangeAimState(bool state)
    {
        OnAimWeapon?.Invoke(state);
    }





    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;


        cma = GetComponent<CharacterMovementArmed>();
        cmu = GetComponent<CharacterMovementUnnarmed>();

        OnAimWeapon += ChangeStates;

        actualMode = cmu;
    }
     


    void ChangeStates(bool weaponAiming)
    {
        aiming = weaponAiming;

        if (aiming && actualMode.Equals(cmu))
        {
            actualMode = cma;
            cma.enabled = true;
            cmu.enabled = false;

        }
        else if (!aiming && actualMode.Equals(cma))
        {
            actualMode = cmu;
            cmu.enabled = true;
            cma.enabled = false;

        }
    }

    private void OnDisable()
    {
        OnAimWeapon -= ChangeStates;
    }


}
