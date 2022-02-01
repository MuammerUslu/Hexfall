using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager instance;
    public static InputManager Instance  // Not Persistent Instance.
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<InputManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(InputManager).Name;
                    instance = obj.AddComponent<InputManager>();
                }
            }
            return instance;
        }
    }

    bool touchAvailable;
    bool selectedGroupRotatable;
    Vector3 firstTouchInputPos;

    public static Action<RotationType> OnGetRotationInput;
    public static Action OnGetHexagonSelectionInput;

    void Update()
    {
        if (GameManager.AreWeInGamePlay())
        {
            if (touchAvailable)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    firstTouchInputPos = Input.mousePosition;
                    selectedGroupRotatable = true;

                }
                if (Input.GetMouseButton(0))
                {
                    if (selectedGroupRotatable)
                    {
                        if (Board.Instance.selectedGroup.Count > 0)
                        {
                            Vector3 dragDist = Input.mousePosition - firstTouchInputPos;
                            if (dragDist.magnitude >= 40f)
                            {
                                RotationType rotType = DetectRotationType(Input.mousePosition, firstTouchInputPos);
                                OnGetRotationInput?.Invoke(rotType);
                                selectedGroupRotatable = false;
                            }
                        }
                    }
                }

                if (Input.GetMouseButtonUp(0))
                {
                    if (Vector2.Distance(firstTouchInputPos, Input.mousePosition) <= 5f)
                    {
                        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
                        if (hit.transform != null)
                        {
                            if (hit.transform.GetComponent<BackgroundHexagonGroupSelector>() != null)
                            {
                                //Debug.Log(hit.transform.name + " is hit");
                                OnGetHexagonSelectionInput?.Invoke();
                                hit.transform.GetComponent<BackgroundHexagonGroupSelector>().SelectGroup(new Vector2(hit.point.x, hit.point.y));

                            }
                        }
                    }

                }
            }
        }
    }

    RotationType DetectRotationType(Vector3 newIputPos, Vector3 firstInputPos)
    {
        selectedGroupRotatable = false;

        Vector3 selectedGroupCenterPoint = Board.Instance.SelectedGroupCenterPoint;
        Vector3 middleScreenPoint = Vector3.zero;

        middleScreenPoint = Camera.main.WorldToScreenPoint(selectedGroupCenterPoint); // value of middle point on screen
        middleScreenPoint = new Vector3(middleScreenPoint.x, middleScreenPoint.y, 0);

        Vector3 midToFirstInput = (firstInputPos - middleScreenPoint).normalized;
        Vector3 firstInputToInput = (newIputPos - firstInputPos).normalized;

        if (Vector3.Cross(midToFirstInput, firstInputToInput).z >= 0) // CrossPoduct gives the direction
        {
            return RotationType.Clockwise;
        }
        else
        {
            return RotationType.CounterClockwise;
        }
    }

    #region MethodsInActions
    private void EnableTouch()
    {
        touchAvailable = true;
    }

    private void DisableTouch()
    {
        touchAvailable = false;
    }

    private void OnEnable()
    {
        TaskManager.OnCycleStart += DisableTouch;
        TaskManager.OnCycleEnd += EnableTouch;
    }

    private void OnDisable()
    {
        TaskManager.OnCycleStart -= DisableTouch;
        TaskManager.OnCycleEnd -= EnableTouch;
    }
    #endregion
}




