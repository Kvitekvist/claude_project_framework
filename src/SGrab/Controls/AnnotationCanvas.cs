using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using SGrab.Common.Undo;
using SGrab.Models.Annotations;
using Brushes = System.Windows.Media.Brushes;
using Color = System.Windows.Media.Color;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;
using Pen = System.Windows.Media.Pen;
using Point = System.Windows.Point;
using Size = System.Windows.Size;

namespace SGrab.Controls;

public enum AnnotationTool
{
    Select,
    Rectangle,
    Ellipse,
    Step,
    Text,
}

/// <summary>
/// Owner-drawn editing surface: renders the capture plus a stack of
/// <see cref="AnnotationObject"/>s and handles create / select / move / resize /
/// delete with full undo/redo. The same <see cref="DrawScene"/> path is reused
/// by the exporter (TICKET-0012) to flatten without selection handles.
/// </summary>
public class AnnotationCanvas : FrameworkElement
{
    private const double HandleSize = 8;

    private readonly List<AnnotationObject> _annotations = new();
    private readonly UndoStack _undo = new();

    private BitmapSource? _image;
    private AnnotationObject? _selected;
    private int _stepCounter;

    // Interaction state.
    private enum DragMode { None, Creating, Moving, Resizing }

    private DragMode _drag = DragMode.None;
    private Point _dragStart;
    private Rect _origBounds;
    private AnnotationObject? _origClone;
    private int _activeHandle = -1;

    public AnnotationCanvas()
    {
        Focusable = true;
        _undo.Changed += (_, _) => UndoStackChanged?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler? SelectionChanged;

    public event EventHandler? UndoStackChanged;

    /// <summary>Raised when a text object should be edited (created or double-clicked).</summary>
    public event EventHandler<TextAnnotation>? TextEditRequested;

    public UndoStack Undo => _undo;

    public IReadOnlyList<AnnotationObject> Annotations => _annotations;

    public AnnotationTool ActiveTool { get; set; } = AnnotationTool.Select;

    public Color CurrentColor { get; set; } = Colors.Red;

    public double CurrentStrokeWidth { get; set; } = 3;

    public AnnotationObject? Selected
    {
        get => _selected;
        private set
        {
            if (!ReferenceEquals(_selected, value))
            {
                _selected = value;
                SelectionChanged?.Invoke(this, EventArgs.Empty);
                InvalidateVisual();
            }
        }
    }

    public BitmapSource? Image
    {
        get => _image;
        set
        {
            _image = value;
            _stepCounter = 0;
            InvalidateMeasure();
            InvalidateVisual();
        }
    }

    /// <summary>Sets the stroke colour of the selection (and the default for new objects), undoably.</summary>
    public void ApplyColorToSelection(Color color)
    {
        CurrentColor = color;
        if (Selected is { } sel)
        {
            var before = sel.Clone();
            sel.Color = color;
            NotifyObjectModified(sel, before);
        }
    }

    /// <summary>Sets the stroke width of the selection (and the default for new objects), undoably.</summary>
    public void ApplyStrokeToSelection(double strokeWidth)
    {
        CurrentStrokeWidth = strokeWidth;
        if (Selected is { } sel)
        {
            var before = sel.Clone();
            sel.StrokeWidth = strokeWidth;
            NotifyObjectModified(sel, before);
        }
    }

    /// <summary>Records an undoable edit to <paramref name="target"/> whose new state is already applied.</summary>
    public void NotifyObjectModified(AnnotationObject target, AnnotationObject before)
    {
        var after = target.Clone();
        _undo.Push(new DelegateAction(
            undo: () =>
            {
                target.CopyFrom(before);
                InvalidateVisual();
                SelectionChanged?.Invoke(this, EventArgs.Empty);
            },
            redo: () =>
            {
                target.CopyFrom(after);
                InvalidateVisual();
                SelectionChanged?.Invoke(this, EventArgs.Empty);
            }));
        InvalidateVisual();
    }

    /// <summary>Removes the selected object (undoable).</summary>
    public void DeleteSelected()
    {
        if (Selected is not { } obj)
        {
            return;
        }

        int index = _annotations.IndexOf(obj);
        _annotations.RemoveAt(index);
        Selected = null;
        InvalidateVisual();

        _undo.Push(new DelegateAction(
            undo: () =>
            {
                _annotations.Insert(Math.Min(index, _annotations.Count), obj);
                InvalidateVisual();
            },
            redo: () =>
            {
                _annotations.Remove(obj);
                if (ReferenceEquals(Selected, obj))
                {
                    Selected = null;
                }

                InvalidateVisual();
            }));
    }

    protected override Size MeasureOverride(Size availableSize)
        => _image is null ? new Size(0, 0) : new Size(_image.Width, _image.Height);

    protected override void OnRender(DrawingContext dc)
    {
        // Fill background so the element receives hit-tests even before an image.
        dc.DrawRectangle(Brushes.Transparent, null, new Rect(RenderSize));
        DrawScene(dc, includeSelection: true);
    }

    /// <summary>Draws the capture and all annotations; selection handles are optional.</summary>
    public void DrawScene(DrawingContext dc, bool includeSelection)
    {
        if (_image is not null)
        {
            dc.DrawImage(_image, new Rect(0, 0, _image.Width, _image.Height));
        }

        double ppd = VisualTreeHelper.GetDpi(this).PixelsPerDip;
        foreach (var annotation in _annotations)
        {
            annotation.Draw(dc, ppd);
        }

        if (includeSelection && Selected is { } selected)
        {
            DrawSelection(dc, selected);
        }
    }

    private static void DrawSelection(DrawingContext dc, AnnotationObject obj)
    {
        var dashed = new Pen(Brushes.DeepSkyBlue, 1) { DashStyle = new DashStyle(new double[] { 3, 3 }, 0) };
        dashed.Freeze();
        dc.DrawRectangle(null, dashed, obj.Bounds);

        var fill = Brushes.White;
        var border = new Pen(Brushes.DeepSkyBlue, 1);
        border.Freeze();
        foreach (var center in HandleCenters(obj.Bounds))
        {
            dc.DrawRectangle(fill, border, new Rect(
                center.X - (HandleSize / 2), center.Y - (HandleSize / 2), HandleSize, HandleSize));
        }
    }

    private static Point[] HandleCenters(Rect b) => new[]
    {
        new Point(b.Left, b.Top),                        // 0 TL
        new Point(b.Left + (b.Width / 2), b.Top),        // 1 TC
        new Point(b.Right, b.Top),                       // 2 TR
        new Point(b.Right, b.Top + (b.Height / 2)),      // 3 RM
        new Point(b.Right, b.Bottom),                    // 4 BR
        new Point(b.Left + (b.Width / 2), b.Bottom),     // 5 BC
        new Point(b.Left, b.Bottom),                     // 6 BL
        new Point(b.Left, b.Top + (b.Height / 2)),       // 7 LM
    };

    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
        base.OnMouseLeftButtonDown(e);
        Focus();
        var pos = e.GetPosition(this);

        if (ActiveTool == AnnotationTool.Select)
        {
            BeginSelectOrEdit(pos, e.ClickCount);
        }
        else
        {
            BeginCreate(pos);
        }

        // Only grab the mouse for interactions that track drag movement.
        if (_drag != DragMode.None)
        {
            CaptureMouse();
        }
    }

