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

            var panel = GetStackPanel();

            panel.Children.Add(GetTitle("Window 1"));
            panel.Children.Add(GetCloseButton());
            panel.Children.Add(GetLine());

            //Entity.Add(new UIComponent()
            //{
            //    Page = new UIPage() { RootElement = _mainCanvas }
            //});

            _mainCanvas.Children.Add(panel);

            var position = new Vector3(0, 0, 0);

            panel.SetCanvasRelativePosition(position);

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

            StackPanel GetStackPanel() => new()
            {
                BackgroundColor = new Color(0, 0, 0, 200),
                Width = 300,
                Height = 200
            };

            UIElement GetTitle(string title) => new TextBlock
            {
                Text = title,
                //Width = 50,
                //Height = 30,
                TextColor = Color.White,
                TextSize = 20,
                Font = font,
                Margin = new Thickness(3, 3, 3, 0)
            };

            UIElement GetLine() => new Border
            {
                BorderColor = Color.White,
                BorderThickness = new Thickness(0, 0, 0, 2)
            };

            UIElement GetCloseButton() => new Button();
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
