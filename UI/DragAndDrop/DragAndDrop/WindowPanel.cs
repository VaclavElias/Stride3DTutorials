using Stride.Core.Mathematics;
using Stride.Graphics;
using Stride.UI;
using Stride.UI.Controls;
using Stride.UI.Panels;

namespace DragAndDrop
{
    public class WindowPanel : Canvas
    {
        private readonly SpriteFont _font;

        public WindowPanel(string title, SpriteFont font, Vector3? position = null)
        {
            BackgroundColor = new Color(0, 0, 0, 200);
            Width = 300;
            Height = 200;
            DefaultWidth = 300;
            DefaultHeight = 200;
            CanBeHitByUser = true;

            this.SetCanvasRelativePosition(position ?? Vector3.Zero);

            _font = font;

            Children.Add(GetTitle(title));
            Children.Add(GetLine());

            var closeButton = GetCloseButton();

            closeButton.PreviewTouchUp += CloseButton_PreviewTouchUp;

            Children.Add(closeButton);
        }

        private void CloseButton_PreviewTouchUp(object? sender, TouchEventArgs e)
        {
            var parent = (Canvas)Parent;

            if (parent is null) return;

            parent.Children.Remove(this);
        }

        //public void Set()
        //{
        //    Children.Add(GetLine());
        //}

        private UIElement GetTitle(string title) => new TextBlock
        {
            Text = title,
            TextColor = Color.White,
            TextSize = 20,
            Font = _font,
            Margin = new Thickness(3, 3, 3, 0),
        };

        private UIElement GetLine() => new Border
        {
            BorderColor = Color.White,
            BorderThickness = new Thickness(0, 0, 0, 2),
            Width = 300,
            Height = 27
        };

        private UIElement GetCloseButton() => new Button
        {
            Content = GetCloseButtonTitle(),
            BackgroundColor = new Color(0, 0, 0, 200),
            Width = 25,
            Height = 25,
            Margin = new Thickness(300 - 25, 0, 0, 0),
        };

        private UIElement GetCloseButtonTitle() => new TextBlock
        {
            Text = "x",
            Width = 20,
            Height = 25,
            TextColor = Color.White,
            TextSize = 20,
            Font = _font,
            TextAlignment = TextAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        };
    }
}
