using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RM6 : MonoBehaviour
{
    [Header("Sýralama (Basamak Ýsimleri)")]
    public List<string> correctSequence;

    [Header("Referanslar")]
    public Door targetDoor; 

    private int currentStepIndex = 0; 
    private List<StepSensor> pressedSteps = new List<StepSensor>(); 

    public void StepPressed(StepSensor pressedStep)
    {
        string stepName = pressedStep.gameObject.name;

        if (currentStepIndex < correctSequence.Count && stepName == correctSequence[currentStepIndex])
        {
            Debug.Log("Doðru basamak: " + stepName);

            pressedStep.LockDown(); 
            pressedSteps.Add(pressedStep); 
            currentStepIndex++; 
            
            if (currentStepIndex == correctSequence.Count)
            {
                CompletePuzzle();
            }
        }
        else
        {
         
            Debug.Log("Yanlýþ basamak! Her þey sýfýrlanýyor.");
            StartCoroutine(WrongStepPenalty(pressedStep));
        }
    }

    void CompletePuzzle()
    {
        Debug.Log("Bulmaca çözüldü! Kapý kilidi açýldý.");
        if (targetDoor != null)
        {
            targetDoor.isLocked = false; 
        }
        this.enabled = false; 
    }

    
    IEnumerator WrongStepPenalty(StepSensor wrongStep)
    {
        
        yield return new WaitForSeconds(0.5f);

       
        foreach (StepSensor step in pressedSteps)
        {
            step.ResetStep();
        }

        if (wrongStep != null)
        {
            wrongStep.ResetStep();
        }

        pressedSteps.Clear();
        currentStepIndex = 0;
    }
}
