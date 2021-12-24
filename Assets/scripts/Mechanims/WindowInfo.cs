using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static Selectable;

public partial class WindowInfo : MonoBehaviour
{
    //  Enums
    public enum Response
    {
        YesNo,
        Ok,
        None
    }

    //  UI elements references
    public GameObject panel;
    public Text title;
    public Text text;
    public GameObject textPanel;
    public Button buttonYes;
    public Button buttonNo;

    private void Start()
    {
        //Close();
    }

    #region Displaying Info Logic
    public void PromptPlayer(string title, string msg, Response responseType, UnityAction<int> callback = null, string buttonNoText = "ok")
    {
        Open();
        buttonYes.onClick.RemoveAllListeners();
        buttonNo.onClick.RemoveAllListeners();

        this.title.text = title;
        text.text = msg;

        buttonYes.GetComponentInChildren<Text>(true).text = "yes";
        buttonNo.GetComponentInChildren<Text>().text = buttonNoText;

        buttonYes.gameObject.SetActive(responseType == Response.YesNo ? true : false);
        buttonNo.gameObject.SetActive(true);

        ((RectTransform)buttonNo.transform).anchorMin = ((RectTransform)buttonYes.transform).anchorMin
            + (responseType == Response.YesNo ? new Vector2(0.45f, 0) : Vector2.zero);

        if (callback != null)
        {
            if (responseType == Response.YesNo)
            {
                buttonYes.onClick.AddListener(delegate { callback(1); });
            }
            buttonNo.onClick.AddListener(delegate { callback(0); });
        }
        else
        {
            buttonNo.onClick.AddListener(Close);
        }
    }
    public void Close()
    {
        panel.SetActive(false);
    }
    private void Open()
    {
        panel.SetActive(true);
    }
    #endregion
}
