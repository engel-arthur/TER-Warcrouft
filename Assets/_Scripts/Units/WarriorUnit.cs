using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Values;
using Player;

public class WarriorUnit : AbstractUnit
{

    private bool isCarryingResources;//implémentation simpliste, on peut éventuellement mettre un nombre de ressources qui diffère en fonction de plusieurs facteurs
       
    float attackCooldown;
    float timeSinceLastAttack;

    protected override void Awake() {
        base.Awake();
        attackCooldown=0.5f;
        timeSinceLastAttack=attackCooldown;
        cost = 30;
        attack = 100;
    }

    protected override void Update() {
        base.Update();
        timeSinceLastAttack+=Time.deltaTime;
    }

    public override void HarvestRoutine() {
    
        //Debug.Log("C'est un travail de péon ça ! Je suis un WARRIOR moi, je me bats.");

    }

    public override void AttackRoutine(){

        if(targetEnemy != null) {
            SetTarget(targetEnemy.transform.position);//On set la target ici et pas dans le input handler car l'ennemi peut bouger (on veut que l'unité s'adapte aux déplacements de l'ennemi)
            if(targetReached()){
                if(timeSinceLastAttack >= attackCooldown){
                    timeSinceLastAttack = 0f;
                    targetEnemy.SubstractHealth(attack);
                }
               
            }

            else{
                MoveUnitTo(target);
            }
        }
        else{
            SetState(Globals.unitStates.idle);
        }
    }
    
}
