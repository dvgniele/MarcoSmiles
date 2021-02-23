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
              52.98770905f, 24.97154236f, 29.72991753f, 23.85840797f, 25.60717583f, 34.48836517f,
                10.40907669f, 9.875915527f, 10.22246742f, 37.64592361f, 175.954834f, 152.2133942f,
                151.710434f, 152.0919647f, 114.0123672f, 28.66732407f, 8.403100014f, 10.74728107f
            };

        //testML = new TestML();
        int maxIndex = TestML.ReteNeurale(features);   //rappresenta la nota che deve essere suonata

        Debug.Log("L'indice che rappresenta la nota da suonare è:  " + maxIndex );

    }


}
