using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class DataToStore
{
    public float FF { get; set; }
    public float NFA { get; set; }

    public DataToStore(float FF, float NFA)
    {
        this.FF = FF;
        this.NFA = NFA;
    }

    public override string ToString()
    {
        return $"FF: {FF}, NFA: {NFA}";
    } 
}
