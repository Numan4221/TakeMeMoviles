using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Llave : MonoBehaviour
{

    private Controlador controlador;
    private DoorController puerta;

    void Start()
    {
        controlador = FindObjectOfType<Controlador>();
        puerta = FindObjectOfType<DoorController>();
    }

        private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Soldado")
        {
            puerta.gotKey = true;
            Debug.Log("LLave cogida");
            controlador.llaveConseguida = true;
            controlador.puertaAbriendose();
            Destroy(this.gameObject);
        }
    }


}
