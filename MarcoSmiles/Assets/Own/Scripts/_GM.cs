using Leap;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Collections;

/// <summary>
/// Game Master Contiene lo stato dell'applicazione.
/// </summary>
public class _GM : MonoBehaviour
{

    /// <summary>
    /// Flag necessario per sapere se il leap motion è connesso
    /// </summary>
    public static bool IsLeapConnected = false;        

    private Button playButton;                              //  Vottone preseente nel main menu per passare a scena "Play"

    public TrainingScript trainer;                          //  Istanza della classe trainer, utilizzata esclusvamente nella scena di training
    public static Hand hand_R;                              //  Dati statici della mano destra
    public static Hand hand_L;                              //  Dati statici della mano sinistra

    public ConfusionTestingScript tester;
                        
    public static List<Position> list_posizioni;            //  Viene usata solo nella scena di training per salvare nel dataset

    public static List<int> trainedNotes;                   //  Contiene una lista di indici, che rappresentano le note allenate (0-23)

    public static bool isActive = false;                    //  true se ci sono le mani, altrimenti false. lo script ProceduralAudioOscillator.cs osserva questa variabile per decidere se deve suonare 
    public static float[] current_Features;                 //  Attualmente le features sono floats, risolviamo sto problemo
    public static int indexPlayingNote;                     //  Indice della nota corrente da suonare, è letta da PCMOscillator
    public static int indexPreviousNote;                    //  Indice della nota precedentemente suonata nel fixed update precedente

    [SerializeField]
    public List<Button> listaPulsanti;                      //  Contiene una lista dei tasti del piano
    public GameObject piano;                                //  Rappresenta il piano all'interno della scena          

    //  Variabili contenenti riferimenti ai pannelli di popup
    public GameObject PopupPanel;                   
    public static GameObject ConnectLeapPanel;

    //  Interfaccia. Per sapere se è stato fatto Learn su configurazione selezionata
    public GameObject ConfLearn;                            // Gameobject contenente immagine che indica che è stato fatto Learn per la conf. selezionata. (TRAINING SCENE)                           
    public GameObject ConfNotLearn;                         // Gameobject contenente immagine che indica che NON è stato fatto Learn per la conf. selezionata. (TRAINING SCENE)                           
    public GameObject DateLatestLearning;                   // Gameobject contenente la data dell'ultimo Learn per la conf. selezionata. (TRAINING SCENE)                           

    //  Variabili animazione loading circle
    public GameObject LoadingCircle;
    public RectTransform mainIcon;
    public static float timeStep = 0.1f;                    //  Intervallo di tempo per l'animazione di caricamento
    public static float oneStepAngle = -36;                 //  Angolo di rotazione per l'animazione di caricamento
    float startTime;

    //  Testo contenente il nome della Configurazione (cartella) selezionata 
    public Text SelectedDatasetText;

    /// <summary>
    /// Enum per le scene unity esistenti nella build
    /// </summary>
    private enum SceneEnum
    {
        Mainpage,
        PlayScene,
        TrainingScene,
        TestingScene
    }
    private SceneEnum currSceneEnum;                        //  Variabile per tenere traccia della scena corrente
    private Scene currentScene;                             //  Oggetto scena, utilizzato per modificare la variabile currSceneEnum


    #region UNITY METH

