using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;



//SOLO PER TESTARE LA LETTURA DA FILE, DA ELIMINARE
public class ReadMLFromFile : MonoBehaviour
{

    private static double[][] W1 = new double[][]
    {

    };


    private static double[][] B1 = new double[][]
    {
    };


    private static double[][] W2 = new double[][]
    {
           
    };


    private double[][] B2 = new double[][]
    {
    };
    // Start is called before the first frame update
    void Start()
    {

        var lines = File.ReadAllLines("../MarcoSmiles/Assets/Own/Datasets/bias_out.txt");
        foreach (string e in lines) {
            Debug.Log(e);
        }
    }

}
