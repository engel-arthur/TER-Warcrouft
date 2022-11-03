using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class MainBuilding : MonoBehaviour
{

    protected int storedResources;

    void Start()
    {
        storedResources = 0;
    }

    void Update()
    {
        //J'actualise dans le bâtiment contenant les ressources, les ressources de bois dont dispose le joueur (ce nombre est initialement stocké dans le PlayerManager)
        storedResources =  PlayerManager.getInstance().WoodStock;
    }
}
