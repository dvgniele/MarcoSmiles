using Leap;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Hosting;
using UnityEngine;
using UnityEngine.SceneManagement;

/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
 * QUESTO SCRIPT IN TEORIA è QUELLO CHE SERVE SOLO ALLA FASE DI TEST (QUANDO DI SUONA).. ANDREBBE FATTO UN GM PER OGNI SCENA (TRAINING E TESTING)
 * COSI DA GESTIRE AL MEGLIO LE COSE ALL'INTERNO DEL METODO UPDATE
 * !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
 */

public class _GM : MonoBehaviour
{
    public TrainingScript trainer;                          // viene usata solo nella scena di training per salvare nel dataset
    public static Hand hand_R;
    public static Hand hand_L;

    [Range(1,2)]
    public static int octaves;

    public static List<Position> list_posizioni;            // viene usata solo nella scena di training per salvare nel dataset

    public bool shouldPlay = false;                         //  decide se bisogna suonare IN BASE ALLA SCENA ATTIVA. true solo se è nella scena di testing

    //public static bool isActive = false;            //  se ci sono le mani, suona, altrimenti va a c'rac
    public static bool isActive = true;            //  se ci sono le mani, suona, altrimenti va a c'rac
    public static double[] current_Features;        //  attualmente le features sono floats, risolviamo sto problemo
    public static int indexPlayingNote;             //  indice della nota da suonare che è letta da PCMOscillator

    //private TestML testML;

    void Start()
    {
        list_posizioni = new List<Position>();
        //list_posizioni = FileUtils.LoadList();
        TestML.Populate();
    }

    void FixedUpdate()
    {
        /* Aggiorna array delle features currentFeatures
         * in modo tale che venga calcolata la nota giusta ad ogni update
         * */
        current_Features = TestingScript.GetCurrentFeatures();

        indexPlayingNote = TestML.ReteNeurale(current_Features);                    //rappresenta la nota che deve essere suonata

        //Debug.Log("L'indice che rappresenta la nota da suonare è:  " + indexPlayingNote);
    }


    public void TrainButtonClick()
    {
        trainer.Trainer();
    }




    #region NAVIGATION

    public void QuitGame() => Application.Quit();

    public void NavigateToMainScene() => SceneManager.LoadScene(0);

    public void NavigateToTestScene() => SceneManager.LoadScene(1);

    public void NavigateToTrainingtScene() => SceneManager.LoadScene(2);

    #endregion
}
