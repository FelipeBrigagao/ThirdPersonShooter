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

    [SerializeField]
    Animator animMovement;

    [SerializeField]
    Animator animWeapon;

    AnimatorOverrideController overrides;

    [SerializeField]
    float animationSmoother;

    Vector2 animInputs = Vector2.zero;

    private void Start()
    {

        overrides = animWeapon.runtimeAnimatorController as AnimatorOverrideController;

        PlayerManager.OnAimWeapon += ChangeArmedState;


    }

    public void PlayMoveAnimation(float speed)
    {
        animMovement.SetFloat("Speed", speed);
    
    }
    public void PlayMoveAnimation(bool idle)
    {
        animMovement.SetBool("Idle", idle);
    }

    public void PlayMoveAnimation(float speed, bool idle)
    {
        animMovement.SetFloat("Speed", speed);

        animMovement.SetBool("Idle", idle);

    }

    public void PlayMoveAnimation(Vector2 inputs)
    {
        animInputs = Vector2.Lerp(animInputs, inputs, animationSmoother * Time.deltaTime);

        animMovement.SetFloat("Input_x", animInputs.x);
        animMovement.SetFloat("Input_y", animInputs.y);

    }


    public void ChangeArmedState(bool aiming)
    {
        animMovement.SetBool("Armed", aiming);
    }


    //Setar as animações de arma
    public IEnumerator SetEquipedWeaponAnimations(AnimationClip holsterAim,  AnimationClip pose, AnimationClip aim, AnimationClip poseHolster)
    {

        yield return new WaitForSeconds(0.001f);

        overrides["GunAnimationHolsterAimEmpty"] = holsterAim;

        overrides["GunAnimationPoseEmpty"] = pose;

        overrides["GunAnimationAimEmpty"] = aim;

        overrides["GunAnimationPoseHolsterEmpty"] = poseHolster;

    }

    
    public IEnumerator EquipWeapon()
    {
        animWeapon.SetTrigger("Equip");

        yield return new WaitForSeconds(1f);

        /*
        do
        {
            yield return new WaitForEndOfFrame();

        } while (animWeapon.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
        */

        //Debug.Log(animWeapon.GetCurrentAnimatorClipInfo(0)[0].clip.name); //Não é o clipe tocando agora, e sim o primeiro clipe da lista de clipes, tem que achar um jeito de saber o tamanho do clipe atual

    }


    public IEnumerator UnequipWeapon()
    {

        animWeapon.SetTrigger("Unequip");

        yield return new WaitForSeconds(1f);

        /*
        do
        {
            yield return new WaitForEndOfFrame();

        } while (animWeapon.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
        */
        
       // Debug.Log(animWeapon.GetCurrentAnimatorClipInfo(0)[0].clip.name);

    }

    public void PlayUnarmedAnimation()
    {
        animWeapon.Play("Unarmed");


    }



    //Colocar a animação de arma mirando ou pose para rodar

    public void ChangeWeaponAimingAnimationsWeight(float weight)
    {
        animWeapon.SetFloat("Aiming", weight);
    }













    private void OnDisable()
    {

        PlayerManager.OnAimWeapon -= ChangeArmedState;
    }

}
