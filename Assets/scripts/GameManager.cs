using ModestTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameManager : MonoBehaviour
{
    //  Singleton
    public static GameManager instance;

    //Dependencies
    public AnimationsManager animationsManager{ get; private set; }
    [Inject]
    public Player player { get; private set; }
    [Inject]
    public WindowInfo wi { get; private set; }
    [Inject]
    public UI ui { get; private set; }
    [Inject]
    public AudioSystem audioSystem { get; private set; }

    //Configuration
    [Header("Configuration")]
    public Text timer;

    #region Unity Callbacks
    private void Awake()
    {
        //  These are GameObjects on the Scene
        //player = FindObjectOfType<Player>();
        //ui = FindObjectOfType<UI>();
        //wi = FindObjectOfType<WindowInfo>(true);
        //audioSystem = FindObjectOfType<AudioSystem>();
    }

    void Start()
    {

        //  Singleton
        if (instance == null)
            instance = this;
        else
            Debug.LogError("Too many GM's: " + this.gameObject);

        // Constructed Runtime
        animationsManager = new AnimationsManager(player.animator);
    }
    #endregion

    #region GamePlay
    #endregion
}
