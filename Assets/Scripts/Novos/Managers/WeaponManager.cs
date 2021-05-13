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

    [SerializeField]
    bool unarmed;

    [SerializeField]
    Rig weaponAimingRigLayer;





    private void Start()
    {
        weaponIsEquiped = false;

        unarmed = true;

        weapons = new WeaponStats[2];

        WeaponStats weaponEquiped = equipedWeaponPivot.GetComponentInChildren<WeaponStats>();

        Equip(weaponEquiped);

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

        if (weaponIsEquiped)
        {
            //Se a arma a equipar for igual a equipada não se equipa nada, mas se desequipa a atual
            if(weapon == equipedWeapon)
            {
                weapon = null;
            }

           //Destroy(equipedWeapon.gameObject);
           Unequip(equipedWeapon);

            weaponIsEquiped = false;
        }


        //Tem que esperar o unequip terminar para continuar e sobrescrever as animações

        if(weapon != null)
        {
            StartCoroutine(EquipWaiting(weapon));                           //com o uso dessa coroutine o jogo vai esperar terminar a animação (event dentro da animação avisa o termino dela) para se trocar as animações da arma que irá equipar

        }
        else
        {
             equipedWeapon = weapon;
        }



    }



    

    IEnumerator EquipWaiting(WeaponStats equipingWeapon)
    {

        yield return new WaitUntil(() => unarmed);          // Vai esperar até a não ter uma arma equipada para se equipar a próxima


        equipingWeapon.crossHairTarget = this.crossHairTarget;

        StartCoroutine(CharacterAnimation.Instance.SetEquipedWeaponAnimations(equipingWeapon.WeaponInfo.weaponAnimationHolsterAim, equipingWeapon.WeaponInfo.weaponAnimationPose, equipingWeapon.WeaponInfo.weaponAnimationAim, equipingWeapon.WeaponInfo.weaponAnimationPoseHolster));        //Troca as animações de pose do animator para as 

        //tocar a animação de equipar a arma
        CharacterAnimation.Instance.EquipWeapon();

        if (equipingWeapon.WeaponInfo.type == WeaponType.Primary)
        {
            equipingWeapon.transform.SetParent(primaryWeaponPivot);

        }
        else
        {
            equipingWeapon.transform.SetParent(secundaryWeaponPivot);

        }

        equipingWeapon.transform.localPosition = equipingWeapon.WeaponInfo.posesInfo.pivotPosition;                                         //Deixar arma no pivot
        equipingWeapon.transform.localRotation = Quaternion.Euler(equipingWeapon.WeaponInfo.posesInfo.pivotRotation);

        weaponIsEquiped = true;

        equipedWeapon = equipingWeapon;

    }






    void Unequip(WeaponStats unequipingWeapon)
    {

        PlayerManager.Instance.CallChangeAimState(false);   //Tirar a pose de mirando quando a arma é desequipada

        if (unequipingWeapon != null)
        {

            // toca a animação de desarmar, o equipedPivot vai ficar no lugar onde a arma deve ficar guardada, colocar o pivot de unarmed com os valores do equiped
            CharacterAnimation.Instance.UnequipWeapon();


            if (unequipingWeapon.WeaponInfo.type == WeaponType.Primary)
            {
                weapons[0] = unequipingWeapon;

               
            }
            else
            {
                weapons[1] = unequipingWeapon;

                
            }




        }
       
    }



    public void ChangeUnarmedStatToFalse()
    {
        unarmed = false;
    }

    public void ChangeUnarmedStatToTrue()
    {
        unarmed = true;
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
