using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GeneradorTerrenoProcedural : MonoBehaviour
{
    #region variables
    #region deInspector

    // tamaño piezas 
    public float longitudMuroX;
    public float longitudMuroZ;
    public float porcentajeMinimoCubos;
    public float probabilidadMuro;
    public float distanciaMinMalos;

    public NavMeshSurface surface;
     public Celda[][] celdas;

    public GameObject[] malos;

    public GameObject cube;
    public GameObject cuboFalso;
    public GameObject soldado;
    public GameObject malo;
    public GameObject camara;
    public GameObject moneda;
    public TextAsset datos;
    public GameObject llave;
    public GameObject puerta;
    public int nivel;
    #endregion
    private int width;
    private int heigth;
    private int cantidadMalos;
    private int numMonedas;
    private float distanciaLlavePuerta = 10f; // dependera de la amplitud del nivel
    #region datosDeFichero


    #endregion

    #region privadas
    private int numMalos;
    private GameObject prota;
    private Controlador controller;
    private NavMeshAgent agent;
    private bool loop = false; // indica si hay que repetir la correción del escenario
    private bool escenarioValido = true;
    private NavMeshAgent[] enemsAgents;
    private float numeroMinCubos;
    private float numeroCasillas;
    private int maxIntentosCrearEscenario = 4;
    private float velocidadMalos;

    #endregion
    #endregion


    private void Awake()
    {
        controller = FindObjectOfType<Controlador>();
        controller.pedirNivel();
    }
    // Start is called before the first frame update
    void Start() // el mapa debe ser siempre multiplo de 2
    {
        leerDatos(nivel);
        malos = new GameObject[cantidadMalos];
        int intentosCrearEscenario = 0;
        this.transform.localScale = new Vector3 (width, this.transform.localScale.y,heigth);
        numeroCasillas = (width / longitudMuroX) * (heigth / longitudMuroZ);
        numeroMinCubos =  ((probabilidadMuro/100) * numeroCasillas*(porcentajeMinimoCubos/100));
        enemsAgents = new NavMeshAgent[cantidadMalos];
        celdas = new Celda[(width / 2)][]; // por esto
        for (int i = 0; i < width / 2; i++)
        {
            celdas[i] = new Celda[heigth / 2];
        }
        numMalos = 0;
        do
        {
            if(prota != null)
            {
                Destroy(prota);
                prota = null;
            }
            CrearEscenario();
            intentosCrearEscenario++;
            surface.BuildNavMesh();
            do
            {
                loop = false;
                retocarEscenario();
                surface.BuildNavMesh();
            } while (loop == true);
            comprobacionCubos();
            if(intentosCrearEscenario >= maxIntentosCrearEscenario)
            {
                escenarioValido = true;
                Debug.LogWarning("Limite de intentos alcazado, se da por valio el escenario");
            }
            if(escenarioValido == false)
            {
                foreach (Celda [] columna in celdas)
                {
                    foreach (Celda celdilla in columna)
                    {
                        if (celdilla.ocupada)
                        {
                            Destroy(celdilla.Cubo);
                            celdilla.ocupada = false;
                            celdilla.miObj = Celda.TipoObjeto.Nada;
                        }
                    }
                }
            }
            Debug.Log(intentosCrearEscenario + " Intentos realizados");
        } while (escenarioValido == false);

        ponerMasCubos(); // crea cubos en aquellos sitios con pocos espacios
        generarObjetos();
        controller.reiniciarControlador();
        surface.BuildNavMesh();
    }

    // Update is called once per frame
    void Update()
    {

        
    }


    void CrearEscenario()
    {

        int indiceGeneral = -1;
        int indexX = -1;

        for (int x = 0; x < width; x += (int)longitudMuroX)
        {
            indexX++;
            int indexZ = -1;
            for (int z = 0; z < heigth; z += (int)longitudMuroZ)
            {
                indiceGeneral++;
                indexZ++;
                celdas[indexX][indexZ] = new Celda ();
                Celda CeldaActual = celdas[indexX][indexZ];
                float numero = Random.Range(0, 100); // sobre 100
                Vector3 pos = new Vector3(x - ((width - longitudMuroX) / 2f), 1f, z - ((heigth - longitudMuroZ) / 2f));
                pos = pos + this.transform.position;
                CeldaActual.pos = pos;
                CeldaActual.indice = indiceGeneral;
                CeldaActual.ocupada = false;
                CeldaActual.miObj = Celda.TipoObjeto.Nada;
                if (x == (int)(width /2) && z == (int)(heigth/2))
                {

                   prota =  Instantiate(soldado, pos, Quaternion.identity); // para guardar las cosas lo haremos de 1 en 1 siendo  el 0,0 la esquina nose cual
                    agent = prota.GetComponent<NavMeshAgent>();
                    camara.GetComponent<MovimientoCamara>().asignarPadre(prota);
                }
                else if ((numero >= 0f) && (numero < probabilidadMuro))
                {
                    CeldaActual.Cubo = Instantiate(cube, pos, Quaternion.identity);
                    CeldaActual.ocupada = true;
                    CeldaActual.miObj = Celda.TipoObjeto.Cubo;

                }
                else
                {
                    /*
                    if (numMalos < cantidadMalos)
                    {
                        if (numero > 95f)
                        {
                             GameObject aux = Instantiate(malo, pos, Quaternion.identity);// mejor no incluirlo
                            enemsAgents[numMalos] = aux.GetComponent<NavMeshAgent>();
                            numMalos++;
                        }
                    }
                    */
                }
            }
        }
    }

    public void generarObjetos() // hace que lo smalos no se choquen entre si, máximo 100 malos
    {
        Celda celdaLibre;
        do
        {
            celdaLibre = encontrarCeldaLibre();
            GameObject aux = Instantiate(malo, celdaLibre.pos, Quaternion.identity);// mejor no incluirlo
            aux.GetComponent<Enemigo>().fijarLimites(heigth, width);
            enemsAgents[numMalos] = aux.GetComponent<NavMeshAgent>();
            malos[numMalos] = aux;
            numMalos++;

        } while (numMalos < cantidadMalos);


        celdaLibre = encontrarCeldaLibre();
        GameObject llaveAux = Instantiate(llave, new Vector3(celdaLibre.pos.x, 0, celdaLibre.pos.z), Quaternion.identity);
        celdaLibre.ocupada = true;
        celdaLibre.miObj = Celda.TipoObjeto.Llave;

        do
        {
            celdaLibre = encontrarCeldaLibre();
        } while (Vector3.Distance(llaveAux.transform.position, celdaLibre.pos) < distanciaLlavePuerta);


        Instantiate(puerta, new Vector3(celdaLibre.pos.x, 0, celdaLibre.pos.z), Quaternion.identity);
        celdaLibre.ocupada = true;
        celdaLibre.miObj = Celda.TipoObjeto.Puerta;

        for (int i = 0; i < numMalos; i++)
        {
            enemsAgents[i].gameObject.GetComponent<Enemigo>().prota = prota;
            enemsAgents[i].avoidancePriority = i;
        }

        for (int i = 0; i < numMonedas; i++)
        {
            celdaLibre = encontrarCeldaLibre();
            Instantiate(moneda, new Vector3(celdaLibre.pos.x, 1.3f, celdaLibre.pos.z), Quaternion.identity);
            celdaLibre.ocupada = true;
            celdaLibre.miObj = Celda.TipoObjeto.Moneda;
        }


    }



    public void ponerMasCubos()
    {
        bool ponerCubo = false;
        for (int i = 0; i < width / 2; i++)
        {
            for (int z = 0; z < heigth / 2; z++)
            {
                if((i == 0) || (i == ((width / 2)-1))|| (z == 0) || (z == (heigth / 2)-1))
                {

                } else
                {
                    if (!celdas[i][z].ocupada)
                    {
                        if (celdas[i - 1][z].ocupada) // izquierda
                        {
                            ponerCubo = false;
                        }
                        else if (celdas[i + 1][z].ocupada) // derecha
                        {
                            ponerCubo = false;
                        }
                        else if (celdas[i + 1][z + 1].ocupada) // arriba derecha
                        {
                            ponerCubo = false;
                        }
                        else if (celdas[i + 1][z - 1].ocupada) // abajo derecha
                        {
                            ponerCubo = false;
                        }
                        else if (celdas[i - 1][z + 1].ocupada) // arriba izquierda
                        {
                            ponerCubo = false;
                        }
                        else if (celdas[i - 1][z - 1].ocupada) // abajo izquierda
                        {
                            ponerCubo = false;
                        }
                        else if (celdas[i][z - 1].ocupada) // abajo
                        {
                            ponerCubo = false;
                        }
                        else if (celdas[i][z + 1].ocupada) // arriba
                        {
                            ponerCubo =false;
                        } else
                        {
                            ponerCubo = true;
                        }

                        if (ponerCubo)
                        {
                            celdas[i][z].ocupada = true;
                            celdas[i][z].Cubo = Instantiate(cube, celdas[i][z].pos, Quaternion.identity);
                            celdas[i][z].miObj = Celda.TipoObjeto.Cubo;
                            Debug.Log("Celda Generada");
                        }
                    }
                }
            }
        }
    }

    public void retocarEscenario() // creo que quitar muros no es realmente aleatorio, habria que cambiarlo.
    {
        List<Celda> listaAux = new List<Celda>(); //celdas Ocupadas que hay que destruir

        for (int i = 0; i < width / 2; i++)
        {
            for (int z = 0; z < heigth / 2; z++)
            {
                Celda CeldaArriba = null;
                Celda CeldaAbajo = null;
                Celda CeldaIzq = null;
                Celda CeldaDer = null;
                Celda CeldaActual = celdas[i][z];
               
                NavMeshPath aux = new NavMeshPath();
                if (!CeldaActual.ocupada && (CeldaActual.miObj != Celda.TipoObjeto.Cubo)) // si la celda esta libre
                {
                    //cogemos las casillas adyacentes
                    if (i == 0) //estamos a la izq
                    {
                        if (z <= ((heigth / 2)-2))
                        {
                            CeldaArriba = celdas[i][z + 1];
                        }
                        if (z >= 1)
                        {
                            CeldaAbajo = celdas[i][z - 1];
                        }
                        CeldaDer = celdas[i + 1][z];
                    }
                    else if (i == (width / 2)-1)
                    {
                        if (z <= ((heigth / 2) - 2))
                        {
                            CeldaArriba = celdas[i][z + 1];
                        }
                        if (z >= 1)
                        {
                            CeldaAbajo = celdas[i][z - 1];
                        }
                        CeldaIzq = celdas[i - 1][z];

                    }
                    else if (z == 0)
                    {
                        CeldaIzq = celdas[i - 1][z];
                        CeldaDer = celdas[i + 1][z];
                        CeldaArriba = celdas[i][z + 1];
                    }
                    else if (z == ((heigth / 2) - 1))
                    {
                        CeldaIzq = celdas[i - 1][z];
                        CeldaDer = celdas[i + 1][z];
                        CeldaAbajo = celdas[i][z - 1];
                    }
                    else
                    {
                        CeldaIzq = celdas[i - 1][z];
                        CeldaDer = celdas[i + 1][z];
                        CeldaAbajo = celdas[i][z - 1];
                        CeldaArriba = celdas[i][z + 1];
                    }

                    agent.CalculatePath(CeldaActual.pos, aux);
                    CeldaActual.visitada = true;
                    if (aux.status == NavMeshPathStatus.PathPartial) // no se puede llegar a esa celda
                    {
                        if(CeldaAbajo != null)
                        {
                            if(CeldaAbajo.ocupada == true)
                            {
                                listaAux.Add(CeldaAbajo);
                            }
                        }
                        if (CeldaArriba != null)
                        {
                            if (CeldaArriba.ocupada == true)
                            {
                                listaAux.Add(CeldaArriba);
                            }
                        }
                        if (CeldaDer != null)
                        {
                            if (CeldaDer.ocupada == true)
                            {
                                listaAux.Add(CeldaDer);
                            }
                        }
                        if (CeldaIzq != null)
                        {
                            if (CeldaIzq.ocupada == true)
                            {
                                listaAux.Add(CeldaIzq);
                            }
                        }
                        Celda borrar = null;
                        foreach(Celda aux2 in listaAux)
                        {
                            borrar = aux2;
                           
                        }
                        if(borrar != null)
                        {
                            Destroy(borrar.Cubo);
                            borrar.ocupada = false;
                            CeldaActual.miObj = Celda.TipoObjeto.Nada;
                            loop = true;
                        }
                        
                    }
                    
                }
            }
        }
            
    }

    public void comprobacionCubos()
    {
        int numCubos = 0;
        for (int i = 0; i < width / 2; i++)
        {
            for (int z = 0; z < heigth / 2; z++)
            {
              if(  celdas[i][z].ocupada)
                {
                    numCubos++;
                }
            }
        }
        Debug.Log(" el numero de cubos es : " + numCubos);
        if(numCubos < numeroMinCubos)
        {
            escenarioValido = false;
        }
    }
    
    public void leerDatos(int nivel)
    {
        string []  aux = datos.text.Split('\n');
        string[] aux1 = aux[nivel].Split(';');
        width = int.Parse( aux1[1]);
        heigth = int.Parse(aux1[2]);
        cantidadMalos = int.Parse(aux1[3]);
        numMonedas = int.Parse(aux1[4]);


    }

    public Celda encontrarCeldaLibre()
    {
        int casillaX = -1;
        int casillaZ = -1;
        Celda aux2;

        do
        {
            casillaX = Random.Range(0, (int)(width / longitudMuroX));
            casillaZ = Random.Range(0, (int)(heigth / longitudMuroZ));
            aux2 = celdas[casillaX][casillaZ];
        } while (aux2.ocupada);

        return aux2;
    }

    public void congelarMov()
    {

        
      Rigidbody protaBody =   prota.GetComponent<Rigidbody>();
        protaBody.velocity = Vector3.zero;
        protaBody.constraints = RigidbodyConstraints.FreezeAll;

        foreach(GameObject malo in malos)
        {
            NavMeshAgent aux = malo.GetComponent<NavMeshAgent>();
            velocidadMalos = aux.speed;
            aux.speed = 0;
        }
        
    }

    public void descongelarMov()
    {
        Rigidbody protaBody = prota.GetComponent<Rigidbody>();
        protaBody.constraints = RigidbodyConstraints.None;
        foreach (GameObject malo in malos)
        {
            NavMeshAgent aux = malo.GetComponent<NavMeshAgent>();
            aux.speed = velocidadMalos;
        }

    }
}


