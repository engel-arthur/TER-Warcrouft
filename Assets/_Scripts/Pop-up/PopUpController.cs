using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEngine.SceneManagement;

using Player;

public class PopUpController : MonoBehaviour
{

    public int nbClic = 0;

    private void Update()
    {
        
        Resources.Load<VisualTreeAsset>("Assets/RW/Editor/PopUpUI.uxml");

        var root = GetComponent<UIDocument>().rootVisualElement;
        Button backToMenuButton = root.Q<Button>("MenuButton");

        backToMenuButton.clickable.clicked += () =>
        {
            if (nbClic < 1)
            {
                Debug.Log("Retour au menu !");
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
                nbClic++;
            }

        };
        

    }
}