    /// <summary>
    /// Chiamato quando viene inizializzato un oggetto contenente lo script _GM.cs
    /// </summary>
    private void Awake()
    {
        /*
         * In unity, possono essere caricati nella build solo determinati tipi di file. File .txt vengono copiati all'interno della cartella 
         * della build.
         * In questo modo riusciamo ad avere lo script .py (che in questo momento è un file .txt) all'interno della build.
         * Dunque, leggiamo il file .txt dalla cartella Resources, e usaando il metodo SavePy, salviamo lo script letto dal file .txt
         * in un file ad estensione .py. Questo file potrà poi essere lanciato su linea di comando.    
         */

        string nameFile = "ML";                                         //  Nome dello script python. 
        var MLFile = Resources.Load<TextAsset>("Text/" + nameFile);     //  Carica lo script dalla cartella Resources di Unity(file .txt)
        FileUtils.SavePy(MLFile.bytes, MLFile.name);                    //  Converte il file .txt in script .py

        currentScene = SceneManager.GetActiveScene();                   //  Prende la scena correntemente attiva

        
        switch (currentScene.buildIndex)                                
        {
            case (0):
                currSceneEnum = SceneEnum.Mainpage;
                break;
            case (1):
                currSceneEnum = SceneEnum.PlayScene;
                break;
            case (2):
                currSceneEnum = SceneEnum.TrainingScene;
                break;
            case (3):
                currSceneEnum = SceneEnum.TestingScene;
                break;
        }

        //  Caso in cui la scena corrente è la scena mainpage(Mainpage)
        if (currSceneEnum == SceneEnum.Mainpage)
        {

        }

        //  Caso in cui la scena corrente è la scena per suonare (PlayScene)
        if (currSceneEnum == SceneEnum.PlayScene)
        {
            TestML.Populate();                                              //  Effettua il caricamento dei file necessari per la scena PlayScene
        }

        // Se la scena corrente è la scena di training (TrainingScene)
        if (currSceneEnum == SceneEnum.TrainingScene)
        {
            if (TestML.Populate()) {                                        //  Restituisce true se trova i file weights.txt e bias.txt
                SetLearnStatus(true);                                       //  Segnala che è stato effettuato il Learning sul dataset selezionato 
                UpdateLatestLearningDate();                                 //  Aggiorna il testo contente la data dell'ultimo training effettuato sul dataset selezionato
            }
            else
                SetLearnStatus(false);                                      //  Segnala che non è stato effettuato il trianing sul dataset selezionato 
        }

        // Se la scena corrente è la scena di testing (TestingScene)
        if (currSceneEnum == SceneEnum.TestingScene)
        {
            TestML.Populate();
        }
    }


    void Start()
    {
        list_posizioni = new List<Position>();

        if (currSceneEnum == SceneEnum.Mainpage)
        {
            playButton = GameObject.Find("PlayButton").GetComponent<Button>();          //  Istanzia il pulsante PlayButton

            //  Controlla se ci sono i file necessari per passare alla scena "Play"
            try
            {
                playButton.interactable = FileUtils.CheckForDefaultFiles();
            }
            catch(Exception ex)
            {
                Debug.Log(ex.Message);
            }


            UpdateSelectedDatasetText();            //  Aggiorna il testo del dataset selezionato

        }

        if (currSceneEnum == SceneEnum.PlayScene)
        {
            ConnectLeapPanel = GameObject.Find("ConnectLeapPanel");         //  Istanzia il popup ConnectLeapPanel
        }

        if (currSceneEnum == SceneEnum.TrainingScene)
        {
            ConnectLeapPanel = GameObject.Find("ConnectLeapPanel");         //  Istanzia il popup ConnectLeapPanel
            ClosePopUp();                                                   //  Chiude il popup

            //  Il programma parte con la prima nota della tastiera selezionata
            listaPulsanti.ElementAt(0).Select();

            FileUtils.UpdateTrainedNotesList(FileUtils.filename);           //  Aggiorna la lista delle note già registrate
            UpdateButtonsKeyboard();                                        //  Aggiorna la tastiera

            startTime = Time.time;              
            LoadingCircle.SetActive(false);
        }

        if(currSceneEnum == SceneEnum.TestingScene)
        {
            ConnectLeapPanel = GameObject.Find("ConnectLeapPanel");         //  Istanzia il popup ConnectLeapPanel
            ClosePopUp();                                                   //  Chiude il popup
        }

    }
    void FixedUpdate()
    {
        if (currSceneEnum == SceneEnum.PlayScene)
        {
            if (isActive)
            {
                // Aggiorna array delle features currentFeatures in modo tale che venga calcolata la nota giusta ad ogni update 
                current_Features = TestingScript.GetCurrentFeatures();

                //salva la nota che si stava suonando nell'update precedente prima di calcolare la nuova nota
                indexPreviousNote = indexPlayingNote;                                       //  Salva in memoria l'indice dell'ultima nota suonata
                indexPlayingNote = TestML.ReteNeurale(current_Features);                    //  Rappresenta la nota che deve essere suonata

                ChangeColor(indexPreviousNote, indexPlayingNote);                           //  Cambia il colore del tasto sulla tastiera corrispondente alla nota che si sta suonando  
            }


            if (!isActive)
            {
                ResetColorNotes();                                                          //  Ripristina i colori delle note al default
            }

       
        }

        if (currSceneEnum == SceneEnum.TrainingScene)
        {
            //  Avvia l'animazione di caricamento se necessario
            if (LoadingCircle.activeSelf)
                StartCircleAnimation(); 
        }
    }

