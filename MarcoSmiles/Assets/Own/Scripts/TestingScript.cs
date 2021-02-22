public class TestingScript
{

    //private static float[] current_features = new float[18];

    /// <summary>
    /// Seleziona la feature corrente
    /// </summary>
    /// <returns>ritorna i valori della feature corrente</returns>
    public static float[] GetCurrentFeatures()
    {
        float[] current_features = new float[18];

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

        current_features[0] = left_hand.FF1;
        current_features[1] = left_hand.FF2;
        current_features[2] = left_hand.FF3;
        current_features[3] = left_hand.FF4;
        current_features[4] = left_hand.FF5;
        current_features[5] = left_hand.NFA1;
        current_features[6] = left_hand.NFA2;
        current_features[7] = left_hand.NFA3;
        current_features[8] = left_hand.NFA4;
        
        current_features[9] = right_hand.FF1;
        current_features[10] = right_hand.FF2;
        current_features[11] = right_hand.FF3;
        current_features[12] = right_hand.FF4;
        current_features[13] = right_hand.FF5;
        current_features[14] = right_hand.NFA1;
        current_features[15] = right_hand.NFA2;
        current_features[16] = right_hand.NFA3;
        current_features[17] = right_hand.NFA4;

        return current_features;
    }
}
