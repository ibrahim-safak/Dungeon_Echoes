using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyObject : MonoBehaviour , IInteractable
{
    public KeyData keyData;

    public void Interact(GameObject player)
    {
        InventoryManager inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryManager>();

        
            inventory.AddKey(keyData);
            Debug.Log(keyData.keyName + " sandýktan alýndý.");
            Destroy(gameObject); // Fiziksel objeyi yok et
        
    }

    
}
