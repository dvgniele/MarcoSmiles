using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine;

public static class FileUtils
{
    static string path = Application.persistentDataPath;

    static string filename = "marcosmiles_dataset.csv";
    //static string ext = "csv";

    private static string GeneratePath(string filename)
    {
        Debug.Log($"{path}/{filename}");

        return $"{path}/{filename}";
    }

    public static string PrintPath()
    {
        return $"{GeneratePath("")}";

    }
    

    public static void Save(List<Position> data)
    {
        System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
        customCulture.NumberFormat.NumberDecimalSeparator = ".";
        System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

        if(File.Exists(GeneratePath(filename)))
        {
            foreach (var item in data)
            {
                var str = $"{item.Left_Hand.FF1}, {item.Left_Hand.FF2}, {item.Left_Hand.FF3}, {item.Left_Hand.FF4}, {item.Left_Hand.FF5}," +
                    $" {item.Left_Hand.NFA1}, {item.Left_Hand.NFA2}, {item.Left_Hand.NFA3}, {item.Left_Hand.NFA4}," +
                    $" {item.Right_Hand.FF1}, {item.Right_Hand.FF2}, {item.Right_Hand.FF3}, {item.Right_Hand.FF4}, {item.Right_Hand.FF5}," +
                    $" {item.Right_Hand.NFA1}, {item.Right_Hand.NFA2}, {item.Right_Hand.NFA3}, {item.Right_Hand.NFA4}," +
                    $" {item.ID}"
                    .ToString(System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));

                str += Environment.NewLine;

                //  stampa la riga che si sta salvando sul file
                //Debug.Log(str);
                File.AppendAllText(GeneratePath(filename), str);
            }

            _GM.list_posizioni.Clear();
            Debug.Log("DATASET SAVED");
        }
        else
        {
            File.Create(GeneratePath(filename)).Dispose();
            Save(data);

            Debug.Log("FILE CREATO");
        }
    }

    public static string LoadFile(string name)
    {
        if (File.Exists(GeneratePath($"{name}")))
        {
            return File.ReadAllText(GeneratePath($"{name}"));
        }
        else
        {
            Debug.LogError("File non trovato");
            return null;
        }
    }

    /*     Converte bytes in un file .py e lo scrive in GeneratePath [AppData/LocalLow]
     */
    public static void SavePy(byte[] file, string name)
    {
        string filename = name + ".py";
        //Debug.Log(file);
        try
        {
            if (!File.Exists(GeneratePath(filename)))
            {
                using (var fs = new FileStream(GeneratePath(filename), FileMode.Create, FileAccess.Write))
                {
                    //Debug.Log(file);                  
                    fs.Write(file, 0, file.Length);
                }
            }
            else
            {
                Debug.Log("FILE GIA ESISTENTE");
            }
        }
        catch (Exception ex)
        {
            Debug.Log("Exception caught in process:" + ex.ToString());
        }
    }

    /*
    public static void Save(DataToStore data)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(GeneratePath("marcosmiles_save"), FileMode.Create);

        bf.Serialize(stream, data);
        stream.Close();
    }
    
    public static void Save(List<DataToStore> data)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(GeneratePath("marcosmiles_save"), FileMode.Create);

        bf.Serialize(stream, data);
        stream.Close();
    }



    public static DataToStore Load()
    {
        if (File.Exists(GeneratePath("marcosmiles_save")))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(GeneratePath("marcosmiles_save"), FileMode.Open);

            DataToStore data = bf.Deserialize(stream) as DataToStore;

            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("File non trovato");
            return null;
        }
    }
    
    public static List<DataToStore> LoadList()
    {
        if (File.Exists(GeneratePath("marcosmiles_save")))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(GeneratePath("marcosmiles_save"), FileMode.Open);

            List<DataToStore> data = new List<DataToStore>();

            try
            {
                data = bf.Deserialize(stream) as List<DataToStore>;
            }
            catch(Exception)
            {
                //  hehe
            }

            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("File non trovato");
            return null;
        }
    }
    */


}