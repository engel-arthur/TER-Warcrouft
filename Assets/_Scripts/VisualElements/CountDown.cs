using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using UnityEngine.UIElements;
using InputManager;

public class CountDown : MonoBehaviour
    {
        //compteur
        public float compteur;

        void Start(){
            
        }

        void Update(){

            //temps écoulé depuis le début du jeu
            compteur = Time.timeSinceLevelLoad;

            //si le compteur est égale à 0 je lance le décompte avant l'arrivée des ennemis (15 secondes première vague)
            if(compteur == 0f){
                StartCoroutine(timer(15f));
            }

        }

        IEnumerator timer(float time){
            while(time > 0){
                time--;
                yield return new WaitForSeconds(1f);
                var root = GetComponent<UIDocument>().rootVisualElement;
                Label countDown = root.Q<Label>("countDown");
                countDown.text = time.ToString()+" secondes";
            }
        }     
    }