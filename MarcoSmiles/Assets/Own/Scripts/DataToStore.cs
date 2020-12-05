using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;

[System.Serializable]
public class DataToStore
{
    public Hand hand { get; set; }
    public double FF1 { get; set; }
    public double FF2 { get; set; }
    public double FF3 { get; set; }
    public double FF4 { get; set; }
    public double FF5 { get; set; }
    public double NFA1 { get; set; }
    public double NFA2 { get; set; }
    public double NFA3 { get; set; }
    public double NFA4 { get; set; }

    public DataToStore(Hand hand, double FF1, double FF2, double FF3, double FF4, double FF5, double NFA1, double NFA2, double NFA3, double NFA4)
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
