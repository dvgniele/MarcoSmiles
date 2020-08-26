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
