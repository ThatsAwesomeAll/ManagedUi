using System;
using UnityEngine;
using UnityEngine.UI;

namespace ManagedUi.GridSystem
{
[ExecuteInEditMode]
public class AdvancedGridLayout : LayoutGroup
{
    public enum LayoutFocus
    {
        Uniform,
        RowsFocused,
        RowsFixed,
        ColumnsFocused,
        ColumnsFixed
    }

    [Header("GridStyle")]
    public Vector2 spacing;
    public LayoutFocus priority;
    
    
    public int rows;
    public int columns;
    public bool fitX;
    public bool fitY;
    public Vector2 cellSize;

    public override void CalculateLayoutInputVertical()
    {
    }

    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();

        Vector2 growFactorMax = Vector2.one;
        foreach (var childs in rectChildren)
        {
            var LayoutGrowable = childs.GetComponent<IGridElement>();
            if (LayoutGrowable == null)
            {
                continue;
            }
            growFactorMax.x = Mathf.Max(growFactorMax.x, LayoutGrowable.VerticalLayoutGrowth());
            growFactorMax.y = Mathf.Max(growFactorMax.y, LayoutGrowable.HorizontalLayoutGrowth());
        }

        switch (priority)
        {
            case LayoutFocus.RowsFocused:
                rows = columns = Mathf.CeilToInt(Mathf.Sqrt(transform.childCount));
                rows = Mathf.CeilToInt(transform.childCount/(float)columns);
                rows = Mathf.CeilToInt(rows*growFactorMax.x);
                columns = Mathf.CeilToInt(columns*growFactorMax.y);
                fitX = fitY = true;
                break;
            case LayoutFocus.ColumnsFocused:
                rows = columns = Mathf.CeilToInt(Mathf.Sqrt(transform.childCount));
                columns = Mathf.CeilToInt(transform.childCount/(float)rows);
                rows = Mathf.CeilToInt(rows*growFactorMax.x);
                columns = Mathf.CeilToInt(columns*growFactorMax.y);
                fitX = fitY = true;
                break;
            case LayoutFocus.ColumnsFixed:
                rows = Mathf.CeilToInt(transform.childCount/(float)columns);
                break;
            case LayoutFocus.RowsFixed:
                columns = Mathf.CeilToInt(transform.childCount/(float)rows);
                break;
            case LayoutFocus.Uniform:
                break;
        }
        
        
        Vector2 parentSize = new Vector2(rectTransform.rect.width, rectTransform.rect.height);

        float spacingX = (spacing.x/columns) * (columns - 1);
        float paddingX = padding.left/(float)columns + padding.right/(float)columns;
        float cellWidth = parentSize.x/columns - spacingX - paddingX;
        float spacingY = (spacing.y/rows) * (rows -1);
        float paddingY = padding.top/(float)rows + padding.bottom/(float)rows;
        float cellHeight = parentSize.y/rows - spacingY - paddingY;

        cellSize.x = fitX ? cellWidth : cellSize.x;
        cellSize.y = fitY ? cellHeight : cellSize.y;


        int counter = 0;
        foreach (var childs in rectChildren)
        {
            int rowCount = counter/columns;
            int columnCount = counter%columns;

            float posX = columnCount*(spacing.x + cellSize.x) + padding.left;
            float posY = rowCount*(spacing.y + cellSize.y) + padding.top;

            SetChildAlongAxis(childs, 0, posX, cellSize.x);
            SetChildAlongAxis(childs, 1, posY, cellSize.y);


            counter++;
        }
    }
    public override void SetLayoutHorizontal()
    {
    }
    public override void SetLayoutVertical()
    {
    }
}
}