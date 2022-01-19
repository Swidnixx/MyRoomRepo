using System;
using System.Collections;
using UnityEngine;

public class Key : Selectable
{
    //  Dependencies
    Player player;
    AudioSystem sa;
    public Chest chest;

    [SerializeField]
    private AudioClip clip;

    private float step = 1;

    public override void Start()
    {
        player = GameManager.instance.player;
        sa = GameManager.instance.audioSystem;
      
        base.Start();
        StartCoroutine(Reveal());
    }

    #region Logic
    public override ICommand Clicked()
    {
        return new Command("Key", "Wanna pick a key?", Pick);
    }
    private string Pick()
    {
        chest.Reset();
        player.SetKey(this);
        Invoke("destroy", 0.1f);
        return "Key picked";
    }
    #endregion

    #region Mechanics
    private IEnumerator Reveal()
    {
        float toPos_y = transform.position.y + 2;

        while(transform.position.y < toPos_y)
        {
            transform.position += new Vector3(0, Time.deltaTime * step, 0);
            transform.Rotate(0, step, 0);
            yield return null;
        }
    }
    private void destroy()
    {
        sa.PlaySoundEffect(clip);
        Destroy(this.gameObject);
    }
    #endregion
}