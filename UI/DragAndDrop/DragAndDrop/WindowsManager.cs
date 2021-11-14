using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Graphics;
using Stride.UI;
using Stride.UI.Controls;

namespace DragAndDrop
{
    public class WindowsManager : StartupScript
    {
        public SpriteFont? Font { get; set; }

        private readonly DragAndDropContainer _mainCanvas = new();
        private CubesGenerator? _cubesGenerator;
        private int _windowId = 1;
        private int _cubesCount = 100;

        public override void Start()
        {
            Log.Info("Windows Manager Started");

            Font ??= LoadDefaultFont();

            _cubesGenerator = new CubesGenerator(Services);

            CreateWindow("Main Window");
            CreateWindow($"Window {_windowId++}", new Vector3(0.02f));

            Entity.Add(new UIComponent()
            {
                Page = new UIPage() { RootElement = _mainCanvas }
            });
        }

        private SpriteFont? LoadDefaultFont()
            => Game.Content.Load<SpriteFont>("StrideDefaultFont");

        private void CreateWindow(string title, Vector3? position = null)
        {
            var panel = new DragAndDropCanvas(title, Font!, position);
            panel.SetPanelZIndex(_mainCanvas.GetNewZIndex());

            var newWindowButton = GetButton("New Window", new Vector2(10, 50));
            newWindowButton.PreviewTouchUp += NewWindowButton_PreviewTouchUp;
            panel.Children.Add(newWindowButton);

            var generateItemsButton = GetButton("Generate Items", new Vector2(10, 90));
            generateItemsButton.PreviewTouchUp += GenerateItemsButton_PreviewTouchUp;
            panel.Children.Add(generateItemsButton);

            _mainCanvas.Children.Add(panel);
        }

        private void GenerateItemsButton_PreviewTouchUp(object? sender, TouchEventArgs e)
            => GenerateCubes(_cubesCount);

        private void NewWindowButton_PreviewTouchUp(object? sender, TouchEventArgs e)
            => CreateWindow($"Window {_windowId++}");

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
            Font = Font,
            TextAlignment = TextAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        };

        private void GenerateCubes(int count)
        {
            if (_cubesGenerator is null) return;

            for (int i = 0; i < count; i++)
            {
                Entity.AddChild(_cubesGenerator.GetCube());
            }
        }
    }
}
