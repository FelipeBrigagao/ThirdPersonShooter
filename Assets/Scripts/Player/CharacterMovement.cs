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

    float finalSpeed;
    float idleSpeed = 0f;
    float runningSpeed = 10f;
    float walkingSpeed = 4.5f;
    
    [SerializeField]
    float turnSpeed = 0.15f;
    [SerializeField]
    float changeSpeedSmoother = 0.2f;


    float dirAngle;
    float smoothDirAngle;

    Vector3 direction;
    Vector3 moveDirection;
    Vector3 velocity;
    Vector3 moveAmount;

    bool running = false;

    Animator anim;


    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        anim = this.GetComponent<Animator>();
        speed = idleSpeed;

    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        direction = new Vector3(horizontalInput, 0, verticalInput).normalized;

        //ver se está correndo

        ChangeSpeed();

        anim.SetFloat("Speed", speed);


    }


    private void FixedUpdate()
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

        rb.MoveRotation(Quaternion.Euler(Vector3.up * smoothDirAngle));

    }


    void ChangeSpeed()
    {

        if (direction.magnitude >= 0.01)
        {

            if (Input.GetKey(KeyCode.LeftShift))
            {
                finalSpeed = runningSpeed;
            }
            else
            {
                finalSpeed = walkingSpeed;
            }

        }
        else
        {
            finalSpeed = idleSpeed;
        }


        speed = Mathf.Lerp(speed, finalSpeed, changeSpeedSmoother);


    }




}
