using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Chest : MonoBehaviour ,IInteractable
{
    [Header("Animasyon Ayarlarý")]
    [SerializeField] private Vector3 openRotation = new Vector3(-110f, 0, 0);
    [SerializeField] private Vector3 closeRotation = Vector3.zero;
    [SerializeField] private float smoothSpeed = 4f;

    private bool _isOpen = false;
    private Quaternion _targetRotation;

    void Start()
    {
        _targetRotation = Quaternion.Euler(closeRotation);
    }

    public void Interact(GameObject player)
    {
        ToggleChest();
    }

    private void ToggleChest()
    {
        _isOpen = !_isOpen;

        Vector3 targetAngle = _isOpen ? openRotation : closeRotation;
        _targetRotation = Quaternion.Euler(targetAngle);

        Debug.Log(_isOpen ? "Sandýk Açýldý!" : "Sandýk Kapandý!");
    }

    void Update()
    {
        
        if (transform.localRotation != _targetRotation)
        {
            transform.localRotation = Quaternion.Slerp( transform.localRotation,_targetRotation,Time.deltaTime * smoothSpeed);
        }
    }
}
