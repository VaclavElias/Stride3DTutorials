using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Graphics;
using Stride.UI;
using Stride.UI.Controls;
using Stride.UI.Panels;

namespace Stride3DTutorials
{
    public class WindowsManager : SyncScript
    {
        private Canvas _mainCanvas = new Canvas();
        private UIElement? _dragElement;
        private Vector2? _offset;

        public override void Start()
        {
            _mainCanvas.PreviewTouchMove += MainCanvasPreviewTouchMove;
            _mainCanvas.PreviewTouchUp += MainCanvasPreviewTouchUp;
            _mainCanvas.CanBeHitByUser = true;

            var font = Game.Content.Load<SpriteFont>("StrideDefaultFont");

            var panel = GetPanel();

            panel.Children.Add(GetLine());
            var closeButton = GetCloseButton();
            //closeButton.SetCanvasAbsolutePosition(new Vector3(0, -1, 0));
            panel.Children.Add(closeButton);
            panel.Children.Add(GetTitle("Window 1"));

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
                Height = 200
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

            UIElement GetCloseButton() => new Button {
                Content = GetCloseButtonTitle(),
                BackgroundColor = new Color(0, 0, 0, 200),
                Width = 25,
                Height = 25,
                Margin = new Thickness(300-25,0,0,0),
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

        private void MainCanvasPreviewTouchUp(object? sender, TouchEventArgs e)
        {
            Log.Info("Release mouse button");
        }

        private void MainCanvasPreviewTouchMove(object? sender, TouchEventArgs e)
        {
            Log.Info("Moving");
        }

        public override void Update()
        {
        }
    }
}
