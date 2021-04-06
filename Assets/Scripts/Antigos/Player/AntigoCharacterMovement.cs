using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntigoCharacterMovement : MonoBehaviour
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
    float walkingSpeed = 3f;

    [SerializeField]
    float armedWalkSpeed = 7f;
    
    [SerializeField]
    float turnSpeed = 0.15f;

    float speedSmoother;

    [SerializeField]
    float increasingSpeedSmoother = 0.01f;

    [SerializeField]
    float decreasingSpeedSmoother = 0.1f;

    [SerializeField]
    float animInputSmoother = 0.1f;


    float dirAngle;
    float smoothDirAngle;

    Vector2 inputs;
    Vector2 animInputs;
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
        animInputs = Vector2.zero;

        PlayerManagerOld.OnEquipWeapon += ChangeArmedState;

    }


    void Update()                       
    {
        inputs.x = Input.GetAxisRaw("Horizontal");                                          //Pega as entradas do teclado 
        inputs.y = Input.GetAxisRaw("Vertical");

        direction = new Vector3(inputs.x, 0, inputs.y).normalized;                  //Faz o vetor de direção com as entradas recebidas

    }


    private void FixedUpdate()
    {
        Move();                                                                                                     //Como a movimentação é igual para armado e desarmado há um método para isso

        if (armed && (actualState != ArmedState))                                                       //Verifica se o estado atual é o armado e se o delegate já está com o método para esse estado armazenado nele
        {
            actualState = ArmedState;                                                                       //Se o estado é o armado mas o actualState não está com o ArmedState, esse é atribuido ao estado atual

        }else if (!armed && (actualState != UnnarmedState))
        {
            actualState = UnnarmedState;

        }

        actualState?.Invoke();                          //Se tiver algo no delegate actualState esse é chamado, chamando o armed ou o unnarmed

    }


    private void Move()                                                                                                                     // Movimentação do personagem 
    {
        if (direction.magnitude >= 0.01f)
        {
            dirAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;

        }

        moveDirection = Quaternion.Euler(Vector3.up * dirAngle) * Vector3.forward;

        velocity = moveDirection * speed;

        moveAmount = velocity * Time.fixedDeltaTime;


        rb.MovePosition(transform.position + moveAmount);

    }

    void ArmedState()
    {

        animInputs = Vector2.Lerp(animInputs, inputs, animInputSmoother * Time.deltaTime);       //Pega inputs para a animação de armado, pois o input para a movimentação não tem os valores intermediários de -1 -> 0 -> 1

        anim.SetFloat("Input_x", animInputs.x);
        anim.SetFloat("Input_y", animInputs.y);


        //Verifica se está andando
        ChangeArmedSpeed();

        rb.MoveRotation(Quaternion.Euler(cam.transform.eulerAngles.y * Vector3.up));  //A rotação para a animação de armado é sempre olhando para a camera
    }

    void UnnarmedState()
    {
        anim.SetBool("Idle", idle);
        anim.SetFloat("Speed", speed);

        //Verifica se está correndo
        ChangeUnarmedSpeed();

        smoothDirAngle = Mathf.LerpAngle(transform.eulerAngles.y, dirAngle, turnSpeed * Time.deltaTime);

        rb.MoveRotation(Quaternion.Euler(Vector3.up * smoothDirAngle));                                                     

    }
    

    void ChangeArmedSpeed()
    {

        if(direction.magnitude >= 0.1f)
        {
            speed = armedWalkSpeed;

        }
        else
        {
            speed = idleSpeed;
        }

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

        speed = Mathf.Lerp(speed, finalSpeed, speedSmoother * Time.deltaTime);

    }

    void ChangeArmedState(bool armedState)
    {
        armed = armedState;

        anim.SetBool("Armed", armed);

        
    }



    private void OnDisable()
    {

        PlayerManagerOld.OnEquipWeapon -= ChangeArmedState;
    }

}
