using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScreenScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        StartCoroutine(ToMainMenu());
    }

    IEnumerator ToMainMenu() {
        yield return new WaitForSeconds(5);
        LoadMenuScene();
    }

    public void LoadMenuScene() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MenuScene");
    }

}
