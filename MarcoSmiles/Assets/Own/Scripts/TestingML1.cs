using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;




public class TestingML1 : MonoBehaviour
{
    float[] features;
   // private TestML testML;
    void Start()
    {
        TestML.Populate();
        features = new float[]             //esempio che rappresenta nota con indice 18
            {
                57.45253f, 142.1957f, 143.3936f, 144.7559f, 146.7362f, 150.8697f, 3.48442f, 7.424838f, 7.013023f, 40.03038f, 51.81815f, 56.33659f, 56.76128f, 58.25762f, 28.27847f, 0.9162119f, 3.219175f, 1.402769f


            };

        //testML = new TestML();
        int maxIndex = TestML.ReteNeurale(features);   //rappresenta la nota che deve essere suonata

        Debug.Log("L'indice che rappresenta la nota da suonare è:  " + maxIndex );

    }


}
