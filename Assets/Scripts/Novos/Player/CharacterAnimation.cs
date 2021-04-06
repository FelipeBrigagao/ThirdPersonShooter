using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    Animator anim;

    [SerializeField]
    float animationSmoother;

    Vector2 animInputs = Vector2.zero;

    private void Start()
    {
        anim = GetComponent<Animator>();
        PlayerManager.OnEquipWeapon += ChangeArmedState;


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


    public void ChangeArmedState(bool armed)
    {
        anim.SetBool("Armed", armed);
    }

    private void OnDisable()
    {

        PlayerManager.OnEquipWeapon -= ChangeArmedState;
    }

}
