using UnityEngine;

public enum KeycardType { Blue, Red, Yellow }

public class Keycard : MonoBehaviour
{
    public KeycardType type;
    private PlayerCharacter player;
    public GameObject door, reader;
    public float maxDistance = 7f;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerCharacter>();
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
    }

    public void Use()
    {
        if (!player.HasKeycard(this)) return;

        player.UseKeycard(this);
        door.GetComponent<Door>().Unlock();
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