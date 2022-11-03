using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Player;

public class CreationBuilding : MonoBehaviour
{

    // Reference to the Prefab. Drag a Prefab into this field in the Inspector.
    public GameObject myPrefab;
    public int storedResources;
    public int boucle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        storedResources =  PlayerManager.getInstance().WoodStock;
        if(storedResources >= 150){
            PlayerManager.getInstance().WoodStock = PlayerManager.getInstance().WoodStock - 150;
            storedResources = storedResources - 150;
            
            Debug.Log("Création de 1 warrior ici!");
            Debug.Log("Utilisation de 200 ressources de bois");

            boucle = 1;
            Vector3 homeCreation = GameObject.Find("UnitsBuilding").transform.position;
            while (boucle != 0)
            {
                Vector2 randomPos = Random.insideUnitCircle.normalized * 4;
                Instantiate(myPrefab, homeCreation + new Vector3(randomPos.x, 0, randomPos.y), Quaternion.identity, PlayerManager.getInstance().playerUnits);
                boucle--;
            }
            
        }
    }
}
