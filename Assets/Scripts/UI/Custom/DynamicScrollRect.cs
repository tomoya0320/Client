using Sirenix.OdinInspector;
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
    [LabelText("Grid模板")]
    [SerializeField]
    private GameObject GridTemplate;
    [LabelText("Grid大小")]
    [SerializeField]
    private Vector2 GridSize;
    [LabelText("Grid间距")]
    [SerializeField]
    private Vector2 Spacing;
    [LabelText("布局方向")]
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
            GridCountPerRow = (int)(viewSize.x / (GridSize.x + Spacing.x)) + 1;
            if (GridCountPerRow * (GridSize.x + Spacing.x) < viewSize.x) {
              GridCountPerRow++;
            }
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
            GridCountPerColumn = (int)(viewSize.y / (GridSize.y + Spacing.y)) + 1;
            if (GridCountPerColumn * (GridSize.y + Spacing.y) < viewSize.y) {
              GridCountPerColumn++;
            }
            int contentSizeCount = GridCountPerRow == 0 ? 0 : TotalCount / GridCountPerRow;
            if (contentSizeCount * GridCountPerRow < TotalCount) {
              contentSizeCount++;
            }
            content.sizeDelta = new Vector2(content.sizeDelta.x, contentSizeCount * (GridSize.y + Spacing.y));
            break;
          }
      }
      GridTemplate.GetComponent<RectTransform>().sizeDelta = GridSize;
      for (int i = 0; i < GridCountPerRow; i++) {
        for (int j = 0; j < GridCountPerColumn; j++) {
          var grid = Instantiate(GridTemplate, content).GetComponent<TGrid>();
          grid.RectTransform.sizeDelta = new Vector2(GridSize.x, GridSize.y);
          grid.RectTransform.anchoredPosition = new Vector2(i * (GridSize.y + Spacing.y), j * (GridSize.x + Spacing.x));
          initGridFunc(grid);
          grid.Refresh(StartIndex + Grids.Count);
          Grids.Add(grid);
        }
      }
      GridTemplate.SetActiveEx(false);
      onValueChanged.AddListener(OnValueChanged);
    }

    private void OnValueChanged(Vector2 normalizedPos) {

    }
  }
}