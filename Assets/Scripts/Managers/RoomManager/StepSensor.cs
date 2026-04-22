using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepSensor : MonoBehaviour
{
    public RM6 manager;

    
    public float pressDepth = 0.1f; 
    public float speed = 5f;        

    private Vector3 startPos;
    private Vector3 targetPos;
    private bool isLockedDown = false; 

    void Start()
    {
       
        startPos = transform.localPosition;
        targetPos = startPos + new Vector3(0, -pressDepth, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
       
        if (other.CompareTag("Player") && !isLockedDown)
        {
            StopAllCoroutines();
            StartCoroutine(MoveStep(targetPos));

            
            manager.StepPressed(this);
        }
    }

    
    public void LockDown()
    {
        isLockedDown = true;
        
    }


    public void ResetStep()
    {
        isLockedDown = false;
        Debug.Log(gameObject.name + " yukarý çýkýyor!");
        StopAllCoroutines();
        StartCoroutine(MoveStep(startPos));
    }

    IEnumerator MoveStep(Vector3 target)
    {
        while (Vector3.Distance(transform.localPosition, target) > 0.001f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, target, Time.deltaTime * speed);
            yield return null;
        }
        transform.localPosition = target;
    }
}
