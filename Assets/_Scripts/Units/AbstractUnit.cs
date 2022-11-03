using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Values;
using Player;
using TMPro;
using InputManager;

public abstract class AbstractUnit : MonoBehaviour
{
    protected int healthMax;
    protected int health;
    protected int cost;
    protected int attack;
    protected string unitName; //prénom de l'unité (Richard, Véronique, Gaétan..)
    protected Globals.unitStates state;

    protected bool carriesResource;
    protected Vector3 home;//là où l'unité revient pour poser ses ressources
    protected Vector3 target;//la position que l'agent veut atteindre (peut être pour un simple déplacement, la position d'une ressource, d'un ennemi..)
    protected AbstractResource targetResource;
    protected AbstractUnit targetEnemy;

    //protected int detectionRadius;//pour détecter si on a atteint la cible ou non (impossible d'être sur une position exacte, donc on utilise un rayon)
    protected SphereCollider actionCollider; //remplace le detectionradius, pour les diverses actions (attaque, récolte, etc..)
    protected GameObject highlightCircleGameObject;//ce qui s'affiche quand on sélectionne l'unité
    private NavMeshAgent navAgent;//pour le mouvement

    public GameObject nbPointViePerte;
    public GameObject viePerte;

    public GameObject nbPointVie;
    private GameObject vie;

    protected virtual void Awake()
    {
        //On assigne un nom aléatoire de la liste des prénoms
        unitName = Globals.RANDOM_NAMES_LIST[Random.Range(0, Globals.RANDOM_NAMES_LIST.Count)];
        carriesResource = false;
        home = GameObject.Find("MainBuilding").transform.position;
        //detectionRadius = 3;//taille de l'unité = 1
        highlightCircleGameObject = transform.Find("Highlight").gameObject;
        actionCollider = transform.Find("ActionCollider").gameObject.GetComponent<SphereCollider>();
        navAgent = GetComponent<NavMeshAgent>();
        healthMax = 1000;
        health = healthMax;
        vie = Instantiate(nbPointVie, transform.position, Quaternion.identity, transform);
        //vie.GetComponent<TextMeshPro>().text = health.ToString();
        vie.GetComponent<TextMeshPro>().transform.rotation = Camera.main.transform.rotation;
    }

    protected virtual void Update() {
        switch(state) {
            case Globals.unitStates.idle:
                break;
            case Globals.unitStates.walking:
                MoveUnitTo(target);
                if(targetReached())
                    SetState(Globals.unitStates.idle);
                break;
            case Globals.unitStates.harvesting:
                HarvestRoutine();
                break;
            case Globals.unitStates.attacking:
                AttackRoutine();
                break;
        }

        vie.GetComponent<TextMeshPro>().transform.rotation = Camera.main.transform.rotation;
        if(health < 1000) {
            vie.GetComponent<TextMeshPro>().text = health.ToString();
        }
    }

    // Méthodes de comportement
    public void MoveUnitTo(Vector3 position) {
        //Debug.Log("Moving towards [ " + position.x + ", " + position.y + ", " + position.z + " ]");
        navAgent.SetDestination(position);
    }

    public abstract void AttackRoutine();

    public abstract void HarvestRoutine();

    public void SubstractHealth(int ammount) {
        if(ammount>=0){
            health-=ammount;
            showLife(ammount);
            if(health<=0)
                Die();
        }
    }

    public void SetHighlightCircleVisibility(bool visibility) {
        highlightCircleGameObject.SetActive(visibility);
    }

    //Getters and Setters
    public int GetCost() => cost;
    public int GetHealth() => health;
    public string GetName() => unitName;

    public Vector3 GetHome() => home;
    public Globals.unitStates GetState() => state;
    
    public void SetState(Globals.unitStates newState) {
        //Debug.Log("Changement d'état");
        state = newState;
    }

    public void SetTarget(Vector3 newTarget) {
        //Debug.Log("Changement de cible ! : [ " + newTarget.x + ", " + newTarget.y + ", " + newTarget.z + " ]") ;
        //petit hack
        newTarget.y = 0;
        target = newTarget;
    }

    public void SetTargetResource(AbstractResource resource) {
        Debug.Log("Encore du travail ?");
        targetResource = resource;
    }

    public void SetTargetEnemy(AbstractUnit unit) {
        targetEnemy = unit;
    }

    public bool targetReached() {
        /*if(Vector3.Distance(transform.position, target) < detectionRadius) {
            Debug.Log("Travail terminé : " + GetName());
            return true;
        }
        else
            return false;*/
        return actionCollider.bounds.Contains(target);
    }

    protected bool isHome() {
        /*
        if(Vector3.Distance(transform.position, home) < detectionRadius) {
            Debug.Log(GetName() + " téléphone maison");
            return true;
        }
        else
            return false;
            */
        return actionCollider.bounds.Contains(home);
    }

    public void showLife(int perte){

        viePerte = Instantiate(nbPointViePerte, transform.position, Quaternion.identity, transform);
        viePerte.GetComponent<TextMeshPro>().text = perte.ToString();
    }

    // Méthodes privées (fonctionnement interne)
    //Léa a changé pour l'utiliser dans warrior (si il a plus de vie)
    protected void Die() {
        InputManager.InputHandler.instance.removeUnitFromList(this.transform);
        Destroy(gameObject);
    }


    //Fonction réception de demande d'aide de la part d'un ennemi
    public void receiveCall(AbstractUnit unitToAttackWithAlly)
    {
        //todo, est-ce qu'on répond à celui qui nous appelle pour qu'il sache ?
        Debug.Log("J'ai reçu un appel à l'aide");

        if (state == Globals.unitStates.attacking)
        {
            if(targetEnemy == unitToAttackWithAlly)
            {
                Debug.Log("J'arrive !");
            }else{
                Debug.Log("Je suis déjà en train d'attaquer un autre ennemi, je ne peux pas (encore) t'aider");
                //continue d'attaquer
            }
            
        }
        else
        {
            Debug.Log("J'arrive pour t'aider !");
            //Je mets l'unité que mon allié me donne en paramètre d'appel, comme cible ennemie pour moi
            SetTargetEnemy(unitToAttackWithAlly);
            //Je me mets en mode attaque
            SetState(Globals.unitStates.attacking);
        }

    }

    //Fonction réception de demande d'aide de la part d'un ennemi pour un gardien de pont
    public void receiveCallBackup(Vector3 localisation)
    {
        SetTarget(localisation);
        Debug.Log("J'arrête de garder le pont, j'arrive !");
        FullAI.enableBackupBehavior = false;     
    }
}
