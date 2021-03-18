[System.Serializable]

/// <summary>
/// Incapsula una configurazione della mano sinistra e destra.
/// </summary>
public class Position
{
    /// <summary>
    /// Informazioni sulla mano sinistra
    /// </summary>
    public DataToStore Left_Hand { get; set; }
    /// <summary>
    /// Informazioni sulla mano destra
    /// </summary>
    public DataToStore Right_Hand { get; set; }
    /// <summary>
    /// Informazioni sull'id della posizione
    /// </summary>
    public int ID { get; set; }

    /// <summary>
    /// Costruttore
    /// </summary>
    /// <param name="left_hand"><paramref name="left_hand"/></param>
    /// <param name="right_hand"><paramref name="right_hand"/></param>
    /// <param name="id"><paramref name="id"/></param>
    public Position(DataToStore left_hand, DataToStore right_hand, int id)
    {
        this.Left_Hand = left_hand;
        this.Right_Hand = right_hand;
        this.ID = id;
    }
}
