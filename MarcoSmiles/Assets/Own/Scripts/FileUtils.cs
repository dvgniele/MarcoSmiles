using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine;

public static class FileUtils
{
    /// <summary>
    /// Path per AppData
    /// </summary>
    static string path = Application.persistentDataPath;

    /// <summary>
    /// Nome del dataset da utilizzare
    /// </summary>
    static string filename = "marcosmiles_dataset.csv";
    //static string ext = "csv";

    /// <summary>
    /// Nome della cartella contenente i datasets importati
    /// </summary>
    static string folderName = "MyDatasets";

    /// <summary>
    /// Nome Dataset
    /// </summary>
    public static string defaultFolder = "DefaultDataset";

    /// <summary>
    /// Restituisce il nome della cartella del dataset
    /// </summary>
    /// <returns>Il nome della cartella del dataset</returns>
    public static string GetFolderName()
    {
        return folderName;
    }

    /// <summary>
    /// Genera il path per il file da utilizzare. il path è formato da: path (La cartella in appdata dell'aplicazione); 
    /// folderName (Cartella dei datasets) e filename (nome del file)
    /// </summary>
    /// <param name="filename">Nome del file</param>
    /// <returns></returns>
    private static string GeneratePath(string filename)
    {
        //Debug.Log($"{path}/{folderName}/{_GM.selectedDataset}/{filename}");

        return $"{path}/{folderName}/{_GM.selectedDataset}/{filename}";
    }

    /// <summary>
    /// Genera il path per il file da utilizzare da una cartella specifica.
    /// il path è formato da: path (La cartella in appdata dell'aplicazione); folderName (Cartella dei datasets);
    /// folder (Cartella passato come parametro, all'interno della quale si vuole effettuare la ricerca) e filename (nome del file)
    /// 
    /// </summary>
    /// <param name="filename">Nome del file</param>
    /// <param name="folder">Nome della cartella</param>
    /// <returns></returns>
    private static string GeneratePath(string filename, string folder)
    {
        //Debug.Log($"{path}/{folderName}/{folder}/{filename}");

        return $"{path}/{folderName}/{folder}/{filename}";
    }

    /// <summary>
    /// Prende il path per il datasaet. Versione public di generatepath. [(è davvero necessaria???? o basta mettere generate path a public?)]
    /// </summary>
    /// <returns>Ritorna il path per il dataset</returns>
    public static string PrintPath()
    {
        return $"{GeneratePath("")}";
    }

    /// <summary>
    /// Prende il path per il datasaet
    /// </summary>
    /// <param name="folder">Nopme della cartella</param>
    /// <returns>Ritorna il path per il dataset</returns>
    public static string PrintPathFolder(string folder)
    {
        return $"{GeneratePath("",$"{folder}")}";
    }

