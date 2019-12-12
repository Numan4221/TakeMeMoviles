using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiMonedas : MonoBehaviour
{

    private Text moneditas;

    // Start is called before the first frame update
    void Start()
    {
        moneditas = this.gameObject.GetComponentInParent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void cambiarTexto( int num)
    {
        moneditas.text = num.ToString();
    }


}
