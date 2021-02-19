using SFB;
using System;
using System.IO;
using System.Linq;
using UnityEngine;

public class PanelUtils
{



    /// <summary>
    /// Apre un pannello per selezionare un dataset da esportare in una qualsiasi directory sul pc
    /// </summary>
    //[MenuItem("Export Dataset")]
    public static void OpenExportPanel()
    {
        var tmp = FileUtils.GeneratePath().Split('/').ToList();
        tmp.Remove(tmp.Last());
        tmp.Remove(tmp.Last());

        var tmp_path = "";
        foreach (var item in tmp)
            tmp_path += item + '/';


        var paths = StandaloneFileBrowser.OpenFolderPanel("Export Dataset", tmp_path, false);
        var path = String.Join("/", paths);


        if (Directory.Exists(path))
        {
            var expPath = StandaloneFileBrowser.OpenFolderPanel("Choose the location to export to", tmp_path, false);
            var finalPath = expPath.Last() + "\\" + paths.Last().Split('\\').ToList().Last();

            Debug.Log(finalPath);


            if (!Directory.Exists(finalPath))
                Directory.CreateDirectory(finalPath);

            FileUtils.Export(path, finalPath);
        }
    }


    /// <summary>
    /// Apre un pannello per selezionare un dataset da importare nella cartella MyDataset
    /// </summary>
    //[MenuItem("Import Dataset")]
    public static void OpenImportPanel()
    {
        var tmp = FileUtils.GeneratePath().Split('/').ToList();
        tmp.Remove(tmp.Last());
        tmp.Remove(tmp.Last());

        var tmp_path = "";
        foreach (var item in tmp)
            tmp_path += item + '/';

        var paths = StandaloneFileBrowser.OpenFolderPanel("Export Dataset", tmp_path, false);
        var path = String.Join("/", paths);

        if (Directory.Exists(path))
        {
            var newdirName = tmp_path + paths.Last().Split('\\').ToList().Last();

            FileUtils.Import(paths.Last(), newdirName);
        }

        FileUtils.CheckForDefaultFiles();

    }

    /// <summary>
    /// Apre un pannello per selezionare il dataset da utilizzare nella cartella MyDataset
    /// </summary>
    //[MenuItem("Select Dataset")]
    public static void OpenPanel()
    {
        var tmp = FileUtils.GeneratePath().Split('/').ToList();
        tmp.Remove(tmp.Last());
        tmp.Remove(tmp.Last());

        var tmp_path = "";
        foreach (var item in tmp)
            tmp_path += item + '/';

        var paths = StandaloneFileBrowser.OpenFolderPanel("Export Dataset", tmp_path, false);
        var path = paths.Last().Split('\\').ToList().Last();

        if (path.Length != 0)
        {
            FileUtils.selectedDataset = paths.Last().Split('\\').ToList().Last();

            //Popola matrici della rete neurale con la nuova configurazione
            TestML.Populate();
        }

        FileUtils.CheckForDefaultFiles();
    }
}

