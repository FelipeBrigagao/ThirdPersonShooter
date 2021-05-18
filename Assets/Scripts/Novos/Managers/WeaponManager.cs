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

    public bool changingWeapons;

    [SerializeField]
    Rig weaponAimingRigLayer;





    private void Start()
    {
        weaponIsEquiped = false;

        changingWeapons = false;

        weapons = new WeaponStats[2];

        WeaponStats weaponEquiped = equipedWeaponPivot.GetComponentInChildren<WeaponStats>();
        
        if (weaponEquiped)
        {
            StartCoroutine(Equip(weaponEquiped));

        }

    }

    

    public void ChangeEquipedWeapon(int weaponSlot)
    {
        StartCoroutine(ChangeWeapons(weapons[weaponSlot]));

    }


    public IEnumerator EquipNewWeapon(WeaponStats weapon)
    {
        changingWeapons = true;

        if (weaponIsEquiped)
        {
            yield return StartCoroutine(Unequip(equipedWeapon));

        }


        if(weapons[(int)weapon.WeaponInfo.type] != null)                                    //Se o slot do tipo de arma a ser equipada já está ocupado
        {
            
            Debug.Log("Destruindo arma");
            Destroy(weapons[(int)weapon.WeaponInfo.type].gameObject);
            
        }


        weapon.SetCrossHairTarget(this.crossHairTarget);

        if (weapon.WeaponInfo.type == WeaponType.Primary)
        {
            weapon.transform.SetParent(primaryWeaponPivot);

        }
        else
        {
            weapon.transform.SetParent(secundaryWeaponPivot);

        }

        weapon.transform.localPosition = weapon.WeaponInfo.posesInfo.pivotPosition;                                         //Deixar arma no pivot
        weapon.transform.localRotation = Quaternion.Euler(weapon.WeaponInfo.posesInfo.pivotRotation);

        yield return StartCoroutine(Equip(weapon));   

        changingWeapons = false;

        weapons[(int)weapon.WeaponInfo.type] = weapon;

    }


    IEnumerator ChangeWeapons(WeaponStats weapon)
    {

        changingWeapons = true;

        if (weaponIsEquiped)
        {
            //Se a arma a equipar for igual a equipada não se equipa nada, mas se desequipa a atual
            if(weapon == equipedWeapon)
            {
                weapon = null;
            }

           //Destroy(equipedWeapon.gameObject);
           yield return StartCoroutine(Unequip(equipedWeapon));
            Debug.Log("Depois de unequip");

        }


        //Tem que esperar o unequip terminar para continuar e sobrescrever as animações

        if(weapon != null)
        {
            yield return StartCoroutine(Equip(weapon));                           //com o uso dessa coroutine o jogo vai esperar terminar a animação para se trocar as animações da arma que irá equipar
           
            Debug.Log("Depois de equip");
        }
        

        changingWeapons = false;

    }



    

    IEnumerator Equip(WeaponStats equipingWeapon)
    {

        yield return StartCoroutine(CharacterAnimation.Instance.SetEquipedWeaponAnimations(equipingWeapon.WeaponInfo.weaponAnimationHolsterAim, equipingWeapon.WeaponInfo.weaponAnimationPose, equipingWeapon.WeaponInfo.weaponAnimationAim, equipingWeapon.WeaponInfo.weaponAnimationPoseHolster));        //Troca as animações de pose do animator para as da próxima arma
        Debug.Log("Depois de change animations");

        //tocar a animação de equipar a arma
        yield return StartCoroutine(CharacterAnimation.Instance.EquipWeapon());

        Debug.Log("Depois de equip weapon");
        
        weaponIsEquiped = true;

        equipedWeapon = equipingWeapon;

    }






    IEnumerator Unequip(WeaponStats unequipingWeapon)
    {

        PlayerManager.Instance.CallChangeAimState(false);   //Tirar a pose de mirando quando a arma é desequipada
            
        weaponIsEquiped = false;

        equipedWeapon = null;

        if (unequipingWeapon != null)
        {

            weapons[(int)unequipingWeapon.WeaponInfo.type] = unequipingWeapon;

            // toca a animação de desarmar, o equipedPivot vai ficar no lugar onde a arma deve ficar guardada, colocar o pivot de unarmed com os valores do equiped
            yield return StartCoroutine(CharacterAnimation.Instance.UnequipWeapon());
            Debug.Log("Depois de unequip weapon");


        }
       
    }



    public void WeaponShootInputs()
    {
        if(weaponAimingRigLayer.weight >= 1)
        {
            equipedWeapon.Shoot();

        }

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
