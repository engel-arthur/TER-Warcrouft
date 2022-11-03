using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using Values;

public class Level3AI : MonoBehaviour
{
    // Start is called before the first frame update
    WarriorUnit unit;
    SphereCollider unitDetectionSphere;
    SphereCollider alliesDetectionSphere;

    List<Transform> enemyUnitsWithinDetectionRange;
    List<Transform> allyUnitsWithinDetectionRange;
    List<AbstractUnit> allyUnitsWithinBigDetectionRange;


    //On veut un compte des guerriers spécifiquement car ils sont les seuls importants pour l'évaluation attaque/fuite, en revanche on veut quand meme compter les ouvriers pour pouvoir les attaquer
    int enemyWarriorsWithinDetectionRange;
    int allyWarriorsWithinDetectionRange;

    bool isRunningAway = false;

    void Awake() {
        enemyUnitsWithinDetectionRange = new List<Transform>();
        allyUnitsWithinDetectionRange = new List<Transform>();
        allyUnitsWithinBigDetectionRange = new List<AbstractUnit>();
    }

    void Start()
    {
        unit = gameObject.GetComponent<WarriorUnit>();
        unitDetectionSphere = gameObject.transform.Find("DetectionSphere").GetComponent<SphereCollider>();
        alliesDetectionSphere = gameObject.transform.Find("DetectionSphereAlly").GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isRunningAway)
        {
            detectUnitsWithinDetectionRange();
            if (enemyUnitsWithinDetectionRange.Count == 0)
            {
                findEnemies();
            }
            else
            {
                if (enemyWarriorsWithinDetectionRange - allyWarriorsWithinDetectionRange > 0)
                {
                    runAway();
                }
                else
                {
                    attackClosestEnemy();
                }
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
        
        if (!isRunningAway)
        {
            isRunningAway = true;

            Debug.Log("Je cours");
            unit.SetTarget(new Vector3(25, 0, 60));//complètement arbitraire, c'est au centre de la partie haute de la carte
            unit.SetState(Globals.unitStates.walking);

            Invoke("CancelRunAway", 10.0f);

        }
        
    }

    void attackClosestEnemy() {
        Debug.Log("A L'ASSAUUUUT");
        unit.SetTargetEnemy(enemyUnitsWithinDetectionRange[0].parent.gameObject.GetComponent<AbstractUnit>());
        unit.SetState(Globals.unitStates.attacking);
        //On met l'ennemi qu'on attaque en paramètre du call pour le donner à ceux qu'on va appeler
        callAllies(enemyUnitsWithinDetectionRange[0].parent.gameObject.GetComponent<AbstractUnit>());
        
    }

    //Fonction de demande d'aide lors d'un lancement d'attaque
    void callAllies(AbstractUnit unitToAttackWithAlly)
    {

        //todo
        //créer liste de mes alliés avec la grosse sphère
        foreach (Transform child in Player.PlayerManager.instance.enemyUnits)
        {


            if (alliesDetectionSphere.bounds.Contains(child.position)) //Si on détecte un allié dans la grande sphère
            {

                    //On l'ajoute à la liste
                    allyUnitsWithinBigDetectionRange.Add(child.gameObject.GetComponent<AbstractUnit>());
                    Debug.Log("J'ai ajouté un allié à la liste! ");
            }
            
        }
        //j'envoie un message de demande d'aide (comportement attaque animal mdr jsais pas lequel)
        foreach (AbstractUnit ally in allyUnitsWithinBigDetectionRange)
        {
            Debug.Log(unit.gameObject.GetComponent<AbstractUnit>() + " :J'appelle mon allié pour qu'il vienne m'aider :", unitToAttackWithAlly);

            if(ally != null)
            {
   
                //fonction pour appeler à l'aide (j'appelle la fonction sur chacun de mes alliés comme substractHealth)
                ally.receiveCall(unitToAttackWithAlly);

            }

            //est-ce qu'on attendrait pas une réponse ? genre si personne répond je me casse (comportement avancé++)
        }

    }

    private void CancelRunAway()
    {
        isRunningAway = false;
        Debug.Log("J'arrete de fuir");
    }


}
