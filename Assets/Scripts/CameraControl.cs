using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraControl : MonoBehaviour
{
    [SerializeField] float flowSpeed = 2f;
    [SerializeField] float yOffSet = 1f;
    [SerializeField] GameObject player;
    
    private void Update()
    {
        if (player is not null)
        {
            Vector3 newPositon = new Vector3(player.transform.position.x, player.transform.position.y + yOffSet, -10);
            transform.position = Vector3.Lerp(transform.position, newPositon, flowSpeed * Time.deltaTime);
        }
        else
        {
            Debug.Log("Player Not found");
        }
    }

    // private void OnSceneLoaded(Screen sc, LoadSceneMode mode)
    // {
    //     GameObject spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint");
    //
    //     if (spawnPoint != null && player != null)
    //     {
    //         player.transform.position = spawnPoint.transform.position;
    //         transform.position = spawnPoint.transform.position;
    //     }
    //     else
    //     {
    //         Debug.Log("No spawn point found");
    //     }
    // }
}