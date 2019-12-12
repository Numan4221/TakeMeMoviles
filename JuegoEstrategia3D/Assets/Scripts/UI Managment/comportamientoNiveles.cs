using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class comportamientoNiveles : MonoBehaviour
{
    public int numero;
    private Controlador controller;

    // Start is called before the first frame update
    void Start()
    {
        controller = FindObjectOfType<Controlador>();
        controller.asignarseBotones(this.gameObject, numero);
        controller.actualizarBotonesSelectorNiveles();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
