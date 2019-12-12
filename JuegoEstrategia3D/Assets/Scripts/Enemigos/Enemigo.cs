using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Enemigo : MonoBehaviour
{
    public NavMeshAgent agent;
    private float distanciaAlJugador;
    private int minX;
    private int minZ;
    private int maxX;
    private int maxZ;


    private int minXPerdido;
    private int maxXPerdido;
    private int minZPerdido;
    private int maxZPerdido;



    private int vecesBuscando; // veces que se ha buscado
    public int radio;
    public int tiempoEsperaMaximo;
    public int vecesMaximoPerdido; // numero de destinos cercanos una vez que se pierde al jugador
    public float nuevaVelocidad;
    public Material colorNormal;
    public Material colorPerdido;
    private Renderer rend;
    public GameObject prota;
    private Vector3 destino;
    private Vector3 posicionDePerdida; // ultimo sitio donde ser vio al prota
    /*private bool encontrado;
    private bool listo;
    private bool esperando;
    private bool perseguir;
    private bool buscando;
    */
    private float temporizador;
    private int numIntentosEncontrar;
    private float velocidadInicial;

    private enum Estado {encontrado,listo,esperando,persiguiendo,buscando, nada,perdido,yendoAlUltimoSitioDePerdida};
    Estado miestado;
    

    // Start is called before the first frame update
    void Start()
    {/*
        encontrado = false;
        perseguir = false;
        listo = true;
        */
        rend = this.GetComponent<Renderer>();
        miestado = Estado.listo;
        destino = Vector3.zero;
        temporizador = 0;
        distanciaAlJugador = 1.5f;
        numIntentosEncontrar = 0;
        velocidadInicial = agent.speed;
    }

    // Update is called once per frame
    void Update()
    {
        if(miestado == Estado.persiguiendo)
        {
            Debug.Log("Periguiendo");
            agent.SetDestination(prota.transform.position);
            destino = prota.transform.position;
            if (Vector3.Distance(prota.transform.position, this.transform.position) < distanciaAlJugador)
            {
                Debug.LogError("JUGADOR COGIDO");
                SceneManager.LoadScene("GameOver");
                
            }
        } 
        else if (miestado == Estado.perdido)
        {
            // mal ya que se llamaria todo el rato
            comprobarDistPerdido();

        }
        else if (miestado == Estado.yendoAlUltimoSitioDePerdida)
        {
            comprobarDistUltimoSitio();
        }
         else if (miestado == Estado.listo) // busco un nuevo destino
        {
                float auxX = Random.Range(minX, maxX);
                float auxZ = Random.Range(minZ, maxZ);
                destino.x = auxX;
                destino.z = auxZ;
                NavMeshPath aux = new NavMeshPath();
                agent.CalculatePath(destino, aux);
                if (aux.status == NavMeshPathStatus.PathPartial)
                {
                   // Debug.Log("no se puede llegar");
                }
                else if (aux.status == NavMeshPathStatus.PathInvalid)
                {
                   // Debug.Log("Path no valido");
                }
                else
                {
                   // Debug.Log("Path valido");
                    agent.SetDestination(destino);
                miestado = Estado.buscando;
                }
            }
        else if(miestado == Estado.buscando)
        {
            comprobarDist();
        } else if(miestado == Estado.esperando)
        {
            Esperar();
        }
    }
    public void comprobarDist()
    {
        if (miestado == Estado.buscando)

        {
                if (Vector3.Distance(destino, this.transform.position) < 2f)
                {
                    miestado = Estado.esperando;
                    temporizador = tiempoEsperaMaximo;
                }
            
         
        }
      
    }

    public void comprobarDistPerdido()
    {
        if (miestado == Estado.perdido)

        {
            if (Vector3.Distance(destino, this.transform.position) < 2f)
            {
                numIntentosEncontrar++;
                buscandoCerca();
            }


        }
    }

    public void comprobarDistUltimoSitio()
    {
        if (miestado == Estado.yendoAlUltimoSitioDePerdida)
        {

            if (Vector3.Distance(destino, this.transform.position) < 2f)
            {
                miestado = Estado.perdido;
                buscandoPerdido();
            }
        }
    }


    public void Esperar()
    {
        if (miestado == Estado.esperando)
        {
            temporizador -= Time.deltaTime;
            if(temporizador<= 0)
            {
                miestado = Estado.listo;
            }
        }
    }

    public void persiguiendo()
    {
        miestado = Estado.persiguiendo;
        agent.speed = velocidadInicial;
        rend.material = colorNormal;
        numIntentosEncontrar = 0;

    }

    public void dejarDePerseguir()
    {
        posicionDePerdida = destino;
        miestado = Estado.yendoAlUltimoSitioDePerdida;
    }

    public void buscandoPerdido()
    {
        rend.material = colorPerdido;
        agent.speed = nuevaVelocidad;
        buscandoCerca();
    }

    public void buscandoCerca()
    {
        if(numIntentosEncontrar < vecesMaximoPerdido)
        {
            // tengo que buscar un detstino nuevo
            NavMeshPath aux = new NavMeshPath();
            do
            {
                float auxX = Random.Range(minXPerdido, maxXPerdido);
                float auxZ = Random.Range(minZPerdido, maxZPerdido);
                auxX += posicionDePerdida.x;
                auxZ += posicionDePerdida.z;
                destino.x = auxX;
                destino.z = auxZ;
                agent.CalculatePath(destino, aux);
            } while (aux.status == NavMeshPathStatus.PathInvalid);
           
            agent.SetDestination(destino);
        } else
        {
            rend.material = colorNormal;
            agent.speed = velocidadInicial;
            numIntentosEncontrar = 0;
            miestado = Estado.esperando;
            temporizador = tiempoEsperaMaximo;
        }
    }

    public void fijarLimites(int heigth, int width)
    {
        minX = - ( width / 2);
        maxX = (width / 2);
        minZ = -(heigth / 2);
        maxZ = (heigth / 2);
        minXPerdido = (int) (minX / 5);
        maxXPerdido = (int)(maxX / 5);
        minZPerdido = (int)(minZ / 5);
        maxZPerdido = (int)(maxZ / 5);
    }

}

