using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindMatchedHexagons
{
    Board _board;
    public FindMatchedHexagons(Board board)
    {
        _board = board;
    }

    public List<BackgroundHexagon> FindHexagonsThatWillExplodeBetweenSelectedHexagons() // returns list of hexagons that should explode
    {
        List<BackgroundHexagon> allHexagonsThatWillExplode = new List<BackgroundHexagon>();
        foreach (BackgroundHexagon bgHex in _board.selectedGroup)
        {
            List<BackgroundHexagon> theSameColorsList = new List<BackgroundHexagon>();
            foreach (BackgroundHexagon neighborBgHex in bgHex.MyNeighborBackgroundHexagons)
            {
                if (bgHex.myHexagon.MyColor == neighborBgHex.myHexagon.MyColor) // if neighbor hexagons have the same color
                {
                    theSameColorsList.Add(neighborBgHex);

                }
            }
            if (theSameColorsList.Count >= 2) //else dont have to use loop, just pass  
            {
                foreach (BackgroundHexagon neighborBgHex in theSameColorsList)
                {
                    for (int k = 0; k < theSameColorsList.Count; k++)
                    {
                        if (theSameColorsList[k].MyNeighborBackgroundHexagons.Contains(neighborBgHex)) // if neighbor hexagons with the same color are neighbor with each other
                        {
                            if (!allHexagonsThatWillExplode.Contains(theSameColorsList[k]))
                            {
                                allHexagonsThatWillExplode.Add(theSameColorsList[k]);
                                if (!allHexagonsThatWillExplode.Contains(bgHex))
                                    allHexagonsThatWillExplode.Add(bgHex);
                                continue;
                            }
                        }

                    }
                }
            }

        }

        return allHexagonsThatWillExplode;
    }

    public List<BackgroundHexagon> FindHexagonsThatWillExplodeBetweenAllHexagons() // returns list of hexagons that should explode
    {
        List<BackgroundHexagon> allHexagonsThatWillExplode = new List<BackgroundHexagon>();
        for (int i = 0; i < _board.RowCount; i++)
        {
            for (int j = 0; j < _board.ColumnCount; j++)
            {
                List<BackgroundHexagon> theSameColorsList = new List<BackgroundHexagon>();
                foreach (BackgroundHexagon bgHex in _board.allBackgroundHexagons[i, j].MyNeighborBackgroundHexagons)
                {
                    if (_board.allBackgroundHexagons[i, j].myHexagon.MyColor == bgHex.myHexagon.MyColor) // if neighbor hexagons have the same color
                    {
                        theSameColorsList.Add(bgHex);

                    }
                }
                if (theSameColorsList.Count >= 2) //else dont have use to loop, just pass  
                {
                    foreach (BackgroundHexagon bgHex in theSameColorsList)
                    {
                        for (int k = 0; k < theSameColorsList.Count; k++)
                        {
                            if (theSameColorsList[k].MyNeighborBackgroundHexagons.Contains(bgHex)) // if neighbor hexagons with the same color are neighbor with each other
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

            }
        }

        return allHexagonsThatWillExplode;
    }
}
