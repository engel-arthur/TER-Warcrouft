using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using UnityEngine.UIElements;
using InputManager;

public class UiController : MonoBehaviour
{
    public GameObject viePerte;
    private float compteur = 0f;
    private int nbEnemiesAlive = 0;
    private int compteurVague = 0;
    public GameObject Team2;
    private bool calledOnce;
    Label textCountDown;

    //pour le bugfix
    public GameObject castle1;
    public GameObject castle2;

    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        Label nbBois = root.Q<Label>("nombreBois");
        nbBois.text = "0";
        calledOnce = false;
        textCountDown = root.Q<Label>("textCountDown");
    }

    // Update is called once per frame
    void Update()
    {
        //Je récupère l'instance du PlayerManager qui contient le nombre de bois récoltés
        int nbBoisAfficher = PlayerManager.getInstance().WoodStock;
        var root = GetComponent<UIDocument>().rootVisualElement;
        Label nbBois = root.Q<Label>("nombreBois");
        nbBois.text = nbBoisAfficher.ToString();

        //On utilise le temps depuis le début du lancement du jeu pour la première vague
        compteur = Time.timeSinceLevelLoad;

        //On compte le nombre d'ennemis
        Team2 = GameObject.Find("Team2");
        nbEnemiesAlive = Team2.transform.childCount;

        //Dès le lancement du jeu on lance le chronomètre
        if(compteur == 0f){
            textCountDown.text = "Les ennemis arrivent dans : ";
            StartCoroutine(timerAndText(15f, "Première vague d'ennemis en approche", 1));
            calledOnce = true;
        }
       
        if(compteurVague==1 && nbEnemiesAlive==0 && calledOnce == false && castle1.GetComponent<CreateEnemyUnits>().calledOnceVague1 && castle2.GetComponent<CreateEnemyUnits>().calledOnceVague1){
            textCountDown.text = "Les ennemis arrivent dans : ";
            calledOnce = true;
            StartCoroutine(timerAndText(15f, "Deuxième vague d'ennemis en approche", 2));    
        }

        if(compteurVague==2 && nbEnemiesAlive==0 && calledOnce == false && castle1.GetComponent<CreateEnemyUnits>().calledOnceVague2 && castle2.GetComponent<CreateEnemyUnits>().calledOnceVague2){
            textCountDown.text = "Les ennemis arrivent dans : ";
            calledOnce = true;
            StartCoroutine(timerAndText(15f, "Troisième vague d'ennemis en approche", 3));  
        }
        
        if(compteurVague==3 && nbEnemiesAlive==0 && calledOnce == false && castle1.GetComponent<CreateEnemyUnits>().calledOnceVague3 && castle2.GetComponent<CreateEnemyUnits>().calledOnceVague3) {
            calledOnce = true;
            Player.PlayerManager.instance.numeroVague = 4;
        }
    }

    IEnumerator timerAndText(float time, string texteVague, int numVague){

        var root = GetComponent<UIDocument>().rootVisualElement;
        Label countDown = root.Q<Label>("countDown");
        Label textCountDown = root.Q<Label>("textCountDown");
            
            while(time > 0){

                if(time == 15){
                    Label vague1 = root.Q<Label>("vague");
                    vague1.text = texteVague;
                }
                if(time == 12){
                    Label vague1 = root.Q<Label>("vague");
                    vague1.text = "";
                }
                
                time--;
                yield return new WaitForSeconds(1f);
                countDown.text = time.ToString()+" secondes";
                if(time == 0){
                    countDown.text = "";
                    textCountDown.text = "Attention, les ennemis arrivent !";
                    compteurVague = numVague;
                    //déclenche le spawn dans createEnemyUnits
                    Player.PlayerManager.instance.numeroVague = numVague;
                    calledOnce = false;
                }
            }  
    }
}
