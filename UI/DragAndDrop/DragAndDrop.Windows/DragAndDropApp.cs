using Stride.Engine;

namespace DragAndDrop
{
    class DragAndDropApp
    {
        static void Main(string[] args)
        {
            using (var game = new Game())
            {
                game.Run();
            }
        }
    }
}
