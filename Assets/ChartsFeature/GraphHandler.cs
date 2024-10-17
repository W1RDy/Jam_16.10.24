using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.Mathematics;
using TMPro;

public class GraphHandler : MonoBehaviour
{
    #region accessible methods

    private bool _isGenerated;

    public void CreatePoint(Vector2 newValue)
    {
        CreatePointInternal(newValue);
    }

    public void UpdateGraph()
    {
        UpdateGraphInternal(UpdateMethod.All);
    }

    public void SetWave(Wave wave)
    {
        if (!_isGenerated)
        {
            CreateGraph(wave);
        }
        else RecreateGraph(wave);

    }

    private void CreateGraph(Wave wave)
    {
        _isGenerated = true;

        foreach (var point in wave.Points)
        {
            CreatePoint(point);
        }

        var middlePointOffset = new Vector2((wave.MaxXValue - wave.MinXValue) / 2, (wave.MaxYValue - wave.MaxYValue) / 2);

        InitZoomAndOffset(new Vector2(0.7f, 0.7f), _defaultOffset + middlePointOffset);
        UpdateGraph();
    }

    private void RecreateGraph(Wave wave)
    {
        for (var i = 0; i < wave.Points.Count(); i++)
        {
            ChangePointInternal(i, wave.Points[i]);
        }
    }

    private void InitZoomAndOffset(Vector2 targetZoom, Vector2 targetMoveOffset)
    {
        zoom = targetZoom;
        moveOffset = targetMoveOffset;
    }

    private void ChangeZoomPoint(Vector2 newZoomPoint)
    {
        absoluteZoomPoint = (newZoomPoint - zoomPoint) * contentScale + absoluteZoomPoint;
        zoomPoint = newZoomPoint;
    }

    private void Start()
    {
        if (CheckForErrors())
            return;
        GS = GetComponent<GraphSettings>();
        PrepareGraph();
    }

    private void UpdatePositionAndScale()
    {
        contentScale = GS.GraphScale * zoom;
        maskObj.sizeDelta = GS.GraphSize;
        contentOffset = absoluteZoomPoint - zoomPoint * contentScale - moveOffset;
        graphContent.anchoredPosition = -GS.GraphSize / 2 + contentOffset;
        graph.sizeDelta = GS.GraphSize;
        backgroundRect.sizeDelta = GS.GraphSize;
        backgroundImage.color = GS.BackgroundColor;
    }
    #endregion

    #region references

    public bool updateGraph = false;
    private GraphSettings GS;
    [SerializeField] private Canvas canvas;
    private RectTransform graph;
    private RectTransform graphContent;

    private Vector2 contentScale = Vector2.zero;
    private List<Vector2> values;
    public List<Vector2> Values { get { return values; } }
    private List<int> sortedIndices;
    private Vector2Int xAxisRange = new Vector2Int(-1, -1);
    public Vector2Int XAxisRange { get { return xAxisRange; } }
    private Vector2Int prevXAxisRange = new Vector2Int(-1, -1);
    public int activePointIndex = -1;
    private Vector2 activePointValue = Vector2.zero;

    private List<GameObject> points;
    private List<Image> pointImages;
    private List<RectTransform> pointRects;
    private List<GameObject> pointOutlines;
    private List<RectTransform> pointOutlineRects;
    private List<Image> pointOutlineImages;
    private List<GameObject> lines;
    private List<RectTransform> lineRects;
    private List<Image> lineImages;
    private List<RectTransform> xGridRects;
    private List<Image> xGridImages;
    private List<TextMeshProUGUI> xAxisTexts;
    private List<RectTransform> xAxisTextRects;
    private List<RectTransform> yGridRects;
    private List<Image> yGridImages;
    private List<TextMeshProUGUI> yAxisTexts;
    private List<RectTransform> yAxisTextRects;