    private void BeginSelectOrEdit(Point pos, int clickCount)
    {
        // Double-click on a text object opens it for editing.
        if (clickCount == 2)
        {
            for (int i = _annotations.Count - 1; i >= 0; i--)
            {
                if (_annotations[i] is TextAnnotation text && text.HitTest(pos))
                {
                    Selected = text;
                    TextEditRequested?.Invoke(this, text);
                    return;
                }
            }
        }

        // Resize handle first (only when something is selected).
        if (Selected is { } sel)
        {
            int handle = HitHandle(sel.Bounds, pos);
            if (handle >= 0)
            {
                _drag = DragMode.Resizing;
                _activeHandle = handle;
                _dragStart = pos;
                _origBounds = sel.Bounds;
                _origClone = sel.Clone();
                return;
            }
        }

        // Otherwise select the topmost object under the cursor.
        AnnotationObject? hit = null;
        for (int i = _annotations.Count - 1; i >= 0; i--)
        {
            if (_annotations[i].HitTest(pos))
            {
                hit = _annotations[i];
                break;
            }
        }

        Selected = hit;
        if (hit is not null)
        {
            _drag = DragMode.Moving;
            _dragStart = pos;
            _origBounds = hit.Bounds;
            _origClone = hit.Clone();
        }
    }

    private void BeginCreate(Point pos)
    {
        switch (ActiveTool)
        {
            case AnnotationTool.Step:
                CreateStep(pos);
                return;
            case AnnotationTool.Text:
                CreateText(pos);
                return;
        }

        AnnotationObject created = ActiveTool == AnnotationTool.Ellipse
            ? new EllipseAnnotation()
            : new RectangleAnnotation();

        created.Color = CurrentColor;
        created.StrokeWidth = CurrentStrokeWidth;
        created.X = pos.X;
        created.Y = pos.Y;
        created.Width = 0;
        created.Height = 0;

        _annotations.Add(created);
        Selected = created;
        _drag = DragMode.Creating;
        _dragStart = pos;
        InvalidateVisual();
    }

