using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovementArmed : MonoBehaviour, ICharacterMovement
{
    Rigidbody rb;
    Camera cam;

    Vector2 movementInputs;
    Vector3 direction;
    Vector3 moveDirection;
    Vector3 velocity;
    Vector3 moveAmount;

    float directionAngle;

    float speed;

    [SerializeField]
    float armedWalkSpeed = 2.5f;
    private float idleSpeed = 0f;

    private void Start()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody>();

    }

    private void FixedUpdate()
    {
        Move();
        ArmedState();

        CharacterAnimation.Instance.PlayMoveAnimation(movementInputs);
    }

    public void SetInput(Vector2 inputs)
    {
        movementInputs = inputs;
        direction = new Vector3(movementInputs.x, 0, movementInputs.y).normalized;
    }

   
    void Move()
    {
        if (direction.magnitude >= 0.01f)
        {
            directionAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;

        }

        moveDirection = Quaternion.Euler(Vector3.up * directionAngle) * Vector3.forward;

        velocity = moveDirection * speed;

        moveAmount = velocity * Time.fixedDeltaTime;

        rb.MovePosition(transform.position + moveAmount);

    }

    void ArmedState()
    {
        //Verifica se está andando
        ChangeArmedSpeed();

        rb.MoveRotation(Quaternion.Euler(cam.transform.eulerAngles.y * Vector3.up));  //A rotação para a animação de armado é sempre olhando para a camera
    }


    void ChangeArmedSpeed()
    {

        if (direction.magnitude >= 0.1f)
        {
            speed = armedWalkSpeed;

        }
        else
        {
            speed = idleSpeed;
        }

    }


}
