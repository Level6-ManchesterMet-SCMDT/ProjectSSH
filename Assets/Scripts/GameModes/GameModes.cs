using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class GameModes : MonoBehaviour
{
    public GameManager gameManager;

    public abstract void RunGameMode(GameObject playerPrefab);
}
