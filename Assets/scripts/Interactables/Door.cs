using System;
using System.Collections;
using UnityEngine;

public class Door : Selectable
{
    public enum State
    {
        Opened,
        Closed
    }

    //Dependecies
    UI ui;
    [SerializeField]
    Finisher finisher;
    //Evaluated Runtime
    Transform door;

    #region Configuration
    public float doorWidth = 2;
    public float closedDoorAngle = 0;
    public float openDoorAngle = -100;
    public float openSpeed = 7;
    #endregion

    #region Mechanics
    bool locked = true;
    public State state { get; private set; } = State.Closed;
    string nope = "You need a key";
    #endregion

    public override void Start()
    {
        base.Start();
        ui = GameManager.instance.ui;
        door = transform.GetChild(0).GetChild(1);
        //door.rotation = Quaternion.Euler(new Vector3(0, 0, closedDoorAngle));
        Vector3 finisherPos = new Vector3(transform.position.x, 0, transform.position.z) + transform.forward * -21;
        Instantiate(finisher, finisherPos, Quaternion.identity);
    }

    #region Logic
    public override ICommand Clicked()
    {
       switch(state)
        {
            case State.Closed:
                if (locked & !ui.player.HasKey)
                {
                    return new Command("Door", "Do you want to open a door?", Nope);
                }

                Chest chest = FindObjectOfType<Chest>();
                if (chest.state == Chest.State.Open)
                {
                    nope = "You didn't close the chest!";
                    return new Command("Door", "Do you want to open a door?", Nope);
                }
                return new Command("Door", "Do you want to open a door?", Open);

            case State.Opened:
                return new Command("Door", "Do you want to close a door?", Close);
        }
        return null;
    }
    public string Open()
    {
        StartCoroutine(RotateToMatch(openDoorAngle));
        state = State.Opened;
        return "Door opened";
    }
    public string Close()
    {
        nope = "You lost a key!";
        StartCoroutine(RotateToMatch( closedDoorAngle));
        state = State.Closed;
        return "Door closed";
    }
    public string Nope()
    {
        return nope;
    }
    #endregion

    #region Mechanics
    IEnumerator RotateToMatch(float rotation)
    {
        Quaternion fromRot = door.localRotation;
        Quaternion toRot = Quaternion.Euler(new Vector3(door.rotation.x, door.rotation.y, rotation));

        for (float i = 0; i < 1; i += Time.deltaTime * openSpeed)
        {
            door.localRotation = Quaternion.Slerp(fromRot, toRot, i);
            yield return null;
        }
    }
    #endregion
}