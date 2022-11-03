using UnityEngine;

namespace camera
{

    public class CameraManager : MonoBehaviour
    {

        private float cameraSpeedUp = 5f;
        private float cameraSpeedDown = 5f;
        private float cameraSpeedLeft = 5f;
        private float cameraSpeedRight = 5f;

        private float maxSpeed = 30f;

        private float borderThickness = 10f;

        private float timer = 0f;

        //Pour choisir les limites du terrain
        private Vector2 cameraLimit;

        void Update()
        {
            Vector3 cameraPosition = transform.position;

            timer += Time.deltaTime;

            if (Input.mousePosition.y >= Screen.height - borderThickness) //si la souris est en haut de l'écran
            {
                if (timer > 0.5f && cameraSpeedUp < maxSpeed) //augmenter la vitesse à chaque demi seconde
                {
                    timer = 0;
                    cameraSpeedUp += 3f;
                }

                //Je déplace la camera vers le haut
                cameraPosition.z += cameraSpeedUp * Time.deltaTime;

            }

            if (Input.mousePosition.y <= borderThickness) //si la souris est en bas de l'écran
            {
                if (timer > 0.5f && cameraSpeedDown < maxSpeed) //augmenter la vitesse à chaque seconde
                {
                    timer = 0;
                    cameraSpeedDown += 3f;
                }
                //Je déplace la camera vers le bas
                cameraPosition.z -= cameraSpeedDown * Time.deltaTime;
            }
            
            if (Input.mousePosition.x <= borderThickness) //si la souris est à gauche de l'écran
            {
                if (timer > 0.5f && cameraSpeedLeft < maxSpeed) //augmenter la vitesse à chaque seconde
                {
                    timer = 0;
                    cameraSpeedLeft += 3f;
                }
                //Je déplace la camera vers la gauche
                cameraPosition.x -= cameraSpeedLeft * Time.deltaTime;
            }

            if (Input.mousePosition.x >= Screen.width - borderThickness) //si la souris est à droite de l'écran
            {
                if (timer > 0.5f && cameraSpeedRight < maxSpeed) //augmenter la vitesse à chaque seconde
                {
                    timer = 0;
                    cameraSpeedRight += 3f;
                }

                //Je déplace la camera vers la droite
                cameraPosition.x += cameraSpeedRight * Time.deltaTime;
            }

            //Empêcher la camera d'aller trop loin
            cameraLimit.x = 50;
            cameraLimit.y = 50;

            cameraPosition.x = Mathf.Clamp(cameraPosition.x, -cameraLimit.x, cameraLimit.x);
            cameraPosition.z = Mathf.Clamp(cameraPosition.z, -cameraLimit.y -30, cameraLimit.y);


            //Appliquer les changements à la camera
            transform.position = cameraPosition;
        }
    }
}

/*A mettre dans le if si on veut utiliser les touches du clavier:
 * 
 * Input.GetKey("z")
 */

/*Camera Acceleration:
 * Check this to enable acceleration when moving the camera.
 * When enabled, the camera initially moves at a speed based on the speed value,
 * and continuously increases speed until movement stops.
 * When disabled, the camera is accelerated to a constant speed based on the Camera Speed.
 */

/*
 * Regarder si la position de la souris sur l'axe y est plus haute que la taille de l'écran
 * moins la taille du bord qu'on a choisi
 */

/*Pour l'accélération:
 * Créer un timer et augmenter la vitesse après chaque seconde qui passe
 */
