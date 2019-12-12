using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMonedasGeneral : MonoBehaviour
{
    private Controlador controller;
    private Text moneditas;
   

    // Start is called before the first frame update
    void Start()
    {
        moneditas = this.gameObject.GetComponentInParent<Text>();
        controller = FindObjectOfType<Controlador>();
        moneditas.text = controller.monedas.ToString();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
