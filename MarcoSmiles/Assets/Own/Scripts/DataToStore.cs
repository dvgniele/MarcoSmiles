using Leap;

[System.Serializable]
///<summary>
/// Incapsula i dati relativi alla configurazione di una mano.
///</summary>
public class DataToStore
{
    /// <summary>
    /// Informazioni sulla mano relativa ai dati da salvare
    /// </summary>
    public Hand hand { get; set; }
    /// <summary>
    /// FF pollice
    /// </summary>
    public float FF1 { get; set; }
    /// <summary>
    /// FF indice
    /// </summary>
    public float FF2 { get; set; }
    /// <summary>
    /// FF medio
    /// </summary>
    public float FF3 { get; set; }
    /// <summary>
    /// FF anulare
    /// </summary>
    public float FF4 { get; set; }
    /// <summary>
    /// FF mignolo
    /// </summary>
    public float FF5 { get; set; }
    /// <summary>
    /// NFA pollice-indice
    /// </summary>
    public float NFA1 { get; set; }
    /// <summary>
    /// NFA indice-medio
    /// </summary>
    public float NFA2 { get; set; }
    /// <summary>
    /// NFA medio anulare
    /// </summary>
    public float NFA3 { get; set; }
    /// <summary>
    /// NFA anulare mignolo
    /// </summary>
    public float NFA4 { get; set; }

    /// <summary>
    /// Costruttore
    /// </summary>
    /// <param name="hand">Informazioni sulla mano relativa ai dati da salvare</param>
    /// <param name="FF1"><paramref name="FF1"/></param>
    /// <param name="FF2"><paramref name="FF2"/></param>
    /// <param name="FF3"><paramref name="FF3"/></param>
    /// <param name="FF4"><paramref name="FF4"/></param>
    /// <param name="FF5"><paramref name="FF5"/></param>
    /// <param name="NFA1"><paramref name="NFA1"/></param>
    /// <param name="NFA2"><paramref name="NFA2"/></param>
    /// <param name="NFA3"><paramref name="NFA3"/></param>
    /// <param name="NFA4"><paramref name="NFA4"/></param>
    public DataToStore(Hand hand, float FF1, float FF2, float FF3, float FF4, float FF5, float NFA1, float NFA2, float NFA3, float NFA4)
    {
        this.hand = hand;

        this.FF1 = FF1;
        this.FF2 = FF2;
        this.FF3 = FF3;
        this.FF4 = FF4;
        this.FF5 = FF5;

        this.NFA1 = NFA1;
        this.NFA2 = NFA2;
        this.NFA3 = NFA3;
        this.NFA4 = NFA4;
    }

    public override string ToString()
    {
        return base.ToString() +
            $" Hand: {hand}," +
            $" FF1: {FF1}"+
            $" FF2: {FF2}"+
            $" FF3: {FF3}"+
            $" FF4: {FF4}"+
            $" FF5: {FF5}"+
            $" NFA1: {NFA1}"+
            $" NFA2: {NFA2}"+
            $" NFA3: {NFA3}"+
            $" NFA4: {NFA4}";
    }

}
