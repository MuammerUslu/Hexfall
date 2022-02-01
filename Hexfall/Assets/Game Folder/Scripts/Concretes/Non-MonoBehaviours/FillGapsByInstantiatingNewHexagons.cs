using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class FillGapsByInstantiatingNewHexagons : IFillGaps
{
    Board _board;
    int lastScoreThatBombIsInstantiated = 0;
    public FillGapsByInstantiatingNewHexagons(Board board)
    {
        _board = board;
        lastScoreThatBombIsInstantiated = _board.BombHexScoreFrequency;
    }

    public void FillEmptyGaps()
    {
        // Fill the empty squares with new InstantiatedBlocks
        for (int i = 0; i < _board.RowCount; i++)
        {
            for (int j = 0; j < _board.ColumnCount; j++)
            {
                if (_board.allBackgroundHexagons[i, j].myHexagon == null)
                {
                    if (ScoreManager.Instance.Score < lastScoreThatBombIsInstantiated)
                    {
                        _board.allBackgroundHexagons[i, j].InstantiateHexagon(Hexagon.HexagonType.Default);
                    }
                    else
                    {
                        lastScoreThatBombIsInstantiated += _board.BombHexScoreFrequency;
                        _board.allBackgroundHexagons[i, j].InstantiateHexagon(Hexagon.HexagonType.Bomb);
                    }
                }
            }
        }
    }
}
