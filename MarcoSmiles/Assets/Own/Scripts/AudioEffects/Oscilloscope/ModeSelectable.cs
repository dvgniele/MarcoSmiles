using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class ModeChanger : MonoBehaviour
{
    public abstract List<Dropdown.OptionData> GetModes();
    public abstract void SetMode(Dropdown dropdown);
}
