using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Yellokiller.Yello_Killer;
using Microsoft.Xna.Framework.Content;
using System;

namespace Yellokiller
{
    class Hero2 : Case
    {
        Vector2 position;
        float vitesse_animation = 0.008f;
        int vitesse_sprite = 1;
        float index = 0;
        int maxIndex = 0;
        Rectangle? sourceRectangle = null;
        Rectangle rectangle;
        Texture2D texture;
        KeyboardState lastKeyboardState, keyboardState;
        int msElapsed = 0;
        public bool ishero2 = false;

        KeyboardState current;
        public bool monter = true, descendre = true, droite = true, gauche = true;

        List<Case> walkingList = new List<Case>();
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

        public Hero2(Vector2 position, Rectangle? sourceRectangle, TypeCase type)
            : base(position, sourceRectangle, type)
        {
            this.position = position;
            this.sourceRectangle = sourceRectangle;
        }

        public void LoadContent(ContentManager content, int maxIndex)
        {
            texture = content.Load<Texture2D>("Ninja2Trans");
            this.maxIndex = maxIndex;
        }

        public void Update(GameTime gameTime, Carte carte, Hero1 hero1, GameplayScreen yk, ref Rectangle camera, List<Shuriken> _shuriken)
        {
            lastKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();

            rectangle = new Rectangle((int)Position.X, (int)Position.Y, 18, 28);
            Moteur_physique.Collision(this.rectangle, hero1.Rectangle, ref droite, ref gauche, ref monter, ref descendre);


            if (keyboardState.IsKeyDown(Keys.RightControl) && !current.IsKeyDown(Keys.RightControl))
            {
                ishero2 = true;
                _shuriken.Add(new Shuriken(yk, new Vector2(position.X, position.Y), this.texture.Width, hero1, this));
                Console.WriteLine("ajout shuriken");
            }
            else
                ishero2 = false;

            rectangle = new Rectangle((int)position.X, (int)position.Y, 18, 28);
            Moteur_physique.Collision(this.rectangle, hero1.Rectangle, ref droite, ref gauche, ref monter, ref descendre);

            if (Keyboard.GetState().IsKeyUp(Keys.Up))                        // arreter le sprite
            {
                if (sourceRectangle.Value.Y == 133)
                    sourceRectangle = new Rectangle(24, 133, 16, 28);
                if (sourceRectangle.Value.Y == 198)
                    sourceRectangle = new Rectangle(24, 198, 16, 28);
                if (sourceRectangle.Value.Y == 230)
                    sourceRectangle = new Rectangle(24, 230, 16, 28);
                if (sourceRectangle.Value.Y == 166)
                    sourceRectangle = new Rectangle(24, 166, 16, 28);
            }

            if (keyboardState.IsKeyUp(Keys.RightShift))
                vitesse_animation = 0.008f;

            if (position.Y > 0 && keyboardState.IsKeyDown(Keys.Up) && monter &&
               (carte.Cases[(int)(position.Y + 6) / 28, (int)(position.X + 15) / 28].Type == TypeCase.herbe ||
                carte.Cases[(int)(position.Y + 6) / 28, (int)(position.X + 15) / 28].Type == TypeCase.herbeFoncee) &&
               (carte.Cases[(int)(position.Y + 6) / 28, (int)position.X / 28].Type == TypeCase.herbe ||
                carte.Cases[(int)(position.Y + 6) / 28, (int)position.X / 28].Type == TypeCase.herbeFoncee))
            {
                index += gameTime.ElapsedGameTime.Milliseconds * vitesse_animation;
                if (index < maxIndex)
                {
                    sourceRectangle = new Rectangle((int)index * 48, 133, 16, 28);
                    position.Y -= vitesse_sprite;
                    if (camera.Y > 0 && position.Y < 28 * Taille_Map.HAUTEUR_MAP - 200)
                        camera.Y -= vitesse_sprite;
                }
                else
                    index = 0f;

                if (keyboardState.IsKeyDown(Keys.RightShift))
                {
                    position.Y -= 2 * vitesse_sprite;
                    if (camera.Y > 0 && position.Y < 28 * Taille_Map.HAUTEUR_MAP - 200)
                        camera.Y -= 2 * vitesse_sprite;
                    vitesse_animation = 0.008f * 2;
                }
            }

            if (position.Y < 28 * (Taille_Map.HAUTEUR_MAP - 1) && keyboardState.IsKeyDown(Keys.Down) && descendre &&
                (carte.Cases[(int)(position.Y / 28) + 1, (int)(position.X + 15) / 28].Type == TypeCase.herbe ||
                 carte.Cases[(int)(position.Y / 28) + 1, (int)(position.X + 15) / 28].Type == TypeCase.herbeFoncee) &&
                (carte.Cases[(int)(position.Y / 28) + 1, (int)(position.X) / 28].Type == TypeCase.herbe ||
                 carte.Cases[(int)(position.Y / 28) + 1, (int)position.X / 28].Type == TypeCase.herbeFoncee))
            {
                index += gameTime.ElapsedGameTime.Milliseconds * vitesse_animation;

                if (index < maxIndex)
                {
                    sourceRectangle = new Rectangle((int)index * 48, 198, 16, 28);
                    position.Y += vitesse_sprite;
                    if (camera.Y + vitesse_sprite < 28 * (Taille_Map.HAUTEUR_MAP - camera.Height) && position.Y > 200)
                        camera.Y += vitesse_sprite;
                }
                else
                    index = 0f;

                if (keyboardState.IsKeyDown(Keys.RightShift))
                {
                    position.Y += 2 * vitesse_sprite;
                    if (camera.Y + 2 * vitesse_sprite < 28 * (Taille_Map.HAUTEUR_MAP - camera.Height) && position.Y > 200)
                        camera.Y += 2 * vitesse_sprite;
                    vitesse_animation = 0.008f * 2;
                }
            }

            if (position.X > 0 && keyboardState.IsKeyDown(Keys.Left) && gauche &&
               (carte.Cases[(int)(position.Y + 27) / 28, (int)(position.X - 1) / 28].Type == TypeCase.herbe ||
                carte.Cases[(int)(position.Y + 27) / 28, (int)(position.X - 1) / 28].Type == TypeCase.herbeFoncee) &&
               (carte.Cases[(int)(position.Y + 7) / 28, (int)(position.X - 1) / 28].Type == TypeCase.herbe ||
                carte.Cases[(int)(position.Y + 7) / 28, (int)(position.X - 1) / 28].Type == TypeCase.herbeFoncee))
            {
                index += gameTime.ElapsedGameTime.Milliseconds * vitesse_animation;
                if (index < maxIndex)
                {
                    sourceRectangle = new Rectangle((int)index * 48, 230, 16, 28);
                    position.X -= vitesse_sprite;
                    if (camera.X > 0 && position.X < 28 * Taille_Map.LARGEUR_MAP - 200)
                        camera.X -= vitesse_sprite;
                }
                else
                    index = 0f;

                if (keyboardState.IsKeyDown(Keys.RightShift))
                {
                    position.X -= 2 * vitesse_sprite;
                    if (camera.X > 0 && position.X < 28 * Taille_Map.LARGEUR_MAP - 200)
                        camera.X -= 2 * vitesse_sprite;
                    vitesse_animation = 0.008f * 2;
                }
            }

            if (position.X < 28 * Taille_Map.LARGEUR_MAP - 18 && keyboardState.IsKeyDown(Keys.Right) && droite &&
                (carte.Cases[(int)(position.Y + 27) / 28, (int)((position.X - 12) / 28) + 1].Type == TypeCase.herbe ||
                 carte.Cases[(int)(position.Y + 27) / 28, (int)((position.X - 12) / 28) + 1].Type == TypeCase.herbeFoncee) &&
                (carte.Cases[(int)(position.Y + 7) / 28, (int)((position.X - 12) / 28) + 1].Type == TypeCase.herbe ||
                 carte.Cases[(int)(position.Y + 7) / 28, (int)((position.X - 12) / 28) + 1].Type == TypeCase.herbeFoncee))
            {
                index += gameTime.ElapsedGameTime.Milliseconds * vitesse_animation;
                if (index < maxIndex)
                {
                    sourceRectangle = new Rectangle((int)index * 48, 166, 16, 28);
                    position.X += vitesse_sprite;
                    if (camera.X + vitesse_sprite < 28 * (Taille_Map.LARGEUR_MAP - camera.Width) && position.X > 200)
                        camera.X += vitesse_sprite;
                }
                else
                    index = 0f;

                if (keyboardState.IsKeyDown(Keys.RightShift))
                {
                    position.X += 2 * vitesse_sprite;
                    if (camera.X + 2 * vitesse_sprite < 28 * (Taille_Map.LARGEUR_MAP - camera.Width) && position.X > 200)
                        camera.X += 2 * vitesse_sprite;
                    vitesse_animation = 0.008f * 2;
                }
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

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, Rectangle camera, Carte carte, Hero2 hero2)
        {
            spriteBatch.Draw(texture, new Vector2(position.X - camera.X, position.Y - camera.Y), sourceRectangle, Color.White);

        }
    }
}