    /// <summary>
    /// Salva la lista di posizioni su file
    /// </summary>
    /// <param name="data">Lista di posizioni da salvare</param>
    public static void Save(List<Position> data)
    {
        //  imposta il separatore dei numeri decimali a "." (nel caso si avesse il pc in lingua che usa "," come separatore si potrebbero avere problemi, quindi lo imposta manualmente)
        System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
        customCulture.NumberFormat.NumberDecimalSeparator = ".";
        System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;


        //  controlla se il file nel path esiste
        if(File.Exists(GeneratePath(filename)))
        {
            //  se il file è presente nel path, crea una stringa contenente tutte le features e id della nota, per salvare sul file nel path

            foreach (var item in data)
            {
                var str = $"{item.Left_Hand.FF1}, {item.Left_Hand.FF2}, {item.Left_Hand.FF3}, {item.Left_Hand.FF4}, {item.Left_Hand.FF5}," +
                    $" {item.Left_Hand.NFA1}, {item.Left_Hand.NFA2}, {item.Left_Hand.NFA3}, {item.Left_Hand.NFA4}," +
                    $" {item.Right_Hand.FF1}, {item.Right_Hand.FF2}, {item.Right_Hand.FF3}, {item.Right_Hand.FF4}, {item.Right_Hand.FF5}," +
                    $" {item.Right_Hand.NFA1}, {item.Right_Hand.NFA2}, {item.Right_Hand.NFA3}, {item.Right_Hand.NFA4}," +
                    $" {item.ID}"
                    .ToString(System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));

                //  va a capo per la prossima feature da salvare
                str += Environment.NewLine;

                //  stampa la riga che si sta salvando sul file
                //Debug.Log(str);

                //  stampa la stringa str sul file, aggiungendo quindi tutte le posizioni registrate per la nota selezionata
                File.AppendAllText(GeneratePath(filename), str);
            }

            //  pulisce la lista delle posizioni, per poter registrare altre posizioni nella stessa sessione, senza dover 
            //  riavviare il programma
            _GM.list_posizioni.Clear();

            //  stampa il messaggio nella console di debug
            Debug.Log("DATASET SAVED");
        }
        else
        {
            //  se il file non è presente nel path, lo crea

            File.Create(GeneratePath(filename)).Dispose();

            //  effettua una chiamata ricorsiva per salvare sul file appena creato
            Save(data);

            //  stampa il messaggio nella console di debug
            Debug.Log("FILE CREATO");
        }
    }

    /// <summary>
    /// Carica il contenuto del file
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static string LoadFile(string name)
    {
        //  se il file è presente nel path, lo legge
        if (File.Exists(GeneratePath($"{name}")))
        {
            return File.ReadAllText(GeneratePath($"{name}"));
        }
        else
        {
            //  se il file non è presente nel path stampa un errore
            Debug.LogError("File non trovato");
            return null;
        }
    }

    /// <summary>
    /// Converte bytes in un file .py e lo scrive in GeneratePath [AppData/LocalLow]
    /// </summary>
    /// <param name="file">File da convertire</param>
    /// <param name="name">Nome con cui salvare il file convertito</param>
    public static void SavePy(byte[] file, string name)
    {
        //  imposta nome ed estensione al file da salvare
        string filename = name + ".py";
        //Debug.Log(file);
        try
        {
            //  se il file non è presente nel path, lo crea
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
                //  se il file è presente nel path, stampa il messaggio nella console di debug
                Debug.Log("FILE GIA ESISTENTE");
            }
        }
        catch (Exception ex)
        {
            Debug.Log("Exception caught in process:" + ex.ToString());
        }
    }


    /// <summary>
    /// Importa il dataset
    /// </summary>
    /// <param name="path">path del dataset da importare</param>
    /// <param name="destination">path destinazione per il dataset da importare</param>
    public static void Import(string path, string destination)
    {
        ProcessDirectory(path, destination, true);
    }

    /// <summary>
    /// Esporta il dataset
    /// </summary>
    /// <param name="path">path del dataset da esportare</param>
    /// <param name="destination">path destinazione per il dataset da esportare</param>
    public static void Export(string path, string destination)
    {
        ProcessDirectory(path, destination);
    }

    /// <summary>
    /// Processa una cartella, salvando tutto il contenuto in un'altra destinazione
    /// </summary>
    /// <param name="dir">cartella da copiare</param>
    /// <param name="destination">path destinazione per la cartella da copiare</param>
    /// <param name="setDS">True se deve essere selezionato il dataset passato in input, False altrimenti</param>
    private static void ProcessDirectory(string dir, string destination, bool setDS = false)
    {
        Directory.CreateDirectory(destination);

        string[] files = Directory.GetFiles(dir);

        foreach (var item in files)
            ProcessFile(item, destination);


        string[] dirs = Directory.GetDirectories(dir);


        var tmp = dir.Split('/').ToList().Last();

        if(setDS)
            _GM.selectedDataset = tmp;
    }

    /// <summary>
    /// Processa un file, salvandolo in un'altra destinazione
    /// </summary>
    /// <param name="file">path del file da copiare</param>
    /// <param name="destination">path destinazione per il file da copiare</param>
    private static void ProcessFile(string file, string destination)
    {
        var tmp = file.Split('/').ToList();
        var tmp1 = tmp.Last().Split('\\').ToList().Last();
        var filename = tmp1;

        string str = destination + '/' + filename;


        if (!File.Exists(str))
        {
            File.Copy(file, str);
        }
    }
}