using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class WeaponManager : MonoBehaviour
{
    #region Singleton

    private static WeaponManager _instance;

    public static WeaponManager Instance
    {
        get
        {
            if(_instance == null)
            {
                Debug.LogWarning("Weapon manager não encontrado, criando um.");

                GameObject go = new GameObject("WeaponManager");

                go.AddComponent<WeaponManager>();

            }

            return _instance;
        }
    }

    private void Awake()
    {
        if(_instance != null)
        {
            Debug.LogWarning("Weapon manager já existente, uma segunda instância não será criada.");
            return;
        }

        Debug.Log("Weapon Manager criado.");
        _instance = this;

    }

    #endregion

    public bool weaponIsEquiped;

    WeaponStats equipedWeapon;

    WeaponStats[] weapons;



    [SerializeField]
    Transform equipedWeaponPivot;

    [SerializeField]
    Transform unequipedWeaponPivot;


   
    [SerializeField]
    Rig handsIKRig;

    [SerializeField]
    Rig weaponPoseRigLayer;

    [SerializeField]
    Rig weaponAwayRigLayer;


    private void Start()
    {
        weapons = new WeaponStats[2];

        WeaponStats weaponEquiped = equipedWeaponPivot.GetComponentInChildren<WeaponStats>();

        WeaponStats[] weaponsUnequiped = unequipedWeaponPivot.GetComponentsInChildren<WeaponStats>();

        Equip(weaponEquiped);

        foreach(WeaponStats w in weaponsUnequiped)
        {
            Unequip(w);
        }


    }

    

    public void ChangeEquipedWeapon(int weaponSlot)
    {

        WeaponStats weaponAux;

        weaponAux = equipedWeapon;

        Equip(weapons[weaponSlot]);

        Unequip(weaponAux);

    }


    public void AddNewWeapon(WeaponStats weapon)
    {
        


    }

    void Equip(WeaponStats weapon)
    {

        if(weapon != null)
        {
            weapon.transform.SetParent(equipedWeaponPivot);

            weapon.transform.localPosition = weapon.WeaponInfo.posesInfo.pivotPosition;                                         //Deixar arma no pivot
            weapon.transform.localRotation = Quaternion.Euler(weapon.WeaponInfo.posesInfo.pivotRotation);

            weaponIsEquiped = true;

            handsIKRig.weight = 1f;

            weaponPoseRigLayer.weight = 1f;

            weaponAwayRigLayer.weight = 0f;



            if (weapon.WeaponInfo.type == WeaponType.Primary)
            {
                weapons[0] = null;
            }
            else
            {
                weapons[1] = null;
            }


        }
        else
        {
            weaponIsEquiped = false;

            handsIKRig.weight = 0f;

            weaponPoseRigLayer.weight = 0f;

        }

        equipedWeapon = weapon;

    }

    void Unequip(WeaponStats weapon)
    {

        if(weapon != null)
        {

            weapon.transform.SetParent(unequipedWeaponPivot);                   //Seta o pai da arma para o pivot da arma desequipada

            weapon.transform.localPosition = weapon.WeaponInfo.posesInfo.pivotPosition;                                       //Deixar arma no pivot
            weapon.transform.localRotation = Quaternion.Euler(weapon.WeaponInfo.posesInfo.pivotRotation);

            weaponAwayRigLayer.weight = 1f;



            if(weapon.WeaponInfo.type == WeaponType.Primary)
            {
                weapons[0] = weapon;
            }
            else
            {
                weapons[1] = weapon;
            }




        }
       



    }



    public void WeaponShootInputs()
    {
        equipedWeapon.Shoot();

    }




}
