using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Classe utilizata per la creazione dei file per le confusion matrix
/// </summary>
public class ConfusionTestingScript : MonoBehaviour
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
    [Range(0, 23)]
    [SerializeField]
    public int currentNoteId;

    // Start is called before the first frame update
    void Start()
    {
        recording_Text.text = TESTING_COUNT_DEF.ToString();
    }

    // Update is called once per frame
    void FixedUpdate() 
    {
        
    }

    /// <summary>
    /// Inizializza il numero di secondi da attendere per il l'inizio della registrazione
    /// </summary>
    int count = 3;
    /// <summary>
    /// Inizializza il numero di oggetti da salvare per ogni posizione
    /// </summary>
    int testing_count = 0;

    /// <summary>
    /// Definisce il tempo di attesa del coutdown iniziale
    /// </summary>
    const int COUNT_DEF = 3;

    /// <summary>
    /// Definisce il numero totale di oggetti da salvare per ogni posizione
    /// </summary>
    const int TESTING_COUNT_DEF = 100;

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
    /// Lista di coppie, rispettivamente: nota teorica, nota prevista
    /// </summary>
    int[,] TestingList = new int[FileUtils.matrixSize, FileUtils.matrixSize];

    /// <summary>
    /// Cambia l'id della nota corrente
    /// </summary>
    /// <param name="note_id"></param>
    public void ChangeNoteId(int note_id)
    {
        currentNoteId = note_id;

        Debug.Log(currentNoteId);
    }

    /// <summary>
    /// Aggiunge nella matrice di confusione la previsione effettuata.
    /// </summary>
    private void DataSelector()
    {
        var features = TestingScript.GetCurrentFeatures();

        var predicted = TestML.ReteNeurale(features);
        TestingList[currentNoteId, predicted] += 1; 
    }

    /// <summary>
    /// Inizia la coroutine per la registrazione delle posizioni
    /// </summary>
    public void Tester()
    {
        if(!counting_flag)
        {
            count = COUNT_DEF + 1;
            testing_count = TESTING_COUNT_DEF;

            StartCoroutine(Waiter());
            counting_flag = true;
        }
    }

    /// <summary>
    /// Salva la matrice di confusione
    /// </summary>
    public void Save()
    {
        FileUtils.Save(TestingList);
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
        if (testing_count > 0)
        {
            //  decrementa di 1 le posizioni totali rimanenti da registrare e mostra a schermo
            testing_count--;
            recording_Text.text = testing_count.ToString();
            position_Text.text = text2;

            //  effettua una pausa di 125 millisecondi
            yield return new WaitForSeconds(0.0625f);

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
        }
    }
}
