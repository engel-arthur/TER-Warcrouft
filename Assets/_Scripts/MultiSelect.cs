using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InputManager
{
    public static class MultiSelect
    {
        private static Texture2D _whiteTexture;

        //Fonction qui regarde si notre whiteTexture existe et si elle existe pas, la crée
        public static Texture2D WhiteTexture
        {
            get
            {
                if (_whiteTexture == null)
                {
                    _whiteTexture = new Texture2D(1, 1);
                    _whiteTexture.SetPixel(0, 0, Color.white);
                    _whiteTexture.Apply();
                }

                return _whiteTexture;
            }
        }

        //Fonction qui dessine un rectangle à l'écran
        public static void DrawScreenRect(Rect rect, Color color)
        {
            GUI.color = color;
            GUI.DrawTexture(rect, WhiteTexture);
        }

        //Fonction pour faire la bordure du rectangle
        public static void DrawScreenRectBord(Rect rect, float thickness, Color color)
        {
            //Top
            DrawScreenRect(new Rect(rect.xMin, rect.yMin, rect.width, thickness), color);
            //Bottom
            DrawScreenRect(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness), color);
            //Left
            DrawScreenRect(new Rect(rect.xMin, rect.yMin, thickness, rect.height), color);
            //Right
            DrawScreenRect(new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height), color);
        }

        public static Rect GetScreenRect(Vector3 screenPos1, Vector3 screenPos2)
        {
            /*Debug.Log("Choppe rectangle");
            //De en bas à droite à en haut à gauche
            screenPos1.y = Screen.height - screenPos1.y;
            screenPos2.y = Screen.height - screenPos2.y;

            //Coins
            Vector3 bR = Vector3.Max(screenPos1, screenPos2);
            Vector3 tL = Vector3.Max(screenPos1, screenPos2);

            //Créer le rectangle
            return Rect.MinMaxRect(tL.x, tL.y, bR.x, bR.y);*/
            return new Rect(screenPos1.x, Screen.height - screenPos1.y, screenPos2.x - screenPos1.x, -1 * ((Screen.height - screenPos1.y) - (Screen.height - screenPos2.y)));

        }

        //Foncion qui vérifie ce qu'on a sélectionné dans le rectangle, si un objet est dedans
        public static Bounds GetVPBounds(Camera cam, Vector3 screenPos1, Vector3 screenPos2)
        {
            Vector3 pos1 = cam.ScreenToViewportPoint(screenPos1);
            Vector3 pos2 = cam.ScreenToViewportPoint(screenPos2);

            Vector3 min = Vector3.Min(pos1, pos2);
            Vector3 max = Vector3.Max(pos1, pos2);

            min.z = cam.nearClipPlane;
            max.z = cam.farClipPlane;

            Bounds bounds = new Bounds();
            bounds.SetMinMax(min, max);

            return bounds;

        }
    }
}

