using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Yeni Anahtar", menuName = "Envanter/Anahtar")]
public class KeyData : ScriptableObject
{
    public string keyName; 
    public Sprite keyIcon; 
    public string keyID; 
}
