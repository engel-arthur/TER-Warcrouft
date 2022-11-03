using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using Values;

public class FullAI : MonoBehaviour
{
    // Start is called before the first frame update
    public static bool enablePatrolBehavior;
    public static bool enableAllyCallingBehavior;
    public static bool enableBackupBehavior;

    WarriorUnit unit;
    SphereCollider unitDetectionSphere;

    SphereCollider alliesDetectionSphere;

    List<Transform> enemyUnitsWithinDetectionRange;
    List<Transform> allyUnitsWithinDetectionRange;
    List<AbstractUnit> allyUnitsWithinBigDetectionRange;
    List<AbstractUnit> backUpList;
    List<Vector3> backUpListBridgesPositions;

    bool isRunningAway = false;
    bool enemyBaseReachedAndNoEnemyFound;

    int patrolAreaDistance;
    int patrolAreaDistanceMin;
    int patrolAreaDistanceMax;
    int patrolSquareAreaCornerNumber;
    bool patrolDirectionChanged;
    bool iambackup;
    Vector3 myBridgePosition;


    //On veut un compte des guerriers spécifiquement car ils sont les seuls importants pour l'évaluation attaque/fuite, en revanche on veut quand meme compter les ouvriers pour pouvoir les attaquer
    int enemyWarriorsWithinDetectionRange;
    int allyWarriorsWithinDetectionRange;

    //Codé en dur, limites de la map
    int minXCoord=-65;
    int maxXCoord=70;
    int minZCoord=-55;
    int maxZCoord=65;

    void Awake() {
        enemyUnitsWithinDetectionRange = new List<Transform>();
        allyUnitsWithinDetectionRange = new List<Transform>();
        allyUnitsWithinBigDetectionRange = new List<AbstractUnit>();
        backUpList = new List<AbstractUnit>();
        backUpListBridgesPositions = new List<Vector3>();

        enemyBaseReachedAndNoEnemyFound = false;
        patrolAreaDistanceMin = 5;
        patrolAreaDistanceMax = 20;
        patrolAreaDistance = patrolAreaDistanceMin;
        patrolSquareAreaCornerNumber=0;
        patrolDirectionChanged=true;
    }
    void Start()
    {
        unit = gameObject.GetComponent<WarriorUnit>();
        unitDetectionSphere = gameObject.transform.Find("DetectionSphere").GetComponent<SphereCollider>();
        alliesDetectionSphere = gameObject.transform.Find("DetectionSphereAlly").GetComponent<SphereCollider>();
        
        //On créé la liste des backups
        getBackups();

        //On remplit les positions des ponts
        backUpListBridgesPositions.Add(new Vector3(0,0,-4f));
        backUpListBridgesPositions.Add(new Vector3(-40f,0,-4f));
        backUpListBridgesPositions.Add(new Vector3(40f,0,-4f));
        
        iambackup = false;
        //Je regarde si je suis dans la liste des backups ou pas
        for(int i=0; i<backUpList.Count; i++){
            if(backUpList[i] == unit){
                Debug.Log("Je suis un gardien de pont !");
                iambackup = true;     
                myBridgePosition = backUpListBridgesPositions[i];
            }
        }
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
                    //J'appelle au secours aux gardiens des ponts et je leur donne ma position
                    callBackup(unit.transform);
                    //enableBackupBehavior = false;
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

        if(enableBackupBehavior && iambackup){

            //jsuis un backup je vais au pont et j'attends et je me mets en attente d'arrivée d'ennemis
            unit.SetTarget(myBridgePosition);
            unit.SetState(Globals.unitStates.walking);
            Debug.Log("Je vais garder le pont !");

        }else{
            Debug.Log("Je cherche");
            if(!enemyBaseReachedAndNoEnemyFound)
            {
                goToEnemyBase();
            }
            else if (enablePatrolBehavior)
            {
                patrolEnemyBase();
            }
        }
    }

    void goToEnemyBase() {
        unit.SetTarget(unit.GetHome());
        unit.SetState(Globals.unitStates.walking);
        if(unit.targetReached()) {
            enemyBaseReachedAndNoEnemyFound=true;
        }
    }

