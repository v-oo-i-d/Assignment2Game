using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScene : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    MenuPlay play;
    MenuQuit quit;
    Scene mainScene;
    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Update is called once per frame
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        mainScene = scene;
        play = GameObject.Find("MainMenu").GetComponent<MenuPlay>();
    }

    public void Restart()
    {
        SceneManager.LoadScene("TestingScene");
        SceneManager.SetActiveScene(mainScene);
        play.StartFade();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
