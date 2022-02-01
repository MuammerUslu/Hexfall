using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BackgroundHexagon : MonoBehaviour
{
    [SerializeField] private Hexagon defaultHexagonPrefab, bombHexagonPrefab;
    [SerializeField] private ParticleSystem explodeParticle;

    public Hexagon myHexagon;
    public int rowIndex, columnIndex;

    private List<BackgroundHexagon> myNeighborBackgroundHexagons = new List<BackgroundHexagon>();
    public List<BackgroundHexagon> MyNeighborBackgroundHexagons
    {
        get
        {
            if (myNeighborBackgroundHexagons.Count == 0)
            {
                for (int i = 0; i < Board.Instance.RowCount; i++)
                {
                    for (int j = 0; j < Board.Instance.ColumnCount; j++)
                    {
                        if (Board.Instance.allBackgroundHexagons[i, j] != this)
                        {
                            if (Vector3.Distance(transform.position, Board.Instance.allBackgroundHexagons[i, j].transform.position) <= 1)
                            {
                                myNeighborBackgroundHexagons.Add(Board.Instance.allBackgroundHexagons[i, j]);
                            }
                        }
                    }
                }
            }
            return myNeighborBackgroundHexagons;
        }

    }

    public int myScore = 5;

    public Hexagon InstantiateHexagon(Hexagon.HexagonType hexagonType)
    {
        Hexagon hexagon;
        if (hexagonType == Hexagon.HexagonType.Default)
        {
            hexagon = Instantiate(defaultHexagonPrefab, transform.position + Vector3.up * 10f, Quaternion.identity);
        }
        else
        {
            hexagon = Instantiate(bombHexagonPrefab, transform.position + Vector3.up * 10f, Quaternion.identity);
        }

        hexagon.transform.parent = transform;
        hexagon.transform.name = "( " + rowIndex + " , " + columnIndex + " )";
        myHexagon = hexagon;
        hexagon.MyBackgroundHexagon = this;

        return hexagon;
    }

    public void DeliverMyHexagonTo(BackgroundHexagon backgroundHexagon)
    {
        backgroundHexagon.myHexagon = myHexagon;
        backgroundHexagon.myHexagon.transform.parent = backgroundHexagon.myHexagon.transform;
        backgroundHexagon.myHexagon.transform.name = "( " + backgroundHexagon.rowIndex + " , " + backgroundHexagon.columnIndex + " )";
        backgroundHexagon.myHexagon.MyBackgroundHexagon = backgroundHexagon;
        backgroundHexagon.myHexagon.MoveToMyBackgroundPosition();
        myHexagon = null;
    }

    public void ExplodeHexagon()
    {
        GetComponent<Animator>().SetTrigger("ScoreTextAnim");
        PlayExplodeParticle();
        Vibration.Vibrate(40);
        ScoreManager.Instance.IncreaseScore(myScore);
        Destroy(myHexagon.gameObject);
        myHexagon = null;
    }

    void PlayExplodeParticle()
    {
        explodeParticle.Play();
        var main = explodeParticle.main;
        main.startColor = myHexagon.MyColor;

    }
    void ExplodeAndFallAtEndGame()
    {
        StartCoroutine(WaitForFall());
    }

    IEnumerator WaitForFall()
    {
        yield return new WaitForSeconds(0.5f);
        if (GameManager.Instance.failReason == GameManager.FailReason.Bomb)
        {
            PlayExplodeParticle();
            if (myHexagon != null)
            {
                myHexagon.gameObject.AddComponent<FallAndDestroySelf>();
                myHexagon = null;
            }
        }
    }

    private void OnEnable()
    {
        GameManager.OnLevelFailed += ExplodeAndFallAtEndGame;
    }

    private void OnDisable()
    {
        GameManager.OnLevelFailed -= ExplodeAndFallAtEndGame;
    }



}
