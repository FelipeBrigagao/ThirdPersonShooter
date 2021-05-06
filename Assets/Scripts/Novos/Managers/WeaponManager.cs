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

    WeaponStats equipedWeapon;

    WeaponStats[] weapons;

    [SerializeField]
    Transform equipedWeaponPivot;

    [SerializeField]
    Transform primaryWeaponPivot;
    
    [SerializeField]
    Transform secundaryWeaponPivot;

    public bool weaponIsEquiped;


    [Header("Animation poses instances")]
    
    [SerializeField]
    Rig handsIKRig;

    [SerializeField]
    Rig weaponPoseRigLayer;

    [SerializeField]
    Transform rightHandPosition;

    [SerializeField]
    Transform rightHandHint;

    [SerializeField]
    Transform leftHandPosition;

    [SerializeField]
    Transform leftHandHint;

    [SerializeField]
    GameObject player;



    private void Start()
    {
        weapons = new WeaponStats[2];

        WeaponStats weaponEquiped = equipedWeaponPivot.GetComponentInChildren<WeaponStats>();

        WeaponStats[] weaponsUnequiped = primaryWeaponPivot.GetComponentsInChildren<WeaponStats>();

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

        weapons[(int)weapon.WeaponInfo.type] = equipedWeapon;

    }


    void Equip(WeaponStats weapon)
    {
        //Se a arma a equipar for igual a equipada não se equipa nada, mas se desequipa a atual
        if(weapon == equipedWeapon)
        {
            weapon = null;
        }


        if (weaponIsEquiped)
        {
           //Destroy(equipedWeapon.gameObject);

           Unequip(equipedWeapon);
        }



        if(weapon != null)
        {
            weapon.crossHairTarget = this.crossHairTarget;
           
            StartCoroutine(CharacterAnimation.Instance.SetEquipedWeaponAnimations(weapon.WeaponInfo.weaponAnimationHolsterAim, weapon.WeaponInfo.weaponAnimationPose, weapon.WeaponInfo.weaponAnimationAim, weapon.WeaponInfo.weaponAnimationPoseHolster));        //Troca as animações de pose do animator para as 

            //tocar a animação de equipar a arma
            StartCoroutine(CharacterAnimation.Instance.EquipWeapon());

            if(weapon.WeaponInfo.type == WeaponType.Primary)
            {
                weapon.transform.SetParent(primaryWeaponPivot);

            }
            else
            {
                weapon.transform.SetParent(secundaryWeaponPivot);

            }

            weapon.transform.localPosition = weapon.WeaponInfo.posesInfo.pivotPosition;                                         //Deixar arma no pivot
            weapon.transform.localRotation = Quaternion.Euler(weapon.WeaponInfo.posesInfo.pivotRotation);

            weaponIsEquiped = true;


        }
        else
        {
            weaponIsEquiped = false;

            // colocar animação de unarmed?

        }


        equipedWeapon = weapon;

    }

    void Unequip(WeaponStats weapon)
    {

        if(weapon != null)
        {

            // toca a animação de desarmar, o equipedPivot vai ficar no lugar onde a arma deve ficar guardada, colocar o pivot de unarmed com os valores do equiped
            StartCoroutine(CharacterAnimation.Instance.UnequipWeapon());


            if (weapon.WeaponInfo.type == WeaponType.Primary)
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




/*

    [ContextMenu("Save equiped weapon pose")]
    void SaveEquipedWeaponPose()
    {
        GameObjectRecorder recorder = new GameObjectRecorder(player);
        recorder.BindComponentsOfType<Transform>(equipedWeaponPivot.gameObject, false);
        recorder.BindComponentsOfType<Transform>(rightHandPosition.gameObject, false);
        recorder.BindComponentsOfType<Transform>(leftHandPosition.gameObject, false);
        recorder.BindComponentsOfType<Transform>(rightHandHint.gameObject, false);
        recorder.BindComponentsOfType<Transform>(leftHandHint.gameObject, false);
        recorder.TakeSnapshot(0.0f);
        recorder.SaveToClip(equipedWeapon.WeaponInfo.weaponAnimationPose);
        UnityEditor.AssetDatabase.SaveAssets();

    }


    [ContextMenu("Save equiped weapon aiming")]
    void SaveEquipedWeaponAiming()
    {
        GameObjectRecorder recorder = new GameObjectRecorder(player);
        recorder.BindComponentsOfType<Transform>(equipedWeaponPivot.gameObject, false);
        recorder.BindComponentsOfType<Transform>(rightHandPosition.gameObject, false);
        recorder.BindComponentsOfType<Transform>(leftHandPosition.gameObject, false);
        recorder.BindComponentsOfType<Transform>(rightHandHint.gameObject, false);
        recorder.BindComponentsOfType<Transform>(leftHandHint.gameObject, false);
        recorder.TakeSnapshot(0.0f);
        recorder.SaveToClip(equipedWeapon.WeaponInfo.weaponAnimationAiming);
        UnityEditor.AssetDatabase.SaveAssets();

    }


    */







}
