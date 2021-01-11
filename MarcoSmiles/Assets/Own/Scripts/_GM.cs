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

    //private TestML testML;



    private void Awake()
    {
        TestML.Populate();
       
    }

    void Start()
    {
        currentScene = SceneManager.GetActiveScene();
        list_posizioni = new List<Position>();
        
    }

    void FixedUpdate()
    {
        if (currentScene.buildIndex == 1)
        {
            // Aggiorna array delle features currentFeatures in modo tale che venga calcolata la nota giusta ad ogni update 
            current_Features = TestingScript.GetCurrentFeatures();

            //salva la nota che si stava suonando nell'update precedente prima di calcolare la nuova nota
            indexPreviousNote = indexPlayingNote;
            indexPlayingNote = TestML.ReteNeurale(current_Features);                    //rappresenta la nota che deve essere suonata

            ChangeColor(indexPreviousNote, indexPlayingNote);
        }



        //Debug.Log("L'indice che rappresenta la nota da suonare è:  " + indexPlayingNote);
    }


    public void TrainButtonClick()
    {
        trainer.Trainer();
    }

    /* NON SO COME SI FA QUEL FATTO DELLA DOCUMENTAZIONE
        ChangeColor cambia il colore della nota da suonare e ripristina il colore di default della nota che si stava suonando in precedenza (se c'è bisogno).
        Altrimenti non fa nulla.
     */

    private void ChangeColor(int id_prev, int id_curr)
    {
        //Quando si suonano note della seconda ottava non resetta il colore del tasto. Daniele aggiusta
        if (id_curr > 11 || id_prev > 11)
        {
            return;
        }

        if (id_prev == id_curr)
        {

            if (listaPulsanti[id_curr].GetComponent<UnityEngine.UI.Image>().color == Color.red)
            {
                return;
            }
        }

        if (id_prev != id_curr)
        {
            listaPulsanti[id_prev].GetComponent<UnityEngine.UI.Image>().color = listaPulsanti[id_prev].colors.normalColor;
            listaPulsanti[id_curr].GetComponent<UnityEngine.UI.Image>().color = Color.red;
        }

    }

    #region NAVIGATION

    public void QuitGame() => Application.Quit();

    public void NavigateToMainScene() => SceneManager.LoadScene(0);

    public void NavigateToTestScene() => SceneManager.LoadScene(1);

    public void NavigateToTrainingtScene() => SceneManager.LoadScene(2);

    #endregion
}
