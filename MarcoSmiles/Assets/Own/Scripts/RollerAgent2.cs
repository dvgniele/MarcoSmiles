using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;
using Unity.MLAgents.Actuators;


/*Si vuole addestrare un agente che decide quale nota suonare, a seconda della posizione di una sfera
 */



public class RollerAgent2 : Agent
{
    Rigidbody rBody;
    //public Transform Target;

    string[] targets;
    string currentTarget;



    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        targets = new string[] { "C", "D", "E", "F", "G", "A", "B" };

    }

    int i;
    public override void OnEpisodeBegin()
    {
        if (this.transform.localPosition.y < 0)
        {
            // If the Agent fell, zero its momentum
            this.rBody.angularVelocity = Vector3.zero;
            this.rBody.velocity = Vector3.zero;
            this.transform.localPosition = new Vector3(0, 0.5f, 0);
        }



        //Sceglie in modo casuale l'indice dell'array relativo alla nota che si sta allenando.
        i = Random.Range(0, 7);

        // Set the target for the current training note
        //currentTarget = targets[i];
        Debug.Log("Current training note is:" + targets[i]);

    }



    public override void CollectObservations(VectorSensor sensor)
    {
        // Target and Agent positions
        sensor.AddObservation(i);
        sensor.AddObservation(this.transform.localPosition);

        // Agent velocity
        // sensor.AddObservation(rBody.velocity.x);
        // sensor.AddObservation(rBody.velocity.z);
    }

    

    int count = 5;
    public float forceMultiplier = 5f;
    int playingNote;
    bool isStill;
    public override void OnActionReceived(float[] vectorAction)
    {
        /* Se  lo script "Behaviour Parameters" di ml-agents ha il valore di Behaviour Type settato su "Default",
         * in questo metodo, il vettore vectorAction, riceve valori casuali.
         */

        //Viene creato il vettore che permette di muovere la sfera, partendo dai valori ricevuti in vectorAction.
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = vectorAction[0];
        controlSignal.z = vectorAction[1];
        Debug.Log("CONTROL SIGNAL ! !:  " + controlSignal);
        rBody.transform.Translate(controlSignal * forceMultiplier * Time.deltaTime);



        /*Timer. Controlla se la sfera rimane ferma per più di 5 secondi. Se lo è setta isStill a true,
         *
         *Il valore del contatore count ritorna a 5 se la sfera riprende a muoversi
        */
        if (controlSignal.x == 0 && controlSignal.z == 0)
        {
            count = count - 1;
            if (count <= 0)
            {
                isStill = true;
            }
            else
            {
                isStill = false;
            }
        }
        else
        {
            count = 5;
            isStill = false;
        }

       //Debug.Log("Contatoree e e! ! !:  " + count);


        //Se la sfera cade dal paviento viene settato un reward di -1
        if (this.transform.localPosition.y < 0)
        {
            SetReward(-1f);
            EndEpisode();
        }

        //Se la sfera è ferma per più di 5 secondi, si setta un reward di 1
        if (isStill)
        {
            count = 5;
            SetReward(1f);
            Debug.Log("CUMULATIVE REWARDS:  " + GetCumulativeReward());
            EndEpisode();
        }


    }

    /*Durante il training, Sembra che non si riesca a far muovere l'agente a proprio piacimento (con un inpuit esterno) 
     * ma serve che si muova con input casuali in OnActionReceived(). 
    */

    public override void Heuristic(float[] actionsOut)
    {

        /* Se  lo script "Behaviour Parameters" di ml-agents ha il valore di Behaviour Type settato su "Heuristic",
         * vengono passati a vectorAction, i valori specificati all'interno del metodo
       */

        actionsOut[0] = Input.GetAxis("Horizontal");
        actionsOut[1] = Input.GetAxis("Vertical");
        actionsOut[2] = i;

        //Debug.Log(Input.GetAxis("Horizontal"));
        //Debug.Log(Input.GetAxis("Vertical"));

    }
}
   