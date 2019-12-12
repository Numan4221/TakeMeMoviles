using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AsignarPosJostick : MonoBehaviour
{
    Controlador controlador;
    // Start is called before the first frame update
    void Start()
    {
       controlador =  FindObjectOfType<Controlador>();
    }

    // Update is called once per frame
    void Update()
    {
        Touch[] touches = Input.touches;

        if(touches.Length > 0)
        {
            foreach( Touch toque in touches)
            {
                
                if((toque.phase == TouchPhase.Moved) || (toque.phase == TouchPhase.Stationary))
                {
                    this.transform.position = toque.position;
                } else if(toque.phase == TouchPhase.Ended)
                {
                    this.transform.position = toque.position;
                    controlador.posJos = this.transform.position;
                    SceneManager.LoadScene("SelectorNiveles");
                }
                
            }
        }
    }

    
    

}
