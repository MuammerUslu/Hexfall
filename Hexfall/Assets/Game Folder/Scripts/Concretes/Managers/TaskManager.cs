using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    private static TaskManager instance;
    public static TaskManager Instance  // Not Persistent Instance.
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<TaskManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(TaskManager).Name;
                    instance = obj.AddComponent<TaskManager>();
                }
            }
            return instance;
        }
    }

    HexagonGroupRotator _hexagonGroupRotator; // Rotate selected hexagons 120 degree
    FindMatchedHexagons _findMatchedHexagons; // Search for bghexagons that should explode and return list of bghexagons
    ExplodeHexagonList _explodeHexagons; // Explode bg hexagons that are found by _explosionSearch
    RefreshHexagonPositions _refreshHexagonPositions; // After explosion fall current hexagons to empty areas below
    IFillGaps _fillGaps; // After gaps at the bottom of the board are filled
    DetectAvailableMoves _detectAvailableMoves; // After each cycle check if there is any available move 

    public static Action OnCycleStart, OnCycleEnd, OnAnExplosion;
    public static Action<bool> OnCycleEndWithoutExplosion;

    bool goNextCycle; // Becomes true before each cycle and becomes false if rotation is interrupted by explosion

    private void Awake()
    {
        _findMatchedHexagons = new FindMatchedHexagons(Board.Instance);
        _explodeHexagons = new ExplodeHexagonList();
        _refreshHexagonPositions = new RefreshHexagonPositions(Board.Instance);
        _hexagonGroupRotator = new HexagonGroupRotator(Board.Instance);
        _fillGaps = new FillGapsByInstantiatingNewHexagons(Board.Instance); // create new hexagons and fill the top part of the board
        _detectAvailableMoves = new DetectAvailableMoves(Board.Instance);
    }

    void RotateAndStartCycle(RotationType rotType)
    {
        StartCoroutine(RotateAndStartCycleIENumerator(rotType));
    }
    IEnumerator RotateAndStartCycleIENumerator(RotationType rotType)
    {
        OnCycleStart?.Invoke();

        goNextCycle = true; // is for cancelling the coroutines, if some hexagons explode before rotating 360 degrees (120,240,360)

        Vector3 selectedCenterPoint = Board.Instance.SelectedGroupCenterPoint;

        for (int i = 0; i < 3; i++) // rotation will be 3 times and each one is 120 degrees 
        {
            if (goNextCycle)
            {
                yield return StartCoroutine(_hexagonGroupRotator.RotateGroup(rotType, selectedCenterPoint));
                _hexagonGroupRotator.DeliverHexagonsToNewBgHexsAfterRotation(Board.Instance.selectedGroup);
                yield return StartCoroutine(Cycle()); // Check explotions and refresh positions if needed
                yield return new WaitForSeconds(0.05f);
            }
        }

        OnCycleEndWithoutExplosion?.Invoke(!goNextCycle); // goNextCycle means explosion occured

        OnCycleEnd?.Invoke();

        // Check For Available Moves At The End
        if (GameManager.AreWeInGamePlay())
        {
            if (!_detectAvailableMoves.AreThereAnyAvailableMove())
            {
                GameManager.Instance.failReason = GameManager.FailReason.NoAvailableMove;
                GameManager.OnLevelFailed?.Invoke();
            }
        }
    }

    IEnumerator Cycle() // Rotates 120 degree, and controls explosion and new positions.
    {
        bool noExplosionsRemaining = false;
        int counter = -1;  // explosion counter

        while (noExplosionsRemaining == false) //if there are explosions cycle ends, else go next cycle
        {
            counter++;
            // We must not check explosions between all hexagons each time.
            // First explosion should be searched between selected group and their neighbors
            // Explosions due to chain reaction should be searched between all hexagons, because every hexagon may be changed their positions.

            //Find hexagons that should explode
            List<BackgroundHexagon> explosionHexagons = counter == 0 ? _findMatchedHexagons.FindHexagonsThatWillExplodeBetweenSelectedHexagons() : _findMatchedHexagons.FindHexagonsThatWillExplodeBetweenAllHexagons();

            //if explosionHexagons is not empty, refresh board by filling the gaps 
            if (explosionHexagons.Count > 0)
            {
                yield return new WaitForSeconds(0.1f);
                _explodeHexagons.ExplodeHexagons(explosionHexagons);  // Explode found hexagons
                OnAnExplosion?.Invoke(); // Inform Explosion
                yield return StartCoroutine(_refreshHexagonPositions.RefreshPositions()); // current hexagons refresh pos
                _fillGaps.FillEmptyGaps(); // instantiate new hexagons
                yield return new WaitForSeconds(0.5f);
                goNextCycle = false; // Rotation cancelled before rotating 360 degree
            }
            else
            {
                noExplosionsRemaining = true; // dont check for chain reaction, break the loop. Already there is no explosion.
            }
        }
    }

    private void OnEnable()
    {
        InputManager.OnGetRotationInput += RotateAndStartCycle;
    }
    private void OnDisable()
    {
        InputManager.OnGetRotationInput -= RotateAndStartCycle;
    }
}
