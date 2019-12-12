using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controlador : MonoBehaviour
{
    public GeneradorTerrenoProcedural Escenario;
    public int nivelActual;
    public bool llaveConseguida = false;
    public DoorController puerta;
    private CamaraSecundaria camaraPuerta;
    public Camera camaraPrincipal;
    public int posJosX;
    public int posJosY;
    public Vector3 posJos;
    public Vector3 posIniJos;
    public bool [] nivelesAccesibles ;
    public GameObject[] selectorNivelesBotones;
    public int numNiveles;
    public int monedas;
    private void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        monedas = 0;
        nivelesAccesibles = new bool[numNiveles];
        selectorNivelesBotones = new GameObject[numNiveles];
        for (int i = 0; i < numNiveles; i++)
        {
            if(i == 1)
            {
                nivelesAccesibles[i] = true;
            } else
            {
                nivelesAccesibles[i] = false;
            }

        }
        posJos = new Vector3(posJosX, posJosY, 0);
        posIniJos = posJos;
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void puertaAbriendose()
    {
        puerta.enabled = true;
       camaraPuerta.camaraSecun.enabled = true;
        camaraPrincipal.enabled = false;
        puerta.activarPuerta();
        Escenario.congelarMov();
    }

    public void reestaurarCamara()
    {
        camaraPuerta.camaraSecun.enabled = false;
        camaraPrincipal.enabled = true;
        Escenario.descongelarMov();
    }

    public void pedirNivel()
    {
        Escenario = FindObjectOfType<GeneradorTerrenoProcedural>();
        Escenario.nivel = nivelActual;
    }

    public void reiniciarControlador()
    {
        puerta = FindObjectOfType<DoorController>();
        camaraPuerta = FindObjectOfType<CamaraSecundaria>();
        Camera[] camaras = FindObjectsOfType<Camera>();
        Debug.Log(camaras.Length + " num camaras");
        foreach (Camera cam in camaras)
        {
            if(cam.tag == "MainCamera")
            {
                camaraPrincipal = cam;
            }
        }
        Debug.Log(camaraPuerta);
        Debug.Log(camaraPrincipal);
        camaraPrincipal.enabled = true;
    }

    public void actualizarBotonesSelectorNiveles()
    {
        for (int i = 0; i < numNiveles; i++)
        {
            if (nivelesAccesibles[i])
            {
                if(selectorNivelesBotones[i] != null)
                {
                    selectorNivelesBotones[i].SetActive(true);
                }

            }  else
            {
                if(selectorNivelesBotones[i] != null)
                {
                    selectorNivelesBotones[i].SetActive(false);
                }
            }
        }
    }

    public void asignarseBotones(GameObject boton, int num)
    {
        selectorNivelesBotones[num] = boton;
    }

    public void desbloquearSigNivel()
    {
        if(nivelActual+1 < numNiveles)
        {
            nivelesAccesibles[nivelActual + 1] = true;
        }
    }

    public void sumarMonedas(int num)
    {
        monedas += num;
    }
    
}



