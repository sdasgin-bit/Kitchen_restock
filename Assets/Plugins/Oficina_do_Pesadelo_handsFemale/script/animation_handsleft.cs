using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animation_handsleft : MonoBehaviour
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
      

        
      
        if (Input.GetKeyDown("a"))
        {
          mao.SetBool("Fist", true);                    
         }
        if (Input.GetKeyUp("a"))
        {
            mao.SetBool("Fist", false);
        }



        if (Input.GetKeyDown("s"))
        {
            mao.SetBool("Fist_thumb", true);
        }
        if (Input.GetKeyUp("s"))
        {
          mao.SetBool("Fist_thumb", false);
        }



        if (Input.GetKeyDown("d"))
        {
            mao.SetBool("one", true);
        }
        if (Input.GetKeyUp("d"))
        {
            mao.SetBool("one", false);
        }



        if (Input.GetKeyDown("f"))
        {
            mao.SetBool("two", true);
        }
        if (Input.GetKeyUp("f"))
        {
           mao.SetBool("two", false); 
        }




        if (Input.GetKeyDown("g"))
        {
            mao.SetBool("three", true);
        }
        if (Input.GetKeyUp("g"))
        {
            mao.SetBool("three", false);
        }


        if (Input.GetKeyDown("h"))
        {
            mao.SetBool("four", true);
        }
        if (Input.GetKeyUp("h"))
        {
            mao.SetBool("four", false);
        }


        if (Input.GetKeyDown("j"))
        {
            mao.SetBool("positive", true);
        }
        if (Input.GetKeyUp("j"))
        {
            mao.SetBool("positive", false);
        }

        if (Input.GetKeyDown("k"))
        {
            mao.SetBool("Grab", true);
        }
        if (Input.GetKeyUp("k"))
        {
            mao.SetBool("Grab", false);
        }
    }
}
