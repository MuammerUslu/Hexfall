using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Board : MonoBehaviour
{
    private static Board instance;
    public static Board Instance  // Not Persistent Instance.
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Board>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(Board).Name;
                    instance = obj.AddComponent<Board>();
                }
            }
            return instance;
        }
    }

    [SerializeField] private BackgroundHexagon backgroundHexagonPrefab;

    [SerializeField] private int rowCount, columnCount;

    [SerializeField] private int bombHexScoreFrequency = 1000;

    [SerializeField] private List<Color> hexagonColors;

    public List<Color> HexagonColors => hexagonColors;
    public int RowCount => rowCount;
    public int ColumnCount => columnCount;
    public int ColorCount => hexagonColors.Count;
    public int BombHexScoreFrequency => bombHexScoreFrequency;

    BoardSetUp _boardSetUp;

    public BackgroundHexagon[,] allBackgroundHexagons;

    [HideInInspector]
    public BackgroundHexagon hitHexagon;

    [HideInInspector]
    public List<BackgroundHexagon> selectedGroup = new List<BackgroundHexagon>();

    public Vector3 SelectedGroupCenterPoint
    {
        get
        {
            Vector3 selectedGroupCenterPoint = Vector3.zero;

            foreach (BackgroundHexagon bgHex in selectedGroup)
            {
                selectedGroupCenterPoint += bgHex.transform.position;
            }

            selectedGroupCenterPoint = selectedGroupCenterPoint / selectedGroup.Count; // middle of selected hexagons is found

            return selectedGroupCenterPoint;
        }
    }

    private void Awake()
    {
        allBackgroundHexagons = new BackgroundHexagon[rowCount, columnCount];

        _boardSetUp = new BoardSetUp(this);
        StartCoroutine(SetUpBoard());

    }
    IEnumerator SetUpBoard()
    {
        yield return StartCoroutine(_boardSetUp.SetUpBackgroundHexagons());
        TaskManager.OnCycleEnd?.Invoke();
    }

    public BackgroundHexagon InstantiateBackgroudHexagonAtPos(int i, int j, Vector2 pos)
    {
        BackgroundHexagon backgroundHexagon = Instantiate(backgroundHexagonPrefab, pos, Quaternion.identity);
        backgroundHexagon.transform.parent = transform;
        backgroundHexagon.transform.name = "( " + i + " , " + j + " )";
        backgroundHexagon.rowIndex = i;
        backgroundHexagon.columnIndex = j;
        allBackgroundHexagons[i, j] = backgroundHexagon;
        return backgroundHexagon;
    }


    void ClearSelectedGroup()
    {
        selectedGroup.Clear();
    }
    private void OnEnable()
    {
        TaskManager.OnAnExplosion += ClearSelectedGroup;
    }

    private void OnDisable()
    {
        TaskManager.OnAnExplosion -= ClearSelectedGroup;
    }

}




