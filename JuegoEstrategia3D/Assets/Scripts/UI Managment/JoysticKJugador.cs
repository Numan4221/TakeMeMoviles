using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoysticKJugador : MonoBehaviour
{
    private Joystick joystick;
    private Rigidbody rigidbody;
    public float velocidad;
    // Start is called before the first frame update
    void Start()
    {
        joystick = FindObjectOfType<Joystick>();
        rigidbody = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rigidbody.velocity = new Vector3(joystick.Horizontal * velocidad, rigidbody.velocity.y, joystick.Vertical* velocidad);
    }
}