    private void CreateStep(Point pos)
    {
        var step = new StepAnnotation
        {
            Number = ++_stepCounter,
            Color = CurrentColor,
            StrokeWidth = CurrentStrokeWidth,
        };
        step.X = pos.X - (step.Width / 2);
        step.Y = pos.Y - (step.Height / 2);

        AddObjectWithUndo(step);
        Selected = step;
        InvalidateVisual();
    }

    private void CreateText(Point pos)
    {
        var text = new TextAnnotation
        {
            Color = CurrentColor,
            X = pos.X,
            Y = pos.Y,
            Text = "Text",
        };

        AddObjectWithUndo(text);
        Selected = text;
        InvalidateVisual();
        TextEditRequested?.Invoke(this, text);
    }

    private void AddObjectWithUndo(AnnotationObject obj)
    {
        _annotations.Add(obj);
        _undo.Push(new DelegateAction(
            undo: () =>
            {
                _annotations.Remove(obj);
                if (ReferenceEquals(Selected, obj))
                {
                    Selected = null;
                }

                InvalidateVisual();
            },
            redo: () =>
            {
                if (!_annotations.Contains(obj))
                {
                    _annotations.Add(obj);
                }

                InvalidateVisual();
            }));
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        if (_drag == DragMode.None || Selected is not { } sel)
        {
            return;
        }

        var pos = e.GetPosition(this);
        switch (_drag)
        {
            case DragMode.Creating:
                sel.X = Math.Min(_dragStart.X, pos.X);
                sel.Y = Math.Min(_dragStart.Y, pos.Y);
                sel.Width = Math.Abs(pos.X - _dragStart.X);
                sel.Height = Math.Abs(pos.Y - _dragStart.Y);
                break;

            case DragMode.Moving:
                sel.X = _origBounds.X + (pos.X - _dragStart.X);
                sel.Y = _origBounds.Y + (pos.Y - _dragStart.Y);
                break;

            case DragMode.Resizing:
                ResizeSelected(sel, pos);
                break;
        }

        InvalidateVisual();
    }

    private void ResizeSelected(AnnotationObject sel, Point pos)
    {
        double dx = pos.X - _dragStart.X;
        double dy = pos.Y - _dragStart.Y;
        double left = _origBounds.Left, top = _origBounds.Top, right = _origBounds.Right, bottom = _origBounds.Bottom;

        switch (_activeHandle)
        {
            case 0: left += dx; top += dy; break;
            case 1: top += dy; break;
            case 2: right += dx; top += dy; break;
            case 3: right += dx; break;
            case 4: right += dx; bottom += dy; break;
            case 5: bottom += dy; break;
            case 6: left += dx; bottom += dy; break;
            case 7: left += dx; break;
        }

        sel.X = Math.Min(left, right);
        sel.Y = Math.Min(top, bottom);
        sel.Width = Math.Abs(right - left);
        sel.Height = Math.Abs(bottom - top);
    }

    protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
    {
        base.OnMouseLeftButtonUp(e);
        if (_drag == DragMode.None)
        {
            return;
        }

        ReleaseMouseCapture();
        var mode = _drag;
        _drag = DragMode.None;

        if (Selected is not { } sel)
        {
            return;
        }

        if (mode == DragMode.Creating)
        {
            FinishCreate(sel);
        }
        else if (_origClone is { } before)
        {
            NotifyObjectModified(sel, before);
        }

        _origClone = null;
        _activeHandle = -1;
        InvalidateVisual();
    }

    private void FinishCreate(AnnotationObject sel)
    {
        sel.Normalize();
        if (sel.Width < 3 && sel.Height < 3)
        {
            _annotations.Remove(sel);
            Selected = null;
            return;
        }

        _undo.Push(new DelegateAction(
            undo: () =>
            {
                _annotations.Remove(sel);
                if (ReferenceEquals(Selected, sel))
                {
                    Selected = null;
                }

                InvalidateVisual();
            },
            redo: () =>
            {
                if (!_annotations.Contains(sel))
                {
                    _annotations.Add(sel);
                }

                InvalidateVisual();
            }));
    }

    private static int HitHandle(Rect bounds, Point pos)
    {
        var centers = HandleCenters(bounds);
        for (int i = 0; i < centers.Length; i++)
        {
            var r = new Rect(
                centers[i].X - HandleSize, centers[i].Y - HandleSize, HandleSize * 2, HandleSize * 2);
            if (r.Contains(pos))
            {
                return i;
            }
        }

        return -1;
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);
        if (e.Key is Key.Delete or Key.Back && Selected is not null)
        {
            DeleteSelected();
            e.Handled = true;
        }
    }
}
