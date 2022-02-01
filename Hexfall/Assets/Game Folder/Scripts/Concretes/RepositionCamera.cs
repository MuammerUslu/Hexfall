using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepositionCamera : MonoBehaviour
{
    Board board;
    public float camera_Z_Offset;
    public float padding = 0;

    float aspectRatio;

    private void Start()
    {
        // Set Up Values
        camera_Z_Offset = transform.position.z;
        board = FindObjectOfType<Board>();
        aspectRatio = (float)Screen.width / (float)Screen.height;

        // Set Camera Position and ortographicSize
        if (board != null)
        {
            ConfigureCameraPorperties(board);
        }
    }

    void ConfigureCameraPorperties(Board board)
    {
        Vector3 boardMidPoint = (board.allBackgroundHexagons[0, 0].transform.position
            + board.allBackgroundHexagons[board.RowCount - 1, board.ColumnCount - 1].transform.position) / 2; // Gives us the middle point of tiles
        transform.position = new Vector3(boardMidPoint.x, boardMidPoint.y, camera_Z_Offset);

        if (Board.Instance.ColumnCount * 0.75f >= Board.Instance.RowCount * 0.5f * Mathf.Sqrt(3) * aspectRatio)
        {
            Camera.main.orthographicSize = (Board.Instance.ColumnCount * 0.75f / 2 + padding) / aspectRatio;
        }
        else
        {
            Camera.main.orthographicSize = Board.Instance.RowCount * 0.5f * Mathf.Sqrt(3) / 2 + padding * 4f;
        }
    }
}
