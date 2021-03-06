﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Classe per la gestione dei file
/// </summary>
public static class FileUtils
{

    /// <summary>
    /// Path per AppData
    /// </summary>
    static string path = Application.persistentDataPath;

    /// <summary>
    /// Nome del dataset da utilizzare
    /// </summary>
    public static string filename = "marcosmiles_dataset.csv";

    /// <summary>
    /// Nome del file per la confusion matrix
    /// </summary>
    public static string confusiongrid_filename = "confusion_grid_data.csv";

    /// <summary>
    /// Nome della cartella contenente i datasets importati
    /// </summary>
    static string folderName = "MyDatasets";

    /// <summary>
    /// Nome Dataset
    /// </summary>
    public static string defaultFolder = "DefaultDataset";

    /// <summary>
    /// Dataset selezionato
    /// </summary>
    public static string selectedDataset = defaultFolder;

    /// <summary>
    /// Dimensione della matrice di confusione
    /// </summary>
    public static int matrixSize = 24;

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
    public static string GeneratePath(string filename)
    {
        return $"{path}/{folderName}/{selectedDataset}/{filename}";
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
        return $"{path}/{folderName}/{folder}/{filename}";
    }

    /// <summary>
    /// Prende il path per il datasaet. Versione public di generatepath. [(è davvero necessaria???? o basta mettere generate path a public?)]
    /// </summary>
    /// <returns>Ritorna il path per il dataset</returns>
    public static string GeneratePath()
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
        //  Imposta il separatore dei numeri decimali a "." (nel caso si avesse il pc in lingua che usa "," come separatore si potrebbero avere problemi, quindi lo imposta manualmente)
        System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
        customCulture.NumberFormat.NumberDecimalSeparator = ".";
        System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;


        //  Controlla se il file nel path esiste
        if(File.Exists(GeneratePath(filename)))
        {
            //  Se il file è presente nel path, crea una stringa contenente tutte le features e id della nota, per salvare sul file nel path

            foreach (var item in data)
            {
                var str = $"{item.Left_Hand.FF1}, {item.Left_Hand.FF2}, {item.Left_Hand.FF3}, {item.Left_Hand.FF4}, {item.Left_Hand.FF5}," +
                    $" {item.Left_Hand.NFA1}, {item.Left_Hand.NFA2}, {item.Left_Hand.NFA3}, {item.Left_Hand.NFA4}," +
                    $" {item.Right_Hand.FF1}, {item.Right_Hand.FF2}, {item.Right_Hand.FF3}, {item.Right_Hand.FF4}, {item.Right_Hand.FF5}," +
                    $" {item.Right_Hand.NFA1}, {item.Right_Hand.NFA2}, {item.Right_Hand.NFA3}, {item.Right_Hand.NFA4}," +
                    $" {item.ID}"
                    .ToString(System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));

                //  Va a capo per la prossima feature da salvare
                str += Environment.NewLine;

                //  Stampa la stringa str sul file, aggiungendo quindi tutte le posizioni registrate per la nota selezionata
                File.AppendAllText(GeneratePath(filename), str);
            }

            //  Pulisce la lista delle posizioni, per poter registrare altre posizioni nella stessa sessione, senza dover Riavviare il programma
            _GM.list_posizioni.Clear();

