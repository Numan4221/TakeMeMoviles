using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorNivel : MonoBehaviour
{

    private int monedasRecogidas = 0;
    private Controlador controller;
    private UiMonedas money;

    // Start is called before the first frame update
    void Start()
    {
        controller = FindObjectOfType<Controlador>();
        money = FindObjectOfType<UiMonedas>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void recogerMoneda()
    {
        monedasRecogidas++;
        money.cambiarTexto(monedasRecogidas);
    }

    public void juntarMonedas()
    {
        controller.sumarMonedas(monedasRecogidas);
    }



}
