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

namespace Space_Invaders
{



    public class Invaders
    {
        public Vector2 bot_position;
        public Texture2D bot_texture;
        public Color bot_color;


        public Invaders(Vector2 position, Texture2D texture, Color color)
        {
            bot_position = position;
            bot_texture = texture;
            bot_color = color;
        }


        public void draw(SpriteBatch sb)
        {

            sb.Draw(bot_texture, bot_position, bot_color);

        }

    }



    public class Soucoupes : Invaders
    {
        public Soucoupes(Vector2 position,SpaceInvaders sp, Color color) :
            base(position, sp.Content.Load<Texture2D>("sprites/classic/soucoupe"), color)
        {
        }


    }

    public class Crabes : Invaders
    {

        public Crabes(Vector2 position, SpaceInvaders sp, Color color) :
            base(position, sp.Content.Load<Texture2D>("sprites/classic/crabe1"), color)
        {
        }


    }


    public class Poulpes : Invaders
    {

        public Poulpes(Vector2 position, SpaceInvaders sp, Color color) :
            base(position, sp.Content.Load<Texture2D>("sprites/classic/poulpe1"), color)
        {
        }
    }

    public class Seches : Invaders
    {


        public Seches(Vector2 position, SpaceInvaders sp, Color color) :

            base(position, sp.Content.Load<Texture2D>("sprites/classic/poulpe1"), color)
        {
        }
    }



}