    private RectTransform zoomSelectionRectTransform;
    private Image zoomSelectionImage;
    private List<RectTransform> zoomSelectionOutlines;
    private List<Image> zoomSelectionOutlineImages;
    private RectTransform pointSelectionRectTransform;
    private Image pointSelectionImage;
    private List<RectTransform> pointSelectionOutlines;
    private List<Image> pointSelectionOutlineImages;

    private RectTransform maskObj;
    private Image backgroundImage;
    private RectTransform backgroundRect;
    private GameObject pointParent;
    private GameObject lineParent;
    private GameObject gridParent;
    private GameObject outlineParent;
    private List<RectTransform> outlines;
    private List<Image> outlineImages;

    private List<int> lockedHoveredPoints;
    private List<int> lockedPoints;
    private List<int> fixedHoveredPoints;
    public int fixedPointIndex = -1;
    private Vector2 contentOffset = Vector2.zero;

    private Vector2 bottomLeft, topRight, center;
    public Vector2 BottomLeft { get { return bottomLeft; } }
    public Vector2 TopRight { get { return topRight; } }
    public Vector2 Center { get { return center; } }

    private Vector2 _defaultOffset = new Vector2(-50, -100);

    public bool IsPrepared { get; private set; }
    public enum MouseActionType
    {
        Move,
        SelectAreaToZoom,
        SelectPoints
    }
    public MouseActionType mouseActionType;
    public enum RectangleType
    {
        Free,
        PreserveAspectRatio,
        OriginalAspectRatio
    }
    public RectangleType rectangleType;
    public enum RectangleSelectionType
    {
        SelectAll,
        SelectUnselect
    }
    public RectangleSelectionType rectangleSelectionType;
    public enum RectangleSelectionPhase
    {
        Moving,
        Release
    }
    public RectangleSelectionPhase rectangleSelectionPhase;
    public enum PointSelectionType
    {
        Select,
        FixZoomPoint
    }
    public PointSelectionType pointSelectionType;
    private List<int> initialLockedPoints;
    private List<int> recentlyLockedPoints;

    private Vector2 zoomPoint = Vector2.zero;
    private Vector2 absoluteZoomPoint = Vector2.zero;

    public Vector2 targetZoom = new Vector2(1f, 1f);
    private Vector2 zoom = new Vector2(1f, 1f);

    private Vector2 moveOffset;
    public Vector2 targetMoveOffset;
    private Vector2 initialMoveOffset = Vector2.zero;

    private bool error;

    private void Awake()
    {
        IsPrepared = false;

        values = new List<Vector2>();
        sortedIndices = new List<int>();
        points = new List<GameObject>();
        pointRects = new List<RectTransform>();
        pointImages = new List<Image>();
        pointOutlines = new List<GameObject>();
        pointOutlineRects = new List<RectTransform>();
        pointOutlineImages = new List<Image>();
        lines = new List<GameObject>();
        lineRects = new List<RectTransform>();
        lineImages = new List<Image>();
        xGridRects = new List<RectTransform>();
        xGridImages = new List<Image>();
        yGridRects = new List<RectTransform>();
        yGridImages = new List<Image>();
        xAxisTexts = new List<TextMeshProUGUI>();
        yAxisTexts = new List<TextMeshProUGUI>();
        xAxisTextRects = new List<RectTransform>();
        yAxisTextRects = new List<RectTransform>();
        zoomSelectionOutlines = new List<RectTransform>();
        zoomSelectionOutlineImages = new List<Image>();
        pointSelectionOutlines = new List<RectTransform>();
        pointSelectionOutlineImages = new List<Image>();
        outlines = new List<RectTransform>();
        outlineImages = new List<Image>();
        lockedHoveredPoints = new List<int>();
        lockedPoints = new List<int>();
        initialLockedPoints = new List<int>();
        recentlyLockedPoints = new List<int>();
        fixedHoveredPoints = new List<int>();
    }

    #endregion

    #region private methods

