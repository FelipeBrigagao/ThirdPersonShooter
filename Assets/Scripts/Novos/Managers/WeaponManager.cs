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

    [SerializeField]
    Transform equipedWeaponPivot;

    [SerializeField]
    Transform unequipedWeaponPivot;

    [SerializeField]
    WeaponStats equipedWeapon;
   
    [SerializeField]
    Rig handsIKRig;

    [SerializeField]
    Rig weaponPoseRigLayer;

    [SerializeField]
    Rig weaponAwayRigLayer;

    [SerializeField]
    WeaponStats[] weapons;


    private void Start()
    {
        weapons = new WeaponStats[2];

        if (equipedWeapon != null)
        {
            weaponIsEquiped = true;
        }
    }

    public void ChangeEquipedWeapon(int weaponSlot)
    {

        Equip(weapons[weaponSlot]);

        Unequip(equipedWeapon);

        WeaponStats aux;

        aux = weapons[weaponSlot];

        weapons[weaponSlot] = equipedWeapon;

        equipedWeapon = aux;

    }

    public void AddNewWeapon(WeaponStats weapon)
    {
        


    }

    void Equip(WeaponStats weapon)
    {
        if(weapon != null)
        {
            weapon.transform.SetParent(equipedWeaponPivot);

            weapon.transform.localPosition = weapon.weapon.posesInfo.pivotPosition;                                         //Deixar arma no pivot
            weapon.transform.localRotation = Quaternion.Euler(weapon.weapon.posesInfo.pivotRotation);

            weaponIsEquiped = true;

            handsIKRig.weight = 1f;

            weaponPoseRigLayer.weight = 1f;

            weaponAwayRigLayer.weight = 0f;

        }
        else
        {
            weaponIsEquiped = false;

            handsIKRig.weight = 0f;

            weaponPoseRigLayer.weight = 0f;

        }


    }

    void Unequip(WeaponStats weapon)
    {

        if(weapon != null)
        {

            weapon.transform.SetParent(unequipedWeaponPivot);                   //Seta o pai da arma para o pivot da arma desequipada

            weapon.transform.localPosition = weapon.weapon.posesInfo.pivotPosition;                                       //Deixar arma no pivot
            weapon.transform.localRotation = Quaternion.Euler(weapon.weapon.posesInfo.pivotRotation);

            weaponAwayRigLayer.weight = 1f;

        }
       



    }

}
