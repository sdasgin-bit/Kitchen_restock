using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New BodyPart", menuName = "BodyParts/New BodyPart")]
public class BodyPart : ScriptableObject
{
    public GameObject[] parts;
    public int currIndex;
    [HideInInspector]public Toggle toggle;
    public bool defaultToggleVal = true;
}
