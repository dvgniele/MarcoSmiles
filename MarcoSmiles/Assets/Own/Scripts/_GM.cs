﻿using System.Collections;
using System.Collections.Generic;
using System.Runtime.Hosting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class _GM : MonoBehaviour
{
    public TrainingScript trainer;

    // Start is called before the first frame update
    void Start()
    {
        
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
