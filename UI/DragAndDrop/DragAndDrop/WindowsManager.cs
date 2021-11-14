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
        private TextBlock? _textBlock;
        private int _windowId = 1;
        private int _cubesCount = 100;
        private int _totalCubes;

        public override void Start()
        {
            Log.Info("Windows Manager Started");

            Font ??= LoadDefaultFont();

            _cubesGenerator = new CubesGenerator(Services);

            _mainCanvas.Children.Add(CreateMainWindow());
            _mainCanvas.Children.Add(CreateWindow($"Window {_windowId++}", new Vector3(0.02f)));

            Entity.Add(new UIComponent()
            {
                Page = new UIPage() { RootElement = _mainCanvas }
            });
        }

        private SpriteFont? LoadDefaultFont()
            => Game.Content.Load<SpriteFont>("StrideDefaultFont");

        private DragAndDropCanvas CreateMainWindow()
        {
            var canvas = CreateWindow("Main Window");

            _textBlock = GetTextBlock(GetTotal());
            _textBlock.Margin = new Thickness(10, 140, 0, 0);

            canvas.Children.Add(_textBlock);

            return canvas;
        }

        private string GetTotal() => $"Total Cubes: {_totalCubes}";

        private DragAndDropCanvas CreateWindow(string title, Vector3? position = null)
        {
            var canvas = new DragAndDropCanvas(title, Font!, position);
            canvas.SetPanelZIndex(_mainCanvas.GetNewZIndex());

            var newWindowButton = GetButton("New Window", new Vector2(10, 50));
            newWindowButton.PreviewTouchUp += NewWindowButton_PreviewTouchUp;
            canvas.Children.Add(newWindowButton);

            var generateItemsButton = GetButton("Generate Items", new Vector2(10, 90));
            generateItemsButton.PreviewTouchUp += GenerateItemsButton_PreviewTouchUp;
            canvas.Children.Add(generateItemsButton);

            return canvas;
        }

        private void GenerateItemsButton_PreviewTouchUp(object? sender, TouchEventArgs e)
        {
            GenerateCubes(_cubesCount);

            if (_textBlock is null) return;

            _textBlock.Text = GetTotal();
        }

        private void NewWindowButton_PreviewTouchUp(object? sender, TouchEventArgs e)
            => _mainCanvas.Children.Add(CreateWindow($"Window {_windowId++}"));

        private UIElement GetButton(string title, Vector2 position) => new Button
        {
            Content = GetTextBlock(title),
            BackgroundColor = new Color(100, 100, 100, 200),
            Margin = new Thickness(position.X, position.Y, 0, 0),
        };

        private TextBlock GetTextBlock(string title) => new TextBlock
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

            _totalCubes += count;
        }
    }
}
