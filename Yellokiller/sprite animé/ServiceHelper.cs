using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace Yellokiller
{
    static class ServiceHelper
    {
        static Game game;

        public static Game Game
        {
            set { game = value; }
        }

        public static void Add<T>(T service) where T : class
        {
            game.Services.AddService(typeof(T), service);
        }

        public static T Get<T>() where T : class
        {
            return game.Services.GetService(typeof(T)) as T;
        }
    }
}
