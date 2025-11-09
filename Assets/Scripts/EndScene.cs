using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScene : MonoBehaviour
{
    private MenuPlay play;
    // private MenuQuit quit;
    private Scene mainScene;

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

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