            //  stampa il messaggio nella console di debug
            Debug.Log("DATASET SAVED");
        }
        else
        {
            //  IL FILE NON ESISTE
            //  crea il file nel path appropriato
            File.Create(GeneratePath(filename)).Dispose();

            //  Effettua una chiamata ricorsiva per salvare sul file appena creato
            Save(data);

            //  stampa il messaggio nella console di debug
            Debug.Log("FILE CREATO");
        }
    }

    /// <summary>
    /// Salva la confusion list su file appropriato
    /// </summary>
    /// <param name="confusionList"></param>
    public static void Save(int[,] confusionList)
    {
        //  Se il file esiste, vi scrive la confusion list
        if(File.Exists(GeneratePath(confusiongrid_filename)))
        {
            var str = "";
            for(int i = 0; i < matrixSize; i++)
            {
                for(int j = 0; j < matrixSize; j++)
                {
                    str += confusionList[i, j];

                    if (j != matrixSize-1)
                        str += ", ";
                }
                str += Environment.NewLine;
            }

            File.WriteAllText(GeneratePath(confusiongrid_filename), str);

            //  stampa il messaggio nella console di debug
            Debug.Log("CONFUSION MATRIX SAVED");

        }
        else
        {
            //  IL FILE NON ESISTE
            File.Create(GeneratePath(confusiongrid_filename)).Dispose();

            //  Effettua una chiamata ricorsiva per salvare sul file appena creato
            Save(confusionList);

            //  stampa il messaggio nella console di debug
            Debug.Log("FILE CREATO");
        }
    }


    /// <summary>
    /// Carica il contenuto del file
    /// </summary>
    /// <param name="name">Filename</param>
    /// <returns></returns>
    public static string LoadFile(string name)
    {
        //  Se il file è presente nel path, lo legge
        if (File.Exists(GeneratePath($"{name}")))
        {
            return File.ReadAllText(GeneratePath($"{name}"));
        }
        else
        {
            //se il file non è presente nel path stampa un errore
            return null;
        }
    }
    /// <summary>
    /// Aggiorna la variabile trainedNotes, contenente tute le note allenate (che sono presenti nel dataset)
    /// </summary>
    /// <param name="filename"></param>
    public static void UpdateTrainedNotesList(string filename)
    {
        var id_list = new List<int>();

        var txt = LoadFile(filename);

        if(txt != null)
        {
            var rows = txt.Split('\n').Select(tag => tag.Trim()).Where(tag => !string.IsNullOrEmpty(tag));          //trim elimina le entrate vuote

            foreach (var item in rows)
            {
                var tmp = int.Parse(item.Split(',').Last());        //  tmp = ultimo elemento della riga. sappiamo che l'ultimo elemento è l'ID

                if (!id_list.Any(x => id_list.Contains(tmp)))       //  se la lista degli ID non contiene tmp
                    id_list.Add(tmp);                               //  aggiungi tmp alla lista degli ID
            }
        }

        //  Aggiorna la lista delle note registrate
        _GM.trainedNotes = id_list;
    }

    /// <summary>
    /// Cancella la nota dal dataset
    /// </summary>
    /// <param name="note">Nota da cancellare</param>
    public static Task DeleteRowsNote(int note)
    {
        return Task.Run(() =>
        {
            var filePath = GeneratePath(filename);
            var txt = LoadFile(filename);

            var rows = txt.Split('\n').Select(tag => tag.Trim()).Where(tag => !string.IsNullOrEmpty(tag));      //  trim elimina le entrate vuote

            //  Toglie tutte le righe dal file del dataset
            File.WriteAllText(filePath, "");

            //  Scrive sul file del dataset solo le note da mantenere
            foreach (var row in rows)
            {
                int tmp_id = int.Parse(row.Split(',').Last());
                if (tmp_id != note)             //  Se l'id a fine riga è diverso dall'id della nota da cancellare, aggiunge sul file
                {
                    var actualRow = "";
                    actualRow = row + "\n";
                    File.AppendAllText(filePath, actualRow);
                }
            }

            //  Aggiorna la lista delle note registrate
            UpdateTrainedNotesList(filename);
        }
        );
    }


    /// <summary>
    /// Salva file di testo
    /// </summary>
    /// <param name="txt"></param>
    public static void SaveTxt(string txt)
    {
        var filePath = GeneratePath(filename);
        try
        {
            //  se il file non è presente nel path, lo crea

            using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                //Debug.Log(file);                  
                File.AppendAllText(filePath, txt);
            }
        }
        catch (Exception ex)
        {
            Debug.Log("Exception caught in process:" + ex.ToString());
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

        string filePath = $"{path}/{folderName}/{filename}";
        //Debug.Log(file);
        try
        {
            //  se il file non è presente nel path, lo crea
            if (!File.Exists(filePath))
            {
                using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
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

    #region Import/Export

    /// <summary>
    /// Importa il dataset
    /// </summary>
    /// <param name="path">Path del dataset da importare</param>
    /// <param name="destination">Path destinazione per il dataset da importare</param>
    public static void Import(string path, string destination)
    {
        ProcessDirectory(path, destination, true);
    }

    /// <summary>
    /// Esporta il dataset
    /// </summary>
    /// <param name="path">Path del dataset da esportare</param>
    /// <param name="destination">Path destinazione per il dataset da esportare</param>
    public static void Export(string path, string destination)
    {
        ProcessDirectory(path, destination);
    }

    #endregion

    #region Dataset Folder/Files

    /// <summary>
    /// Processa una cartella, salvando tutto il contenuto in un'altra destinazione
    /// </summary>
    /// <param name="dir">Cartella da copiare</param>
    /// <param name="destination">Path destinazione per la cartella da copiare</param>
    /// <param name="setDS">true se deve essere selezionato il dataset passato in input, false altrimenti</param>
    private static void ProcessDirectory(string dir, string destination, bool setDS = false)
    {
        Debug.Log(dir);

        Directory.CreateDirectory(destination);

        string[] files = Directory.GetFiles(dir);

        foreach (var item in files)
            ProcessFile(item, destination);


        string[] dirs = Directory.GetDirectories(dir);


        var tmp = dir.Split('/').ToList().Last();

        if(setDS)
            selectedDataset = tmp;
    }

    /// <summary>
    /// Processa un file, salvandolo in un'altra destinazione
    /// </summary>
    /// <param name="file">Path del file da copiare</param>
    /// <param name="destination">Path destinazione per il file da copiare</param>
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

    #endregion


    /// <summary>
    /// Controlla che ci siano tutti i file necessari per entrare nella sessione di play
    /// </summary>
    /// <returns>true se può suonare, false altrimenti</returns>
    public static bool CheckForDefaultFiles()
    {
        if (!Directory.Exists(GeneratePath()))
        {
            Directory.CreateDirectory(GeneratePath());     
            return false;
        }
        else
        {
            if(!File.Exists(GeneratePath("ML.py")))
                File.Copy($"{path}/{folderName}/ML.py",GeneratePath("ML.py"));
            
            if (!File.Exists(GeneratePath("bias_out.txt")) || !File.Exists(GeneratePath("weights_out.txt")))
                return false;   
        }
        return true;
    }

    /// <summary>
    /// Elimina tutti i file nel Dataset selezionato
    /// </summary>
    public static void ClearDefaultDatasetDirectory()
    {
        if(Directory.Exists(GeneratePath()))
        {
            var dir = new DirectoryInfo(GeneratePath());

            foreach(var file in dir.EnumerateFiles())
            {
                file.Delete();
            }

            foreach(var folder in dir.EnumerateDirectories())
            {
                folder.Delete(true);
            }
        }

        string nameFile = "ML";                                         //  Nome del file python. 
        var MLFile = Resources.Load<TextAsset>("Text/" + nameFile);     //  Carica lo script dalla cartella resources (file .txt)
        SavePy(MLFile.bytes, MLFile.name);                              //  Converte il file .txt in script .py

        UpdateTrainedNotesList(filename);
    }
}