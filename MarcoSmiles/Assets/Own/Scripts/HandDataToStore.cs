using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;

[System.Serializable]
public class HandDataToStore
{
    //public Hand hand { get; set; }
    public double FF { get; set; }
    public double NFA { get; set; }

    public HandDataToStore(double FF, double NFA)
    {
        this.FF = FF;
        this.NFA = NFA;
    }

    public override string ToString()
    {
        return $"FF: {FF}, NFA: {NFA}";
    }
}
