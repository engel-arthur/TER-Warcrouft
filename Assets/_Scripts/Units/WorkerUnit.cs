using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Values;
using Player;
using TMPro;


public class WorkerUnit : AbstractUnit
{
    private bool isCarryingResources;//implémentation simpliste, on peut éventuellement mettre un nombre de ressources qui diffère en fonction de plusieurs facteurs
    public int nbBoisTotal = 0;

    protected override void Awake() {
        base.Awake();
        cost = 30;
        attack = 100;
    }

    public override void HarvestRoutine() {
        //while(state==Globals.unitStates.harvesting)
        if(carriesResource) {
            if(isHome()){
                carriesResource=false;
                PlayerManager playerManager = PlayerManager.getInstance();
                playerManager.WoodStock = playerManager.WoodStock + 10;
            }
            else
                MoveUnitTo(home);
        }

        else if(targetReached()) {
            if(targetResource!=null){
                Debug.Log("Harvesting!\n");
                targetResource.SubstractResource(10);
                carriesResource=true;
            }
            else
                MoveUnitTo(home);
        }

        else if(targetResource!=null){
            MoveUnitTo(target);
        }

        else{
            SetTarget(home);
            SetState(Globals.unitStates.walking);
        }
    }

    public override void AttackRoutine(){
        //Debug.Log("J'ai vraiment très peur je veux pas y aller");
    }

}

