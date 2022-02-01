using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefreshHexagonPositions
{
    Board _board;
    public RefreshHexagonPositions(Board board)
    {
        _board = board;
    }

    public IEnumerator RefreshPositions() // Refresh Positions of hexagons which already exist
    {
        // Fill empty  squares with current ones
        for (int j = 0; j < _board.ColumnCount; j++) // Scan Each Column, to find empty positions in each column
        {
            int nullCountInColumn = 0;
            for (int i = _board.RowCount - 1; i >= 0; i--) // Start Scanning from bottom. 
            {
                if (_board.allBackgroundHexagons[i, j].myHexagon == null)
                {
                    nullCountInColumn++;  // Find Number of Null positions below the hexagon. It will give how many squares should a hexagon go down.
                }
                else
                {
                    if (nullCountInColumn > 0)
                    {
                        _board.allBackgroundHexagons[i, j].DeliverMyHexagonTo(_board.allBackgroundHexagons[i + nullCountInColumn, j]);
                        yield return null;
                    }
                }
            }
        }
        yield return new WaitForSeconds(0.5f);

    }



}
