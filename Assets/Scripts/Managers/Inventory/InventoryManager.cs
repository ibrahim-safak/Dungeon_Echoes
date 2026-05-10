using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<KeyData> keys = new List<KeyData>();

    public void AddKey(KeyData key)
    {
        if (!keys.Contains(key))
        {
            keys.Add(key);
            Debug.Log(key.keyName + " envantere alýndý.");
        }
    }

    public bool HasKey(string id)
    {
        return keys.Exists(k => k.keyID == id);
    }

    public void RemoveKey(string id)
    {
        keys.Remove(keys.Find(k => k.keyID == id));
    }
}
