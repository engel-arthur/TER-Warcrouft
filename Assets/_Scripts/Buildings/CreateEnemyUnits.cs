using UnityEngine;

using Player;

public class CreateEnemyUnits : MonoBehaviour
{
    
    public GameObject myPrefab;
    private int nbEnemy = 0;
    //private float compteur = 0f;
    public bool calledOnceVague1 = false;
    public bool calledOnceVague2 = false;
    public bool calledOnceVague3 = false;
    public GameObject Team2;
    private int nbEnemiesAlive = 0;

    void Update()
    {

        //Coordonnées spawn
        Vector3 pos2;
        pos2.x = -2;
        pos2.y = 0;
        pos2.z = -5;

        //On utilise le temps depuis le début du lancement du jeu pour la première vague
        //compteur = Time.timeSinceLevelLoad;

        //On compte le nombre d'ennemis
        Team2 = GameObject.Find("Team2");
        nbEnemiesAlive = Team2.transform.childCount;

        //pour faire des tests
        //FullAI.enablePatrolBehavior = true;
        //    FullAI.enableAllyCallingBehavior = true;
        //    FullAI.enableBackupBehavior = true;

        if(Player.PlayerManager.instance.numeroVague==1 && calledOnceVague1==false){
            while (nbEnemy < 3)
            {
                Debug.Log("Création d'un ennemi avec IA level 1 dans la base!");
                Vector3 pos = transform.position + pos2;
                Instantiate(myPrefab, pos, Quaternion.identity, PlayerManager.getInstance().enemyUnits);
                nbEnemy++;
                pos2.x += 2;
            }
            nbEnemy = 0;
            calledOnceVague1 = true;
        }
       
        else if(Player.PlayerManager.instance.numeroVague==2 && calledOnceVague2==false){
            FullAI.enablePatrolBehavior = true;
            while (nbEnemy < 4)
            {
                Debug.Log("Création d'un ennemi avec IA level 2 dans la base!");
                Vector3 pos = transform.position + pos2;
                Instantiate(myPrefab, pos, Quaternion.identity, PlayerManager.getInstance().enemyUnits);
                nbEnemy++;
                pos2.x += 2;
            }
            nbEnemy = 0; 
            calledOnceVague2 = true;
        }

        else if(Player.PlayerManager.instance.numeroVague==3 && calledOnceVague3==false){
            FullAI.enablePatrolBehavior = true;
            FullAI.enableAllyCallingBehavior = true;
            FullAI.enableBackupBehavior = true;
            while (nbEnemy < 4)
            {
                Debug.Log("Création d'un ennemi avec IA level 3 dans la base!");
                Vector3 pos = transform.position + pos2;
                Instantiate(myPrefab, pos, Quaternion.identity, PlayerManager.getInstance().enemyUnits);
                nbEnemy++;
                pos2.x += 2;
            }
            nbEnemy = 0;
            calledOnceVague3 = true;
        }

    }
}