    /// <summary>
    /// Aggiorna il nome della configurazione selezionata all'interno del Menu Principale
    /// </summary>
    public void UpdateSelectedDatasetText()
    {
        SelectedDatasetText.text = "Selected Configuration: " + FileUtils.selectedDataset;
    }

    /// <summary>
    /// Aggiorna la data dell'ultimo addestramento effettuato all'interno della scena Training
    /// </summary>
    public void UpdateLatestLearningDate()
    {
        DateLatestLearning.GetComponent<Text>().text = "Latest Learning: \n " + TestML.DateLatestLearning.ToString();
    }

    #endregion








    #region Configurations Management

    /// <summary>
    /// Lanciato quando viene premuto il pulsante di training per la nota selezionata
    /// </summary>
    public void TrainButtonClick()
    {
        if (currSceneEnum == SceneEnum.TrainingScene)
            trainer.Trainer();
        else if (currSceneEnum == SceneEnum.TestingScene)
            tester.Tester();
    }

    /// <summary>
    /// Lanciato quando viene premuto il pulsante di rimozione nota da dataset per la nota selezionata
    /// </summary>
    public async void RemoveButtonClick()
    {
        LoadingCircle.SetActive(true);
        
        if (await trainer.RemoveNote())
        {
            UpdateButtonsKeyboard();
            LoadingCircle.SetActive(false);
        }
    }

    /// <summary>
    /// Elimina tutti i file nel Dataset selezionato
    /// </summary>
    public void ClearDefaultDatasetDirectory()
    {
        FileUtils.ClearDefaultDatasetDirectory();
        ResetColorNotes();
    }

    #endregion

    #region Keyboard Buttons

    /// <summary>
    /// Cambia il colore della nota da suonare, ripristinando al colore di default la nota precedentemente suonata (se necessario)
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
            //  resetta il colore di default al tasto corrispondente alla nota precedentemente suonata
            //  il colore di default è "salvato" nella variabile DisabledColor del ColorBlock del Button
            Button b_prev = listaPulsanti[id_prev];
            ColorBlock cb_prev = b_prev.colors;
            cb_prev.normalColor = cb_prev.disabledColor;
            b_prev.colors = cb_prev;

