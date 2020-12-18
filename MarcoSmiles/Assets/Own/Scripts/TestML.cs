using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine;

public static class TestML
{
    private static double[][] W1;

    private static double[][] B1;

    private static double[][] W2;

    private static double[][] B2;

    /*ReadArraysFromFormattedFile Restituisce una lista di array double. Gli array sono inseriti nella lista nell'ordine in cui vengono letti dal file.
     * La lista è formattata nel seguente modo:
     * Finchè leggendo il file vengono letti vettori, allora questi vettori vengono aggiunti alla lista e restituiti.
     * Nel momento in cui la funzione capisce di aver letto una matrice, inserisce nella lista un array vuoto [].
     * 
     * Ad esempio:
     * - nel file bias.txt, sono contenuti array. Ogni array rappresenta un bias: 
     *      Il primo array rappresenta B1; il secondo array B2 etc...
     * 
     * - nel file weights.txt, sono contenute matrici, ognuna contiene una lista di array. Quindi tutti gli array contenuti nella lista restituita,
     *   finchè non si legge un array vuoto [], faranno parte della prima matrice letta.
     *   In questo modo, si formatta la lista in n blocchi di array, dove n è il numero di matrici contenuti nel file:
     *      Tutti gli array contenuti nel primo blocco di array (quelli che si trovano prima del primo array vuoto[]) rappresentano la matrice W1,
     *      Tutti gli array contenuti nel secondo blocco di array (quelli che si trovano dopo il primo array vuoto[] e prima del secondo array vuoto [])
     *      rappresentano la matrice W2 
     *      etc...
     * */
    private static List<double[]> ReadArraysFromFormattedFile(string path)
    {
        var text = File.ReadAllText(path);

        text = text.Replace("[", "");
        text = text.Replace("\n", "");

        string[] vettori = text.Split(']');       // StringSplitOptions.RemoveEmptyEntries toglie le entrate vuote (potrebbe servire lasciarle) 


        List<string[]> temp = new List<string[]>();

        foreach (string vettore in vettori)
        {
            //temp.Add(vettore.Split(' ', StringSplitOptions.RemoveEmptyEntries) );      //divide per ' ', ma lascia entrate vuote

            temp.Add(vettore.Split(' ')                                       //divide per ' ' e mette l'array di stringhe all'interno della lista temp
                .Select(tag => tag.Trim())                                    //elimina le entrate vuote
                .Where(tag => !string.IsNullOrEmpty(tag)).ToArray());
        }


        List<double[]> listOfReadArrays = new List<double[]>();

        foreach (string[] arr in temp)
        {
            double[] t = new double[arr.Length];
            for (int i = 0; i < arr.Length; i++)
            {
                t[i] = double.Parse(arr[i], CultureInfo.InvariantCulture);
            }
            listOfReadArrays.Add(t);
            // listOfReadArrays.Add( arr.ToList().ConvertAll(x => Convert.ToDouble(x)).ToArray() );       
        }
        return listOfReadArrays;
    }






    public static void Populate()
    {
        List<double[]> biasArrays = ReadArraysFromFormattedFile("../MarcoSmiles/Assets/Own/Datasets/bias_out.txt");

        B1 = new double[biasArrays.ElementAt(0).Length][];
        B2 = new double[biasArrays.ElementAt(1).Length][];

        B1[0] = biasArrays.ElementAt(0);
        B2[0] = biasArrays.ElementAt(1);

        List<double[]> weightsArrays = ReadArraysFromFormattedFile("../MarcoSmiles/Assets/Own/Datasets/weights_out.txt");

        //finchè non arrivo ad un array uguale a {} sono array che rappresentano W1
        int j = 0; 
        while (Enumerable.SequenceEqual(weightsArrays.ElementAt(j), new double[] { } ) == false)
        {
            j++;
        }

        //Debug.Log(j.ToString());

        W1 = new double[j][];

        for(int i = 0; i < j ; i++)
        {
            W1[i] = weightsArrays.ElementAt(i);     

        }

        //da j+1 alla fine sono array che rappresentano W2
        int k = j+1;
        while (Enumerable.SequenceEqual(weightsArrays.ElementAt(k), new double[] { }) == false)
        {
            k++;
        }

        W2 = new double[k - (j + 1)][];

        for (int i = 0 ; i < k - (j + 1) ; i++)
        {
            W2[i] = weightsArrays.ElementAt(j + 1 + i);

        }
         /*
        foreach (double[] arr in W2)
        {
            foreach (double e in arr)
            {
                Debug.Log(e);
            }
        }
        */
    }

    public static int ReteNeurale(double[] features)
    {
        // output_hidden1 ha lo stesso numero di elementi di B1
        var output_hidden1 = new double[B1.Length];
        // output_hidden2 ha lo stesso numero di elementi di B2
        var output_hidden2 = new double[B2.Length];
        
        double x, w, r;
        var flag = false;

        for (int i = 0; i < output_hidden1.Length; i++)
        {
            output_hidden1[i] += B1[0][i];
            for (int j = 0; j < features.Length; j++)
            {
                x = features[j];
                w = W1[j][i];
                r = x * w;

                output_hidden1[i] += r;
            }

            if (output_hidden1[i] <= 0)
                output_hidden1[i] = 0;
        }

        for (int i = 0; i < output_hidden2.Length; i++)
        {
            output_hidden2[i] += B2[0][i];
            for (int j = 0; j < output_hidden1.Length; j++)
            {
                x = output_hidden1[j];
                w = W2[j][i];
                r = x * w;

                output_hidden2[i] += r;
            }
        }

        double sum = 0;
        foreach (var item in output_hidden2)
        {
            sum += Mathf.Exp((float)item);
        }

        var toRet = new double[output_hidden2.Length];
        for (int i = 0; i < output_hidden2.Length; i++)
        {
            toRet[i] = Mathf.Exp((float)output_hidden2[i]) / sum;
        }

        return  toRet.ToList().IndexOf(toRet.Max()); 
    }
}


//Funzione per scalare i valori: converte le features in valori da 0 a 1
/*
private static double[] ScaleValues(double[] unscaledFeatures)
{
    var scaledFeatures = new double[unscaledFeatures.Length];
    var minValues = new double[unscaledFeatures.Length];
    var maxValues = new double[unscaledFeatures.Length];

    string[] readText = File.ReadAllLines("Assets/Own/Datasets/min&max_values_dataset_out.txt");        //primo elemento contiene riga contenente valori min
                                                                                                        //secondo elemento contiene riga contenente valori max
    string[] min = readText[0].Split(' ');
    string[] max = readText[1].Split(' ');

    for (int i=0 ; i<unscaledFeatures.Length; i++)
    {
        minValues[i] = Double.Parse(min[i], CultureInfo.InvariantCulture);
        maxValues[i] = Double.Parse(max[i], CultureInfo.InvariantCulture);
    }

    for (int i = 0; i < minValues.Length; i++)
    {
        Debug.Log("index " + i +" (valori min): " + minValues[i]);
    }
    for (int i = 0; i < maxValues.Length; i++)
    {
        Debug.Log("index " + i + " (valori max): " + maxValues[i]);
    }


    for (int i = 0; i<unscaledFeatures.Length; i++)
    {
        scaledFeatures[i] = (unscaledFeatures[i] - minValues[i]) / (maxValues[i] - minValues[i]);
    }

   //foreach (double e in scaledFeatures){
   //     Debug.Log("features scalate  : " + e);
   // }

    return scaledFeatures;

}

*/


