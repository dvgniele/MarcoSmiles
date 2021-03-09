using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine;

public static class TestML
{
    private static List<List<float>> W1 = new List<List<float>>();

    private static List<float> B1 = new List<float>();

    private static List<List<float>> W2 = new List<List<float>>();

    private static List<float> B2 = new List<float>();

    public static System.DateTime DateLatestLearning;


    /// <summary>
    ///  ReadArraysFromFormattedFile Restituisce una lista di array float. Gli array sono inseriti nella lista nell'ordine in cui vengono letti dal file.
    ///  La lista è formattata nel seguente modo:
    /// Finchè leggendo il file vengono letti vettori, allora questi vettori vengono aggiunti alla lista e restituiti.
    /// Nel momento in cui la funzione capisce di aver letto una matrice, inserisce nella lista un array vuoto [].
    ///
    /// Ad esempio:
    /// - nel file bias.txt, sono contenuti array. Ogni array rappresenta un bias: 
    /// Il primo array rappresenta B1; il secondo array B2 etc...
    ///  
    ///  - nel file weights.txt, sono contenute matrici, ognuna contiene una lista di array. Quindi tutti gli array contenuti nella lista restituita,
    /// finchè non si legge un array vuoto [], faranno parte della prima matrice letta.
    /// In questo modo, si formatta la lista in n blocchi di array, dove n è il numero di matrici contenuti nel file:
    ///  Tutti gli array contenuti nel primo blocco di array (quelli che si trovano prima del primo array vuoto[]) rappresentano la matrice W1,
    /// Tutti gli array contenuti nel secondo blocco di array (quelli che si trovano dopo il primo array vuoto[] e prima del secondo array vuoto [])
    /// rappresentano la matrice W2 
    /// etc...
    /// </summary>
    /// <param name="name">Nome del file da aprire</param>
    /// <returns>
    /// Una lista di float contenente i numeri letti da file. La lista e formattato logicamente in modo tale da poter costrutire gli eventuali array contenuti nel file.
    /// </returns>
    private static List< List<float> > ReadArraysFromFormattedFile(string name)
    {
        //stringa
        var text = FileUtils.LoadFile(name);

        if (text == null)
            return null;

        DateLatestLearning = File.GetLastWriteTime(FileUtils.GeneratePath(name));

        text = text.Replace("[", "");
        text = text.Replace("\n", "");

        string[] vettori = text.Split(']');       // StringSplitOptions.RemoveEmptyEntries toglie le entrate vuote (potrebbe servire lasciarle) 

        //lista degli array letti, i valori degli array sono strighe
        List< List <string> > temp = new List< List<string> >();

        foreach (string vettore in vettori)
        {
            //temp.Add(vettore.Split(' ', StringSplitOptions.RemoveEmptyEntries) );      //divide per ' ', ma lascia entrate vuote

            temp.Add(vettore.Split(' ')                                       //divide per ' ' e mette l'array di stringhe all'interno della lista temp
                .Select(tag => tag.Trim())                                    //elimina le entrate vuote
                .Where(tag => !string.IsNullOrEmpty(tag)).ToList());
        }

        //lista degli array letti che verrà restituita
        List<List<float>> listOfReadArrays = new List< List<float> >();


        //Converte tutti i valori degli array letti, da string a float.
        foreach ( var arr in temp)
        {
            float[] t = new float[arr.Count];
            for (int i = 0; i < arr.Count; i++)
            {
                t[i] = float.Parse(arr[i], CultureInfo.InvariantCulture);
            }
            listOfReadArrays.Add(t.ToList());
            // listOfReadArrays.Add( arr.ToList().ConvertAll(x => Convert.Tofloat(x)).ToArray() );       
        }
        return listOfReadArrays;
    }


