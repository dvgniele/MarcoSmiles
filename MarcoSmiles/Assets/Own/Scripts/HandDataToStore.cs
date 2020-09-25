using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;

[System.Serializable]
public class HandDataToStore
{
    //public Hand hand { get; set; }
    public float FF { get; set; }
    public float NFA { get; set; }

    public HandDataToStore(float FF, float NFA)
    {
        this.FF = FF;
        this.NFA = NFA;
    }

    public override string ToString()
    {
        return $"FF: {FF}, NFA: {NFA}";
    }
}
