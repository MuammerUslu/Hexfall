using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelRegulator : MonoBehaviour
{
    public void Awake()
    {
        GameManager.gameCanvas = FindObjectOfType<GameCanvas>();
    }
    void Start()
    {
        GameManager.Instance.StaticValuesFalse();
    }


}