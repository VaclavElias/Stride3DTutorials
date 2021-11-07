using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Graphics;
using Stride.UI;
using Stride.UI.Controls;
using Stride.UI.Panels;

namespace DragAndDrop
{
    public class WindowsManager : SyncScript
    {
        private Canvas _mainCanvas = new Canvas();
        private UIElement? _dragElement;
        private Vector2? _offset;

        public override void Start()
        {
            Log.Warning("Application Started");

            _mainCanvas.PreviewTouchMove += MainCanvas_PreviewTouchMove;
            _mainCanvas.PreviewTouchUp += MainCanvas_PreviewTouchUp;
            _mainCanvas.CanBeHitByUser = true;

            var font = Game.Content.Load<SpriteFont>("StrideDefaultFont");

            var panel = GetPanel();

            panel.Children.Add(GetLine());
            var closeButton = GetCloseButton();
            //closeButton.SetCanvasAbsolutePosition(new Vector3(0, -1, 0));
            panel.Children.Add(closeButton);
            panel.Children.Add(GetTitle("Window 1"));
            panel.CanBeHitByUser = true;
            panel.PreviewTouchDown += Panel_PreviewTouchDown;

            //Entity.Add(new UIComponent()
            //{
            //    Page = new UIPage() { RootElement = _mainCanvas }
            //});

            _mainCanvas.Children.Add(panel);

            //var position = new Vector3(0, 0, 0);

            //panel.SetCanvasRelativePosition(position);

            Entity.Add(new UIComponent()
            {
                Page = new UIPage() { RootElement = _mainCanvas }
            });

            //var uiEntity = new Entity();

            //uiEntity.Add(new UIComponent()
            //{
            //    Page = new UIPage() { RootElement = _mainCanvas }
            //});

            //SceneSystem.SceneInstance.RootScene.Entities.Add(uiEntity);

            Canvas GetPanel() => new()
            {
                BackgroundColor = new Color(0, 0, 0, 200),
                Width = 300,
                Height = 200,
                DefaultWidth = 300,
                DefaultHeight = 200
            };

            UIElement GetTitle(string title) => new TextBlock
            {
                Text = title,
                TextColor = Color.White,
                TextSize = 20,
                Font = font,
                Margin = new Thickness(3, 3, 3, 0),
            };

            UIElement GetLine() => new Border
            {
                BorderColor = Color.White,
                BorderThickness = new Thickness(0, 0, 0, 2),
                Width = 300,
                Height = 27
            };

            UIElement GetCloseButton() => new Button
            {
                Content = GetCloseButtonTitle(),
                BackgroundColor = new Color(0, 0, 0, 200),
                Width = 25,
                Height = 25,
                Margin = new Thickness(300 - 25, 0, 0, 0),
            };

            UIElement GetCloseButtonTitle() => new TextBlock
            {
                Text = "x",
                Width = 20,
                Height = 25,
                TextColor = Color.White,
                TextSize = 20,
                Font = font,
                TextAlignment = TextAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
        }

        private void Panel_PreviewTouchDown(object? sender, TouchEventArgs e)
        {
            _dragElement = sender as UIElement;
            var touchPosition = new Vector2(_mainCanvas.ActualWidth * e.ScreenPosition.X, _mainCanvas.ActualHeight * e.ScreenPosition.Y);

            var relativeElementPosition = _dragElement.GetCanvasRelativePosition();

            var position = new Vector2(_mainCanvas.ActualWidth * relativeElementPosition.X, _mainCanvas.ActualHeight * relativeElementPosition.Y);

            _offset = e.ScreenPosition - (Vector2)relativeElementPosition;

            var distance = touchPosition - position;

            var ratio = new Vector2(distance.X / 300, distance.Y / 200);

            //_dragElement.SetCanvasRelativePosition((Vector3)_offset);
            //_dragElement.SetCanvasPinOrigin((Vector3)ratio);

            //_offset = (Vector2?)_dragElement.GetCanvasRelativePosition();

            //_dragElement.SetCanvasRelativePosition(_dragElement.GetCanvasRelativePosition()- new Vector3(0.5f));
            //_dragElement.SetCanvasPinOrigin(new Vector3(0.5f));

            //_dragElement.SetCanvasPinOrigin((Vector3)(e.ScreenPosition - (Vector2)_dragElement.GetCanvasRelativePosition()));

            Log.Warning($"Panel Touched\n{e.ScreenPosition},\n{e.ScreenTranslation},\n{e.WorldPosition},\n{e.WorldTranslation},\nPanel Anchor:{_dragElement.GetCanvasPinOrigin()},\nPanel Position:{_dragElement.GetCanvasRelativePosition()}\n{e.ScreenPosition - (Vector2)_dragElement.GetCanvasRelativePosition()}\nTouch: {distance}\n\n");

            //Log.Warning($"Panel Touched {e.ScreenPosition}, {e.ScreenTranslation}, {e.WorldPosition}, {e.WorldTranslation}");

        }
        private void MainCanvas_PreviewTouchMove(object? sender, TouchEventArgs e)
        {
            if (_dragElement == null) return;

            var position = e.ScreenPosition;

            _dragElement.SetCanvasRelativePosition((Vector3)(position));

            Log.Info("Moving");
        }

        private void MainCanvas_PreviewTouchUp(object? sender, TouchEventArgs e)
        {
            _dragElement = null;
        }

        public override void Update()
        {
        }
    }
}
