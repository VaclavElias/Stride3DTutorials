using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Graphics;
using Stride.UI;
using Stride.UI.Panels;

namespace DragAndDrop
{
    public class WindowsManager : SyncScript
    {
        private readonly Canvas _mainCanvas = new() { CanBeHitByUser = true };
        private UIElement? _dragElement;
        private Vector2? _offset;

        public override void Start()
        {
            var font = Game.Content.Load<SpriteFont>("StrideDefaultFont");

            var panel = new WindowPanel("Window1", font);

            panel.PreviewTouchDown += Panel_PreviewTouchDown;

            _mainCanvas.PreviewTouchMove += MainCanvas_PreviewTouchMove;
            _mainCanvas.PreviewTouchUp += MainCanvas_PreviewTouchUp;

            _mainCanvas.Children.Add(panel);

            Entity.Add(new UIComponent()
            {
                Page = new UIPage() { RootElement = _mainCanvas }
            });
        }

        private void Panel_PreviewTouchDown(object? sender, TouchEventArgs e)
        {
            _dragElement = sender as UIElement;

            if (_dragElement is null) return;

            _offset = e.ScreenPosition - (Vector2)_dragElement.GetCanvasRelativePosition();
        }

        private void MainCanvas_PreviewTouchMove(object? sender, TouchEventArgs e)
        {
            if (_dragElement == null) return;

            _dragElement.SetCanvasRelativePosition((Vector3)(e.ScreenPosition - _offset ?? Vector2.Zero));
        }

        private void MainCanvas_PreviewTouchUp(object? sender, TouchEventArgs e)
            => _dragElement = null;

        public override void Update()
        {
        }
    }
}
