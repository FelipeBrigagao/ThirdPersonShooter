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

    [SerializeField]
    float runningSpeed = 10f;
   
    [SerializeField]
    float walkingSpeed = 4.5f;
    
    [SerializeField]
    float turnSpeed = 0.15f;

    float speedSmoother;
    [SerializeField]
    float increasingSpeedSmoother = 0.2f;
    [SerializeField]
    float decreasingSpeedSmoother = 0.2f;


    float dirAngle;
    float smoothDirAngle;

    Vector3 direction;
    Vector3 moveDirection;
    Vector3 velocity;
    Vector3 moveAmount;

    bool idle = true;

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

        anim.SetBool("Idle", idle);
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

        if(direction.magnitude >= 0.1f )
        {
            idle = false;

            if (!Input.GetKey(KeyCode.LeftShift))
            {
                finalSpeed = walkingSpeed;

                if (speed < (walkingSpeed - 0.1f))
                {
                    speedSmoother = increasingSpeedSmoother;

                }else if(speed > (walkingSpeed + 0.1f))
                {
                    speedSmoother = decreasingSpeedSmoother;

                }

            }else if (Input.GetKey(KeyCode.LeftShift))
            {
                finalSpeed = runningSpeed;
                speedSmoother = increasingSpeedSmoother;
            }

        }else if (direction.magnitude < 0.1f)
        {
            idle = true;

            finalSpeed = idleSpeed;
            speedSmoother = decreasingSpeedSmoother;
        }

        speed = Mathf.Lerp(speed, finalSpeed, speedSmoother);

    }

}
