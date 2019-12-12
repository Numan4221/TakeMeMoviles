using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DoorController : MonoBehaviour
{
    public bool keyNeeded = false;              //Is key needed for the door
    public bool gotKey;                  //Has the player acquired key

    private bool playerInZone;                  //Check if the player is in the zone
    private bool doorOpened;                    //Check if door is currently opened or not
    private Camera camara;

    private Animation doorAnim;
    private BoxCollider doorCollider;           //To enable the player to go through the door if door is opened else block him

    private Controlador controller;
    private ControladorNivel minivel;
    private bool abierta = false;



    enum DoorState
    {
        Closed,
        Opened,
        Jammed
    }

    DoorState doorState = new DoorState();      //To check the current state of the door

    /// <summary>
    /// Initial State of every variables
    /// </summary>
    private void Start()
    {
        controller = FindObjectOfType<Controlador>();
        minivel = FindObjectOfType<ControladorNivel>();
        gotKey = false;
        doorOpened = false;                     //Is the door currently opened
        playerInZone = false;                   //Player not in zone
        doorState = DoorState.Closed;           //Starting state is door closed


        doorAnim = transform.parent.gameObject.GetComponent<Animation>();
        doorCollider = transform.parent.gameObject.GetComponent<BoxCollider>();

        //this.gameObject.SetActive(false);
        
        //If Key is needed and the KeyGameObject is not assigned, stop playing and throw error
      
    }

    private void Update()
    {
        if (abierta)
        {
            if (doorAnim.isPlaying)
            {

            } else
            {
                controller.reestaurarCamara();
                abierta = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Soldado")
        {
            if (gotKey)
            {
                SceneManager.LoadScene("NivelSuperado");
                minivel.juntarMonedas();
                controller.desbloquearSigNivel();
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Soldado")
        {
            playerInZone = false;
        }
    }



    public void activarPuerta()
    {
        doorAnim.Play("Door_Open");
        doorState = DoorState.Opened;
        abierta = true;
    }
}
