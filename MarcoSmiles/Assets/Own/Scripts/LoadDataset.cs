using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;

public class LoadDataset : MonoBehaviour
{
    [MenuItem("Example/Overwrite File")]
    static void OpenPanel()
    {
        EditorUtility.DisplayDialog("Select Dataset Folder", "Select the Dataset folder you prefer.", "OK.");

        string path = EditorUtility.OpenFilePanel("Ma che ooooh", "", "*");
        if(path.Length != 0)
        {
            Debug.Log(path);
        }

    }


}
