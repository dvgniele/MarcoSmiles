using System.IO;
using UnityEngine;



//SOLO PER TESTARE LA LETTURA DA FILE, DA ELIMINARE
public class ReadMLFromFile : MonoBehaviour
{

    private static float[][] W1 = new float[][]
    {

    };


    private static float[][] B1 = new float[][]
    {
    };


    private static float[][] W2 = new float[][]
    {
           
    };


    private float[][] B2 = new float[][]
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
