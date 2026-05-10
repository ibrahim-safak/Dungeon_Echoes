using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerCharacter : BaseCharacter 
{
    private float interactionDistance = 3f; // Etkileþim mesafesi
    [SerializeField] private LayerMask interactableLayer; // Etkileþim yapýlabilir nesnelerin katmaný

    // interactionMask yerine interactableLayer kullanýlmalý
    protected override void Start()
    {
        base.Start(); 
    }
    

    public override void die()
    {
        Debug.Log("OYUNCU ÖLDÜ! Game Over.");
    }

    public virtual void Attack()
    {
        // Boþ - alt sýnýflar implement eder
    }
    public virtual void Interact()
    {
        Transform origin = Camera.main != null ? Camera.main.transform : transform;
        RaycastHit hit;
        Debug.DrawRay(origin.position, origin.forward * interactionDistance, Color.red, 1f); // Ray'i görselleþtir
        bool didHit;
        

        if (interactableLayer.value == 0)
        {
            didHit = Physics.Raycast(origin.position, origin.forward, out hit, interactionDistance);
            
        }
        else
        {
            didHit = Physics.Raycast(origin.position, origin.forward, out hit, interactionDistance, interactableLayer);
        }

        if (!didHit) return;

        var interactable = hit.collider.GetComponent<IInteractable>();
        if (interactable != null)
        {
            interactable.Interact(this.gameObject);
        }
    }
}
