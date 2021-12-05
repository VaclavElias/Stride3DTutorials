namespace Stride.Engine.Builder
{
    // Or GameEngine?
    // I am following a bit this https://docs.microsoft.com/en-us/aspnet/core/tutorials/min-web-api?view=aspnetcore-6.0&tabs=visual-studio
    public class GameApplication
    {
        // This will bootstrap all boilerplate
        // Maybe it could have these options
        // GameType.2D, GameType.3D (Default)
        // GameWorldType.Simple (Default), GameWorldType.Ocean, GameWordlType.Grass
        //  - where this would bring better lighting, added ground and sky box
        public static Game CreateBuilder()
        {
            return new MinimalGame2();
        }
    }
}