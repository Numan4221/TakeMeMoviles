using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Camera cam;
    public GameObject seleccionado;
    // Start is called before the first frame update
    void Start()
    {
        seleccionado = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray,out hit))
            {
                GameObject aux = hit.collider.gameObject;

                if(aux.tag == "Soldado" || aux.tag == "General")
                {
                    seleccionado = aux;
                    Debug.Log(seleccionado);
                }
            }
        } else if(Input.GetMouseButtonDown(1))
        {
            if (seleccionado != null)
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    GameObject aux = hit.collider.gameObject;
                    if(aux.tag == "General") // si toco a un general, le asigno un soldado
                    {
                        if(seleccionado.tag == "Soldado")
                        {
                           // aux.GetComponent<General>().AsignarSoldado(seleccionado.GetComponent<Soldado>());
                        }
                    } else if(aux.tag == "Ground") // si clico en el suelo, me muevo alli
                    {
                        if (seleccionado.tag == "Soldado")
                        {
                            Debug.Log("Moviendo soldado");

                           // Soldado sol = seleccionado.GetComponent<Soldado>();
                            seleccionado.GetComponent<Soldado>().MoverTropa(hit.point);
                        } else if(seleccionado.tag == "General")
                        {
                            Debug.Log("Moviendo General");
                          //  seleccionado.GetComponent<General>().MoverTropa(hit.point);
                        }
                    }
                }
            }
        }
        
    }
}