    /// <summary>
    /// Riempe le Matrici W1 ; B1 ; W2; B2. 
    /// Usa il metodo ReadArraysFromFormattedFile, per leggere da un file una lista di arrays di tipo float. 
    /// Questa lista restituita è formattata logicamente.
    /// </summary>
    public static bool Populate()
    { 
        B1.Clear(); B2.Clear(); W1.Clear(); W2.Clear();
        
        List<List<float>> biasArrays = ReadArraysFromFormattedFile("bias_out.txt");
        if (biasArrays == null)
            return false;

        B1 = biasArrays.ElementAt(0);
        B2 = biasArrays.ElementAt(1);

        List<List<float>> weightsArrays = ReadArraysFromFormattedFile("weights_out.txt");
        if (weightsArrays == null)
            return false;

        
        //finchè non arrivo ad un array uguale a {} sono array che rappresentano W1
        int j = 0; 
        while (Enumerable.SequenceEqual(weightsArrays.ElementAt(j), new List<float> { } ) == false)
        {
            j++;
        }

        for(int i = 0; i < j ; i++)
        {
            W1.Add(weightsArrays.ElementAt(i));     
        }

        //da j+1 alla fine sono array che rappresentano W2
        int k = j+1;
        while (Enumerable.SequenceEqual(weightsArrays.ElementAt(k), new List<float> { } ) == false)
        {
            k++;
        }

        for (int i = 0 ; i < k - (j + 1) ; i++)
        {
            W2.Add(weightsArrays.ElementAt(j + 1 + i));
        }


        return true;
        
    }

    /// <summary>
    /// Predice la nota associata alle features passato come parametro.
    /// Usa le matrici W1, B1, W2 e B2 per effttuare i calcoli.
    /// Restituisce un indice che va da 0 a 23, corrispondente alla nota da suonare.
    /// La funzione supporta il caso in cui il dataset contenga meno note rispetto agli indici 0-23. Questo è fatto calcolando in fase di
    /// computazione la grandezza delle matrici W1, B1, W2, B2.
    /// Conseguentemente, la funzione ReteNeurale, restituirà indici che si trovano fra il range di note allenate.
    /// </summary>
    /// <param name="features">nota da trovare</param>
    /// <returns>Id nota trovata</returns>
    public static int ReteNeurale(float[] features)
    {
      
        
        float[] scaledFeatures = ScaleValues(features);         
        for (int i = 0; i < scaledFeatures.Length; i++)
        {
            features[i] = scaledFeatures[i];
        }


        // output_hidden1 ha lo stesso numero di elementi di B1
        var output_hidden1 = new float[B1.Count];
        // output_hidden2 ha lo stesso numero di elementi di B2
        var output_hidden2 = new float[B2.Count];





        float x, w, r;

        for (int i = 0; i < output_hidden1.Length; i++)
        {
            output_hidden1[i] += B1.ElementAt(i);
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
            output_hidden2[i] += B2.ElementAt(i);
            for (int j = 0; j < output_hidden1.Length; j++)
            {
                x = output_hidden1[j];
                w = W2[j][i];
                r = x * w;

                output_hidden2[i] += r;
            }
        }

        float sum = 0;
        foreach (var item in output_hidden2)
        {
            sum += Mathf.Exp((float)item);
        }

        var toRet = new float[output_hidden2.Length];
        for (int i = 0; i < output_hidden2.Length; i++)
        {
            toRet[i] = Mathf.Exp((float)output_hidden2[i]) / sum;
        }

        return toRet.ToList().IndexOf(toRet.Max()); 
    }


    /// <summary>
    /// Scala i valori convertendo le features non scalate in valori tra 0 e 1
    /// </summary>
    /// <param name="unscaledFeatures">features da scalare</param>
    /// <returns>features scalate con valori tra 0 e 1</returns>
    private static float[] ScaleValues(float[] unscaledFeatures)
    {
        var scaledFeatures = new float[unscaledFeatures.Length];
        var minValues = new float[unscaledFeatures.Length];
        var maxValues = new float[unscaledFeatures.Length];

        string[] readText = File.ReadAllLines(FileUtils.GeneratePath("min&max_values_dataset_out.txt"));       
        
        //primo elemento contiene riga contenente valori min                                                                                           
        //secondo elemento contiene riga contenente valori max
        string[] min = readText[0].Split(' ');
        string[] max = readText[1].Split(' ');

        for (int i = 0; i < unscaledFeatures.Length; i++)
        {
            minValues[i] = float.Parse(min[i], CultureInfo.InvariantCulture);
            maxValues[i] = float.Parse(max[i], CultureInfo.InvariantCulture);
        }


        for (int i = 0; i < unscaledFeatures.Length; i++)
        {
            scaledFeatures[i] = (unscaledFeatures[i] - minValues[i]) / (maxValues[i] - minValues[i]);
        }

        return scaledFeatures;

    }
}







