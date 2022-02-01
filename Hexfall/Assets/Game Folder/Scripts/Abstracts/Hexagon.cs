using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Hexagon : MonoBehaviour
{
    public Color MyColor { get; private set; }

    private BackgroundHexagon myBackgroundHexagon;

    public BackgroundHexagon MyBackgroundHexagon
    {
        get
        {
            if (myBackgroundHexagon == null)
            {
                float minDist = Mathf.Infinity;
                BackgroundHexagon closestBGHexagon = null;
                foreach (BackgroundHexagon bgHexagon in Board.Instance.selectedGroup)
                {
                    if (Vector3.Distance(bgHexagon.transform.position, transform.position) <= minDist)
                    {
                        minDist = Vector3.Distance(bgHexagon.transform.position, transform.position);
                        closestBGHexagon = bgHexagon;
                    }
                }
                transform.position = closestBGHexagon.transform.position;
                transform.rotation = closestBGHexagon.transform.rotation;

                myBackgroundHexagon = closestBGHexagon;
            }
            return myBackgroundHexagon;
        }
        set
        {
            myBackgroundHexagon = value;
        }
    }

    public GameObject outlineImage;
    public enum HexagonType { Default, Bomb }
    public HexagonType hexagonType;

    protected virtual void Start()
    {
        MoveToMyBackgroundPosition();
        if (MyColor == new Color(0, 0, 0, 0))
        {
            SelectRandomColor();
        }
    }
    public void SelectRandomColor()
    {
        int rand = UnityEngine.Random.Range(0, Board.Instance.ColorCount);
        MyColor = Board.Instance.HexagonColors[rand];
        GetComponentInChildren<SpriteRenderer>().color = MyColor;
    }

    public void MoveToMyBackgroundPosition()
    {
        float time = 4f / (myBackgroundHexagon.rowIndex + 8);
        StartCoroutine(SmoothLerp(time));
    }

    public void HighlightHexagonWhenSelected()
    {
        GetComponent<Hexagon>().outlineImage.SetActive(true);
    }

    public void DeselectHexagon()
    {
        GetComponent<Hexagon>().outlineImage.SetActive(false);
    }

    IEnumerator SmoothLerp(float time)
    {
        Vector3 startingPos = transform.position;
        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            transform.position = Vector3.Lerp(startingPos, myBackgroundHexagon.transform.position, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = myBackgroundHexagon.transform.position;
        SmoothLerpEnd();
    }

    protected virtual void SmoothLerpEnd()
    {
        // Implement
    }

}




