using System.Collections;
using UnityEngine;

internal partial class Chest : Selectable
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

    public State state { get; private set; } = State.Closed;

    #region Logic
    public override ICommand Clicked()
    {
        switch (state)
        {
            case State.Closed:
                return new Command("Chest", "Do you want to open a chest?", Open);

            case State.Open:
                return new Command("Chest", "Do you want to close a chest?", Close);
        }

        return null;
    }
    private string Close()
    {
        GameManager.instance.player.SetKey(null);

        StartCoroutine(RotateToMatch(0));
        state = State.Closed;

        Door door = FindObjectOfType<Door>();
        door.Close();

        return "Chest has been closed";
    }
    private string Open()
    {
        StartCoroutine(RotateToMatch(-130));
        state = State.Open;

        Instantiate(key, transform.position, Quaternion.identity);

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
            ChestTop.rotation *= Quaternion.AngleAxis(Time.deltaTime * openSpeed, Vector3.back);
            yield return null;
        }
    }
    #endregion
}