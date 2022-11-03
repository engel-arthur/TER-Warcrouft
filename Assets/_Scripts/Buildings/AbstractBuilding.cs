using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractBuilding : MonoBehaviour
{
    protected int storedResources;
    
    protected virtual void Awake() {
        storedResources=0;
    }

    protected void addResources(int ammount) {
        storedResources++;
        Debug.Log("Ressource ajoutée! total : " + storedResources);
    }
}
