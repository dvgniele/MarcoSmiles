using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Leap;

public class DatasetHandler : MonoBehaviour
{
    public Vector PalmPositionDx { get; set; }
    public Vector3 PalmRotationDx { get; set; }
    public Vector PalmPositionSx { get; set; }
    public Vector3 PalmRotationSx { get; set; }
    public Vector3 DxSxDistance { get; set; }


}
