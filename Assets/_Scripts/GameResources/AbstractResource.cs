using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractResource : MonoBehaviour
{
    //TODO Modulariser le code (unités, bâtiments et ressources sont sélectionnables, et ils possèdent tous de la vie -> composant séparé)
    protected int resourceAmmount;
    // Start is called before the first frame update

    protected virtual void Awake(){}
    
    public void SubstractResource(int ammount) {
        if(ammount>=0){
            resourceAmmount-=ammount;
            Debug.Log("Ressource soustraite, total restant : " + resourceAmmount);
            if(resourceAmmount<=0)
                Die();
        }
    }
    private void Die() => Destroy(gameObject);
    
}
