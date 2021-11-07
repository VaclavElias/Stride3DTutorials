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
        private SpriteFont _font = null!;
        private UIElement? _dragElement;
        private Vector2? _offset;
        private int _lastZIndex = 1;
        private int _windowId = 1;

        public override void Start()
        {
            Log.Info("Windows Manager Started");

            _font = Game.Content.Load<SpriteFont>("StrideDefaultFont");

            CreateWindow("Main Window");
            CreateWindow($"Window {_windowId++}", new Vector3(0.02f));

            _mainCanvas.PreviewTouchMove += MainCanvas_PreviewTouchMove;
            _mainCanvas.PreviewTouchUp += MainCanvas_PreviewTouchUp;

            Entity.Add(new UIComponent()
            {
                Page = new UIPage() { RootElement = _mainCanvas }
            });
        }

        private void CreateWindow(string title, Vector3? position = null)
        {
            var panel = new WindowPanel(title, _font, position);

            panel.PreviewTouchDown += Panel_PreviewTouchDown;

            _mainCanvas.Children.Add(panel);
        }

        private void Panel_PreviewTouchDown(object? sender, TouchEventArgs e)
        {
            _dragElement = sender as UIElement;

            if (_dragElement is null) return;

            // we need to increase ZIndex so the active windows is on the top, let's hope you will close the game by the time this hits the max :)
            _dragElement.SetPanelZIndex(_lastZIndex++);

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
