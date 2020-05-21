using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BurntCounter : MonoBehaviour
{
    public Text TextBox;
    public int BurntAmount { get; private set; }

    public void CountBurnt()
    {
        BurntAmount++;
        TextBox.text = BurntAmount.ToString();
    }
}
