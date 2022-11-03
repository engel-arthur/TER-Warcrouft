using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Values;

//TODO externaliser la gestion des états? (un peu bizarre de l'avoir dans inputhandler)
namespace InputManager {

    public class InputHandler : MonoBehaviour
    {
        public static InputHandler instance;
        private List<Transform> selectedUnits = new List<Transform>();
        private Ray ray;
        private RaycastHit hit;

        public GameObject clickIndicator;

        private bool isDragging = false;

        private Vector3 mousePos;


        // Start is called before the first frame update
        void Start()
        {
            instance = this;
        }


        private void OnGUI()
        {
            if (isDragging)
            {
                Rect rect = MultiSelect.GetScreenRect(mousePos, Input.mousePosition);
                MultiSelect.DrawScreenRect(rect, new Color(0f, 0f, 0f, 0.25f)); //new Color(0f, 0f, 0f, 0.25f)
                MultiSelect.DrawScreenRectBord(rect, 3, Color.blue);
            }
        }


        public void HandleUnitMovement()
        {
            if(Input.GetMouseButtonDown(0)) {
                // Clic gauche enfoncé
                mousePos = Input.mousePosition;
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if(Physics.Raycast(ray, out hit)) {
                    LayerMask layerHit = hit.transform.gameObject.layer;

                    switch(layerHit.value) {
                        case 6:
                            SelectUnit(hit.transform, Input.GetKey(KeyCode.LeftShift));
                            break;
                        default:
                            isDragging = true;
                            UnselectSelectedUnits();
                            break;
                    }
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                foreach (Transform child in Player.PlayerManager.instance.playerUnits)
                {
                    foreach(Transform unit in child) //Les "child" sont Workers et Warriors, donc pour toutes les unités qui se trouvent dans Workers et Warriors
                    {
                        if (isWithinSelectionBounds(unit))
                        {
                             SelectUnit(unit, true);
                        }        
                    }
                }
                /*foreach (Transform child in Player.PlayerManager.instance.enemyUnits)
                {
                    foreach (Transform unit in child) //Les "child" sont Workers et Warriors, donc pour toutes les unités qui se trouvent dans Workers et Warriors
                    {
                        if (isWithinSelectionBounds(unit))
                        {
                            SelectUnit(unit, true);
                        }
                    }
                }*/
                isDragging = false;
            }

            if(Input.GetMouseButtonDown(1)) {
                // Clic droit enfoncé
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(ray, out hit))
                    {
                        LayerMask layerHit = hit.transform.gameObject.layer;
                        Vector3 clickIndicatorPosition = hit.point;
                        clickIndicatorPosition.y = 0;
                        GameObject clickIndicatorObject = Instantiate(clickIndicator, clickIndicatorPosition, Quaternion.identity) as GameObject;
                        if(HaveSelectedUnits()) {
                            switch (layerHit.value) 
                            {
                                case 6: 
                                    //couche des unités
                                    //Faire quelque chose
                                    break;
                                case 7: //Ennemis
                                    AbstractUnit enemyUnitObject = getUnitComponentFromTransform(hit.transform);
                                    foreach(Transform unit in selectedUnits){
                                        AbstractUnit unitObject = getUnitComponentFromTransform(unit);
                                        unitObject.SetTargetEnemy(enemyUnitObject);
                                        unitObject.SetState(Globals.unitStates.attacking);
                                    }
                                    break;

                                case 8: //Ressources
                                    AbstractResource resourceObject = getResourceComponentFromTransform(hit.transform);
                                    foreach(Transform unit in selectedUnits){
                                        AbstractUnit unitObject = getUnitComponentFromTransform(unit);
                                        unitObject.SetTargetResource(resourceObject);
                                        unitObject.SetTarget(hit.point);
                                        unitObject.SetState(Globals.unitStates.harvesting);
                                    }
                                    break;

                                default:
                                    foreach(Transform unit in selectedUnits){
                                        if(unit != null){ //pour pouvoir continuer à bouger la sélection même si l'un d'entre eux est mort
                                            AbstractUnit unitObject = getUnitComponentFromTransform(unit);
                                            unitObject.SetTarget(hit.point);
                                            unitObject.SetState(Globals.unitStates.walking);
                                        }
                                    }
                                    break;
                            }
                        }
                    }
            }
        }

        private void SelectUnit(Transform unit, bool canMultiSelect = false) {
            if(!canMultiSelect)
                UnselectSelectedUnits();
            
            selectedUnits.Add(unit);
            AbstractUnit unitObject = getUnitComponentFromTransform(unit);
            //Debug.Log("Selection de " + unitObject.GetName());
            if(unitObject!=null){
                unitObject.SetHighlightCircleVisibility(true);
            }
        }

        private void UnselectSelectedUnits() {
            Debug.Log("Déselection");
            AbstractUnit unitObject;
            foreach(Transform unit in selectedUnits) {
                if(unit){

                unitObject = getUnitComponentFromTransform(unit);
                unitObject.SetHighlightCircleVisibility(false);
                }
            }
            selectedUnits.Clear();
        }

        private bool HaveSelectedUnits() => selectedUnits.Count > 0;

        private bool isWithinSelectionBounds(Transform tf)
        {
            if (!isDragging)
            {
                return false;
            }

            Camera cam = Camera.main;
            Bounds vpBounds = MultiSelect.GetVPBounds(cam, mousePos, Input.mousePosition);
            return vpBounds.Contains(cam.WorldToViewportPoint(tf.position)); 
        }

        //TODO changer ces fonctions en une fonction polymorphique getComponentFromTransform<T>
        private AbstractUnit getUnitComponentFromTransform(Transform unit) {
            //besoin de prendre le parent du transform car le collider détecté par le raycast est un enfant dans les prefab des unités (alors que le script est au sommet de la hiérarchie)
                return unit.parent.gameObject.GetComponent<AbstractUnit>();
        }

        private AbstractResource getResourceComponentFromTransform(Transform resource) {
            return resource.gameObject.GetComponent<AbstractResource>();
        }

        public void removeUnitFromList(Transform unit) {
            if(selectedUnits.Remove(unit)) {
                Debug.Log("On supprime une unité de la sélection!");
            }
            else {
                Debug.Log("Pas trouvé l'unité à supprimer :(");
            }
        }
    }
}
