using Leap;

[System.Serializable]
///<summary>
/// Incapsula i dati relativi alla configurazione di una mano.
///</summary>
public class DataToStore
{
    public Hand hand { get; set; }
    public float FF1 { get; set; }
    public float FF2 { get; set; }
    public float FF3 { get; set; }
    public float FF4 { get; set; }
    public float FF5 { get; set; }
    public float NFA1 { get; set; }
    public float NFA2 { get; set; }
    public float NFA3 { get; set; }
    public float NFA4 { get; set; }

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
