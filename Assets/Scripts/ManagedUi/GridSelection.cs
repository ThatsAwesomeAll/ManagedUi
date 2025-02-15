using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI.ManagedUi
{
    public class GridSelection : MonoBehaviour, ISelectableManager
    {
        public UiInputManager inputManager;

        Vector2Int _lastSelectedIndex;
        SelectableParent _lastSelectedElement;
        enum Direction
        {
            Up,
            Down,
            Left,
            Right
        };

        private SelectableParent[] _selectables = Array.Empty<SelectableParent>();
        private Dictionary<Vector2Int, SelectableParent> _grid = new Dictionary<Vector2Int, SelectableParent>();
        Vector2 _currentPos = Vector2.zero;

        public void OnEnable()
        {
            _selectables = GetComponentsInChildren<SelectableParent>();
            if (inputManager != null)
            {
                inputManager.OnDown += () => { MoveSelection(Direction.Down); };
                inputManager.OnUp += () => { MoveSelection(Direction.Up); };
                inputManager.OnLeft += () => { MoveSelection(Direction.Left); };
                inputManager.OnRight += () => { MoveSelection(Direction.Right); };
            }

            if (_selectables.Length == 0)
            {
                return;
            }

            DeselectGrid();
            if (_lastSelectedElement != null)
            {
                _lastSelectedElement.selected = true;
            }
            SetUpGrid();
            _lastSelectedIndex = _lastSelectedElement.gridPosition;
        }

        void DeselectGrid()
        {
            foreach (SelectableParent selectable in _selectables)
            {
                selectable.selected = false;
            }
        }
        
        void SetUpGrid()
        {
            Vector2 minSize = GetMinimalSize(_selectables);
            foreach (var selectable in _selectables)
            {
                selectable.gridPosition = new Vector2Int((int)(selectable.anchoredPosition.x / minSize.x), (int)(selectable.anchoredPosition.y / minSize.y));
                _grid.Add(selectable.gridPosition, selectable);
            }
        }
        
        Vector2 GetMinimalSize(SelectableParent[] selectables)
        {
            Vector2 minSize = Vector2.one * float.MaxValue;
            // compute the minimal size of all selectables
            foreach (SelectableParent selectable in selectables)
            {
                var currentSize = selectable.size;
                minSize = minSize.magnitude > currentSize.magnitude ? currentSize : minSize;
            }
            return minSize;
        }

        private void MoveSelection(Direction dir)
        {
            
            
            
        }
    }
}