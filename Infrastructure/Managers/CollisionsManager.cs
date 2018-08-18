using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using C16_Ex03_Yakir_201049475_Omer_300471430.Interfaces;

namespace C16_Ex03_Yakir_201049475_Omer_300471430
{
    public class CollisionsManager : GameComponent, ICollisionsManager
    {
        protected readonly List<ICollidable> m_Collidables = new List<ICollidable>();

        public CollisionsManager(Game i_Game)
            : base(i_Game)
        {
            Game.Components.Add(this);
            Game.Services.AddService(typeof(ICollisionsManager), this);
        }

        public void AddObjectToMonitor(ICollidable i_Collidable)
        {
            if (!this.m_Collidables.Contains(i_Collidable))
            {
                this.m_Collidables.Add(i_Collidable);
                i_Collidable.PositionChanged += collidable_Changed;
                i_Collidable.SizeChanged += collidable_Changed;
                i_Collidable.VisibleChanged += collidable_Changed;
                i_Collidable.Disposed += collidable_Disposed;
            }
        }

        private void collidable_Disposed(object sender, EventArgs e)
        {
            ICollidable collidable = sender as ICollidable;

            if (collidable != null && this.m_Collidables.Contains(collidable))
            {
                collidable.PositionChanged -= collidable_Changed;
                collidable.SizeChanged -= collidable_Changed;
                collidable.VisibleChanged -= collidable_Changed;
                collidable.Disposed -= collidable_Disposed;
                m_Collidables.Remove(collidable);
            }
        }

        private void collidable_Changed(object sender, EventArgs e)
        {
            checkCollision(sender as ICollidable);
        }

        private void checkCollision(ICollidable i_Source)
        {
            List<ICollidable> collidedComponents;

            if (i_Source.Visible)
            {
                collidedComponents = new List<ICollidable>();
                foreach (ICollidable target in m_Collidables)
                {
                    if (i_Source != target && target.Visible)
                    {
                        if (target.CheckCollision(i_Source))
                        {
                            collidedComponents.Add(target);
                        }
                    }
                }

                foreach (ICollidable target in collidedComponents)
                {
                    target.Collided(i_Source);
                    i_Source.Collided(target);
                }
            }
        }
    }
}