using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectoresDeNivel : MonoBehaviour
{
    private Controlador controller;

    private void Start()
    {
        controller = FindObjectOfType<Controlador>();
        controller.actualizarBotonesSelectorNiveles();
    }

    public void irNivel(int num)
    {
        controller.nivelActual = num;
        SceneManager.LoadScene("GameScene");

    }

    public void irAAjustes()
    {
        SceneManager.LoadScene("Ajustes");
    }



}
