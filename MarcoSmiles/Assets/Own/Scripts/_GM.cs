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
    public TrainingScript trainer;
    public static Hand hand_R;
    public static Hand hand_L;

    public static List<Position> list_posizioni;            // ?sarebbe l'array delle features?

    public static bool isActive;                //if true sound is played (isactive is true if hands are detected) [se isactive = false, gain in PCMOscillator = 0]
    public static double[] currentFeatures;     //attualmente le features sono floats, risolviamo sto problemo
    public static int indexPlayingNote;         //indice della nota da suonare che è letta da PCMOscillator


    void Start()
    {
        list_posizioni = new List<Position>();
        //list_posizioni = FileUtils.LoadList();
    }

    void Update()
    {
        /* Aggiornare array delle features currentFeatures
         * in modo tale che venga calcolata la nota giusta ad ogni update
         * */


        var res = new double[12];        //12 è il numero delle note (classi)
        res = TestML.ReteNeurale(currentFeatures);

        indexPlayingNote = res.ToList().IndexOf(res.Max());                     //rappresenta la nota che deve essere suonata

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
