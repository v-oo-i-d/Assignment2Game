using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainDoorKeypad : MonoBehaviour
{
    public static List<ButtonType> code = new List<ButtonType>(4);
    public static List<ButtonType> correctCode = new List<ButtonType> { ButtonType.Green, ButtonType.Purple, ButtonType.Orange, ButtonType.Orange };
    public static List<Material> displays = new List<Material>(4);
    public GameObject door;
    public Material orange, purple, green, gray;
    private bool isOpen = false;
    private bool cooldown = false;

    void Start()
    {
        displays.Add(transform.Find("First")?.gameObject.GetComponent<Renderer>().material);
        displays.Add(transform.Find("Second")?.gameObject.GetComponent<Renderer>().material);
        displays.Add(transform.Find("Third")?.gameObject.GetComponent<Renderer>().material);
        displays.Add(transform.Find("Fourth")?.gameObject.GetComponent<Renderer>().material);
    }
    void Update()
    {
        if (cooldown) return;
        int displayNumber = 0;
        foreach (ButtonType button in code)
        {
            Material input = displays[displayNumber];
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
            displays[displayNumber].color = gray.color;
            displayNumber++;
        }
        if (code.Count >= 4)
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
                cooldown = true;
                StartCoroutine(CorrectCode());
            }
            else
            {
                cooldown = true;
                StartCoroutine(InvalidCode());
            }
            return;
        }
    }
    IEnumerator InvalidCode()
    {
        for (int i = 0; i < 4; i ++)
        {
            foreach (Material d in displays)
            {
                d.color = Color.red;
            }
            yield return new WaitForSeconds(0.4f);
            foreach (Material d in displays)
            {
                d.color = gray.color;
            }
            yield return new WaitForSeconds(0.4f);
        }
        code.Clear();
        cooldown = false;
    } 
    IEnumerator CorrectCode()
    {
        for (int i = 0; i < 4; i ++)
        {
            int displayNumber = 0;
            foreach (ButtonType button in code)
            {
                Material input = displays[displayNumber];
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
            yield return new WaitForSeconds(0.4f);
            foreach (Material d in displays)
            {
                d.color = green.color;
            }
            yield return new WaitForSeconds(0.4f);
        }
        code.Clear();
    }
    public void OpenDoor()
    {
        if (!isOpen)
        door.GetComponent<Door>().Unlock();
    }
}