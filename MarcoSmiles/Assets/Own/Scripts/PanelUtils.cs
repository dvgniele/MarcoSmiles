using SFB;
using System;
using System.IO;
using System.Linq;
using UnityEngine;

/// <summary>
/// Contiene i metodi per gestire i pannelli per cambiare, importare ed esportare configurazioni di sistema.
/// </summary>
public class PanelUtils
{
    /// <summary>
    /// Apre un pannello per selezionare un dataset da esportare in una qualsiasi directory sul pc
    /// </summary>
    public static void OpenExportPanel()
    {
        //  Naviga fino alla cartella contenente tutti i datasets
        var tmp = FileUtils.GeneratePath().Split('/').ToList();
        tmp.Remove(tmp.Last());
        tmp.Remove(tmp.Last());

        var tmp_path = "";
        foreach (var item in tmp)
            tmp_path += item + '/';

        //  Apre il pannello
        var paths = StandaloneFileBrowser.OpenFolderPanel("Export Dataset", tmp_path, false);
        var path = String.Join("/", paths);

        //  Se il dataset esiste, esegue le procedure di export
        if (Directory.Exists(path))
        {
            var expPath = StandaloneFileBrowser.OpenFolderPanel("Choose the location to export to", tmp_path, false);
            var finalPath = expPath.Last() + "\\" + paths.Last().Split('\\').ToList().Last();

            if (!Directory.Exists(finalPath))
                Directory.CreateDirectory(finalPath);

            FileUtils.Export(path, finalPath);
        }
    }


    /// <summary>
    /// Apre un pannello per selezionare un dataset da importare nella cartella MyDataset
    /// </summary>
    public static void OpenImportPanel()
    {
        //  Naviga fino alla cartella contenente tutti i datasets
        var tmp = FileUtils.GeneratePath().Split('/').ToList();
        tmp.Remove(tmp.Last());
        tmp.Remove(tmp.Last());

        var tmp_path = "";
        foreach (var item in tmp)
            tmp_path += item + '/';

        //  Apre il pannello
        var paths = StandaloneFileBrowser.OpenFolderPanel("Export Dataset", tmp_path, false);
        var path = String.Join("/", paths);

        //  Se il dataset esiste, esegue le procedure di import
        if (Directory.Exists(path))
        {
            var newdirName = tmp_path + paths.Last().Split('\\').ToList().Last();

            FileUtils.Import(paths.Last(), newdirName);
        }

        //  Controlla l'esistenza dei file necessari per suonare
        FileUtils.CheckForDefaultFiles();
    }

    /// <summary>
    /// Apre un pannello per selezionare la configurazione (ed il dataset) da utilizzare nella cartella MyDataset
    /// </summary>
    public static void OpenPanel()
    {
        //  Naviga fino alla cartella contenente tutti i datasets
        var tmp = FileUtils.GeneratePath().Split('/').ToList();
        tmp.Remove(tmp.Last());
        tmp.Remove(tmp.Last());

        var tmp_path = "";
        foreach (var item in tmp)
            tmp_path += item + '/';

        //  Apre il pannello
        var paths = StandaloneFileBrowser.OpenFolderPanel("Change Dataset", tmp_path, false);
        if ( paths.Length > 0 ) {
            var path = paths.Last().Split('\\').ToList().Last();

            if (path.Length != 0)
            {
                FileUtils.selectedDataset = paths.Last().Split('\\').ToList().Last();

                //Popola matrici della rete neurale con la nuova configurazione
                TestML.Populate();
            }
        }

        //  Controlla l'esistenza dei file necessari per suonare
        FileUtils.CheckForDefaultFiles();
    }
}

