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
    float speed = 7f;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

    }


    private void FixedUpdate()
    {
        Vector3 direction = new Vector3(horizontalInput, 0, verticalInput).normalized;

        float dirAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;

        Vector3 moveDirection = Quaternion.Euler(Vector3.up * dirAngle) * Vector3.forward;

        Vector3 velocity = moveDirection * speed;

        Vector3 moveAmount = velocity * Time.fixedDeltaTime;

        if(direction.magnitude >= 0.01f)
        {
            rb.MovePosition(transform.position + moveAmount);
            rb.MoveRotation(Quaternion.Euler(Vector3.up * dirAngle));

        }


    }




}