    private void PrepareGraph()
    {
        if (canvas == null)
        {
            canvas = new GameObject("GraphCanvas").AddComponent<Canvas>();
        }
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.gameObject.AddComponent<GraphicRaycaster>();

        if (GetComponent<RectTransform>() == null)
            this.gameObject.AddComponent<RectTransform>();
        graph = this.gameObject.GetComponent<RectTransform>();
        //graph.SetParent(canvas.transform);
        graph.anchoredPosition = Vector2.zero;
        graph.sizeDelta = GS.GraphSize;

        maskObj = new GameObject("MaskObj").AddComponent<RectTransform>();
        maskObj.SetParent(graph);
        maskObj.anchoredPosition = Vector2.zero;
        maskObj.gameObject.AddComponent<Image>();
        Mask mask = maskObj.gameObject.AddComponent<Mask>();
        mask.showMaskGraphic = false;

        backgroundRect = new GameObject("Background").AddComponent<RectTransform>();
        backgroundRect.SetParent(maskObj);
        backgroundRect.anchoredPosition = Vector2.zero;
        backgroundImage = backgroundRect.gameObject.AddComponent<Image>();

        graphContent = new GameObject("GraphContent").AddComponent<RectTransform>();
        graphContent.SetParent(backgroundRect.transform);
        graphContent.sizeDelta = Vector2.zero;

        gridParent = CreateParent("GridParent");
        lineParent = CreateParent("LineParent");
        pointParent = CreateParent("PointParent");

        outlineParent = CreateParent("OutlineParent");
        CreateOutlines();
        outlineParent.transform.SetParent(graph);

        fixedPointIndex = -1;
        //SetCornerValues(Vector2.zero, new Vector2(3f, 3f * GS.GraphSize.y / GS.GraphSize.x));
        CreateselectionTypes();
        UpdateGraphInternal(UpdateMethod.All);

        IsPrepared = true;
    }

    private GameObject CreateParent(string name)
    {
        GameObject parent = new GameObject(name);
        parent.transform.SetParent(name == "OutlineParent" ? graph : graphContent);
        Image image = parent.AddComponent<Image>();
        image.color = new Color(0, 0, 0, 0);
        image.raycastTarget = false;
        parent.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        return parent;
    }

    private void CreateOutlines()
    {
        for (int i = 0; i < 4; i++)
        {
            Image outlineImage = new GameObject("Outline").AddComponent<Image>();
            RectTransform outline = outlineImage.GetComponent<RectTransform>();
            outline.SetParent(outlineParent.transform);
            outlineImage.color = GS.OutlineColor;
            outlineImage.raycastTarget = false;
            outlines.Add(outline);
            outlineImages.Add(outlineImage);
        }
    }

    private void CreatePointInternal(Vector2 value)
    {
        int i = points.Count;
        GameObject outline = CreatePointOutline(i);
        GameObject point = new GameObject("Point" + i);

        points.Add(point);
        values.Add(value);

        point.transform.SetParent(outline.transform);

        Image image = point.AddComponent<Image>();
        image.color = GS.PointColor;
        pointImages.Add(image);
        RectTransform pointRectTransform = point.GetComponent<RectTransform>();
        pointRectTransform.sizeDelta = Vector2.one * GS.PointRadius;
        pointRects.Add(pointRectTransform);
        image.sprite = GS.PointSprite;

        if (points.Count > 1)
        {
            GameObject line = new GameObject("Line");
            line.transform.SetParent(lineParent.transform);
            lineImages.Add(line.AddComponent<Image>());
            line.GetComponent<Image>().color = GS.LineColor;
            lineRects.Add(line.GetComponent<RectTransform>());
            lines.Add(line);
            if (value.x < bottomLeft.x || value.x > topRight.x)
                line.SetActive(false);
        }
        lockedHoveredPoints.Add(i);
        SortIndices();
        if (value.x < bottomLeft.x || value.x > topRight.x)
            outline.SetActive(false);
    }

    private void ChangePointInternal(int index, Vector2 newValue)
    {
        values[index] = newValue;
        SortIndices();
    }

