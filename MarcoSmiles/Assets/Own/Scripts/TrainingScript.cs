using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TrainingScript : MonoBehaviour
{
    public Text countDown_Text;
    public Text recording_Text;
    public Text position_Text;

    int count = 3;
    int record_count = 5;

    bool counting_flag = false;
    bool recording_flag = false;

    string text1 = "Choose a position.";
    string text2 = "Hold the position.";



    /*
    
        GEPPA LAVORA IN QUESTA

    */
    private void FUNZIONE_DI_GEPAO()
    {
        var skrt = new DataToStore(
            _GM.hand_R,
            DatasetHandler.getFF(_GM.hand_R.Fingers[0]),
            DatasetHandler.getFF(_GM.hand_R.Fingers[1]),
            DatasetHandler.getFF(_GM.hand_R.Fingers[2]),
            DatasetHandler.getFF(_GM.hand_R.Fingers[3]),
            DatasetHandler.getFF(_GM.hand_R.Fingers[4]),

            DatasetHandler.getNFA(_GM.hand_R.Fingers[0], _GM.hand_R.Fingers[1]),
            DatasetHandler.getNFA(_GM.hand_R.Fingers[1], _GM.hand_R.Fingers[2]),
            DatasetHandler.getNFA(_GM.hand_R.Fingers[2], _GM.hand_R.Fingers[3]),
            DatasetHandler.getNFA(_GM.hand_R.Fingers[3], _GM.hand_R.Fingers[4]));

        FileUtils.Save(skrt);
        Debug.Log(skrt.ToString());


        var skrt2 = FileUtils.Load();
        Debug.Log("load" + skrt2.ToString());
    }


    private void FUNZIONE_DI_GEPAO_2()
    {
        var skrt = new DataToStore(
            _GM.hand_R,
            DatasetHandler.getFF(_GM.hand_R.Fingers[0]),
            DatasetHandler.getFF(_GM.hand_R.Fingers[1]),
            DatasetHandler.getFF(_GM.hand_R.Fingers[2]),
            DatasetHandler.getFF(_GM.hand_R.Fingers[3]),
            DatasetHandler.getFF(_GM.hand_R.Fingers[4]),

            DatasetHandler.getNFA(_GM.hand_R.Fingers[0], _GM.hand_R.Fingers[1]),
            DatasetHandler.getNFA(_GM.hand_R.Fingers[1], _GM.hand_R.Fingers[2]),
            DatasetHandler.getNFA(_GM.hand_R.Fingers[2], _GM.hand_R.Fingers[3]),
            DatasetHandler.getNFA(_GM.hand_R.Fingers[3], _GM.hand_R.Fingers[4]));

            skrt.ID = _GM.listaRobaccia.Count;

        _GM.listaRobaccia.Add(skrt);
    }









    public void Trainer()
    {
        if(!counting_flag)
        {
            count = 4;

            StartCoroutine(Waiter());
            counting_flag = true;
        }
    }

    IEnumerator Waiter()
    {
        if (count > 0)
        {
            count--;
            countDown_Text.text = count.ToString();
            position_Text.text = text1;

            yield return new WaitForSeconds(1);
            StartCoroutine(Waiter());
        }
        else
        {
            counting_flag = false;

            StartCoroutine(WaiterRecording());
        }

    }

    IEnumerator WaiterRecording()
    {
        if (record_count > 0)
        {
            record_count--;
            recording_Text.text = count.ToString();
            position_Text.text = text1;


            yield return new WaitForSeconds(1f);

            //try
            //{
            //    FUNZIONE_DI_GEPAO_2();
            //}
            //catch(Exception ex)
            //{
            //    Debug.Log($"SKKKKKKKKRT: {ex.Message}");
            //}

            FUNZIONE_DI_GEPAO_2();


            StartCoroutine(Waiter());
        }
        else
        {
            //
            //      DA SISTEMARE!!!!!!11!!1!11!!11!!!!1!  carica doppie robe
            //


            recording_flag = false;

            FileUtils.Save(_GM.listaRobaccia);

            var skrt2 = FileUtils.LoadList();

            Debug.Log($"lista attuale: {skrt2.Count.ToString()}");
            Debug.Log($"lista caricata: {_GM.listaRobaccia.Count.ToString()}");

            foreach(var item in _GM.listaRobaccia)
            {
                Debug.Log(item.ToString());
            }
        }
    }


}
