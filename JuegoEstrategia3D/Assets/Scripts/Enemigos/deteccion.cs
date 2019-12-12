using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deteccion : MonoBehaviour
{
    public Enemigo malo;
    private Renderer rend;
    public Material matSinDetectar;
    public Material matDetectado;
    int layerMask = 11 << 11 ; // mascara de bits, desplazamos las capas 9 bits para que coja las 9 y la 11, es decir que el rayo colisione con muros y con el prota
    bool detectado;
    
    // Start is called before the first frame update
    void Start()
    {
        rend = this.GetComponent<Renderer>(); //   rend.material.SetColor("_Color", Color.blue);
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Soldado")
        {
            Debug.Log("JUGADOR DETECTADO en el rango");
            RaycastHit hit;
            Debug.DrawRay(malo.transform.position - (new Vector3(0,0.5f,0)), (other.gameObject.transform.position - (new Vector3(0, 0.5f, 0))) - (malo.transform.position - (new Vector3(0, 0.5f, 0))), Color.black, 10);
            if(Physics.Raycast(malo.transform.position - (new Vector3(0, 0.5f, 0)), (other.gameObject.transform.position - (new Vector3(0, 0.5f, 0))) - (malo.transform.position - (new Vector3(0, 0.5f, 0))), out hit ,6,layerMask)) // no hay que poner en el segundo la posicion del otro sino el vector que lleva de uno a otro
            {
                Debug.Log("Lanzando rayo");
                if(hit.collider.gameObject.layer == 12)
                {
                    detectado = true;
                    rend.material = matDetectado;
                    malo.persiguiendo();
                    Debug.Log("Colisiono con el prota");
                } else if (hit.collider.gameObject.layer == 11)
                {
                    Debug.Log("Colisiono con pared");
                } else
                {
                    Debug.Log(" no se con que toco");
                }
            }

        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(!detectado)
        {
            RaycastHit hit;
            if (Physics.Raycast(malo.transform.position - (new Vector3(0, 0.5f, 0)), (other.gameObject.transform.position - (new Vector3(0, 0.5f, 0))) - (malo.transform.position - (new Vector3(0, 0.5f, 0))), out hit, 6, layerMask)) // no hay que poner en el segundo la posicion del otro sino el vector que lleva de uno a otro
            {
                Debug.Log("Lanzando rayo");
                if (hit.collider.gameObject.layer == 12)
                {
                    detectado = true;
                    rend.material = matDetectado;
                    malo.persiguiendo();
                    Debug.Log("Colisiono con el prota");
                }
                else if (hit.collider.gameObject.layer == 11)
                {
                    Debug.Log("Colisiono con pared");
                }
                else
                {
                    Debug.Log(" no se con que toco");
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Soldado")
        {
            if(detectado == true)
            {
                detectado = false;
                Debug.Log("JUGADOR PERDIDO");
                rend.material = matSinDetectar;
                malo.dejarDePerseguir();
            }
           
        }
    }
}
