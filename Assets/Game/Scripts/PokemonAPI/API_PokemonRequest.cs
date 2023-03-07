using UnityEngine;
using System.Net;
using System.IO;
using RSG;

public static class API_PokemonRequest
{
    public static Pokemon GetRandomPokemon()
    {
        return GetValidatedPokemon(Random.Range(1, 100));
    }

    public static Pokemon GetValidatedPokemon(int id)
    {
        Pokemon p = null;
        GetPokemon(id).Then((Pokemon po) => {  p = po; });
        return p;

    }

    private static Promise<Pokemon> GetPokemon(int id)
    {
        Promise<Pokemon> promise = new Promise<Pokemon>();
        // GET https://pokeapi.co/api/v2/pokemon/{id or name}/
        HttpWebRequest request = WebRequest.Create("https://pokeapi.co/api/v2/pokemon/" + id.ToString() + "/") as HttpWebRequest;
        HttpWebResponse response = request.GetResponse() as HttpWebResponse;
        // HttpStatusCode.
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string json = reader.ReadToEnd();
        promise.Resolve(JsonUtility.FromJson<Pokemon>(json));
        return promise;
    }
}