    void patrolEnemyBase() {
        if(patrolDirectionChanged){
            patrolDirectionChanged=false;
            int xCoordSign;
            int zCoordSign;
            switch(patrolSquareAreaCornerNumber) {
                case 0:
                    xCoordSign=1;
                    zCoordSign=1;
                break;
                case 1:
                    xCoordSign=1;
                    zCoordSign=-1;
                break;
                case 2:
                    xCoordSign=-1;
                    zCoordSign=-1;
                break;
                default://3
                    xCoordSign=-1;
                    zCoordSign=1;
                break;
            }
            Vector3 targetOffset = new Vector3(patrolAreaDistance*xCoordSign, 0, patrolAreaDistance*zCoordSign);

            Vector3 newTarget = unit.GetHome() + targetOffset;
            if(newTarget.x > maxXCoord)
                newTarget.x = maxXCoord;
            else if(newTarget.x < minXCoord)
                newTarget.x = minXCoord;
            if(newTarget.z > maxZCoord)
                newTarget.z = maxZCoord;
            else if(newTarget.z < minZCoord)
                newTarget.z = minZCoord;


            unit.SetTarget(newTarget);
            unit.SetState(Globals.unitStates.walking);
        }
        else {
            if(unit.targetReached()) {
                patrolSquareAreaCornerNumber = (patrolSquareAreaCornerNumber + 1) % 4;
                patrolDirectionChanged=true;
                //Si on commence un nouveau tour de patrouille
                if(patrolSquareAreaCornerNumber == 0) {
                    if(patrolAreaDistance < patrolAreaDistanceMax)
                        patrolAreaDistance++;
                    else
                        patrolAreaDistance = patrolAreaDistanceMin;
                }
            }
        }

    }

    void runAway() {
        if (!isRunningAway)
        {
            isRunningAway = true;

            Debug.Log("Je cours");
            unit.SetTarget(new Vector3(0, 0, 55));
            unit.SetState(Globals.unitStates.walking);

            Invoke("CancelRunAway", 10.0f);

        }
    }

    void attackClosestEnemy() {
        enemyBaseReachedAndNoEnemyFound=false;
        Debug.Log("A L'ASSAUUUUT");
        unit.SetTargetEnemy(enemyUnitsWithinDetectionRange[0].parent.gameObject.GetComponent<AbstractUnit>());
        unit.SetState(Globals.unitStates.attacking);

        if(enableAllyCallingBehavior)
            callAllies(enemyUnitsWithinDetectionRange[0].parent.gameObject.GetComponent<AbstractUnit>());
    }

    void callAllies(AbstractUnit unitToAttackWithAlly)
    {

        //créer liste de mes alliés avec la grosse sphère
        foreach (Transform child in Player.PlayerManager.instance.enemyUnits)
        {
            if (alliesDetectionSphere.bounds.Contains(child.position))
            {
                    allyUnitsWithinBigDetectionRange.Add(child.gameObject.GetComponent<AbstractUnit>());
                    Debug.Log("J'ai ajouté un allié à la liste! ");
            }
        }

        foreach (AbstractUnit ally in allyUnitsWithinBigDetectionRange)
        {
            Debug.Log(unit.gameObject.GetComponent<AbstractUnit>() + " :J'appelle mon allié pour qu'il vienne m'aider :", unitToAttackWithAlly);
            if(ally != null)
            {
                ally.receiveCall(unitToAttackWithAlly);
            }
        }
    }

    private void CancelRunAway()
    {
        isRunningAway = false;
        Debug.Log("J'arrete de fuir");
    }

    private void getBackups()
    {
        foreach (Transform child in Player.PlayerManager.instance.enemyUnits)
        {
            if(backUpList.Count < 3)
            {
                //J'ajoute 3 alliés dans la liste des backups
                backUpList.Add(child.gameObject.GetComponent<AbstractUnit>());     
            }       
        }
    }

    private void callBackup(Transform localisation){

        Debug.Log("J'appelle mon allié gardien de pont pour qu'il vienne m'aider");
    
        //J'envoie un message de demande d'aide au backup des ponts
        foreach (AbstractUnit gardien in backUpList)
        {
            if(gardien != null)
            {
                //fonction pour appeler à l'aide aux backups et leur donner ma position de détresse
                gardien.receiveCallBackup(localisation.position);
            }
        }
    }
}