    private void SortIndices()
    {
        sortedIndices = values.Select((vector, index) => new { vector, index })
            .OrderBy(item => item.vector.x)
            .ThenBy(item => item.vector.y)
            .Select(item => item.index)
            .ToList();
    }

    private GameObject CreatePointOutline(int i)
    {
        GameObject outline = new GameObject("PointOutline" + i);
        pointOutlines.Add(outline);

        if (pointParent != null)
        {
            outline.transform.SetParent(pointParent.transform);
        }

        Image image = outline.AddComponent<Image>();
        image.color = GS.PointColor;
        pointOutlineImages.Add(image);
        RectTransform rectTransform = outline.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(GS.PointRadius, GS.PointRadius);
        pointOutlineRects.Add(rectTransform);

        Sprite sprite = GS.PointSprite;
        image.sprite = sprite;

        return outline;
    }

    private void CreateGridLines(bool createX)
    {
        if (createX)
        {
            GameObject xGrid = new GameObject("xGrid" + xGridRects.Count);
            xGrid.transform.SetParent(gridParent.transform);
            Image xGridImage = xGrid.AddComponent<Image>();
            xGridImage.raycastTarget = false;
            xGridRects.Add(xGrid.GetComponent<RectTransform>());
            xGridImages.Add(xGridImage);
        }
        else
        {
            GameObject yGrid = new GameObject("yGrid" + yGridRects.Count);
            yGrid.transform.SetParent(gridParent.transform);
            Image yGridImage = yGrid.AddComponent<Image>();
            yGridImage.raycastTarget = false;
            yGridRects.Add(yGrid.GetComponent<RectTransform>());
            yGridImages.Add(yGridImage);
        }
    }

    private void CreateselectionTypes()
    {
        GameObject selectionParent = new GameObject("SelectionParent");
        selectionParent.transform.SetParent(graphContent);
        selectionParent.AddComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
        zoomSelectionImage = new GameObject("ZoomSelection").AddComponent<Image>();
        zoomSelectionRectTransform = zoomSelectionImage.GetComponent<RectTransform>();
        zoomSelectionRectTransform.SetParent(selectionParent.transform);
        for (int i = 0; i < 4; i++)
        {
            Image image = new GameObject("Outline").AddComponent<Image>();
            RectTransform rect = image.GetComponent<RectTransform>();
            rect.SetParent(zoomSelectionRectTransform);
            zoomSelectionOutlineImages.Add(image);
            zoomSelectionOutlines.Add(rect);
        }
        zoomSelectionRectTransform.gameObject.SetActive(false);
        pointSelectionImage = new GameObject("PointSelection").AddComponent<Image>();
        pointSelectionRectTransform = pointSelectionImage.GetComponent<RectTransform>();
        pointSelectionRectTransform.SetParent(selectionParent.transform);
        for (int i = 0; i < 4; i++)
        {
            Image image = new GameObject("Outline").AddComponent<Image>();
            RectTransform rect = image.GetComponent<RectTransform>();
            rect.SetParent(pointSelectionRectTransform);
            pointSelectionOutlineImages.Add(image);
            pointSelectionOutlines.Add(rect);
        }
        pointSelectionRectTransform.gameObject.SetActive(false);
    }

