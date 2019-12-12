using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoCamara : MonoBehaviour
{
    public float incremento;
    private GameObject prota = null;
    private Vector3 posIni;
    // Start is called before the first frame update
    void Start()
    {
        posIni = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = this.transform.position;
        
        if (prota!= null)
        {
            pos = prota.transform.position + posIni;
            this.transform.position = pos;
        }
        
        /*
        if (Input.GetKey(KeyCode.W))
        {
            pos.x -= incremento;
        }
        if (Input.GetKey(KeyCode.S))
        {
            pos.x += incremento;
        }
        if (Input.GetKey(KeyCode.A))
        {
            pos.z -= incremento;
        }
        if (Input.GetKey(KeyCode.D))
        {
            pos.z += incremento;
        }*/
        if (Input.GetKey(KeyCode.Q))
        {
            pos.y -= incremento;
        }
        if (Input.GetKey(KeyCode.E))
        {
            pos.y += incremento;
        }

        this.transform.SetPositionAndRotation(pos, this.transform.rotation);
    }


    public void asignarPadre(GameObject personaje)
    {
        prota = personaje;
    }
}
