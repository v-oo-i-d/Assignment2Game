using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFinish : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public AudioClip leavingRelief;
    private float maxDistance = 7.0f;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (LookingAtButton())
            {
                PressButton();
            }
        }
    }
    private bool LookingAtButton()
    {
        Camera cam = Camera.main;
        Ray ray = new(cam.transform.position, cam.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
        {
            if (hit.transform == transform)
            {
                return true;
            }
        }

        return false;
    }
    void PressButton()
    {
        AudioSource door = GetComponent<AudioSource>();
        StartCoroutine(PlayAndWait(door, leavingRelief));
    }
    IEnumerator PlayAndWait(AudioSource source, AudioClip clip)
    {
        source.PlayOneShot(clip);
        yield return new WaitForSeconds(clip.length);
        SceneManager.LoadScene("TrafficScene");
    }
}
