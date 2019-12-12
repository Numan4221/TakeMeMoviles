using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moneda : MonoBehaviour
{
    ControladorNivel levelController;
    // Start is called before the first frame update
    void Start()
    {
        levelController = FindObjectOfType<ControladorNivel>();
        this.transform.Rotate(new Vector3(0, 0, 1), 90f);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(new Vector3(1, 0, 0), 0.4f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Soldado")
        {
            levelController.recogerMoneda();
            Destroy(this.gameObject);
        }
    }
}
