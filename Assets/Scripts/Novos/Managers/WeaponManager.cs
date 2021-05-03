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


    [Header("Animation poses instances")]
    
    [SerializeField]
    Rig handsIKRig;

    [SerializeField]
    Rig weaponPoseRigLayer;

    [SerializeField]
    Rig weaponAwayRigLayer;

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

           // Destroy(equipedWeapon.gameObject);

            Unequip(equipedWeapon);
        }

        if(weapon != null)
        {
            weapon.crossHairTarget = this.crossHairTarget;

            weapon.transform.SetParent(equipedWeaponPivot);

            weapon.transform.localPosition = weapon.WeaponInfo.posesInfo.pivotPosition;                                         //Deixar arma no pivot
            weapon.transform.localRotation = Quaternion.Euler(weapon.WeaponInfo.posesInfo.pivotRotation);

            weaponIsEquiped = true;

            handsIKRig.weight = 1f;

            //weaponAwayRigLayer.weight = 0f;            //Vai ser sempre 1


            StartCoroutine(CharacterAnimation.Instance.SetEquipedWeaponAnimations(weapon.WeaponInfo.weaponAnimationPose, weapon.WeaponInfo.weaponAnimationAiming));        //Troca as animações de pose do animator para as 
                                                                                                                                                                                                                                        //animações da arma atual

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

        }

        CharacterAnimation.Instance.ChangeWeaponLayerWeight(weaponIsEquiped);               //Muda o weight da Layer de animação de pose da arma com relação a se tem uma arma equipada ou não

        equipedWeapon = weapon;

    }

    void Unequip(WeaponStats weapon)
    {

        if(weapon != null)
        {

            weapon.transform.SetParent(primaryWeaponUnequipedPivot);                   //Seta o pai da arma para o pivot da arma desequipada

            weapon.transform.localPosition = weapon.WeaponInfo.posesInfo.pivotPosition;                                       //Deixar arma no pivot
            weapon.transform.localRotation = Quaternion.Euler(weapon.WeaponInfo.posesInfo.pivotRotation);

            //weaponAwayRigLayer.weight = 1f;           //Vai ser sempre 1



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










}
