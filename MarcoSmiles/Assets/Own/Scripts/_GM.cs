using Leap;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.IO;


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

    public static List<int> trainedNotes;

    public bool shouldPlay = false;                         //  decide se bisogna suonare IN BASE ALLA SCENA ATTIVA. true solo se è nella scena di testing


    public static bool isActive = false;                     // se ci sono le mani, suona, altrimenti no, lo script oscillator osserva questa variabile per deicdere se deve suonare 
    public static float[] current_Features;                //  attualmente le features sono floats, risolviamo sto problemo
    public static int indexPlayingNote;                     //  indice della nota da suonare che è letta da PCMOscillator
    public static int indexPreviousNote;                    //  indice della nota suonata nel fixed update precedente

    [SerializeField]
    public List<Button> listaPulsanti;
    public GameObject piano;
    //private TestML testML;
    public GameObject PopupPanel;
    public GameObject LoadingCircle;

    public static bool HasSavedNote = false;

    //  inizializza la cariabile selectedDataset con la cartella FileUtils.defaultFolder (DefaultDataset)
    //public static string selectedDataset = "DefaultDataset";

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


    #region UNITY METH

    /// <summary>
    /// Chiamato quando viene inizializzato un oggetto con lo script _GM.cs
    /// </summary>
    private void Awake()
    {
        //selectedDataset = FileUtils.defaultFolder;
        string nameFile = "ML";           //Nome del file python. 
        var MLFile = Resources.Load<TextAsset>("Text/" + nameFile);     //carica lo script dalla cartella resources (file .txt)
        FileUtils.SavePy(MLFile.bytes, MLFile.name);                    //Converte il file .txt in script .py

        if (FileUtils.CheckForDefaultFiles())
        {
            try
            {
                var playButton = GameObject.Find("TestButton").GetComponent<Button>();
                playButton.interactable = false;
            }
            catch (Exception ex)
            {

            }
        }
        else
        {
            try
            {
                var playButton = GameObject.Find("TestButton").GetComponent<Button>();
                playButton.interactable = true;
            }
            catch (Exception ex)
            {

            }
        }


        currentScene = SceneManager.GetActiveScene();

        switch (currentScene.buildIndex)
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

        if (currSceneEnum == SceneEnum.Suonah)
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

        list_posizioni = new List<Position>();

        if (currSceneEnum == SceneEnum.TrainingScene)
        {
            PopupPanel = GameObject.FindGameObjectWithTag("PopupPanel");
            LoadingCircle = GameObject.FindGameObjectWithTag("Circle");

            ClosePopUp();
            DeactivateCircle();


            //  il programma parte con la prima nota della tastiera selezionata
            listaPulsanti.ElementAt(0).Select();

            FileUtils.UpdateTrainedNotesList(FileUtils.filename);
            UpdateButtonsKeyboard();


        }
        else if (currSceneEnum == SceneEnum.Mainpage)
        {
            UpdateSelectedDatasetText();
        }


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
                ResetColorNotes();
            }
           

        }
        //Debug.Log("L'indice che rappresenta la nota da suonare è:  " + indexPlayingNote);

        if(currSceneEnum == SceneEnum.TrainingScene)
        {
            //  Debug.Log(FileUtils.selectedDataset);

            if (HasSavedNote)
            {
                FileUtils.UpdateTrainedNotesList(FileUtils.filename);
                UpdateButtonsKeyboard();

                HasSavedNote = false;
            }

        }
    }

    public static void UpdateSelectedDatasetText()
    {
        var selectedDatasetText = GameObject.Find("SelectedDatasetText").GetComponent<Text>();
        selectedDatasetText.text = "Configurazione Selezionata: " + FileUtils.selectedDataset;
    }


    #endregion


    /// <summary>
    /// Lanciato quando viene premuto il pulsante di training per la nota selezionata
    /// </summary>
    public void TrainButtonClick()
    {
        trainer.Trainer();
    }

    /// <summary>
    /// Lanciato quando viene premuto il pulsante di rimozione nota da dataset per la nota selezionata
    /// </summary>
    public void RemoveButtonClick()
    {
        //start animazione
        if(trainer.RemoveNote())
            UpdateButtonsKeyboard();
        //fine
    }

    public void ClearDefaultDatasetDirectory()
    {
        FileUtils.ClearDefaultDatasetDirectory();
    }

    #region Keyboard Buttons

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
                //trainer.ChangeNoteId(id_curr);
                return;
            }
            
        }

    }

    /// <summary>
    /// Viene chiamato ogni volta che un pulsante della tastiera (del pianoforte) viene premuto, per far sì che venga cambiato l'id
    /// della nota corrente
    /// </summary>
    /// <param name="sender"></param>
    public void GetClickedKey(Button sender)
    {
       // var previousIndexTrainNote = trainer.currentNoteId;
        //Debug.Log(previousIndexTrainNote);
        
        var skrtino = listaPulsanti.IndexOf(listaPulsanti.FirstOrDefault(x => x.gameObject.Equals(sender.gameObject)));
        //Debug.Log(skrtino);
        trainer.ChangeNoteId(skrtino);

        //Debug.Log($"{listaPulsanti[skrtino].gameObject.name}, {skrtino}");
    }

    public void UpdateButtonsKeyboard()
    {
        ResetColorNotes();

        //  evidenzia in giallo tutte le note della tastiera che sono state già allenate (le note che sono presenti nel dataset selezionato)
        foreach (var item in trainedNotes)
        {
            Button btn = listaPulsanti[item];
            ColorBlock btn_color = btn.colors;
            btn_color.normalColor = new Color(0.13f, 1f, 0.1f, 0.3f) ;
            btn.colors = btn_color;

        }
    }



    public void ResetColorNotes()
    {
        foreach (var button in listaPulsanti)
        {
            ColorBlock cb_curr = button.colors;
            cb_curr.normalColor = cb_curr.disabledColor;
            button.colors = cb_curr;
        }
    }
    #endregion


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
    public void NavigateToTestScene() => SceneManager.LoadScene(1);

    /// <summary>
    /// Effettua la navigazione alla scena di training
    /// </summary>
    public void NavigateToTrainingtScene() => SceneManager.LoadScene(2);

    #endregion

    #region Panels

    public void OpenPanel()
    {
        PanelUtils.OpenPanel();

        if(currSceneEnum == SceneEnum.Mainpage)
            UpdateSelectedDatasetText();
    }

    /// <summary>
    /// Apre un pannello per selezionare un dataset da importare nella cartella MyDataset
    /// </summary>
    public void OpenImportPanel()
    {
        PanelUtils.OpenImportPanel();

        if (currSceneEnum == SceneEnum.Mainpage)
            UpdateSelectedDatasetText();
    }

    /// <summary>
    /// Apre un pannello per selezionare un dataset da esportare in una qualsiasi directory sul pc
    /// </summary>
    public void OpenExportPanel()
    {
        PanelUtils.OpenExportPanel();
    }


    //Apre il PopUp dopo aver cliccato il bottone info
    public void OpenPopUp()
    {
        PopupPanel.SetActive(true);

        /*
        var popup = GameObject.FindGameObjectsWithTag("PopupPanel");
        Debug.Log(popup.Length);
        /*
        popup.SetActive(true);


        if (popup != null)
            popup.SetActive(true);
        */

    }

    public void ClosePopUp()
    {
        PopupPanel.SetActive(false);

        /*
        var popup = GameObject.FindGameObjectWithTag("PopupPanel");

        if (popup != null)
            popup.SetActive(false);
        */
    }

    public void DeactivateCircle()
    {
        LoadingCircle.SetActive(false);
    }


    #endregion


}
