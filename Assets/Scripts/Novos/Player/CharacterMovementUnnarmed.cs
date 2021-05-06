using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovementUnnarmed : MonoBehaviour, ICharacterMovement
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
    private float finalSpeed;
    private float idleSpeed = 0f;
    private float turnSpeed = 6f;
    private float walkingSpeed = 3f;
    private float runningSpeed = 10f;


    float smoothDirAngle;
    private float speedSmoother;
    private float increasingSpeedSmoother = 2f;
    private float decreasingSpeedSmoother = 6f;

    private bool idle;

    private void Start()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody>();

    }


    private void FixedUpdate()
    {
        Move();
        UnnarmedState();

        CharacterAnimation.Instance.PlayMoveAnimation(speed, idle);

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

    void UnnarmedState()
    {
        ChangeUnarmedSpeed();

        smoothDirAngle = Mathf.LerpAngle(transform.eulerAngles.y, directionAngle, turnSpeed * Time.deltaTime);

        rb.MoveRotation(Quaternion.Euler(Vector3.up * smoothDirAngle));

    }

    void ChangeUnarmedSpeed()
    {

        if (direction.magnitude >= 0.1f)    //Se está tendo entrada
        {
            idle = false;                           //Não está idle, para a animação

            if (!Input.GetKey(KeyCode.LeftShift))           //Se não está correndo
            {
                finalSpeed = walkingSpeed;                      // A velocidade a se alcançar é a de "andando"

                if (speed < (walkingSpeed - 0.1f))              //Se não está correndo mas está tendo input e a velocidade é menor que a de andando quer dizer que estava parado
                {
                    speedSmoother = increasingSpeedSmoother;  //Por isso usa o smoother de increasing speed

                }
                else if (speed > (walkingSpeed + 0.1f))                 //Se não está correndo mas está tendo input e a velocidade é maior que a de andando quer dizer que estava correndo
                {
                    speedSmoother = decreasingSpeedSmoother;      //Por isso usa o smoother de decreasing speed

                }

            }
            else if (Input.GetKey(KeyCode.LeftShift))          //Se ele está correndo
            {
                finalSpeed = runningSpeed;                                             //Velocidade final de correndo, e não importa se estava parado ou andando, o smoother vai ser de increasing speed
                speedSmoother = increasingSpeedSmoother;
            }

        }
        else if (direction.magnitude < 0.1f)                                       //Se não tem entrada 
        {
            idle = true;                                                                   //Quer dizer que quer estar parado, por isso a velocidade deve diminuir para a de idle

            finalSpeed = idleSpeed;
            speedSmoother = decreasingSpeedSmoother;
        }

        speed = Mathf.Lerp(speed, finalSpeed, speedSmoother * Time.deltaTime);

    }



}
