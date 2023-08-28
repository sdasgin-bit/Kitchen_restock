using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animation_handsright : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator mao;
   


    

    void Start()
    {
        mao = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
      

        
      
        if (Input.GetKeyDown("1"))
        {
          mao.SetBool("Fist", true);                    
         }
        if (Input.GetKeyUp("1"))
        {
            mao.SetBool("Fist", false);
        }



        if (Input.GetKeyDown("2"))
        {
            mao.SetBool("Fist_thumb", true);
        }
        if (Input.GetKeyUp("2"))
        {
          mao.SetBool("Fist_thumb", false);
        }



        if (Input.GetKeyDown("3"))
        {
            mao.SetBool("one", true);
        }
        if (Input.GetKeyUp("3"))
        {
            mao.SetBool("one", false);
        }



        if (Input.GetKeyDown("4"))
        {
            mao.SetBool("two", true);
        }
        if (Input.GetKeyUp("4"))
        {
           mao.SetBool("two", false); 
        }




        if (Input.GetKeyDown("5"))
        {
            mao.SetBool("three", true);
        }
        if (Input.GetKeyUp("5"))
        {
            mao.SetBool("three", false);
        }


        if (Input.GetKeyDown("6"))
        {
            mao.SetBool("four", true);
        }
        if (Input.GetKeyUp("6"))
        {
            mao.SetBool("four", false);
        }


        if (Input.GetKeyDown("7"))
        {
            mao.SetBool("positive", true);
        }
        if (Input.GetKeyUp("7"))
        {
            mao.SetBool("positive", false);
        }

        if (Input.GetKeyDown("8"))
        {
            mao.SetBool("Grab", true);
        }
        if (Input.GetKeyUp("8"))
        {
            mao.SetBool("Grab", false);
        }

    }
}
