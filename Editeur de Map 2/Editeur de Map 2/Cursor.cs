using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace Editeur_de_Map_2
{
    public class Cursor
    {
        Texture2D texture;
        Vector2 position;

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        KeyboardState keyboardState, lastKeyboardState;

        public Cursor(ContentManager content)
        {
            position = new Vector2(0, 0);
            LoadContent(content);
        }

        private void LoadContent(ContentManager Content)
        {
            texture = Content.Load<Texture2D>("cursor");
        }

        public void Update(GameTime gameTime, int largeurMap, int hauteurMap)
        {
            lastKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Left) && lastKeyboardState.IsKeyUp(Keys.Left) && position.X > 0)
                position.X--;
            if (keyboardState.IsKeyDown(Keys.Right) && lastKeyboardState.IsKeyUp(Keys.Right) && position.X < largeurMap - 1)
                position.X++;
            if (keyboardState.IsKeyDown(Keys.Up) && lastKeyboardState.IsKeyUp(Keys.Up) && position.Y > 0)
                position.Y--;
            if (keyboardState.IsKeyDown(Keys.Down) && lastKeyboardState.IsKeyUp(Keys.Down) && position.Y < hauteurMap - 1)
                position.Y++;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Vector2(position.X * texture.Width, position.Y * texture.Height), Color.White);
        }
    }
}
