using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine;

public static class FileUtils
{
    static string path = Application.persistentDataPath;

    static string filename = "marcosmiles_dataset";
    static string ext = "csv";

    private static string GeneratePath(string filename)
    {
        Debug.Log($"{path}/{filename}.{ext}");

        return $"{path}/{filename}.{ext}";
    }

    public static void Save(List<Position> data)
    {
        if(File.Exists(GeneratePath(filename)))
        {
            foreach (var item in data)
            {
                var str = $"{item.Left_Hand.FF1}, {item.Left_Hand.FF2}, {item.Left_Hand.FF3}, {item.Left_Hand.FF4}, {item.Left_Hand.FF5}," +
                    $" {item.Left_Hand.NFA1}, {item.Left_Hand.NFA2}, {item.Left_Hand.NFA3}, {item.Left_Hand.NFA4}," +
                    $" {item.Right_Hand.FF1}, {item.Right_Hand.FF2}, {item.Right_Hand.FF3}, {item.Right_Hand.FF4}, {item.Right_Hand.FF5}," +
                    $" {item.Right_Hand.NFA1}, {item.Right_Hand.NFA2}, {item.Right_Hand.NFA3}, {item.Right_Hand.NFA4}," +
                    $" {item.ID}" +
                    Environment.NewLine;

                Debug.Log(str);
                File.AppendAllText(GeneratePath(filename), str);
            }

            Debug.Log("Saved dataset");
        }
        else
        {
            File.Create(GeneratePath(filename)).Dispose();
            Save(data);

            Debug.Log("FILE CREATO");
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