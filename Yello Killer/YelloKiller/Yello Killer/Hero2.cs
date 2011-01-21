using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yellokiller.Yello_Killer;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

namespace Yellokiller
{
    class Hero2
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
        List<Shuriken> _shuriken;
        KeyboardState current;
        public bool monter = true, descendre = true, droite = true, gauche = true;

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
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
        
        public Hero2(Vector2 position, Rectangle? sourceRectangle)
        {
            this.position = position;
            this.sourceRectangle = sourceRectangle;
            _shuriken = new List<Shuriken>();
        }

        public void LoadContent(ContentManager content, int maxIndex)
        {
            texture = content.Load<Texture2D>("Ninja2Trans");
            this.maxIndex = maxIndex;
        }

        public void Update(GameTime gameTime, Carte carte, Hero1 hero1, GameplayScreen yk)
        {
            lastKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();

            if (Keyboard.GetState().IsKeyDown(Keys.RightControl) && !current.IsKeyDown(Keys.RightControl))
            {
                _shuriken.Add(new Shuriken(yk, position, this.texture.Width));
            }

            current = Keyboard.GetState();

            rectangle = new Rectangle((int)Position.X, (int)Position.Y, 18, 28);
            Moteur_physique.Collision(this.rectangle, hero1.Rectangle, ref droite, ref gauche, ref monter, ref descendre);

            if (Keyboard.GetState().IsKeyUp(Keys.Up))                        // arreter le sprite
            {
                if (sourceRectangle.Value.Y == 133)
                    sourceRectangle = new Rectangle(24, 133, 16, 28);
                if (sourceRectangle.Value.Y == 198)
                    sourceRectangle = new Rectangle(24, 197, 16, 28);
                if (sourceRectangle.Value.Y == 230)
                    sourceRectangle = new Rectangle(24, 229, 16, 28);
                if (sourceRectangle.Value.Y == 166)
                    sourceRectangle = new Rectangle(24, 165, 16, 28);
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
                    position.Y -= 1 * vitesse_sprite;
                }
                else
                {
                    index = 0f;
                }

                if (keyboardState.IsKeyDown(Keys.RightShift))
                {
                    position.Y -= 1 * vitesse_sprite * 2;
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
                    position.Y += 1 * vitesse_sprite;
                }
                else
                {
                    index = 0f;
                }

                if (keyboardState.IsKeyDown(Keys.RightShift))
                {
                    position.Y += 1 * vitesse_sprite * 2;
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
                    position.X -= 1 * vitesse_sprite;
                }
                else
                {
                    index = 0f;
                }

                if (keyboardState.IsKeyDown(Keys.RightShift))
                {
                    position.X -= 1 * vitesse_sprite * 2;
                    vitesse_animation = 0.008f * 2;
                }
            }

            if (position.X < 28 * Taille_Map.LARGEUR_MAP - 16 && keyboardState.IsKeyDown(Keys.Right) && droite &&
                (carte.Cases[(int)(position.Y + 27) / 28, (int)((position.X - 12) / 28) + 1].Type == TypeCase.herbe ||
                 carte.Cases[(int)(position.Y + 27) / 28, (int)((position.X - 12) / 28) + 1].Type == TypeCase.herbeFoncee) &&
                (carte.Cases[(int)(position.Y + 7) / 28, (int)((position.X - 12) / 28) + 1].Type == TypeCase.herbe ||
                 carte.Cases[(int)(position.Y + 7) / 28, (int)((position.X - 12) / 28) + 1].Type == TypeCase.herbeFoncee))
            {
                index += gameTime.ElapsedGameTime.Milliseconds * vitesse_animation;
                if (index < maxIndex)
                {
                    sourceRectangle = new Rectangle((int)index * 48, 166, 16, 28);
                    position.X += 1 * vitesse_sprite;
                }
                else
                {
                    index = 0f;
                }

                if (keyboardState.IsKeyDown(Keys.RightShift))
                {
                    position.X += 1 * vitesse_sprite * 2;
                    vitesse_animation = 0.008f * 2;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, Rectangle camera)
        {
            spriteBatch.Draw(texture, new Vector2(position.X - camera.X, position.Y - camera.Y), sourceRectangle, Color.White);
            for (int i = 0; i < _shuriken.Count; i++)
            {
                Shuriken m = _shuriken[i];
                m.Update(gameTime);
                m.draw(spriteBatch);

                if (m.Get_Y() < -14)
                {
                    _shuriken.Remove(m);
                }
            }
        }
    }
}
