using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.UI {
  public interface IScrollGrid {
    RectTransform RectTransform { get; }
    void Refresh(int index);
  }

  public abstract class ScrollGrid<T> : MonoBehaviour, IScrollGrid {
    [SerializeField]
    private GameObject SelectGo;
    public RectTransform RectTransform { get; private set; }
    public bool Selected {
      get => SelectGo ? SelectGo.activeSelf : false;
      protected set {
        if (SelectGo) {
          SelectGo.SetActiveEx(value);
        }
      }
    }
    protected Func<T, bool> CheckSelected;
    protected int DataIndex;
    protected T Data => DataIndex >= 0 && DataIndex < DataList.Count ? DataList[DataIndex] : default;
    protected List<T> DataList;

    public virtual ScrollGrid<T> Init(List<T> dataList, Func<T, bool> onSelected = null, Func<T, bool> onUnselected = null, Func<T, bool> checkSelected = null) {
      DataIndex = -1;
      Selected = false;
      DataList = dataList;
      CheckSelected = checkSelected;
      RectTransform = GetComponent<RectTransform>();
      var button = GetComponent<Button>();
      if (button) {
        button.onClick.AddListener(() => {
          if (Selected) {
            if (onUnselected != null && onUnselected(Data)) {
              Selected = false;
            }
          } else {
            if (onSelected != null && onSelected(Data)) {
              Selected = true;
            }
          }
        });
      }
      return this;
    }

    public void Refresh(int index) {
      if (DataIndex == index) {
        return;
      }
      if (index < 0 || index >= DataList.Count) {
        DataIndex = -1;
        Selected = false;
        gameObject.SetActiveEx(false);
        return;
      }
      DataIndex = index;
      Selected = CheckSelected?.Invoke(Data) ?? false;
      RefreshInternal(Data);
      gameObject.SetActiveEx(true);
    }

    protected abstract void RefreshInternal(T data);
  }

  public enum LayoutDirection {
    [InspectorName("水平")]
    HORIZONTAL = 0,
    [InspectorName("垂直")]
    VERTICAL = 1,
  }

  public class DynamicScrollRect : ScrollRect {
    public GameObject GridTemplate;
    public Vector2 GridSize;
    public Vector2 Spacing;
    public LayoutDirection LayoutDirection;
    private int TotalCount;
    private int StartIndex;
    private int GridCountPerRow;
    private int GridCountPerColumn;
    private List<IScrollGrid> Grids = new List<IScrollGrid>();

    public void Init<TGrid, TData>(List<TData> dataList, Func<TData, bool> onSelected = null, Func<TData, bool> onUnselected = null, Func<TData, bool> checkSelected = null) where TGrid : ScrollGrid<TData> {
      StartIndex = 0;
      TotalCount = dataList.Count;
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
      GridTemplate.SetActiveEx(false);
      for (int i = 0; i < GridCountPerColumn; i++) {
        for (int j = 0; j < GridCountPerRow; j++) {
          var grid = Instantiate(GridTemplate, content).GetComponent<TGrid>();
          Grids.Add(grid.Init(dataList, onSelected, onUnselected, checkSelected));
        }
      }
      RefreshGrids();
      RefreshScrollBar();
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