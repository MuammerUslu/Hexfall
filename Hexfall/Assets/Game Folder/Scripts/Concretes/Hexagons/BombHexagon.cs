using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BombHexagon : Hexagon
{
    [SerializeField] private TextMeshPro counterText;
    [SerializeField] private int minBombCounter, maxBombCounter;

    int myCounter;
    bool firstExplosionPassed;

    protected override void Start()
    {
        base.Start();
        hexagonType = HexagonType.Bomb;
        myCounter = Random.Range(minBombCounter, maxBombCounter);
        counterText.text = myCounter.ToString();
    }

    void DecresaseMyCounterIfCycleEndWithExplosion(bool explosion) // Get true from Action if there are any explosions. -->(Rotating 360 degrees does not count as a move in this game and it means no explosion)
    {
        if (explosion)
        {
            if (firstExplosionPassed) // Cycle ends after the bomb hexagons is instantiated. Therefore, we should neglect first cycle.
            {
                myCounter--;
                counterText.text = myCounter.ToString();
                GetComponent<Animator>().SetTrigger("PlayAnim");
                if (myCounter <= 0)
                {
                    GameManager.Instance.failReason = GameManager.FailReason.Bomb;
                    GameManager.OnLevelFailed?.Invoke();
                }
            }
            firstExplosionPassed = true;
        }
    }

    private void OnEnable()
    {
        TaskManager.OnCycleEndWithoutExplosion += DecresaseMyCounterIfCycleEndWithExplosion;
    }

    private void OnDisable()
    {
        TaskManager.OnCycleEndWithoutExplosion -= DecresaseMyCounterIfCycleEndWithExplosion;
    }
}
