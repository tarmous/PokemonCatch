using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using RSG;

public class PokemonDetailsCanvas : MonoBehaviour
{
    public Player player;
    public static PokemonDetailsCanvas instance;
    [SerializeField]
    private GameObject MainPanel, AddButton, RemoveButton;
    [SerializeField]
    private Image pokemonImage;
    [SerializeField]
    private TextMeshProUGUI nameText, base_experience, height, weight;
    private PromiseTimer promiseTimer = new PromiseTimer();


    public Pokemon p;

    public void HandleAddPokemonToTeam()
    {
        Hide();
        instance.player.AddPokemon(instance.p);
    }

    public void HandleRemovePokemonFromTeam()
    {
        Hide();
        instance.player.RemovePokemon(instance.p);
    }

    public void Show(bool add = false, bool remove = false)
    {
        instance.player = player;

        AddButton.SetActive(add);
        RemoveButton.SetActive(remove);

        MainPanel.SetActive(true);
    }

    public void Hide()
    {
        MainPanel.SetActive(false);
    }

    ~PokemonDetailsCanvas()
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
        // instance.p = API_PokemonRequest.GetPokemon(1);
    }

    void Start()
    {
        UpdateDetails(p);
    }

    void Update()
    {
        promiseTimer.Update(Time.deltaTime);
    }

    public void UpdateDetails(Pokemon p)
    {
        //if (instance == null) Debug.Log("instance null");
        instance.p = p;
        nameText.text = p.name;
        base_experience.text = p.base_experience.ToString();
        height.text = p.height.ToString();
        weight.text = p.weight.ToString();

        DownloadImage(p.sprites.front_default).Catch((System.Exception e) =>
        {
            //Debug.Log(e.Message);
        });
    }

    // Normaly i should cache and manage downloaded images instead of requesting them every time
    IPromise DownloadImage(string MediaUrl)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
        Promise promise = new Promise();

        var webRequest = request.SendWebRequest();
        promiseTimer.WaitUntil(_ =>
            {
                return webRequest.isDone;
            }).Then(() =>
            {
                if ((request.result == UnityWebRequest.Result.ConnectionError) || (request.result == UnityWebRequest.Result.ProtocolError))
                {
                    pokemonImage.sprite = Sprite.Create(null, new Rect(0, 0, 96f, 96f), new Vector2(0.5f, 0.5f));
                    //Debug.Log(request.error);
                    promise.Reject(new System.Exception(request.error));
                }
                else
                {
                    Texture2D t = ((DownloadHandlerTexture)request.downloadHandler).texture;
                    pokemonImage.sprite = Sprite.Create(t, new Rect(0, 0, t.width, t.height), pokemonImage.rectTransform.pivot);
                    promise.Resolve();
                }

            });
        return promise;
    }
}
