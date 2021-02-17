using Leap;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


/*__________!Ci Converrebbe fare una classe contenente tutte le costanti,  contenente ad esempio il numero delle note etc....!___________*/



public class _GM : MonoBehaviour
{
    private Scene currentScene;

    public TrainingScript trainer;                          // viene usata solo nella scena di training per salvare nel dataset
    public static Hand hand_R;
    public static Hand hand_L;

    //public static int currentNoteId;

    [Range(1, 2)]
    public static int octaves;

    public static List<Position> list_posizioni;            // viene usata solo nella scena di training per salvare nel dataset

    public bool shouldPlay = false;                         //  decide se bisogna suonare IN BASE ALLA SCENA ATTIVA. true solo se è nella scena di testing


    public static bool isActive = false;                     // se ci sono le mani, suona, altrimenti no, lo script oscillator osserva questa variabile per deicdere se deve suonare 
    public static double[] current_Features;                //  attualmente le features sono floats, risolviamo sto problemo
    public static int indexPlayingNote;                     //  indice della nota da suonare che è letta da PCMOscillator
    public static int indexPreviousNote;                    //  indice della nota suonata nel fixed update precedente

    [SerializeField]
    public List<Button> listaPulsanti;
    public GameObject piano;
    //private TestML testML;


    //inizializza la cariabile selectedDataset con la cartella FileUtils.defaultFolder (DefaultDataset)
    public static string selectedDataset = "DefaultDataset";

    /// <summary>
    /// Enum per le scene unity esistenti
    /// </summary>
    private enum SceneEnum
    {
        Mainpage,
        Suonah,
        TrainingScene
    }
    private SceneEnum currSceneEnum;

    /// <summary>
    /// Chiamato quando viene inizializzato un oggetto con lo script _GM.cs
    /// </summary>
    private void Awake()
    {
        selectedDataset = FileUtils.defaultFolder;

        currentScene = SceneManager.GetActiveScene();

        switch(currentScene.buildIndex)
        {
            case (0):
                currSceneEnum = SceneEnum.Mainpage;
                break;
            case (1):
                currSceneEnum = SceneEnum.Suonah;
                break;
            case (2):
                currSceneEnum = SceneEnum.TrainingScene;
                break;
        }

        if (currentScene.buildIndex == 1)
        {
            TestML.Populate();
        }
    }

    void Start()
    {
        /*
         * In unity, possono essere caricati nella build solo determinati tipi di file. File .txt vengono copiati all'interno della cartella 
         * della build.
         * In questo modo riusciamo ad avere lo script .py (che in questo momento è un file .txt) all'interno della build.
         * Dunque, leggiamo il file .txt dalla cartella Resources, e usaando il metodo SavePy, salviamo lo script letto dal file .txt
         * in un file ad estensione .py. Questo file potrà poi essere lanciato su linea di comando.    
         */

        string nameFile = "ML";           //Nome del file python. 
        var MLFile = Resources.Load<TextAsset>("Text/" + nameFile);     //carica lo script dalla cartella resources (file .txt)
        FileUtils.SavePy(MLFile.bytes, MLFile.name);                    //Converte il file .txt in script .py

        list_posizioni = new List<Position>();

    }

    void FixedUpdate()
    {
        if (currSceneEnum == SceneEnum.Suonah)
        {
            if(isActive)
            {
                // Aggiorna array delle features currentFeatures in modo tale che venga calcolata la nota giusta ad ogni update 
                current_Features = TestingScript.GetCurrentFeatures();

                //salva la nota che si stava suonando nell'update precedente prima di calcolare la nuova nota
                indexPreviousNote = indexPlayingNote;
                indexPlayingNote = TestML.ReteNeurale(current_Features);                    //rappresenta la nota che deve essere suonata

                ChangeColor(indexPreviousNote, indexPlayingNote);
            }


            if (!isActive)
            {
                //Se non ci sono le mani ristabilisci colore di default dell'ultima nota suonata
                Button b_curr = listaPulsanti[indexPlayingNote];
                ColorBlock cb_curr = b_curr.colors;
                cb_curr.normalColor = cb_curr.disabledColor;
                b_curr.colors = cb_curr;
            }
           

        }
        //Debug.Log("L'indice che rappresenta la nota da suonare è:  " + indexPlayingNote);

        if(currSceneEnum == SceneEnum.TrainingScene)
        {

        }
    }


    /// <summary>
    /// Lanciato quando viene premuto il pulsante di training
    /// </summary>
    public void TrainButtonClick()
    {
        trainer.Trainer();
    }

