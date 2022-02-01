using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultHexagon : Hexagon
{
    protected override void Start()
    {
        base.Start();
        hexagonType = HexagonType.Default;
    }

    protected override void SmoothLerpEnd()
    {
        GetComponent<Animator>().SetTrigger("Hop");
    }
}
