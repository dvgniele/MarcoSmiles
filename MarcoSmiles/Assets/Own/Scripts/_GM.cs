using Leap;
using System.Collections;
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

    //public static bool isActive = false;            //  se ci sono le mani, suona, altrimenti va a c'rac

    public static bool isActive = true;            //  se ci sono le mani, suona, altrimenti va a c'rac
    public static double[] current_Features;        //  attualmente le features sono floats, risolviamo sto problemo
    public static int indexPlayingNote;             //  indice della nota da suonare che è letta da PCMOscillator
    public static int indexPreviousNote;             //  indice della nota suonata nel fixed update precedente

    [SerializeField]
    public List<Button> listaPulsanti;
    public GameObject piano;
    //private TestML testML;

    private  enum SceneEnum
    {
        Mainpage,
        Suonah,
        TrainingScene
    }

    private SceneEnum currSceneEnum;

    private void Awake()
    {
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
        string nameFile = "ML";           //Nome del file python. 
        var MLFile = Resources.Load<TextAsset>("Text/" + nameFile);     //carica lo script dalla cartella resources (file .txt)
        FileUtils.SavePy(MLFile.bytes, MLFile.name);                    //Converte il file .txt in script .py

        list_posizioni = new List<Position>();

    }

    void FixedUpdate()
    {
        if (currSceneEnum == SceneEnum.Suonah)
        {
            // Aggiorna array delle features currentFeatures in modo tale che venga calcolata la nota giusta ad ogni update 
            current_Features = TestingScript.GetCurrentFeatures();

            //salva la nota che si stava suonando nell'update precedente prima di calcolare la nuova nota
            indexPreviousNote = indexPlayingNote;
            indexPlayingNote = TestML.ReteNeurale(current_Features);                    //rappresenta la nota che deve essere suonata

            ChangeColor(indexPreviousNote, indexPlayingNote);
        }
        //Debug.Log("L'indice che rappresenta la nota da suonare è:  " + indexPlayingNote);

        if(currSceneEnum == SceneEnum.TrainingScene)
        {

        }
    }


    public void TrainButtonClick()
    {
        trainer.Trainer();
    }

    /* NON SO COME SI FA QUEL FATTO DELLA DOCUMENTAZIONE
        ChangeColor cambia il colore della nota da suonare e ripristina il colore di default della nota che si stava suonando in precedenza (se c'è bisogno).
        Altrimenti non fa nulla (caso in cui la nota precedentemente suonata è uguale a quella che si sta suonando adesso).
     */

    private void ChangeColor(int id_prev, int id_curr)
    {
        if (id_prev == id_curr)
        {
            return;
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

    public void GetClickedKey(Button sender)
    {
        var skrtino = listaPulsanti.IndexOf(listaPulsanti.FirstOrDefault(x => x.gameObject.Equals(sender.gameObject)));
        trainer.ChangeNoteId(skrtino);

        //Debug.Log($"{listaPulsanti[skrtino].gameObject.name}, {skrtino}");
    }

    #region NAVIGATION

    public void QuitGame() => Application.Quit();

    public void NavigateToMainScene() => SceneManager.LoadScene(0);

    public void NavigateToTestScene() => SceneManager.LoadScene(1);

    public void NavigateToTrainingtScene() => SceneManager.LoadScene(2);

    #endregion
}
