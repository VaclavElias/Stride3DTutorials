using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.UI;
using Stride.UI.Controls;
using Stride.UI.Panels;
using System;

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

            var panel = GetPanel();

            panel.Children.Add(GetTitle("Window 1"));

            //Entity.Add(new UIComponent()
            //{
            //    Page = new UIPage() { RootElement = _mainCanvas }
            //});

            _mainCanvas.Children.Add(panel);

            var position = new Vector3(0.25f, 0.25f, 0);

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

            StackPanel GetPanel() => new()
            {
                BackgroundColor = new Color(0, 0, 0, 200),
                Width = 300,
                Height = 200
            };

            UIElement GetTitle(string title) => new TextBlock
            {
                Text = title,
                //TextSize = 12,
                //TextColor = Color.White,
            };
        }

        private void MainCanvasPreviewTouchUp(object? sender, TouchEventArgs e) => throw new NotImplementedException();

        private void MainCanvasPreviewTouchMove(object? sender, TouchEventArgs e)
        {
            throw new NotImplementedException();
        }

        public override void Update()
        {
        }
    }
}
