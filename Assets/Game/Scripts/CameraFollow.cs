using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private Player player;

    void Awake()
    {
        player = FindObjectOfType<Player>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (player == null) return;
        transform.position = Vector3.Lerp(transform.position, player.transform.position, 0.1f);
    }
}
