using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Soldado : MonoBehaviour
{
    public NavMeshAgent agent;
    public int id;
    public bool asigando;
    private int monedasEnNivel;
    // Start is called before the first frame update
    void Start()
    {
        monedasEnNivel = 0;
        asigando = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoverTropa(Vector3 pos)
    {
       
            agent.SetDestination(pos);
        
    }

    public void cogerMonedas()
    {
        monedasEnNivel++;
    }

    public void limpiarMonedas()
    {
        monedasEnNivel = 0;
    }
    
}
