using System;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class StartLearning : MonoBehaviour
{

    public void Learn()
    {
        UnityEngine.Debug.Log("Python Starting");
        UnityEngine.Debug.Log(FileUtils.PrintPath());

        // Provide arguments
        string path = FileUtils.PrintPath();
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

    /*          
               PER AVERE OUTPUT NEL DEBUG DI UNITY COPIARE QUESTO DOPO AVER ESEGUITO IL PROCESSO

                using (StreamReader reader = process.StandardOutput)
                {

                    string resline = string.Empty;
                    while (reader.Peek() >= 0)
                    {
                        resline = reader.ReadLine();
                        Debug.Log(resline);
                    }

                    using (StreamReader readerErr = process.StandardError)
                    {
                        string reslineErr = string.Empty;
                        while (readerErr.Peek() >= 0)
                        {
                            reslineErr = readerErr.ReadLine();
                            Debug.Log(reslineErr);
                        }

                    }
                }

    */


}

