using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrainingScript : MonoBehaviour
{
    public Text countDown_Text;
    int count = 3;

    bool counting_flag = false;


    /*
    
        GEPPA LAVORA IN QUESTA

    */
    private void FUNZIONE_DI_GEPAO()
    {
        var skrt = new DataToStore(DatasetHandler.getFF(_GM.hand_R.Fingers[1]), DatasetHandler.getNFA(_GM.hand_R.Fingers[1], _GM.hand_R.Fingers[2]));

        FileUtils.Save(skrt);
        Debug.Log(skrt.ToString());

        var skrt2 = FileUtils.Load();
        Debug.Log("load" + skrt2.ToString());
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

            yield return new WaitForSeconds(1);
            StartCoroutine(Waiter());
        }
        else
        {
            counting_flag = false;

            FUNZIONE_DI_GEPAO();
        }

    }
}
