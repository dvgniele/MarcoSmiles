using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


namespace DebugStuff { 
public class DebugBuild : MonoBehaviour
{
    //#if !UNITY_EDITOR
    static string myLog = "";
    private string output;
    private string stack;

    void OnEnable()
    {
        Application.logMessageReceived += Log;
            
    }

    void OnDisable()
    {
        Application.logMessageReceived -= Log;
    }

    public void Log(string logString, string stackTrace, LogType type)
    {
        output = logString;
        stack = stackTrace;
        myLog = output + "\n" + myLog;
        if (myLog.Length > 5000)
        {
            myLog = myLog.Substring(0, 4000);
        }

            using (StreamWriter outputFile = new StreamWriter(Path.Combine("C:/Users/Gianluca Mannini/Desktop", "LogBuild.txt")))  //Inserire path che si desidera
            { 
                    outputFile.WriteLine(myLog);
            }
        }

    void OnGUI()
    {
        //if (!Application.isEditor) //Do not display in editor ( or you can use the UNITY_EDITOR macro to also disable the rest)
        {
            myLog = GUI.TextArea(new Rect(10, 10, Screen.width - 10, Screen.height - 10), myLog);
        }
    }
    //#endif
}
}

