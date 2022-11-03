using UnityEngine;



public class CreatePopUp : MonoBehaviour
{
    public GameObject myPopupWinPrefab;
    public GameObject myPopupLosePrefab;
    public GameObject Team1;
    public GameObject Team2;
    public int nbPopUpWin = 0;
    public int nbPopUpLose = 0;
    public float compteur;

    private void Update()
    {
        Team1 = GameObject.Find("Team1");
        Team2 = GameObject.Find("Team2");

        //compteur = Time.timeSinceLevelLoad;

        //Quand il n'y a plus d'ennemi et qu'on a passé les trois vagues
        if (Team2.transform.childCount == 0 && nbPopUpWin != 1 && Player.PlayerManager.instance.numeroVague == 4)
        {
            //Création de la pop-up
            Debug.Log("Création de la pop up Win");

            Instantiate(myPopupWinPrefab);
            nbPopUpWin++;
        }

        //S'il n'y a plus de worker ni de warrior on a perdu, peu importe la vague
        if ( (Team1.transform.childCount == 0) && nbPopUpLose != 1)
        {
            //Création de la pop-up
            Debug.Log("Création de la pop up Lose");

            Instantiate(myPopupLosePrefab);
            nbPopUpLose++;
        }


    }
}


//Enlever le Find("EnemyWorkerUnit") quand on aura fini les tests parce que ceux qu'on crée automatiquement ont un (Clone) à la fin
//Et surtout penser à changer en EnemyWarriorUnit ou EnemyUnit selon le nom qu'on leur aura donné

