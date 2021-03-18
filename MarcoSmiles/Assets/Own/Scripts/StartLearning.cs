using System;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

/// <summary>
/// Classe che permette di lanciare un file py su linea di comando, apre la bash di windows
/// </summary>
public class StartLearning : MonoBehaviour
{

    /// <summary>
    /// Avvia lo script ML,py su linea di comando, avvia quindi il learning sul dataset e la configurazione selezionata
    /// </summary>
    public void Learn()
    {
        Debug.Log("Python Starting");
        Debug.Log(FileUtils.GeneratePath());

        // Provide arguments
        string path = FileUtils.GeneratePath();
        string script = @"ML.py";        //Nome del file .py

        ProcessStartInfo pythonInfo = new ProcessStartInfo();

        pythonInfo.WorkingDirectory = path;
        pythonInfo.FileName = @"python";
        pythonInfo.Arguments = $"{script}";  // nome del file da lanciare e evenutali argomenti 
                                             // se si vuole passare un path bisogna aggiungere le " a inizio e fine, altrimenti cerca di lanciare quel path
        pythonInfo.CreateNoWindow = false;
        pythonInfo.UseShellExecute = false;

        // pythonInfo.UseShellExecute = false;       //per ottenere stream di input,  output error, e scriverli nel log di unity (serve a testare)
        //pythonInfo.UseShellExecute = true;         //metti a true quando finisci di testare cose, cosi da non fermare unity quando questo processo viene eseguito
        /*
         //fa il redirect degli stream di input, output e error. Funziona solo se UseShellExecute = false
        pythonInfo.RedirectStandardOutput = true;
        pythonInfo.RedirectStandardInput = true;
        pythonInfo.RedirectStandardError = true;
        */
        UnityEngine.Debug.Log("Python Starting");

        try
        {
            using (Process process = Process.Start(pythonInfo))
            {
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }

        UnityEngine.Debug.Log("Python Has finished!");

    }

}

