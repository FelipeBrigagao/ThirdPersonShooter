using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{

    Rigidbody rb;
    
    [SerializeField]
    Camera cam;

    float horizontalInput;
    float verticalInput;

    float speed;
    float runningSpeed = 10f;
    float walkingSpeed = 5f;
    
    [SerializeField]
    float turnSpeed = 0.15f;


    float dirAngle;
    float smoothDirAngle;

    Vector3 direction;
    Vector3 moveDirection;
    Vector3 velocity;
    Vector3 moveAmount;

    bool running = false;


    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        speed = 0;

    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        direction = new Vector3(horizontalInput, 0, verticalInput).normalized;

        //ver se está correndo






    }


    private void FixedUpdate()
    {
       
        dirAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;

        smoothDirAngle = Mathf.LerpAngle(transform.eulerAngles.y, dirAngle, turnSpeed);

        moveDirection = Quaternion.Euler(Vector3.up * dirAngle) * Vector3.forward;

        velocity = moveDirection * speed;

        moveAmount = velocity * Time.fixedDeltaTime;

        if(direction.magnitude >= 0.01f)
        {
            rb.MovePosition(transform.position + moveAmount);
            rb.MoveRotation(Quaternion.Euler(Vector3.up * smoothDirAngle));

        }


    }


    void ChangeSpeed()
    {

        if(direction.magnitude >= 0.01)
        {

            //lerp até a velocidade de andando

            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                //lerp até a velocidade de correndo
            }else if ()
            {
                //lerp até a velocidade de andando
            }


        }else if ()
        {
            //lerp até a velocidade de parado
        }



    }




}
