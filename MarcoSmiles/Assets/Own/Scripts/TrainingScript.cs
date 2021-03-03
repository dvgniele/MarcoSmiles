using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class TrainingScript : MonoBehaviour
{
    /// <summary>
    /// Referenza alla TextBox mostrata a schermo per il countdown
    /// </summary>
    public Text countDown_Text;
    /// <summary>
    /// Referenza alla TextBox mostrata a schermo per la registrazione
    /// </summary>
    public Text recording_Text;
    /// <summary>
    /// Referenza alla TextBox mostrata a schermo per il testo da mostrare
    /// </summary>
    public Text position_Text;

    /// <summary>
    /// Id nota corrente, valore possibile tra 0 e 23
    /// </summary>
    [Range(0,23)]
    [SerializeField]
    public int currentNoteId;

    public int aumenta;

    private void Start()
    {
        recording_Text.text = RECORD_COUNT_DEF.ToString();
    }

    /// <summary>
    /// Chiamato per ogni frame 
    /// </summary>
    private void FixedUpdate()
    {
        //ChangeNoteId(aumenta);
    }

    /// <summary>
    /// Inizializza il numero di secondi da attendere per il l'inizio della registrazione
    /// </summary>
    int count = 3;
    /// <summary>
    /// Inizializza il numero di oggetti da salvare per ogni posizione
    /// </summary>
    int record_count = 0;

    /// <summary>
    /// Definisce il tempo di attesa del coutdown iniziale
    /// </summary>
    const int COUNT_DEF = 3;

    /// <summary>
    /// Definisce il numero totale di oggetti da salvare per ogni posizione
    /// </summary>
    const int RECORD_COUNT_DEF = 500;

    /// <summary>
    /// Flag impostato a true se si sta effettuando il countdown, false altrimenti
    /// </summary>
    bool counting_flag = false;
    /// <summary>
    /// Flag impostato a true se si sta effettuando la registrazione delle posizioni, false altrimenti
    /// </summary>
    bool recording_flag = false;

    /// <summary>
    /// Testo mostrato in fase di "Selezione"
    /// </summary>
    string text1 = "Choose a position.";
    /// <summary>
    /// Testo mostrato in fase di "Registrazione"
    /// </summary>
    string text2 = "Hold the position.";


    /// <summary>
    /// Cambia l'id della nota corrente
    /// </summary>
    /// <param name="note_id"></param>
    public void ChangeNoteId(int note_id)
    {
        //_GM.currentNoteId = note_id;
        //currentNoteId = _GM.currentNoteId;
        currentNoteId = note_id;

        Debug.Log(currentNoteId);
    }

    /// <summary>
    /// Aggiunge nella lista di posizioni la posizione assunta nel momento della chiamata
    /// </summary>
    private void DataSelector()
    {
        //  mano sinistra
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

        //  mano destra
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

        //  aggiunge la posizione registrata alla lista delle posizioni da salvare
        _GM.list_posizioni.Add(new Position(left_hand: left_hand, right_hand: right_hand, id: currentNoteId));
    }


    public async Task<bool> RemoveNote()
    {
        await FileUtils.DeleteRowsNote(currentNoteId);
        return true;
    }



    /// <summary>
    /// Inizia la coroutine per la registrazione delle posizioni
    /// </summary>
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

    /// <summary>
    /// Countdown per la registrazione delle posizioni
    /// </summary>
    /// <returns>yield</returns>
    IEnumerator Waiter()
    {
        if (count > 0)
        {
            //  decrementa di 1 il numero di secondi da attendere prima della registrazione e lo mostra a schermo
            count--;
            countDown_Text.text = count.ToString();
            position_Text.text = text1;
            Debug.Log($"COUNTDOWN: {count.ToString()}");


            //  effettua una pausa di 1 secondo
            yield return new WaitForSeconds(1);
            
            //  inizia la coroutine per il prossimo secondo da scalare
            StartCoroutine(Waiter());
        }
        else
        {
            //  countdown terminato
            counting_flag = false;
        
            //  inizia la coroutine per la prima posizione da salvare
            StartCoroutine(WaiterRecording());
        }
    }

    /// <summary>
    /// Gestisce la Coroutine che registra la posizione assunta correntemente per record_count volte
    /// </summary>
    /// <returns>yield</returns>
    IEnumerator WaiterRecording()
    {
        if (record_count > 0)
        {
            //  decrementa di 1 le posizioni totali rimanenti da registrare e mostra a schermo
            record_count--;
            recording_Text.text = record_count.ToString();
            position_Text.text = text2;

            //  effettua una pausa di 125 millisecondi
            yield return new WaitForSeconds(0.125f);

            //  aggiunge la posizione corrente in lista
            DataSelector();

            //  inizia una nuova coroutine per la prossima posizione da salvare
            StartCoroutine(WaiterRecording());
        }
        else
        {
            //  registrazione terminata
            recording_flag = false;

            //  salva su file le posizioni registrate 
            FileUtils.Save(_GM.list_posizioni);
        }
    }


}
