using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teste : MonoBehaviour
{

    Animator anim;
    Vector2 inputs;



    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("Armed", true);
    }

    // Update is called once per frame
    void Update()
    {
        inputs.x = Input.GetAxisRaw("Horizontal");                                          //Pega as entradas do teclado 
        inputs.y = Input.GetAxisRaw("Vertical");

        anim.SetFloat("Input_x", inputs.x);
        anim.SetFloat("Input_y", inputs.y);

    }
}