    public void UpdateGraphInternal(UpdateMethod methodsToUpdate)
    {
        if (methodsToUpdate.HasFlag(UpdateMethod.UpdatePositionAndScale) || methodsToUpdate.HasFlag(UpdateMethod.All))
            UpdatePositionAndScale();
        CalculateCornerValues();
        if (methodsToUpdate.HasFlag(UpdateMethod.UpdateOutlines) || methodsToUpdate.HasFlag(UpdateMethod.All))
            UpdateOutlines();
        if (methodsToUpdate.HasFlag(UpdateMethod.UpdateContent) || methodsToUpdate.HasFlag(UpdateMethod.All))
            HandleActiveObjects();
        if (methodsToUpdate.HasFlag(UpdateMethod.UpdatePointVisuals) || methodsToUpdate.HasFlag(UpdateMethod.All))
            UpdatePointVisuals();
        if (methodsToUpdate.HasFlag(UpdateMethod.UpdateContent) || methodsToUpdate.HasFlag(UpdateMethod.All))
            UpdateContent();
        if (methodsToUpdate.HasFlag(UpdateMethod.UpdateGridLines) || methodsToUpdate.HasFlag(UpdateMethod.All))
            UpdateGridLines();
    }
    private void UpdateOutlines()
    {
        for (int i = 0; i < outlines.Count; i++)
        {
            if (i % 2 == 0) // Left and Right outlines
            {
                outlines[i].sizeDelta = new Vector2(GS.OutlineWidth, GS.GraphSize.y + GS.OutlineWidth * 2);
                outlines[i].anchoredPosition = new Vector2((i == 0 ? -1 : 1) * (GS.GraphSize.x + GS.OutlineWidth) / 2, 0);
            }
            else // Top and Bottom outlines
            {
                outlines[i].sizeDelta = new Vector2(GS.GraphSize.x + GS.OutlineWidth * 2, GS.OutlineWidth);
                outlines[i].anchoredPosition = new Vector2(0, (i == 1 ? -1 : 1) * (GS.GraphSize.y + GS.OutlineWidth) / 2);
            }
            outlineImages[i].color = GS.OutlineColor;
        }
    }
    private void CalculateCornerValues()
    {
        topRight = new Vector2(Mathf.Clamp(topRight.x, bottomLeft.x, Mathf.Infinity), Mathf.Clamp(topRight.y, bottomLeft.y, Mathf.Infinity));
        bottomLeft = -contentOffset / contentScale;
        topRight = bottomLeft + GS.GraphSize / contentScale;
        center = (topRight - bottomLeft) / 2f + bottomLeft;
    }
    private void UpdateContent()
    {
        if (xAxisRange.x == -1 || xAxisRange.y == -1)
            return;
        Vector2 bounds = new Vector2(bottomLeft.y, topRight.y);
        for (int i = xAxisRange.x - 1; i <= xAxisRange.y + 1; i++)
        {
            if (i < 0 || i > sortedIndices.Count - 1)
                continue;
            int index = sortedIndices[i];
            float currentValue = values[index].y;
            float prevValue = values[Mathf.Clamp(index - 1, 0, values.Count - 1)].y;
            float nextValue = values[Mathf.Clamp(index + 1, 0, values.Count - 1)].y;

            if ((currentValue < bounds.x && prevValue < bounds.x && nextValue < bounds.x) ||
                (currentValue > bounds.y && prevValue > bounds.y && nextValue > bounds.y))
            {
                continue;
            }
            UpdateAnchoredPosition(pointOutlineRects[index], CalculatePosition(i));
            if (lines.Count > 0 && index < lines.Count)
            {
                Vector2 point1 = CalculatePosition(index);
                Vector2 point2 = CalculatePosition(index + 1);
                float distance = Vector2.Distance(point1, point2);
                UpdateAnchoredPosition(lineRects[index], (point2 + point1) / 2f);
                UpdateSizeDelta(lineRects[index], new Vector2(distance, GS.LineWidth));
                Vector2 direction = point2 - point1;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                lineRects[index].rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                lineImages[index].color = GS.LineColor;
            }
        }
    }
    private void HandleActiveObjects()
    {
        if (prevXAxisRange.x < xAxisRange.x)
        {
            for (int i = prevXAxisRange.x - 1; i < xAxisRange.x - 1; i++)
            {
                if (i < 0)
                    continue;
                pointOutlines[sortedIndices[i]].SetActive(false);
                if (i < lines.Count)
                    lines[sortedIndices[i]].SetActive(false);
            }
        }
        else if (prevXAxisRange.x > xAxisRange.x && xAxisRange.x >= 0)
        {
            for (int i = xAxisRange.x - 1; i < prevXAxisRange.x; i++)
            {
                if (i < 0)
                    continue;
                pointOutlines[sortedIndices[i]].SetActive(true);
                if (i < lines.Count)
                    lines[sortedIndices[i]].SetActive(true);
            }
        }
        if (prevXAxisRange.y > xAxisRange.y)
        {
            for (int i = xAxisRange.y + 2; i <= prevXAxisRange.y + 2; i++)
            {
                if (i > pointOutlines.Count - 1 || i < 0)
                    continue;
                pointOutlines[sortedIndices[i]].SetActive(false);
                if (i < lines.Count)
                    lines[sortedIndices[i]].SetActive(false);
            }
        }
        else if (xAxisRange.y > prevXAxisRange.y)
        {
            for (int i = prevXAxisRange.y + 2; i <= xAxisRange.y + 1; i++)
            {
                if (i > pointOutlines.Count - 1 || i < 0)
                    continue;
                pointOutlines[sortedIndices[i]].SetActive(true);
                if (i < lines.Count)
                    lines[sortedIndices[i]].SetActive(true);
            }
        }
        prevXAxisRange = xAxisRange;
        xAxisRange = new Vector2Int(MinMaxBinarySearch(true), MinMaxBinarySearch(false));
    }
    private Vector2 CalculatePosition(int i)
    {
        return values[i] * contentScale;
    }

