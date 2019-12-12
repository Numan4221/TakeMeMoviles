using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManejadorBotones: MonoBehaviour
{
    private Controlador controller;

    private void Start()
    {
        controller = FindObjectOfType<Controlador>();
    }

    public void volverAJugar()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void salir()
    {
        Application.Quit();
    }

    public void seleccionarNivel()
    {
        SceneManager.LoadScene("SelectorNiveles");
        controller.actualizarBotonesSelectorNiveles();
    }

    public void siguienteNivel()
    {
        controller.nivelActual++;
        SceneManager.LoadScene("GameScene");
    }

}
