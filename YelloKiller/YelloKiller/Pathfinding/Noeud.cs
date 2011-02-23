﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace YelloKiller
{
    class Noeud
    {
        //Case _case;
        Vector2 position;
        Noeud parent;
        float estimatedMovement;

        public Noeud(Vector2 position, Noeud parent, Vector2 destination, Rectangle camera)
        {
            //this._case = _case;
            this.position = position;
            this.parent = parent;
            this.estimatedMovement = Math.Abs(position.X - destination.X/* + (int)camera.X / 28*/) + Math.Abs(position.Y - destination.Y /*+ (int)camera.Y / 28*/);
        }

        /*public Case Case
        {
            get { return _case; }
        }*/

        public Vector2 Position
        {
            get { return position; }
        }

        public Noeud Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        public float EstimatedMovement
        {
            get { return estimatedMovement; }
        }
        
        public List<Noeud> NoeudPossibles(Carte carte, Vector2 destination, Rectangle camera)
        {
            List<Noeud> result = new List<Noeud>();

            // Bas
            if (carte.Cases[(int)position.Y + 1, (int)position.X].Type > 0)
                result.Add(new Noeud(new Vector2(position.X, position.Y + 1), this, destination, camera/* new Rectangle(camera.X, camera.Y + 1, camera.Width, camera.Height)*/));
            // Droite
            if (carte.Cases[(int)position.Y, (int)position.X + 1].Type > 0)
                result.Add(new Noeud(new Vector2(position.X + 1, position.Y), this, destination, camera/*new Rectangle(camera.X + 1, camera.Y, camera.Width, camera.Height)*/));
            // Haut
            if ((int)position.Y - 1 >= 0 && carte.Cases[(int)position.Y - 1, (int)position.X].Type > 0)
                result.Add(new Noeud(new Vector2(position.X, position.Y - 1), this, destination, camera/*new Rectangle(camera.X, camera.Y - 1, camera.Width, camera.Height)*/));
            // Gauche
            if ((int)position.X - 1 >= 0 && carte.Cases[(int)position.Y, (int)position.X - 1].Type > 0)
                result.Add(new Noeud(new Vector2(position.X - 1, position.Y), this, destination, camera/*new Rectangle(camera.X - 1, camera.Y, camera.Width, camera.Height)*/));

            return result;
        }
    }
}