using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AboutCanvas : MonoBehaviour
{

    public static AboutCanvas instance;
    [SerializeField]
    private GameObject MainPanel;

    public void Show()
    {
        MainPanel.SetActive(true);
    }

    public void Hide()
    {
        MainPanel.SetActive(false);
    }


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Debug.Log("instance created: " + this.gameObject.name);
        }
        else
        {
            DestroyImmediate(this.gameObject);
        }

        Hide();
    }
}
