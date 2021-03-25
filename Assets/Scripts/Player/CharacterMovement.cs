using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{

    Rigidbody rb;
    Animator anim;

    [SerializeField]
    Camera cam;

    float speed;
    float finalSpeed;
    float idleSpeed = 0f;

    [SerializeField]
    float runningSpeed = 10f;
   
    [SerializeField]
    float walkingSpeed = 4.5f;

    [SerializeField]
    float armedWalkSpeed = 3f;
    
    [SerializeField]
    float turnSpeed = 0.15f;

    float speedSmoother;

    [SerializeField]
    float increasingSpeedSmoother = 0.01f;

    [SerializeField]
    float decreasingSpeedSmoother = 0.1f;


    float dirAngle;
    float smoothDirAngle;

    Vector2 inputs;
    Vector3 direction;
    Vector3 moveDirection;
    Vector3 velocity;
    Vector3 moveAmount;

    bool idle = true;
    bool armed = false;


    private delegate void ChangeStates();
    private ChangeStates actualState;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;

        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        speed = idleSpeed;

    }


    void Update()
    {
        inputs.x = Input.GetAxisRaw("Horizontal");
        inputs.y = Input.GetAxisRaw("Vertical");

        direction = new Vector3(inputs.x, 0, inputs.y).normalized;

        if (Input.GetKeyDown(KeyCode.T))
        {
            armed = !armed;

            anim.SetBool("Armed", armed);
        }

    }


    private void FixedUpdate()
    {
        Move();

        if (armed && (actualState != ArmedState))
        {
            actualState = ArmedState;

        }else if (!armed && (actualState != UnnarmedState))
        {
            actualState = UnnarmedState;

        }

        actualState?.Invoke();

    }


    private void Move()
    {
        if (direction.magnitude >= 0.01f)
        {
            dirAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;

        }

        smoothDirAngle = Mathf.LerpAngle(transform.eulerAngles.y, dirAngle, turnSpeed);

        moveDirection = Quaternion.Euler(Vector3.up * dirAngle) * Vector3.forward;

        velocity = moveDirection * speed;

        moveAmount = velocity * Time.fixedDeltaTime;


        rb.MovePosition(transform.position + moveAmount);

    }

    void ArmedState()
    {
        //ver se está correndo

        anim.SetFloat("Input_x", inputs.x);
        anim.SetFloat("Input_y", inputs.y);

        ChangeArmedSpeed();

        rb.MoveRotation(Quaternion.Euler(cam.transform.eulerAngles.y * Vector3.up));
    }

    void UnnarmedState()
    {
        anim.SetBool("Idle", idle);
        anim.SetFloat("Speed", speed);

        //ver se está correndo

        ChangeUnarmedSpeed();

        rb.MoveRotation(Quaternion.Euler(Vector3.up * smoothDirAngle));

    }
    

    void ChangeArmedSpeed()
    {
        speed = armedWalkSpeed;

    }



    void ChangeUnarmedSpeed()
    {

        if(direction.magnitude >= 0.1f )    //Se está tendo entrada
        {
            idle = false;                           //Não está idle, para a animação

            if (!Input.GetKey(KeyCode.LeftShift))           //Se não está correndo
            {
                finalSpeed = walkingSpeed;                      // A velocidade a se alcançar é a de "andando"

                if (speed < (walkingSpeed - 0.1f))              //Se não está correndo mas está tendo input e a velocidade é menor que a de andando quer dizer que estava parado
                {
                    speedSmoother = increasingSpeedSmoother;  //Por isso usa o smoother de increasing speed

                }else if(speed > (walkingSpeed + 0.1f))                 //Se não está correndo mas está tendo input e a velocidade é maior que a de andando quer dizer que estava correndo
                {
                    speedSmoother = decreasingSpeedSmoother;      //Por isso usa o smoother de decreasing speed

                }

            }else if (Input.GetKey(KeyCode.LeftShift))          //Se ele está correndo
            {
                finalSpeed = runningSpeed;                                             //Velocidade final de correndo, e não importa se estava parado ou andando, o smoother vai ser de increasing speed
                speedSmoother = increasingSpeedSmoother;
            }

        }else if (direction.magnitude < 0.1f)                                       //Se não tem entrada 
        {
            idle = true;                                                                   //Quer dizer que quer estar parado, por isso a velocidade deve diminuir para a de idle

            finalSpeed = idleSpeed;
            speedSmoother = decreasingSpeedSmoother;
        }

        speed = Mathf.Lerp(speed, finalSpeed, speedSmoother);

    }

}
