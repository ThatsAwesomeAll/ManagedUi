using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ManagedUi
{
public class GridSelection : MonoBehaviour, ISelectableManager
{
    public UiInputManager inputManager;

    Vector2Int _currentSelectedIndex = Vector2Int.zero;
    SelectableParent _currentSelected;

    enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
    
    Vector2Int GetVector2IntFromDirection(Direction dir)
    {
        Vector2Int direction = Vector2Int.zero;

        switch (dir)
        {
            case Direction.Up:
                direction = new Vector2Int(0, 1);
                break;
            case Direction.Down:
                direction = new Vector2Int(0, -1);
                break;
            case Direction.Left:
                direction = new Vector2Int(-1, 0);
                break;
            case Direction.Right:
                direction = new Vector2Int(1, 0);
                break;
            default:
                break;
        }

        return direction;
    }

    private SelectableParent[] _selectables = Array.Empty<SelectableParent>();
    private Dictionary<Vector2Int, SelectableParent> _grid = new Dictionary<Vector2Int, SelectableParent>();

    public void OnEnable()
    {
        _selectables = GetComponentsInChildren<SelectableParent>();
        if (inputManager != null)
        {
            inputManager.OnDown += () =>
            {
                MoveSelection(Direction.Down);
            };
            inputManager.OnUp += () =>
            {
                MoveSelection(Direction.Up);
            };
            inputManager.OnLeft += () =>
            {
                MoveSelection(Direction.Left);
            };
            inputManager.OnRight += () =>
            {
                MoveSelection(Direction.Right);
            };    
            inputManager.OnConfirm += () =>
            {
                Confirmed();
            };
        }

        if (_selectables.Length == 0)
        {
            return;
        }

        StartCoroutine(SetUp());

    }

    IEnumerator SetUp()
    {
        yield return new WaitForEndOfFrame();
        DeselectGrid();
        SetUpGrid();
        if (_currentSelected != null)
        {
            _currentSelected.SetSelected(true);
            _currentSelectedIndex = _currentSelected.gridPosition;
        }
        // Set most center element to be default
        Vector2Int currentMostCenter = new Vector2Int(0, 0);
        if (_grid.ContainsKey(currentMostCenter))
        {
            _grid[currentMostCenter].SetSelected(true);
            yield break;
        }

        float maxDistance = float.MaxValue;
        foreach (var selectable in _grid)
        {
            currentMostCenter = selectable.Key;
            maxDistance = currentMostCenter.magnitude < maxDistance ? currentMostCenter.magnitude : maxDistance;
        }
        _grid[currentMostCenter].SetSelected(true);
    }

    void DeselectGrid()
    {
        foreach (SelectableParent selectable in _selectables)
        {
            selectable.SetSelected(false);
        }
    }

    void SetUpGrid()
    {
        Vector2 minSize = GetMinimalSize(_selectables);
        foreach (var selectable in _selectables)
        {
            selectable.gridPosition = new Vector2Int((int)(selectable.anchoredPosition.x/minSize.x), (int)(selectable.anchoredPosition.y/minSize.y));
            _grid.Add(selectable.gridPosition, selectable);
        }
    }

    Vector2 GetMinimalSize(SelectableParent[] selectables)
    {
        Vector2 minSize = Vector2.one*float.MaxValue;
        // compute the minimal size of all selectables
        foreach (SelectableParent selectable in selectables)
        {
            var currentSize = selectable.size;
            minSize = minSize.magnitude > currentSize.magnitude ? currentSize : minSize;
        }
        return minSize;
    }


    public Vector2Int? GetMatchingElementDirection(Vector2Int current, Vector2Int direction)
    {
        Vector2Int? nextBest = null;
        int minDistance = int.MaxValue;

        foreach (var element in _grid.Keys)
        {
            Vector2Int diff = element - current;
            float angle = Vector2.Angle(direction, diff);
            if (angle <= 45)
            {
                int distance = Mathf.Abs(diff.x) + Mathf.Abs(diff.y);
                if (distance > 0 && distance < minDistance)
                {
                    minDistance = distance;
                    nextBest = element;
                }
            }
        }
        return nextBest;
    }

    private void Confirmed()
    {
        if (_currentSelected == null)
        {
            return;
        }
        _currentSelected.SetConfirmed();
    }

    private void MoveSelection(Direction dir)
    {
        Debug.Log("Move Selection called with " + dir);
        var selectionDirection = GetVector2IntFromDirection(dir);
        Vector2Int? nextBest = GetMatchingElementDirection(_currentSelectedIndex, selectionDirection);
        if (nextBest.HasValue)
        {
            DeselectGrid();
            _currentSelected = _grid[nextBest.Value];
            _currentSelected.SetSelected(true);
            _currentSelectedIndex = nextBest.Value;
        }
    }
    
    public void TriggerExternalSelect(SelectableParent selectable)
    {
        if (selectable == _currentSelected)
        {
            return;
        }
        DeselectGrid();
        _currentSelected = selectable;
        _currentSelectedIndex = selectable.gridPosition;
        _currentSelected.SetSelected(true);
    }
    public void TriggerExternalDeSelect(SelectableParent selectable)
    {
        DeselectGrid();
        _currentSelected = null;
        _currentSelectedIndex = new Vector2Int(0,0);
    }

    public void TriggerExternalConfirm(SelectableParent selectable)
    {
        if (selectable == _currentSelected)
        {
            return;
        }
        DeselectGrid();
        _currentSelected = selectable;
        _currentSelected.SetConfirmed();
        _currentSelectedIndex = selectable.gridPosition;
    }
}
}