            //  evidenzia il tasto corrispondente alla nota che si sta suonando
            //  il colore che evidenzia il tasto è "salvato" nella variabile PressedColor del ColorBlock del Button
            Button b_curr = listaPulsanti[id_curr];
            ColorBlock cb_curr = b_curr.colors;
            cb_curr.normalColor = cb_curr.pressedColor;
            b_curr.colors = cb_curr;

                    

        }

    }

    /// <summary>
    /// Viene chiamato ogni volta che un tasto del pianoforte viene premuto, in modo che venga cambiato l'id
    /// della nota selezionata. Serve alla classe Trainer.
    /// </summary>
    /// <param name="sender"></param>
    public void GetClickedKey(Button sender)
    {

        var skrtino = listaPulsanti.IndexOf(listaPulsanti.FirstOrDefault(x => x.gameObject.Equals(sender.gameObject)));

        //  Cambia l'id della nota che si allenando all'interno della classe Trainer
        if(currSceneEnum == SceneEnum.TrainingScene)
            trainer.ChangeNoteId(skrtino);

        //  Serve per testing... Usa un altro script trainer (ConfusionTestingScript.cs)
        if (currSceneEnum == SceneEnum.TestingScene)
            tester.ChangeNoteId(skrtino);

    }

    /// <summary>
    /// Evidenzia tutte le note della tastiera che sono state già allenate (le note che sono presenti nel dataset selezionato e dunque nella lista trainedNotes)
    /// </summary>
    public void UpdateButtonsKeyboard()
    {
        ResetColorNotes();
        foreach (var item in trainedNotes)
        {
            Button btn = listaPulsanti[item];
            ColorBlock btn_color = btn.colors;
            btn_color.normalColor = new Color(0.13f, 1f, 0.1f, 0.3f);
            btn.colors = btn_color;

        }
    }


    /// <summary>
    /// Resetta il colore dei tasti
    /// </summary>
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
    public void NavigateToMainScene()
    {
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Effettua la navigazione alla scena di test
    /// </summary>
    public void NavigateToTestScene() => SceneManager.LoadScene(1);


    /// <summary>
    /// Effettua la navigazione alla sena di testing della matrice
    /// </summary>
    public void NavigateToTestingMatrixScene() => SceneManager.LoadScene(3);

    /// <summary>
    /// Effettua la navigazione alla scena di training
    /// </summary>
    public void NavigateToTrainingtScene() => SceneManager.LoadScene(2);

    #endregion

    #region Panels (Managing Configurations)

    /// <summary>
    /// Apre il pannello per selezionare una configurazione.
    /// </summary>
    public void OpenPanel()
    {
        PanelUtils.OpenPanel();

        Debug.Log(FileUtils.selectedDataset);
        if (currSceneEnum == SceneEnum.Mainpage)
        {
            UpdateSelectedDatasetText();
            playButton.interactable = FileUtils.CheckForDefaultFiles();
        }
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

    #endregion


    #region Popups
    /// <summary>
    /// Apre il PopUp dopo aver cliccato il bottone info
    /// </summary>
    public void OpenPopUp()
    {
        PopupPanel.SetActive(true);
    }

    /// <summary>
    /// Chiude il PopUp dopo aver cliccato il bottone info
    /// </summary>
    public void ClosePopUp()
    {
        PopupPanel.SetActive(false);
    }

    /// <summary>
    /// Mostra il popup che richiede di connettere il LeapMotion
    /// </summary>
    public static void ShowConnectLeapPopup()
    {
        ConnectLeapPanel.SetActive(true);
    }

    /// <summary>
    /// Nasconde il popup che richiede di connettere il LeapMotion
    /// </summary>
    public static void HideConnectLeapPopup()
    {
        ConnectLeapPanel.SetActive(false);
    }

    #endregion

    ///<summary>
    ///  Avvia l'animazione del caricamento
    ///</summary>
    public void StartCircleAnimation()
    {
        if (Time.time - startTime >= timeStep)
        {
            Vector3 iconAngle = mainIcon.localEulerAngles;
            iconAngle.z += oneStepAngle;

            mainIcon.localEulerAngles = iconAngle;

            startTime = Time.time;
        }
    }

    /// <summary>
    /// Visualizza la spunta vicino tasto "learn" per comunicare che per la configurazione selezionata è stato fatto precedentemente il learning
    /// </summary>
    /// <param name="state">true se è stato effettuato il learning, false altrimenti</param>
    public void SetLearnStatus(bool state)
    {
        ConfLearn.SetActive(state);
        DateLatestLearning.SetActive(state);
        ConfNotLearn.SetActive(!state);
    }
}
