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

    [Range(0,23)]
    [SerializeField]
    public int currentNoteId;

    public int aumenta;

    private void FixedUpdate()
    {
        //ChangeNoteId(aumenta);
    }

    int count = 3;
    int record_count = 0;

    const int COUNT_DEF = 3;
    const int RECORD_COUNT_DEF = 20;

    bool counting_flag = false;
    bool recording_flag = false;

    string text1 = "Choose a position.";
    string text2 = "Hold the position.";



    public void ChangeNoteId(int note_id)
    {
        //_GM.currentNoteId = note_id;
        //currentNoteId = _GM.currentNoteId;
        currentNoteId = note_id;

        Debug.Log(currentNoteId);
    }

    private void DataSelector()
    {
        var left_hand = new DataToStore(
            _GM.hand_L,
            DatasetHandler.getFF(_GM.hand_L.Fingers[0], true),
            DatasetHandler.getFF(_GM.hand_L.Fingers[1]),
            DatasetHandler.getFF(_GM.hand_L.Fingers[2]),
            DatasetHandler.getFF(_GM.hand_L.Fingers[3]),
            DatasetHandler.getFF(_GM.hand_L.Fingers[4]),

            DatasetHandler.getNFA(_GM.hand_L.Fingers[0], _GM.hand_L.Fingers[1]),
            DatasetHandler.getNFA(_GM.hand_L.Fingers[1], _GM.hand_L.Fingers[2]),
            DatasetHandler.getNFA(_GM.hand_L.Fingers[2], _GM.hand_L.Fingers[3]),
            DatasetHandler.getNFA(_GM.hand_L.Fingers[3], _GM.hand_L.Fingers[4]));

        var right_hand = new DataToStore(
            _GM.hand_R,
            DatasetHandler.getFF(_GM.hand_R.Fingers[0], true),
            DatasetHandler.getFF(_GM.hand_R.Fingers[1]),
            DatasetHandler.getFF(_GM.hand_R.Fingers[2]),
            DatasetHandler.getFF(_GM.hand_R.Fingers[3]),
            DatasetHandler.getFF(_GM.hand_R.Fingers[4]),

            DatasetHandler.getNFA(_GM.hand_R.Fingers[0], _GM.hand_R.Fingers[1]),
            DatasetHandler.getNFA(_GM.hand_R.Fingers[1], _GM.hand_R.Fingers[2]),
            DatasetHandler.getNFA(_GM.hand_R.Fingers[2], _GM.hand_R.Fingers[3]),
            DatasetHandler.getNFA(_GM.hand_R.Fingers[3], _GM.hand_R.Fingers[4]));

        _GM.list_posizioni.Add(new Position(left_hand: left_hand, right_hand: right_hand, id: currentNoteId));
    }



    public void Trainer()
    {
        if(!counting_flag)
        {
            count = COUNT_DEF + 1;
            record_count = RECORD_COUNT_DEF;

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
            recording_Text.text = record_count.ToString();
            position_Text.text = text2;

            
            yield return new WaitForSeconds(0.125f);

            DataSelector();

            StartCoroutine(WaiterRecording());
        }
        else
        {
            recording_flag = false;

            FileUtils.Save(_GM.list_posizioni);

            /*
            foreach(var item in _GM.list_posizioni)
            {
                Debug.Log(item.ToString());
            }
            */
        }
    }


}
