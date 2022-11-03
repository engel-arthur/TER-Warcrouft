using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Player;

public class UITutoController : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        Label nbBois = root.Q<Label>("nombreBois");
        nbBois.text = "0";
    }

    void Update()
    {
        //Je récupère l'instance du PlayerManager qui contient le nombre de bois récoltés
        int nbBoisAfficher = PlayerManager.getInstance().WoodStock;
        var root = GetComponent<UIDocument>().rootVisualElement;
        Label nbBois = root.Q<Label>("nombreBois");
        nbBois.text = nbBoisAfficher.ToString();
    }
}
