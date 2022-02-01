using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RotationType { Clockwise, CounterClockwise }
public class HexagonGroupRotator
{
    Board _board;
    public HexagonGroupRotator(Board board)
    {
        _board = board;
    }

    public IEnumerator RotateGroup(RotationType rotType, Vector3 centerPoint) // Rotates 120 degree
    {
        if (rotType == RotationType.Clockwise || rotType == RotationType.CounterClockwise)
        {
            int rotVal = rotType == RotationType.Clockwise ? 1 : -1;// rotType  -->  1 for clockwise ,-1 for counter-clockwise ;bgHexs --> bgHExs that will rotate

            float elapsedTime = 0;
            float time = 0.1f;
            while (elapsedTime < time)
            {
                elapsedTime += Time.deltaTime;
                foreach (BackgroundHexagon bgHex in _board.selectedGroup)
                {
                    bgHex.myHexagon.transform.RotateAround(centerPoint, Vector3.forward, rotVal * (Time.deltaTime / time) * 120f);
                }

                yield return null;
            }
        }
    }
    public void DeliverHexagonsToNewBgHexsAfterRotation(List<BackgroundHexagon> bgHexs)
    {
        List<Hexagon> hexagons = new List<Hexagon>();
        for (int i = 0; i < bgHexs.Count; i++)
        {
            hexagons.Add(bgHexs[i].myHexagon);
        }

        for (int i = 0; i < hexagons.Count; i++)
        {
            hexagons[i].MyBackgroundHexagon = null; // To Refresh backgoundHexagon
            hexagons[i].MyBackgroundHexagon = hexagons[i].MyBackgroundHexagon;
            hexagons[i].MyBackgroundHexagon.myHexagon = hexagons[i];
        }
    }





}
