using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSetUp
{
    Board _board;
    int counter;
    public BoardSetUp(Board board)
    {
        _board = board;
    }

    public IEnumerator SetUpBackgroundHexagons()
    {
        List<BackgroundHexagon> bgHexagons = new List<BackgroundHexagon>();

        for (int i = 0; i < _board.RowCount; i++)
        {
            for (int j = 0; j < _board.ColumnCount; j++)
            {
                // Positioning the tiles

                Vector2 pos;

                if (j % 2 == 0)
                {
                    pos = new Vector2(j * 0.75f, -i * (0.5f * Mathf.Sqrt(3)));
                }
                else
                {
                    pos = new Vector2(j * 0.75f, -i * (0.5f * Mathf.Sqrt(3)) - 0.25f * Mathf.Sqrt(3));
                }

                BackgroundHexagon backgroundHexagon = _board.InstantiateBackgroudHexagonAtPos(i, j, pos);
                backgroundHexagon.gameObject.SetActive(false);
                bgHexagons.Add(backgroundHexagon);
            }
        }

        bgHexagons.Reverse();

        // all hexagons should be instantiated before finding neighbors. Reason why this for loop exists

        foreach (BackgroundHexagon bHex in bgHexagons)  // Initiate backgroundHex and Hex variable values
        {
            bHex.InstantiateHexagon(Hexagon.HexagonType.Default);
            bHex.myHexagon.SelectRandomColor();
        }

        FindHexagonsWillExplodeAndChangeColor(bgHexagons); // check if a group that should explode exist, change recursively

        foreach (BackgroundHexagon bHex in bgHexagons) // For loop with delay: to fall each hexagon at different time at the beginning as in the example game
        {
            bHex.gameObject.SetActive(true);
            yield return null;
        }
    }

    public void FindHexagonsWillExplodeAndChangeColor(List<BackgroundHexagon> allBgHexagons) //Dont let a group that should explode to be existed at the beginning
    {

        // Find All Hexagons Which Should Explode

        List<BackgroundHexagon> allHexagonsThatWillExplode = new List<BackgroundHexagon>();
        foreach (BackgroundHexagon bgHex in allBgHexagons)
        {
            List<BackgroundHexagon> theSameColorsList = new List<BackgroundHexagon>();
            foreach (BackgroundHexagon neighborBgHex in bgHex.MyNeighborBackgroundHexagons)
            {
                if (bgHex.myHexagon.MyColor == neighborBgHex.myHexagon.MyColor)
                {
                    theSameColorsList.Add(neighborBgHex);
                }
            }
            foreach (BackgroundHexagon neighborBgHex in theSameColorsList)
            {
                for (int k = 0; k < theSameColorsList.Count; k++)
                {
                    if (theSameColorsList[k].MyNeighborBackgroundHexagons.Contains(neighborBgHex))
                    {
                        if (!allHexagonsThatWillExplode.Contains(theSameColorsList[k]))
                        {
                            allHexagonsThatWillExplode.Add(theSameColorsList[k]);
                            continue;
                        }
                    }
                }
            }

        }

        // Change Color Recursively Until There are No Explosion remaining 

        List<BackgroundHexagon> targetGroupToChangeColor = counter <= 10 ? allHexagonsThatWillExplode : allBgHexagons;

        if (allHexagonsThatWillExplode.Count > 0)
        {
            counter++;
            if (counter > 10)
            {
                counter = 0; //Dont change color of every hexagons each time, after chaging target group one time go on with hexagonsThatWillExplode
            }
            foreach (BackgroundHexagon bgHex in targetGroupToChangeColor) // only change color of hexagons that should explode several times, but if it does not work change all hexagon colors one time. then repeat process
            {
                bgHex.myHexagon.SelectRandomColor();
            }
            FindHexagonsWillExplodeAndChangeColor(allBgHexagons); //Change Colors Recursively
        }
        //Debug.Log(counter);

    }
}
