using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RSG;
using System.Linq;
using UnityEngine.SceneManagement;

public class SceneReset : MonoBehaviour
{

    [SerializeField]
    private Pokeball[] pokeballs;
    PromiseTimer promiseTimer = new PromiseTimer();

    void Awake()
    {
        pokeballs = FindObjectsOfType<Pokeball>();
        CheckInactiveHandler();
    }

    private bool CheckActive()
    {
        bool b = false;
        for (int i = 0; i < pokeballs.Length; i++)
        {
            b = (pokeballs[i] ? pokeballs[i].gameObject.activeSelf : false);
            if (b) break;
        }
        return b;
    }

    private IPromise CheckInactiveHandler()
    {
        return promiseTimer.WaitUntil(_ =>
        {
            return !CheckActive();
        }).Then(() =>
        {
            return promiseTimer.WaitFor(5f).Then(() =>
            {
                SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
            });

        });
    }


    // Update is called once per frame
    void Update()
    {
        promiseTimer.Update(Time.deltaTime);
    }
}
