using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    #region Singleton

    private static CharacterAnimation _instance;

    public static CharacterAnimation Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Character Animation não encontrado, criando um.");

                GameObject go = new GameObject("CharacterAnimation");

                go.AddComponent<CharacterAnimation>();

            }
            return _instance;
        }
    }


    private void Awake()
    {
        if (_instance != null)
        {
            Debug.LogWarning("Character Animation já existente, uma segunda instância não será criada.");
            return;
        }

        Debug.Log("Character Animation criado.");
        _instance = this;
    }

    #endregion


    Animator anim;
    AnimatorOverrideController overrides;

    [SerializeField]
    float animationSmoother;

    Vector2 animInputs = Vector2.zero;

    private void Start()
    {
        anim = GetComponent<Animator>();
        overrides = anim.runtimeAnimatorController as AnimatorOverrideController;

        PlayerManager.OnAimWeapon += ChangeArmedState;


    }

    public void PlayMoveAnimation(float speed)
    {
        anim.SetFloat("Speed", speed);
    
    }
    public void PlayMoveAnimation(bool idle)
    {
        anim.SetBool("Idle", idle);
    }

    public void PlayMoveAnimation(float speed, bool idle)
    {
        anim.SetFloat("Speed", speed);

        anim.SetBool("Idle", idle);

    }

    public void PlayMoveAnimation(Vector2 inputs)
    {
        animInputs = Vector2.Lerp(animInputs, inputs, animationSmoother * Time.deltaTime);

        anim.SetFloat("Input_x", animInputs.x);
        anim.SetFloat("Input_y", animInputs.y);

    }


    public void ChangeArmedState(bool aiming)
    {
        anim.SetBool("Armed", aiming);
    }


    //Colocar a layer de arma ou sem
    public void ChangeWeaponLayerWeight(bool weaponEquiped)
    {
        if (weaponEquiped)
        {
            anim.SetLayerWeight(1, 1f);
        }
        else
        {
            anim.SetLayerWeight(1, 0f);
        }

    }

    //Setar as animações para arma mirando e pose
    public IEnumerator SetWeaponAnimations(AnimationClip pose, AnimationClip aiming)
    {

        yield return new WaitForSeconds(0.001f);

        overrides["GunAnimationAimingEmpty"] = aiming;

        overrides["GunAnimationPoseEmpty"] = pose;


    }

    
    //Colocar a animação de arma mirando ou pose para rodar

    public void ChangeWeaponAnimationsWeight(float weight)
    {
        anim.SetFloat("Aiming", weight);
    }













    private void OnDisable()
    {

        PlayerManager.OnAimWeapon -= ChangeArmedState;
    }

}
