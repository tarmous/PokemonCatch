using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;

public class Player : MonoBehaviour
{
    private NavMeshAgent agent;
    private List<Pokemon> currentTeam = new List<Pokemon>(3);

    public delegate void TeamRefreshedDelegate(List<Pokemon> team);
    public TeamRefreshedDelegate TeamRefreshedEvent;

    public List<Pokemon> CurrentTeam { get { return currentTeam; } }
    public void AddPokemon(Pokemon p)
    {
        // max party of 3
        if (currentTeam.Count >= 3) return;
        currentTeam.Add(p);
        SaveData();
        TeamRefreshedEvent?.Invoke(currentTeam);
    }

    public void RemovePokemon(int index)
    {
        currentTeam.RemoveAt(index);
        SaveData();
        TeamRefreshedEvent?.Invoke(currentTeam);
    }

    public void RemovePokemon(Pokemon p)
    {
        currentTeam.Remove(p);
        SaveData();
        TeamRefreshedEvent?.Invoke(currentTeam);
    }
    private void LoadData()
    {
        GameLoader gl = new GameLoader();
        SaveGameData data = gl.LoadFromFile();
        currentTeam = data.currentTeam;
    }

    private void SaveData()
    {
        SaveGame sg = new SaveGame(new SaveGameData(this.currentTeam));
        // sg.saveGameData.currentTeam = this.currentTeam;
        sg.SaveToFile();
    }

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }


    void Start()
    {
        LoadData();
    }

    void Update()
    {
        bool isOverUI = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
        if (isOverUI) return;
        if (Input.GetMouseButtonDown(0))
        {
            //create a ray cast and set it to the mouses cursor position in game
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                NavMeshHit nHit;
                NavMesh.SamplePosition(hit.point, out nHit, 1.0f, NavMesh.AllAreas);
                agent.SetDestination(nHit.position);
            }
        }
    }
}
