using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Yellokiller.Yello_Killer;
using Microsoft.Xna.Framework.Content;
using System;


namespace Yellokiller
{
    class Ennemi : Case
    {

        Vector2 position;
        float vitesse_animation = 0.008f;
        int vitesse_sprite = 1;
        float index = 0;
        int maxIndex = 0;
        Rectangle? sourceRectangle = null;
        Rectangle rectangle;
        Texture2D texture;
        public bool monter = true, descendre = true, droite = true, gauche = true;
        List<Case> walkingList = new List<Case>();
        float autochemin = 0f;
        int msElapsed = 0;

        public Ennemi(Vector2 position, Rectangle? sourceRectangle, TypeCase type)
            : base(position, sourceRectangle, type)
        {
            this.position = position;
            this.sourceRectangle = sourceRectangle;
        }

        public List<Case> WalkingList
        {
            set { if (value != null) walkingList = value; }
        }

        public Texture2D Texture
        {
            get { return texture; }
        }

        public Rectangle? SourceRectangle
        {
            get { return sourceRectangle; }
            set { sourceRectangle = value; }
        }

        public Rectangle Rectangle
        {
            get { return rectangle; }
        }  

        public void LoadContent(ContentManager content, int maxIndex)
        {
            texture = content.Load<Texture2D>("ennemi");
            this.maxIndex = maxIndex;
        }

        public void Update(GameTime gameTime, Carte carte, GameplayScreen yk, Hero1 hero1, Hero2 hero2)
        {
            rectangle = new Rectangle((int)position.X, (int)position.Y, 18, 28);
            autochemin += gameTime.ElapsedGameTime.Milliseconds * 0.001f;

            if (this.position.Y - hero1.Position.Y < 5 && this.position.Y - hero1.Position.Y > -5
                || this.position.Y - hero2.Position.Y < 5 && this.position.Y - hero2.Position.Y > -5)
            {
                autochemin += gameTime.ElapsedGameTime.Milliseconds * 0.001f;
                Console.WriteLine("posision = " + (this.position.Y - hero1.Position.Y));            
            }

            if (sourceRectangle.Value.Y == 1)
                sourceRectangle = new Rectangle(5, 1, 16, 23);
            if (sourceRectangle.Value.Y == 33)
                sourceRectangle = new Rectangle(5, 33, 16, 23);
            if (sourceRectangle.Value.Y == 65)
                sourceRectangle = new Rectangle(5, 65, 16, 23);
            if (sourceRectangle.Value.Y == 98)
                sourceRectangle = new Rectangle(5, 98, 16, 23);

            if (autochemin < 5 &&  (position.X < 28 * Taille_Map.LARGEUR_MAP - 18 && droite &&
            (carte.Cases[(int)(position.Y + 27) / 28, (int)((position.X - 12) / 28) + 1].Type == TypeCase.herbe ||
             carte.Cases[(int)(position.Y + 27) / 28, (int)((position.X - 12) / 28) + 1].Type == TypeCase.herbeFoncee) &&
            (carte.Cases[(int)(position.Y + 7) / 28, (int)((position.X - 12) / 28) + 1].Type == TypeCase.herbe ||
             carte.Cases[(int)(position.Y + 7) / 28, (int)((position.X - 12) / 28) + 1].Type == TypeCase.herbeFoncee)))
            {

                index += gameTime.ElapsedGameTime.Milliseconds * vitesse_animation;
                if (index < maxIndex)
                {
                    sourceRectangle = new Rectangle((int)index * 5, 33, 16, 23);
                    position.X += vitesse_sprite;
                }
                else
                    index = 0f;
            }
            else if (autochemin < 10 && (position.Y < 28 * (Taille_Map.HAUTEUR_MAP - 1) && descendre &&
            (carte.Cases[(int)(position.Y / 28) + 1, (int)(position.X + 15) / 28].Type == TypeCase.herbe ||
             carte.Cases[(int)(position.Y / 28) + 1, (int)(position.X + 15) / 28].Type == TypeCase.herbeFoncee) &&
            (carte.Cases[(int)(position.Y / 28) + 1, (int)(position.X) / 28].Type == TypeCase.herbe ||
             carte.Cases[(int)(position.Y / 28) + 1, (int)position.X / 28].Type == TypeCase.herbeFoncee)))
            {
                index += gameTime.ElapsedGameTime.Milliseconds * vitesse_animation;
                if (index < maxIndex)
                {
                    sourceRectangle = new Rectangle((int)index * 5, 65, 16, 23);
                    position.Y += vitesse_sprite;
                }
                else
                    index = 0f;
            }
            else if (autochemin < 15 && (position.X > 0 && gauche &&
           (carte.Cases[(int)(position.Y + 27) / 28, (int)(position.X - 1) / 28].Type == TypeCase.herbe ||
            carte.Cases[(int)(position.Y + 27) / 28, (int)(position.X - 1) / 28].Type == TypeCase.herbeFoncee) &&
           (carte.Cases[(int)(position.Y + 7) / 28, (int)(position.X - 1) / 28].Type == TypeCase.herbe ||
            carte.Cases[(int)(position.Y + 7) / 28, (int)(position.X - 1) / 28].Type == TypeCase.herbeFoncee)))
            {

                index += gameTime.ElapsedGameTime.Milliseconds * vitesse_animation;
                if (index < maxIndex)
                {
                    sourceRectangle = new Rectangle((int)index * 5, 98, 16, 23);
                    position.X -= vitesse_sprite;
                }
                else
                    index = 0f;
            }
            else if (autochemin < 20 && (position.Y > 0 && monter &&
           (carte.Cases[(int)(position.Y + 6) / 28, (int)(position.X + 15) / 28].Type == TypeCase.herbe ||
            carte.Cases[(int)(position.Y + 6) / 28, (int)(position.X + 15) / 28].Type == TypeCase.herbeFoncee) &&
           (carte.Cases[(int)(position.Y + 6) / 28, (int)position.X / 28].Type == TypeCase.herbe ||
            carte.Cases[(int)(position.Y + 6) / 28, (int)position.X / 28].Type == TypeCase.herbeFoncee)))
            {
                index += gameTime.ElapsedGameTime.Milliseconds * vitesse_animation;
                if (index < maxIndex)
                {
                    sourceRectangle = new Rectangle((int)index * 5, 1, 16, 23);
                    position.Y -= vitesse_sprite;
                }
                else
                    index = 0f;
            }
            else
            {
                autochemin = 0;
            }

            msElapsed += gameTime.ElapsedGameTime.Milliseconds;
            if (walkingList.Count != 0)
            {
                if (msElapsed >= 100)
                {
                    msElapsed = 0;
                    position.X = walkingList[walkingList.Count - 1].Position.X;
                    position.Y = walkingList[walkingList.Count - 1].Position.Y;
                    Position = walkingList[walkingList.Count - 1].Position;
                    walkingList.RemoveAt(walkingList.Count - 1);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle camera)
        {
            spriteBatch.Draw(texture, new Vector2(position.X - camera.X, position.Y - camera.Y), sourceRectangle, Color.White);
        }
    }
}


       


    

           
       

           