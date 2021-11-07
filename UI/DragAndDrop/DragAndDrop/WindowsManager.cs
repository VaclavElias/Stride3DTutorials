using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Graphics;
using Stride.Rendering;
using Stride.Rendering.ProceduralModels;
using Stride.UI;
using Stride.UI.Controls;
using Stride.UI.Panels;
using System;

namespace DragAndDrop
{
    // Do I need to remove handlers?
    // Do I need to default the font as I did?
    // Can we simplify anything else?
    // Why is not my font sharp?
    // The font is not white https://github.com/stride3d/stride/issues/1154
    public class WindowsManager : SyncScript
    {
        private readonly Canvas _mainCanvas = new() { CanBeHitByUser = true };
        private SpriteFont _font = null!;
        private UIElement? _dragElement;
        private Vector2? _offset;
        private int _lastZIndex = 1;
        private int _windowId = 1;
        private Random _random = new Random();

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

            var newWindowButton = GetButton("New Window", new Vector2(10, 50));
            newWindowButton.PreviewTouchUp += NewWindowButton_PreviewTouchUp;

            var generateItemsButton = GetButton("Generate Items", new Vector2(10, 90));
            generateItemsButton.PreviewTouchUp += GenerateItemsButton_PreviewTouchUp;

            panel.Children.Add(newWindowButton);
            panel.Children.Add(generateItemsButton);

            _mainCanvas.Children.Add(panel);
        }

        private void GenerateItemsButton_PreviewTouchUp(object? sender, TouchEventArgs e)
        {
            GenerateCube();
        }

        private void NewWindowButton_PreviewTouchUp(object? sender, TouchEventArgs e)
        {
            CreateWindow($"Window {_windowId++}");
        }

        private void Panel_PreviewTouchDown(object? sender, TouchEventArgs e)
        {
            _dragElement = sender as UIElement;

            if (_dragElement is null) return;

            // we need to increase ZIndex so the active window is on the top, let's hope you will close the game by the time this hits the max :)
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

        private UIElement GetButton(string title, Vector2 position) => new Button
        {
            Content = GetButtonTitle(title),
            BackgroundColor = new Color(100, 100, 100, 200),
            Margin = new Thickness(position.X, position.Y, 0, 0),
        };

        private UIElement GetButtonTitle(string title) => new TextBlock
        {
            Text = title,
            TextColor = Color.White,
            TextSize = 18,
            Font = _font,
            TextAlignment = TextAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        };

        private void GenerateCube()
        {
            var cube = new CubeProceduralModel();

            var model = new Model();
            cube.Generate(Services, model);

            var entity = new Entity();
            entity.Transform.Scale = new Vector3(0.1f);
            entity.Transform.Position = new Vector3(
                -3 + (float)(_random.NextDouble() * 6),
                (float)(_random.NextDouble() * 1) + 2,
                -3 + (float)(_random.NextDouble() * 6));

            entity.GetOrCreate<ModelComponent>().Model = model;

            Entity.AddChild(entity);
        }

        public override void Update()
        {
        }
    }
}
