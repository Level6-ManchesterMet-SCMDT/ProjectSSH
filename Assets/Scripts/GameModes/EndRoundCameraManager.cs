using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndRoundCameraManager : MonoBehaviour
{

    [SerializeField] GameObject ScoreboardUpdater;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        transform.LookAt(ScoreboardUpdater.transform);
        transform.Translate(Vector3.right * Time.deltaTime);
    }
}
