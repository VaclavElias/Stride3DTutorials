namespace Minimal
{
    // This opens black screen
    static class Program
    {
        static void Main(string[] args)
        {
            using var game = new MyGame2();

            game.Run();
        }
    }
}
