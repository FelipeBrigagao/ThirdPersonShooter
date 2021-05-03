using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEditor.Animations;

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

    [Header("Equiping weapons instances")]

    [SerializeField]
    Transform crossHairTarget;

    public bool weaponIsEquiped;

    WeaponStats equipedWeapon;

    WeaponStats[] weapons;

    [SerializeField]
    Transform equipedWeaponPivot;

    [SerializeField]
    Transform primaryWeaponUnequipedPivot;
    
    [SerializeField]
    Transform secundaryWeaponUnequipedPivot;




    private void Start()
    {
        weapons = new WeaponStats[2];

        WeaponStats weaponEquiped = equipedWeaponPivot.GetComponentInChildren<WeaponStats>();

        WeaponStats[] weaponsUnequiped = primaryWeaponUnequipedPivot.GetComponentsInChildren<WeaponStats>();

        Equip(weaponEquiped);

        foreach(WeaponStats w in weaponsUnequiped)
        {
            Unequip(w);
        }


    }

    

    public void ChangeEquipedWeapon(int weaponSlot)
    {
        Equip(weapons[weaponSlot]);

    }


    public void AddNewWeapon(WeaponStats weapon)
    {
        Equip(weapon);

    }

    void Equip(WeaponStats weapon)
    {

        if (weaponIsEquiped)
        {

            Destroy(equipedWeapon.gameObject);

            //Unequip(equipedWeapon);
        }

        if(weapon != null)
        {
            weapon.crossHairTarget = this.crossHairTarget;

            weapon.transform.SetParent(equipedWeaponPivot);

            weapon.transform.localPosition = weapon.WeaponInfo.posesInfo.pivotPosition;                                         //Deixar arma no pivot
            weapon.transform.localRotation = Quaternion.Euler(weapon.WeaponInfo.posesInfo.pivotRotation);

            weaponIsEquiped = true;

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

        }

        

        equipedWeapon = weapon;

    }

    void Unequip(WeaponStats weapon)
    {

        if(weapon != null)
        {



            //weaponAwayRigLayer.weight = 1f;           //Vai ser sempre 1



            if(weapon.WeaponInfo.type == WeaponType.Primary)
            {
                weapon.transform.SetParent(primaryWeaponUnequipedPivot);                   //Seta o pai da arma para o pivot da arma desequipada
              
                weapons[0] = weapon;
            
            }
            else
            {
                weapon.transform.SetParent(secundaryWeaponUnequipedPivot);

                weapons[1] = weapon;
            }


            weapon.transform.localPosition = weapon.WeaponInfo.posesInfo.pivotPosition;                                       //Deixar arma no pivot
            weapon.transform.localRotation = Quaternion.Euler(weapon.WeaponInfo.posesInfo.pivotRotation);


        }
       
    }



    public void WeaponShootInputs()
    {
        equipedWeapon.Shoot();

    }

}
