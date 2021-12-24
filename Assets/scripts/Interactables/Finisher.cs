using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finisher : MonoBehaviour
{
    //Dependencies
    Door door;
    UI ui;
    WindowInfo wi;

    bool triggered;

    private void Start()
    {
        door = FindObjectOfType<Door>();
        ui = GameManager.instance.ui;
        wi = GameManager.instance.wi;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !triggered)
        {
            triggered = true;
            FinishGame();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        triggered = false;
    }

    private void FinishGame()
    {
        if(door.state == Door.State.Opened)
        {
            ui.Prompt("Game", "You didn't close the door!");
        }
        else
        {
            ui.Finish();
        }
    }
}
