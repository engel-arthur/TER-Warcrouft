using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class CountDownTuto : MonoBehaviour
{

    private void Start()
    {
        StartCoroutine(timer(60));
    }

    IEnumerator timer(float time)
    {
        while(time > 0)
        {
            time--;
            yield return new WaitForSeconds(1f);
            var root = GetComponent<UIDocument>().rootVisualElement;
            Label countdown = root.Q<Label>("tutoCountDown");
            countdown.text = time.ToString() + " secondes";

        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
