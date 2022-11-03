using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using InputManager;

namespace Player
{
    public class PlayerManager : MonoBehaviour
    {
            public static PlayerManager instance;

            public Transform playerUnits;
            public Transform enemyUnits;
            public int WoodStock;
            public int numeroVague;

            void Start()
            {
                instance = this;
            }

            void Update()
            {
                InputHandler.instance.HandleUnitMovement();
            }

            public static PlayerManager getInstance(){
                return instance;
            }
    }
}