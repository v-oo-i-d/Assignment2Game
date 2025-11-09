using System.Collections.Generic;
using UnityEngine;

public class MainDoorKeypad : MonoBehaviour
{
    public static List<ButtonType> code = new List<ButtonType>(4);
    public static List<ButtonType> correctCode = new List<ButtonType> { ButtonType.Green, ButtonType.Purple, ButtonType.Orange, ButtonType.Orange };
    public static List<GameObject> display = new List<GameObject>(4);
    public GameObject door;
    public Material orange, purple, green, gray;
    private bool isOpen = false;

    void Start()
    {
        display.Add(transform.Find("First")?.gameObject);
        display.Add(transform.Find("Second")?.gameObject);
        display.Add(transform.Find("Third")?.gameObject);
        display.Add(transform.Find("Fourth")?.gameObject);
    }
    void Update()
    {
        int displayNumber = 0;
        foreach (ButtonType button in code)
        {
            Material input = display[displayNumber].GetComponent<Renderer>().material;
            switch (button)
            {
                case ButtonType.Green:
                    input.color = green.color;
                    break;
                case ButtonType.Purple:
                    input.color = purple.color;
                    break;
                case ButtonType.Orange:
                    input.color = orange.color;
                    break;
                default:
                    input.color = gray.color;
                    break;
            }
            displayNumber++;
        }
        while (displayNumber < 4)
        {
            display[displayNumber].GetComponent<Renderer>().material.color = gray.color;
            displayNumber++;
        }
        if (code.Count == 4)
        {
            int amountCorrect = 0;
            for (int i = 0; i < 4; i++)
            {
                if (code[i] == correctCode[i])
                {
                    amountCorrect++;
                }
            }
            if (amountCorrect == 4)
            {
                OpenDoor();
            
            }
            else
            {
                code.Clear();
            }
        }
    }
    public void OpenDoor()
    {
        if (!isOpen)
        door.GetComponent<Door>().Unlock();
    }
}