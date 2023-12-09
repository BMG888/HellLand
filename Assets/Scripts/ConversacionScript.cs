using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ConversacionScript
{
    [TextArea(3, 6)] // tama�o del text box
    public string[] oraciones; // se crea una lista de oraciones, las cuales se enumaran en el inspector
}
