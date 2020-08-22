using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine;

public static class FileUtils
{
    static string path = Application.persistentDataPath;

    public static void Save(DataToStore data)
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

    private static string GeneratePath(string filename)
    {
        return $"{path}/{filename}.gepao";
    }
}