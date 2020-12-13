using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;




public class TestingML1 : MonoBehaviour
{
    double[] features;
   // private TestML testML;
    void Start()
    {
        TestML.Populate();
        features = new double[]             //esempio che rappresenta nota con indice 18
            {
              52.98770905, 24.97154236, 29.72991753, 23.85840797, 25.60717583, 34.48836517,
                10.40907669, 9.875915527, 10.22246742, 37.64592361, 175.954834, 152.2133942,
                151.710434, 152.0919647, 114.0123672, 28.66732407, 8.403100014, 10.74728107
            };

        //testML = new TestML();
        int maxIndex = TestML.ReteNeurale(features);   //rappresenta la nota che deve essere suonata

        Debug.Log("L'indice che rappresenta la nota da suonare è:  " + maxIndex );

    }


}
