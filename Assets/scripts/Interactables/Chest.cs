using System.Collections;
using UnityEngine;

public class Chest : Selectable
{
    public enum State
    {
        Open,
        Closed
    }

    //Dependecies
    public Key key;

    #region Configuration
    public Transform ChestTop;
    public float openSpeed = 7;
    #endregion
    private bool switchingState = false;
    private bool keyLocked = false;
    public State state { get; private set; } = State.Closed;

    #region Logic
    public override ICommand Clicked()
    {
        if (switchingState)
            return null;
        switch (state)
        {
            case State.Closed:
                return new Command("Chest", "Do you want to open a chest?", Open);

            case State.Open:
                return new Command("Chest", "Do you want to close a chest?", Close);
        }

        return null;
    }
    public void Reset()
    {
        keyLocked = false;
    }
    private string Close()
    {
        GameManager.instance.player.SetKey(null);

        state = State.Closed;
        switchingState = true;
        StartCoroutine(RotateToMatch(0));

        Door door = FindObjectOfType<Door>();
        door.Close();

        return "Chest has been closed";
    }
    private string Open()
    {
        state = State.Open;
        switchingState = true;
        StartCoroutine(RotateToMatch(-130));

        if (!keyLocked)
        {
            Instantiate(key, transform.position, Quaternion.identity).GetComponent<Key>().chest = this;
            keyLocked = true;
        }

        return "Chest has been opened";
    }
    #endregion

    #region Mechanics
    IEnumerator RotateToMatch(float targetRot)
    {
        Quaternion fromRot = ChestTop.localRotation;
        Quaternion toRot = Quaternion.Euler(new Vector3(ChestTop.rotation.x, ChestTop.rotation.y, targetRot));
        while (Quaternion.Angle(ChestTop.localRotation, toRot) > 3.5f)
        {
            if(state == State.Open)
                ChestTop.rotation *= Quaternion.AngleAxis(Time.deltaTime * openSpeed, Vector3.back);
            else
                ChestTop.rotation *= Quaternion.AngleAxis(Time.deltaTime * openSpeed, Vector3.forward);
            yield return null;
        }
        ChestTop.rotation = toRot;
        switchingState = false;
    }
    #endregion
}