    private void UpdatePointVisuals()
    {
        if (xAxisRange.x == -1 || xAxisRange.y == -1)
            return;
        for (int i = xAxisRange.x; i <= xAxisRange.y; i++)
        {
            if (activePointIndex == sortedIndices[i])
                continue;
            lockedHoveredPoints.Add(sortedIndices[i]);
            fixedHoveredPoints.Add(sortedIndices[i]);
        }
    }

    private void UpdateGridLines()
    {
        Vector2 GridStartPoint;
        Vector2 spacing = CalculateGridSpacing();
        GridStartPoint = new Vector2(Mathf.Ceil(bottomLeft.x * spacing.x) / spacing.x, Mathf.Ceil(bottomLeft.y * spacing.y) / spacing.y) * contentScale;
        int2 eventualOverlay = new int2(-1, -1);
        int requiredYGridlines = Mathf.CeilToInt((topRight.y - bottomLeft.y) * spacing.y) + 1;
        int requiredXGridlines = Mathf.CeilToInt((topRight.x - bottomLeft.x) * spacing.x) + 1;
        while (xGridRects.Count <= requiredXGridlines)
        {
            CreateGridLines(true);
        }
        while (yGridRects.Count <= requiredYGridlines)
        {
            CreateGridLines(false);
        }

        for (int i = 0; i < requiredXGridlines; i++)
        {
            RectTransform rect = xGridRects[i];
            Image rectImage = xGridImages[i];
            if (!rect.gameObject.activeSelf)
                rect.gameObject.SetActive(true);
            if (i == 0)
            {
                UpdateSizeDelta(rect, new Vector2(GS.XAxisWidth, GS.GraphSize.y * 2f));
                rectImage.color = GS.XAxisColor;
                UpdateAnchoredPosition(rect, new Vector2(0, center.y * contentScale.y));
            }
            else
            {
                UpdateSizeDelta(rect, new Vector2(GS.XGridWidth, GS.GraphSize.y * 2f));
                rectImage.color = GS.XGridColor;

                if (Mathf.Round(GridStartPoint.x + (i + eventualOverlay.x) / spacing.x * contentScale.x) == 0)
                    eventualOverlay.x = 0;

                UpdateAnchoredPosition(rect, new Vector2(GridStartPoint.x + (i + eventualOverlay.x) / spacing.x * contentScale.x, center.y * contentScale.y));
            }
        }

        for (int i = 0; i < requiredYGridlines; i++)
        {
            RectTransform rect = yGridRects[i];
            Image rectImage = yGridImages[i];
            if (!rect.gameObject.activeSelf)
                rect.gameObject.SetActive(true);
            if (i == 0)
            {
                UpdateSizeDelta(rect, new Vector2(GS.GraphSize.x * 2f, GS.YAxisWidth));
                rectImage.color = GS.YAxisColor;
                UpdateAnchoredPosition(rect, new Vector2(center.x * contentScale.x, 0));
            }
            else
            {
                UpdateSizeDelta(rect, new Vector2(GS.GraphSize.x * 2f, GS.YGridWidth));
                rectImage.color = GS.YGridColor;

                if (Mathf.Round(GridStartPoint.y + (i + eventualOverlay.y) / spacing.y * contentScale.y) == 0)
                    eventualOverlay.y = 0;

                UpdateAnchoredPosition(rect, new Vector2(center.x * contentScale.x, GridStartPoint.y + (i + eventualOverlay.y) / spacing.y * contentScale.y));
            }
        }
        for (int i = requiredXGridlines; i < xGridRects.Count; i++)
        {
            if (xGridRects[i].gameObject.activeSelf)
                xGridRects[i].gameObject.SetActive(false);
        }
        for (int i = requiredYGridlines; i < yGridRects.Count; i++)
        {
            if (yGridRects[i].gameObject.activeSelf)
                yGridRects[i].gameObject.SetActive(false);
        }
    }
    private Vector2 CalculateGridSpacing()
    {
        int exponentX = Mathf.FloorToInt(Mathf.Log(zoom.x, 2));
        int exponentY = Mathf.FloorToInt(Mathf.Log(zoom.y, 2));
        float closestX = Mathf.Pow(2, exponentX);
        float closestY = Mathf.Pow(2, exponentY);
        return new Vector2(closestX, closestY) * GS.GridSpacing;
    }

