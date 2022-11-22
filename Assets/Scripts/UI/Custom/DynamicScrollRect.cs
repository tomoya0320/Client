using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.UI {
  public interface IScrollGrid {
    RectTransform RectTransform { get; }
    void Refresh(int index);
  }

  public enum LayoutDirection {
    [InspectorName("水平")]
    HORIZONTAL = 0,
    [InspectorName("垂直")]
    VERTICAL = 1,
  }

  public class DynamicScrollRect : ScrollRect {
    [SerializeField]
    private GameObject GridTemplate;
    [SerializeField]
    private Vector2 GridSize;
    [SerializeField]
    private Vector2 Spacing;
    [SerializeField]
    private LayoutDirection LayoutDirection;
    private int TotalCount;
    private int StartIndex;
    private int GridCountPerRow;
    private int GridCountPerColumn;
    private List<IScrollGrid> Grids = new List<IScrollGrid>();

    public void Init<TGrid>(int totalCount, Action<TGrid> initGridFunc) where TGrid : Component, IScrollGrid {
      TotalCount = totalCount;
      StartIndex = 0;
      var viewSize = viewport.rect.size;
      switch (LayoutDirection) {
        case LayoutDirection.HORIZONTAL: {
            GridCountPerRow = (int)(viewSize.x / (GridSize.x + Spacing.x));
            GridCountPerRow += GridCountPerRow * (GridSize.x + Spacing.x) < viewSize.x ? 2 : 1;
            GridCountPerColumn = (int)(viewSize.y / (GridSize.y + Spacing.y));
            int contentSizeCount = GridCountPerColumn == 0 ? 0 : TotalCount / GridCountPerColumn;
            if (contentSizeCount * GridCountPerColumn < TotalCount) {
              contentSizeCount++;
            }
            content.sizeDelta = new Vector2(contentSizeCount * (GridSize.x + Spacing.x), content.sizeDelta.y);
            break;
          }
        case LayoutDirection.VERTICAL: {
            GridCountPerRow = (int)(viewSize.x / (GridSize.x + Spacing.x));
            GridCountPerColumn = (int)(viewSize.y / (GridSize.y + Spacing.y));
            GridCountPerColumn += GridCountPerColumn * (GridSize.y + Spacing.y) < viewSize.y ? 2 : 1;
            int contentSizeCount = GridCountPerRow == 0 ? 0 : TotalCount / GridCountPerRow;
            if (contentSizeCount * GridCountPerRow < TotalCount) {
              contentSizeCount++;
            }
            content.sizeDelta = new Vector2(content.sizeDelta.x, contentSizeCount * (GridSize.y + Spacing.y));
            break;
          }
      }
      GridTemplate.GetComponent<RectTransform>().sizeDelta = GridSize;
      for (int i = 0; i < GridCountPerColumn; i++) {
        for (int j = 0; j < GridCountPerRow; j++) {
          var grid = Instantiate(GridTemplate, content).GetComponent<TGrid>();
          grid.RectTransform.sizeDelta = new Vector2(GridSize.x, GridSize.y);
          initGridFunc(grid);
          Grids.Add(grid);
        }
      }
      RefreshGrids();
      RefreshScrollBar();
      GridTemplate.SetActiveEx(false);
      onValueChanged.AddListener(OnValueChanged);
    }

    private void OnValueChanged(Vector2 _) {
      if (GridCountPerRow * GridCountPerColumn >= TotalCount) {
        return;
      }

      int newStartIndex;
      switch (LayoutDirection) {
        case LayoutDirection.HORIZONTAL:
          //注意，左右的方向是和上下的是相反的
          newStartIndex = Mathf.Clamp((int)(-content.anchoredPosition.x / (GridSize.x + Spacing.x)) * GridCountPerColumn, 0, TotalCount - 1);
          break;
        case LayoutDirection.VERTICAL:
          newStartIndex = Mathf.Clamp((int)(content.anchoredPosition.y / (GridSize.y + Spacing.y)) * GridCountPerRow, 0, TotalCount - 1);
          break;
        default:
          newStartIndex = StartIndex;
          break;
      }
      if (newStartIndex != StartIndex) {
        int length = Mathf.Min(Mathf.Abs(newStartIndex - StartIndex), Grids.Count);
        for (int i = 0; i < length; i++) {
          if (newStartIndex > StartIndex) {
            // TODO:优化
            var grid = Grids[0];
            Grids.RemoveAt(0);
            Grids.Add(grid);
          } else {
            // TODO:优化
            var grid = Grids[Grids.Count - 1];
            Grids.RemoveAt(Grids.Count - 1);
            Grids.Insert(0, grid);
          }
        }
        StartIndex = newStartIndex;
        RefreshGrids();
      }
    }

    private void RefreshGrids() {
      for (int i = 0; i < Grids.Count; i++) {
        var grid = Grids[i];
        int index = StartIndex + i;
        Vector2 pos;
        switch (LayoutDirection) {
          case LayoutDirection.HORIZONTAL:
            pos = new Vector2(index / GridCountPerColumn * (GridSize.x + Spacing.x), -(index % GridCountPerColumn) * (GridSize.y + Spacing.y));
            break;
          case LayoutDirection.VERTICAL:
            pos = new Vector2(index % GridCountPerRow * (GridSize.x + Spacing.x), -(index / GridCountPerRow) * (GridSize.y + Spacing.y));
            break;
          default:
            pos = Vector2.zero;
            break;
        }
        grid.RectTransform.anchoredPosition = pos;
        grid.Refresh(index);
      }
    }

    private void RefreshScrollBar() {
      if (verticalScrollbar) {
        verticalScrollbar.value = normalizedPosition.y;
      }
      if (horizontalScrollbar != null) {
        horizontalScrollbar.value = normalizedPosition.x;
      }
    }
  }
}