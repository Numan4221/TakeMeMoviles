using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portada : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SceneManager.LoadScene("SelectorNiveles");
        }
        Touch []  toques = Input.touches;
        if(toques.Length > 0)
        {
            SceneManager.LoadScene("SelectorNiveles");
        }

    }
}
