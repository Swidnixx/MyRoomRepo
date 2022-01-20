using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;
using static Selectable;

public class UI : MonoBehaviour
{
    //  Dependencies
    [Inject]
    public Player player { get; private set; }
    public PlayerControls playerControls { get; private set; }
    [Inject]
    private WindowInfo wi { get; set; }
    [Inject]
    Camera mainCamera;

    //  UI elements
    [SerializeField]
    private Text timer;

    //  Private Mechanics Fields
    float startTime;

    #region Interaction Mechanics Fields
    [SerializeField]
    private float maxSelectingDistance = 14;
    private Selectable currentSelected;     // currently selected interactive object
    private ICommand currentCommand;        // Action to perform on interaction finalisation
    private bool locked;
    #endregion

    #region Unity Callbacks
    private void Awake()
    {
       // player = FindObjectOfType<Player>();
       // wi = FindObjectOfType<WindowInfo>(true);
        playerControls = new PlayerControls();

        playerControls.Player.Click.canceled += OnClicked;
    }
    private void Start()
    {
        Lock();
        wi.PromptPlayer("Game", "Hit Start to start the Game", WindowInfo.Response.Ok, StartTimer, "Start");
        //wi.PromptPlayer("Game", "Hit Start to start the Game", StartTimer, "Start");
    }
    private void Update()
    {
        if (currentSelected != null)
        {
            currentSelected.Deselected();
            currentSelected = null;
        }

        //Vector2 mousePosition = playerControls.UI.Point.ReadValue<Vector2>();
        //Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f,0));
        RaycastHit hitInfo;

        bool hit = Physics.Raycast(ray, out hitInfo, maxSelectingDistance, LayerMask.GetMask("Selectables"));
        if (hit)
        {
            Selectable selected = hitInfo.collider.GetComponent<Selectable>();
            if (selected != null)
            {
                selected.Selected();
                currentSelected = selected;
            }
        }

    }
    #endregion

    #region Input Callbacks
    public void Lock()
    {
        player.OnDisable();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        locked = true;
    }
    public void Unlock()
    {
        player.OnEnable();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        locked = false;
    }
    void OnClicked(InputAction.CallbackContext ctx)
    {
        if (currentSelected && !locked)
        {
            currentCommand = currentSelected.Clicked();
            if (currentCommand == null)
                return;
            Lock();
            wi.PromptPlayer(currentCommand.title, currentCommand.msg, WindowInfo.Response.YesNo, OnPlayerResponded, "no");
        }
    }
    void OnPlayerResponded(int response)      // 1 - yes, 0 - no
    {
        if (response == 1)
        {
            string commandResult = currentCommand.command?.Invoke();
            wi.PromptPlayer(currentCommand.title, commandResult, WindowInfo.Response.Ok, OnPlayerResponded);
        }
        else
        {
            currentCommand = null;
            wi.Close();
            Unlock();
        }
    }
    public void Prompt(string title, string msg)
    {
        Lock();
        wi.PromptPlayer(title, msg, WindowInfo.Response.Ok, OnPlayerResponded);
    }
    #endregion

    #region Timer Logic
    private void StartTimer(int i)
    {
        Unlock();
        playerControls.Enable();

        startTime = Time.time;
        StartCoroutine(Stopper());
        wi.Close();
    }
    private IEnumerator Stopper()
    {
        while (true)
        {
            timer.text = FormatTime(Time.time - startTime);
            yield return new WaitForSeconds(1);
        }
    }
    private string FormatTime(float time)
    {
        TimeSpan ts = new TimeSpan((long)(time * 10000000));
        return ts.ToString("mm':'ss");
    }
    public void Finish()
    {
        Lock();
        StopAllCoroutines();

        float endTime = Time.time;
        float time = endTime - startTime;

        float bestScore;
        if (!PlayerPrefs.HasKey("BestScore"))
        {
            bestScore = 0;
            wi.PromptPlayer("Game", $"Your score: {FormatTime(time)}", WindowInfo.Response.Ok, Restart, "Restart");
        }
        else
        {
            bestScore = PlayerPrefs.GetFloat("BestScore");
            if (time < bestScore)
            {
                PlayerPrefs.SetFloat("BestScore", time);
                wi.PromptPlayer("Game", $"Your score: {FormatTime(time)}\nPrevious best score: {FormatTime(bestScore)}", WindowInfo.Response.Ok, Restart, "Restart");
            }
            else
            {
                wi.PromptPlayer("Game", $"Your score: {FormatTime(time)}\nBest score: {FormatTime(bestScore)}", WindowInfo.Response.Ok, Restart, "Restart");
            }
        }
    }
    void Restart(int i)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    #endregion
}