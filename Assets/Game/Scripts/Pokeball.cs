using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pokeball : MonoBehaviour
{
    [SerializeField]
    private Pokemon p;

    private Vector3 o, v = Vector3.zero;

    private void OnTriggerEnter(Collider other)
    {
        PokemonDetailsCanvas.instance.UpdateDetails(p);
        PokemonDetailsCanvas.instance.Show(true);
        Destroy(this.gameObject);
    }


    void Awake()
    {
        p = API_PokemonRequest.GetRandomPokemon();
        o = transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        v.y = Mathf.Sin(Time.time * 2f) / 4f;
        this.transform.position = o + v;
    }
}