    /// <summary>
    /// Cambia il colore della nota da suonare, ripristinando al colore di dafault la nota precedentemente premuta (se necessario)
    /// Se la nota da suonare è la stessa della precedente non cambia nulla
    /// </summary>
    /// <param name="id_prev">id nota precedenteme</param>
    /// <param name="id_curr">id nota da suonare</param>
    private void ChangeColor(int id_prev, int id_curr)
    {
        if (id_prev == id_curr)
        {
            
           // return;
        }
        else
        {
            //resetta il colore di default al tasto corrispondente alla nota precedente
            //il colore di default è "salvato" nella variabile DisabledColor del ColorBlock del Button
            Button b_prev = listaPulsanti[id_prev];
            ColorBlock cb_prev = b_prev.colors;
            cb_prev.normalColor = cb_prev.disabledColor;
            b_prev.colors = cb_prev;

            //evidenzia il tasto corrispondente alla nota che si sta suonando
            //il colore che evidenzia il tasto è "salvato" nella variabile PressedColor del ColorBlock del Button
            Button b_curr = listaPulsanti[id_curr];
            ColorBlock cb_curr = b_curr.colors;
            cb_curr.normalColor = cb_curr.pressedColor;
            b_curr.colors = cb_curr;


            //  se è la scena di training
            if(currSceneEnum == SceneEnum.TrainingScene)
            {
                //  cambia l'id della nota nel trainer
                trainer.ChangeNoteId(id_curr);
            }
            
        }

    }


    private void HighlightNote(int id_prev, int id_curr)
    {
    
    }

        /// <summary>
        /// Viene chiamato ogni volta che un pulsante della tastiera (del pianoforte) viene premuto, per far sì che venga cambiato l'id
        /// della nota corrente
        /// </summary>
        /// <param name="sender"></param>
        public void GetClickedKey(Button sender)
    {
        var skrtino = listaPulsanti.IndexOf(listaPulsanti.FirstOrDefault(x => x.gameObject.Equals(sender.gameObject)));
        trainer.ChangeNoteId(skrtino);

        //Debug.Log($"{listaPulsanti[skrtino].gameObject.name}, {skrtino}");
    }

    #region NAVIGATION

    /// <summary>
    /// Chiude l'applicazione
    /// </summary>
    public void QuitGame() => Application.Quit();

    /// <summary>
    /// Effettua la navigazione alla scena principale
    /// </summary>
    public void NavigateToMainScene() => SceneManager.LoadScene(0);

    /// <summary>
    /// Effettua la navigazione alla scena di test
    /// </summary>
    public void NavigateToTestScene()
    {
        if(selectedDataset == FileUtils.defaultFolder)
            OpenPanel();

        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// Effettua la navigazione alla scena di training
    /// </summary>
    public void NavigateToTrainingtScene() => SceneManager.LoadScene(2);

    #endregion

    /*
    /// <summary>
    /// Apre un pannello per selezionare il dataset da utilizzare nella cartella MyDataset
    /// </summary>
    [MenuItem("Select Dataset")]
    public void OpenPanel()
    {
        //EditorUtility.DisplayDialog("Select Dataset Folder", "Select the Dataset folder you prefer.", "OK.");
        
        string path = EditorUtility.OpenFolderPanel("Select Dataset", $"{FileUtils.PrintPath()}", "");
        if (path.Length != 0)
        {
            selectedDataset = path.Split('/').Last();
        }
    }
    */

    public void OpenPanel()
    {
        PanelUtils.OpenPanel();
    }


    /*
    /// <summary>
    /// Apre un pannello per selezionare un dataset da importare nella cartella MyDataset
    /// </summary>
    [MenuItem("Import Dataset")]
    public void OpenImportPanel()
    {
        var tmp = FileUtils.PrintPath().Split('/').ToList();
        tmp.Remove(tmp.Last());
        tmp.Remove(tmp.Last());

        var tmp_path = "";
        foreach (var item in tmp)
            tmp_path += item + '/';

        var path = EditorUtility.OpenFolderPanel("Import Dataset", tmp_path, FileUtils.PrintPath());

        if(Directory.Exists(path))
        {
            var newdirName = tmp_path + path.Split('/').ToList().Last();

            FileUtils.Import(path, newdirName);
        }

    }
    */

    /// <summary>
    /// Apre un pannello per selezionare un dataset da importare nella cartella MyDataset
    /// </summary>
    public void OpenImportPanel()
    {
        PanelUtils.OpenImportPanel();
    }

    /*
    /// <summary>
    /// Apre un pannello per selezionare un dataset da esportare in una qualsiasi directory sul pc
    /// </summary>
    [MenuItem("Export Dataset")]
    public void OpenExportPanel()
    {
        var tmp = FileUtils.PrintPath().Split('/').ToList();
        tmp.Remove(tmp.Last());
        tmp.Remove(tmp.Last());

        var tmp_path = "";
        foreach (var item in tmp)
            tmp_path += item + '/';

        var path = EditorUtility.OpenFolderPanel("Export Dataset", tmp_path, FileUtils.PrintPath());

        if (Directory.Exists(path))
        {
            var expPath = EditorUtility.OpenFolderPanel("Choose the location to export to", tmp_path, "");
            expPath += "/" + path.Split('/').ToList().Last();

            Debug.Log(expPath);

            if(!Directory.Exists(expPath))
                Directory.CreateDirectory(expPath);

            FileUtils.Export(path, expPath);
        }
    }
    */

    /// <summary>
    /// Apre un pannello per selezionare un dataset da esportare in una qualsiasi directory sul pc
    /// </summary>
    public void OpenExportPanel()
    {
        PanelUtils.OpenExportPanel();
    }



}
