using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using RSG;

public class PokemonTeamCanvas : MonoBehaviour
{
    public Player player;
    private List<Pokemon> currentTeam = new List<Pokemon>(3);

    public static PokemonTeamCanvas instance;
    [SerializeField]
    private GameObject MainPanel;
    [SerializeField]
    private Image pokemon1, pokemon2, pokemon3;
    private PromiseTimer promiseTimer = new PromiseTimer();

    ~PokemonTeamCanvas()
    {
        //if (instance == this) instance = null;
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

    void Update()
    {
        promiseTimer.Update(Time.deltaTime);
    }

    public void Show(Player player)
    {
        instance.player = player;
        instance.currentTeam = instance.player.CurrentTeam;
        instance.player.TeamRefreshedEvent += OnTeamRefreshed;
        RefreshView();
        MainPanel.SetActive(true);
    }
    public void Hide()
    {
        MainPanel.SetActive(false);
        instance.player.TeamRefreshedEvent -= OnTeamRefreshed;
    }


    private void OnTeamRefreshed(List<Pokemon> team)
    {
        instance.currentTeam = team;
        RefreshView();
    }

    private void RefreshView()
    {
        for (int i = 0; i < 3; i++)
        {
            if (instance == null) Debug.Log("instance null");
            // if (instance.currentTeam.Count <= i) break;
            //if (instance.currentTeam[i] == null) continue;

            DownloadImage((instance.currentTeam.Count <= i) ? string.Empty : instance.currentTeam[i].sprites.front_default, i).Catch(
                 (System.Exception e) => 
            {
                //Debug.Log(e.Message);
            });
        }
    }
    public void ShowDetails(int i)
    {
        // expecting 0, 1 or 2
        // Debug.Log("ShowDetails: " + i + ":: list size: " + instance.currentTeam.Count);
        if (instance.currentTeam.Count <= i) return;
        PokemonDetailsCanvas.instance.UpdateDetails(instance.currentTeam[i]);
        PokemonDetailsCanvas.instance.Show(false, true);
    }

    // Normaly i should cache and manage downloaded images instead of requesting them every time
    IPromise DownloadImage(string MediaUrl, int i)
    {
        Sprite s = Sprite.Create(null, new Rect(0, 0, 96f, 96f), new Vector2(0.5f, 0.5f));
        Promise promise = new Promise();


        if (!string.IsNullOrEmpty(MediaUrl))
        {
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
            //yield return request.SendWebRequest();
            var webRequest = request.SendWebRequest();
            promiseTimer.WaitUntil(_ =>
             {
                 return webRequest.isDone;
             }).Then(() =>
             {
                 if ((request.result == UnityWebRequest.Result.ConnectionError) || (request.result == UnityWebRequest.Result.ProtocolError))
                 {
                     //Debug.Log(request.error);
                     promise.Reject(new System.Exception(request.error));
                 }
                 else
                 {
                     Texture2D t = ((DownloadHandlerTexture)request.downloadHandler).texture;
                     s = Sprite.Create(t, new Rect(0, 0, t.width, t.height), new Vector2(0.5f, 0.5f));
                     promise.Resolve();
                 }
             }).Then(() =>
             {
                 SetSprite(i, s);
             });

        }
        else
        {
            SetSprite(i, s);
            promise.Reject(new System.Exception("Empty String, empty white image set"));
        }
        return promise;

    }

    private void SetSprite(int i, Sprite s)
    {
        switch (i)
        {
            case 0:
                pokemon1.sprite = s;
                break;
            case 1:
                pokemon2.sprite = s;
                break;
            case 2:
                pokemon3.sprite = s;
                break;
            default:
                break;
        }
    }
}
