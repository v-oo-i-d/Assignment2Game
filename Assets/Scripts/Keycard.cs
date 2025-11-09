using UnityEngine;
using UnityEngine.UI;

public enum KeycardType { Blue, Red, Yellow }

public class Keycard : MonoBehaviour
{
    public KeycardType type;
    private PlayerCharacter player;
    public GameObject door, reader;
    public float maxDistance = 7f;

    private Image keycardImage;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerCharacter>();
        keycardImage = GameObject.Find("GUIs").transform.Find("Keycard").transform.Find("Image").GetComponent<Image>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (LookingAtKeycard()) Pickup();
            if (LookingAtReader()) Use();
        }
    }

    public void Pickup()
    {
        foreach (Transform child in transform) child.transform.gameObject.SetActive(false);
        player.PickupKeycard(this);
        keycardImage.sprite = Resources.Load<Sprite>($"{type}Keycard");
        keycardImage.preserveAspect = true;
        keycardImage.color = new(1,1,1,1);
    }

    public void Use()
    {
        if (!player.HasKeycard(this)) return;

        player.UseKeycard(this);
        SoundManager.PlaySound(SoundType.Beep);
        door.GetComponent<Door>().Unlock();
        keycardImage.sprite = null;
        keycardImage.color = new(1,1,1,0);
        Destroy(gameObject);
    }

    private bool LookingAtKeycard()
    {
        Camera cam = Camera.main;
        Ray ray = new(cam.transform.position + cam.transform.forward * 1.1f, cam.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
        {
            return hit.transform == transform;
        }

        return false;
    }

    private bool LookingAtReader()
    {
        Camera cam = Camera.main;
        Ray ray = new(cam.transform.position, cam.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
        {
            return hit.transform == reader.transform;
        }

        return false;
    }
}