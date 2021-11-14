using Stride.Core.Mathematics;
using Stride.UI;
using Stride.UI.Panels;

namespace DragAndDrop
{
    public class DragAndDropContainer : Canvas
    {
        private UIElement? _dragElement;
        private int _lastZIndex = 1;
        private Vector2? _offset;

        public DragAndDropContainer()
        {
            CanBeHitByUser = true;

            PreviewTouchMove += MainCanvas_PreviewTouchMove;
            PreviewTouchUp += MainCanvas_PreviewTouchUp;
        }

        public int GetNewZIndex() => _lastZIndex++;

        public void SetOffset(Vector2 vector) => _offset = vector;

        public void SetDragElement(UIElement element) => _dragElement = element;
        
        private void MainCanvas_PreviewTouchMove(object? sender, TouchEventArgs e)
        {
            //if (_dragElement is null) return;

            _dragElement?.SetCanvasRelativePosition((Vector3)(e.ScreenPosition - _offset ?? Vector2.Zero));
        }

        private void MainCanvas_PreviewTouchUp(object? sender, TouchEventArgs e)
            => _dragElement = null;
    }
}
