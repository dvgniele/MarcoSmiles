[System.Serializable]

///<summary>
///Incapsula una configurazione della mano sinistra e destra.
/// </summary>
public class Position
{
    public DataToStore Left_Hand { get; set; }
    public DataToStore Right_Hand { get; set; }
    public int ID { get; set; }

    public Position(DataToStore left_hand, DataToStore right_hand, int id)
    {
        this.Left_Hand = left_hand;
        this.Right_Hand = right_hand;
        this.ID = id;
    }
}
