using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectAvailableMoves
{
    Board _board;

    public DetectAvailableMoves(Board board)
    {
        _board = board;
    }


    // Search for the same color hexagons between neighbors of each neighbor hexagon.Those neighbors should be already neighbor with each other. The hexagon must not be counted.
    // Do it for each hexagon
    // Break loops and return if find an available move
    public bool AreThereAnyAvailableMove()
    {
        for (int i = 0; i < _board.RowCount; i++)
        {
            for (int j = 0; j < _board.ColumnCount; j++)
            {
                foreach (BackgroundHexagon neighborBgHex in _board.allBackgroundHexagons[i, j].MyNeighborBackgroundHexagons)
                {
                    List<BackgroundHexagon> theSameColorNeighborsNeighborsList = new List<BackgroundHexagon>(); //The list will contain the hexagon's neighbor's neighbors with the same color with board.allBackgroundHexagons[i, j]

                    foreach (BackgroundHexagon neighborBgHexOfNeighborHex in neighborBgHex.MyNeighborBackgroundHexagons)
                    {
                        if (neighborBgHexOfNeighborHex != _board.allBackgroundHexagons[i, j]) //The hexagon's neighbor's neighbors cannot be board.allBackgroundHexagons[i, j]
                        {
                            if (_board.allBackgroundHexagons[i, j].myHexagon.MyColor == neighborBgHexOfNeighborHex.myHexagon.MyColor)
                            {
                                theSameColorNeighborsNeighborsList.Add(neighborBgHexOfNeighborHex);
                            }
                        }
                    }

                    // The same color neighbor's neighbors should be neighbor of each other to be able to explode when _board.allBackgroundHexagons[i, j] arrives
                    foreach (BackgroundHexagon neighborBgHexOfNeighborHex in theSameColorNeighborsNeighborsList)
                    {
                        for (int k = 0; k < theSameColorNeighborsNeighborsList.Count; k++)
                        {
                            if (theSameColorNeighborsNeighborsList[k].MyNeighborBackgroundHexagons.Contains(neighborBgHexOfNeighborHex))
                            {
                                //Debug.Log(_board.allBackgroundHexagons[i, j] + " can explode in next move");
                                return true;
                            }
                        }
                    }
                }
            }
        }

        return false;  // Could Not Detect Hexagon That Can Explode In Next Move

    }
}
