using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BackgroundHexagonGroupSelector : MonoBehaviour
{
    public List<BackgroundHexagon> twoClosestNeighborBGHexs;
    BackgroundHexagon myBackgroundHexagon;

    private void Awake()
    {
        myBackgroundHexagon = GetComponent<BackgroundHexagon>();
    }
    public void SelectGroup(Vector2 clickPos) // Selection of group by clicking on them.  Closest hexagons to the click point will be selected
    {
        Board.Instance.selectedGroup.Clear();
        Board.Instance.hitHexagon = myBackgroundHexagon;
        twoClosestNeighborBGHexs = FindTwoClosestNeighborToAPoint(myBackgroundHexagon.MyNeighborBackgroundHexagons, clickPos);
        if (Board.Instance.hitHexagon == myBackgroundHexagon)
        {
            foreach (BackgroundHexagon bgHex in twoClosestNeighborBGHexs)
            {
                bgHex.myHexagon.HighlightHexagonWhenSelected();

            }
            myBackgroundHexagon.myHexagon.HighlightHexagonWhenSelected();
        }
        Board.Instance.selectedGroup.Add(myBackgroundHexagon);
        Board.Instance.selectedGroup.Add(twoClosestNeighborBGHexs[0]);
        Board.Instance.selectedGroup.Add(twoClosestNeighborBGHexs[1]);

    }

    void CloseOutlineWhenSelectedHexagonChanged() // "Selected Image" should be closed after any new selection or explosion 
    {
        if (Board.Instance.hitHexagon == myBackgroundHexagon)
        {
            Board.Instance.hitHexagon = null;
            foreach (BackgroundHexagon bgHex in twoClosestNeighborBGHexs)
            {
                if (bgHex.myHexagon != null)
                {
                    bgHex.myHexagon.DeselectHexagon();
                }
            }
            if (myBackgroundHexagon.myHexagon != null)
            {
                myBackgroundHexagon.myHexagon.DeselectHexagon();
            }
        }

        twoClosestNeighborBGHexs.Clear();
    }

    void OnEnable()
    {
        TaskManager.OnAnExplosion += CloseOutlineWhenSelectedHexagonChanged;
        InputManager.OnGetHexagonSelectionInput += CloseOutlineWhenSelectedHexagonChanged;
    }

    void OnDisable()
    {
        TaskManager.OnAnExplosion -= CloseOutlineWhenSelectedHexagonChanged;
        InputManager.OnGetHexagonSelectionInput -= CloseOutlineWhenSelectedHexagonChanged;
    }

    List<BackgroundHexagon> FindTwoClosestNeighborToAPoint(List<BackgroundHexagon> neighborsList, Vector2 clickPos)
    {
        List<BackgroundHexagon> tempBGHexList = new List<BackgroundHexagon>();
        List<BackgroundHexagon> twoClosest = new List<BackgroundHexagon>();

        tempBGHexList = neighborsList.OrderBy(x => Vector2.Distance(clickPos, x.transform.position)).ToList();

        twoClosest.Add(tempBGHexList[0]); // Closest hexagon is selected, Second one should first one's neighbor and this bgHexagons's neighbor

        foreach (BackgroundHexagon bgHex in tempBGHexList) //searching from closest to far, therefore we can break loop after finding first one
        {
            if (bgHex.MyNeighborBackgroundHexagons.Contains(myBackgroundHexagon) && bgHex.MyNeighborBackgroundHexagons.Contains(tempBGHexList[0]))
            {
                twoClosest.Add(bgHex);
                break;
            }
        }
        return twoClosest;
    }
}
