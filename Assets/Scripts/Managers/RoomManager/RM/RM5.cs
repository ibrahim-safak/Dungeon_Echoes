using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RM5 : MonoBehaviour
{
    [Header("Fog Settings")]
    public float maxFogDensity = 0.4f;
    public float fogChangeSpeed = 0.5f;
    private float targetDensity = 0f;

    public bool isFogDisabled = false;

    void Start()
    {
        RenderSettings.fog = true;
    }

    void Update()
    {
        if (isFogDisabled)
        {
            targetDensity = 0f;
        }

        RenderSettings.fogDensity = Mathf.MoveTowards(RenderSettings.fogDensity, targetDensity, Time.deltaTime * (fogChangeSpeed * 0.1f));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isFogDisabled)
        {
            targetDensity = maxFogDensity;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            targetDensity = 0f;
        }
    }
}
