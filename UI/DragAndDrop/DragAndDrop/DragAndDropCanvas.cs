using Stride.Core.Mathematics;
using Stride.Graphics;
using Stride.UI;
using Stride.UI.Controls;
using Stride.UI.Panels;

namespace DragAndDrop
{

    public class DragAndDropCanvas : Canvas
    {
        private readonly SpriteFont _font;
        private int _width = 300;
        private int _height = 200;
        private int _buttonSize = 25;

        public DragAndDropCanvas(string title, SpriteFont font, Vector3? position = null)
        {
            BackgroundColor = new Color(0, 0, 0, 200);
            Width = _width;
            Height = _height;
            CanBeHitByUser = true;

            this.SetCanvasRelativePosition(position ?? Vector3.Zero);

            _font = font;

            AddTitle(title);
            AddLine();
            AddCloseButton();

            PreviewTouchDown += Panel_PreviewTouchDown;
        }

        private void AddTitle(string title) => Children.Add(new TextBlock
        {
            Text = title,
            TextColor = Color.White,
            TextSize = 20,
            Font = _font,
            Margin = new Thickness(3, 3, 3, 0),
        });

        private void AddLine() => Children.Add(new Border
        {
            BorderColor = Color.White,
            BorderThickness = new Thickness(0, 0, 0, 2),
            Width = _width,
            Height = 27
        });

        private void AddCloseButton()
        {
            var button = new Button
            {
                Content = GetCloseButtonTitle(),
                BackgroundColor = new Color(0, 0, 0, 200),
                Width = _buttonSize,
                Height = _buttonSize,
                Margin = new Thickness(_width - _buttonSize, 0, 0, 0),
            };

            button.PreviewTouchUp += CloseButton_PreviewTouchUp;

            Children.Add(button);
        }

        private UIElement GetCloseButtonTitle() => new TextBlock
        {
            Text = "x",
            Width = _buttonSize,
            Height = _buttonSize,
            TextColor = Color.White,
            TextSize = 20,
            Font = _font,
            TextAlignment = TextAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        };

        private void CloseButton_PreviewTouchUp(object? sender, TouchEventArgs e)
        {
            if (Parent is not Canvas parent) return;

            parent.Children.Remove(this);
        }

        private void Panel_PreviewTouchDown(object? sender, TouchEventArgs e)
        {
            if (sender is not UIElement dragElement) return;

            if (dragElement.Parent is not DragAndDropContainer dragAndDropContainer) return;

            dragElement.SetPanelZIndex(dragAndDropContainer.GetNewZIndex());

            dragAndDropContainer.SetDragElement(dragElement);

            dragAndDropContainer.SetOffset(e.ScreenPosition - (Vector2)dragElement.GetCanvasRelativePosition());
        }
    }
}
