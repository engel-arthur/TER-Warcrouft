using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using Values;

public class BasicAI : MonoBehaviour
{
    // Start is called before the first frame update
    WarriorUnit unit;
    SphereCollider unitDetectionSphere;

    List<Transform> enemyUnitsWithinDetectionRange;
    List<Transform> allyUnitsWithinDetectionRange;


    //On veut un compte des guerriers spécifiquement car ils sont les seuls importants pour l'évaluation attaque/fuite, en revanche on veut quand meme compter les ouvriers pour pouvoir les attaquer
    int enemyWarriorsWithinDetectionRange;
    int allyWarriorsWithinDetectionRange;

    void Awake() {
        enemyUnitsWithinDetectionRange = new List<Transform>();
        allyUnitsWithinDetectionRange = new List<Transform>();
    }
    void Start()
    {
        unit = gameObject.GetComponent<WarriorUnit>();
        unitDetectionSphere = gameObject.transform.Find("DetectionSphere").GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        detectUnitsWithinDetectionRange();
        if(enemyUnitsWithinDetectionRange.Count == 0) {
                findEnemies();
        }
        else {
            if(enemyWarriorsWithinDetectionRange - allyWarriorsWithinDetectionRange > 0) {
                runAway();
            }
            else {
                attackClosestEnemy();
            }
        }
    }

    void detectUnitsWithinDetectionRange() {
        enemyUnitsWithinDetectionRange.Clear();
        allyUnitsWithinDetectionRange.Clear();

        enemyWarriorsWithinDetectionRange = 0;
        allyWarriorsWithinDetectionRange = 0;
        
        foreach (Transform child in Player.PlayerManager.instance.enemyUnits)//Celà peut prêter à confusion, on compre les "enemyUnits" comme des alliés étant donné qu'il s'agit d'un script pour les ennemis.
        {
            foreach(Transform allyUnit in child) //Les "child" sont Workers et Warriors, donc pour toutes les unités qui se trouvent dans Workers et Warriors
            {
                if (unitDetectionSphere.bounds.Contains(allyUnit.position))
                {
                    allyUnitsWithinDetectionRange.Add(allyUnit);
                    if(allyUnit.parent.gameObject.GetComponent<WarriorUnit>())
                        allyWarriorsWithinDetectionRange++;
                }        
            }
        }

        foreach (Transform child in Player.PlayerManager.instance.playerUnits)
        {
            foreach(Transform enemyUnit in child) //Les "child" sont Workers et Warriors, donc pour toutes les unités qui se trouvent dans Workers et Warriors
            {
                if (unitDetectionSphere.bounds.Contains(enemyUnit.position))
                {
                    enemyUnitsWithinDetectionRange.Add(enemyUnit);
                    if(enemyUnit.parent.gameObject.GetComponent<WarriorUnit>())
                        enemyWarriorsWithinDetectionRange++;
                }        
            }
        }
        //Debug.Log("Unités dans mon champ de vision : " + allyWarriorsWithinDetectionRange + " guerrier alliés " + (enemyUnitsWithinDetectionRange.Count - enemyWarriorsWithinDetectionRange) + " ouvriers ennemis " + enemyWarriorsWithinDetectionRange + " guerrier ennemis");
    }

    void findEnemies() {
        Debug.Log("Je cherche");
        unit.SetTarget(unit.GetHome());//TODO Hacky, faudra faire ça plus propre. Actuellement on met la target à Home pour trouver des ennemis car toutes les unités ont le MainBuilding du joueur en home
        unit.SetState(Globals.unitStates.walking);
    }

    void runAway() {
        Debug.Log("Je cours");
        unit.SetTarget(new Vector3(10, 0, 10));//complètement arbitraire, c'est au centre de la partie haute de la carte
        unit.SetState(Globals.unitStates.walking);
    }

    void attackClosestEnemy() {
        Debug.Log("A L'ASSAUUUUT");
        unit.SetTargetEnemy(enemyUnitsWithinDetectionRange[0].parent.gameObject.GetComponent<AbstractUnit>());
        unit.SetState(Globals.unitStates.attacking);
    }
}
