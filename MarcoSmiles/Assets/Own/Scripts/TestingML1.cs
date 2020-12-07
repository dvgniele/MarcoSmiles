using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;




public class TestingML1 : MonoBehaviour
{
    double[] features;

    void Start()
    {
        features = new double[]             //esempio che rappresenta nota con indice 0  
            {
                61.4049, 146.6705, 145.6753, 145.5838, 147.2906, 153.1232,
                1.755243, 9.343422, 10.85496, 50.51425, 152.7829, 149.1697,
                149.6153, 149.4406, 150.7445, 4.826088, 8.815902, 9.580413,
            };

        int maxIndex = TestML.ReteNeurale(features);   //rappresenta la nota che deve essere suonata

        Debug.Log("L'indice che rappresenta la nota da suonare è:  " + maxIndex );

    }


}