    private int MinMaxBinarySearch(bool findLeft) //important for large numbers of points
    {
        //this function finds the points that are closest to the sides of the graph window
        float target;
        target = findLeft ? bottomLeft.x : topRight.x;
        int min = 0;
        int max = sortedIndices.Count - 1;
        float value;
        while (min <= max)
        {
            int middle = min + (max - min) / 2;
            value = values[sortedIndices[middle]].x;
            if ((findLeft ? value >= target : value <= target))
            {
                if ((findLeft && (middle == 0 || values[sortedIndices[middle - 1]].x < target)) ||
                    (!findLeft && (middle == sortedIndices.Count - 1 || values[sortedIndices[middle + 1]].x > target)))
                {
                    return middle;
                }

                if (findLeft)
                    max = middle - 1;
                else
                    min = middle + 1;
            }
            else
            {
                if (findLeft)
                    min = middle + 1;
                else
                    max = middle - 1;
            }
        }
        return -1;
    }
    private void UpdateSizeDelta(RectTransform rect, Vector2 size)
    {
        if (Mathf.Abs(rect.sizeDelta.x - size.x) > 0.1f || Mathf.Abs(rect.sizeDelta.y - size.y) > 0.1f)
            rect.sizeDelta = size;
    }
    private void UpdateAnchoredPosition(RectTransform rect, Vector2 position)
    {
        if (Mathf.Abs(rect.sizeDelta.x - position.x) > 0.1f || Mathf.Abs(rect.sizeDelta.y - position.y) > 0.1f)
            rect.anchoredPosition = position;
    }
    [Flags]
    public enum UpdateMethod
    {
        UpdatePositionAndScale = 1 << 0,
        UpdateOutlines = 1 << 1,
        UpdatePointVisuals = 1 << 2,
        UpdateContent = 1 << 3,
        MouseZoom = 1 << 4,
        MouseAction = 1 << 5,
        UpdateGridLines = 1 << 6,
        All = 1 << 7
    }
    private bool CheckForErrors()
    {
        if (GetComponent<GraphSettings>() == null)
        {
            Debug.LogError("This GameObject has no GraphSettings script attached. Attach GraphSettings and restart");
            error = true;
            return true;
        }
        if (GetComponent<GraphSettings>().GridTextFont == null)
        {
            Debug.LogError("No font was found. Assign a font for GraphSettings.GridTextFont and restart");
            error = true;
        }
        if (error)
            return true;
        return false;
    }
    #endregion
}
