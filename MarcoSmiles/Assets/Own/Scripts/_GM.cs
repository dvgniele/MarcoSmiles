using Leap;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Hosting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class _GM : MonoBehaviour
{
    public TrainingScript trainer;
    public static Hand hand_R;
    public static Hand hand_L;

    public static List<DataToStore> listaRobaccia;


    // Start is called before the first frame update
    void Start()
    {
        listaRobaccia = new List<DataToStore>();
        listaRobaccia = FileUtils.LoadList();
    }

    // Update is called once per frame
    void Update()
    {
        
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
