using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeResource : AbstractResource
{
    // Start is called before the first frame update
    private Renderer[] rends;
    private List<Color> originalColors = new List<Color>();
    protected override void Awake() {
        rends = GetComponentsInChildren<Renderer>();
        resourceAmmount = 100;
    }

    void OnMouseEnter(){
        foreach(Renderer rend in rends){
            originalColors.Add(rend.material.color);
            rend.material.color = Color.yellow;
        }
    }
    void OnMouseExit(){
        for(int i=0; i<rends.Length; i++){
            rends[i].material.color = originalColors[i];
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
