using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEngine.SceneManagement;


public class PopUpLoseController : MonoBehaviour
{
    public int nbClic = 0;

    void Update()
    {
        Resources.Load<VisualTreeAsset>("Assets/RW/Editor/PopUpLose.uxml");

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
