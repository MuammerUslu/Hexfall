using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeHexagonList
{
    public void ExplodeHexagons(List<BackgroundHexagon> explosionHexagons)
    {
        foreach (BackgroundHexagon bgHex in explosionHexagons)
        {
            bgHex.ExplodeHexagon();
        }
    }

}
