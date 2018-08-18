using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using C16_Ex03_Yakir_201049475_Omer_300471430.CustomEventArgs;
using C16_Ex03_Yakir_201049475_Omer_300471430.Infrastructure.Models.Menu;

namespace C16_Ex03_Yakir_201049475_Omer_300471430
{
    public abstract class CompositeDrawableComponent<ComponentType>
        : DrawableGameComponent, ICollection<ComponentType>
        where ComponentType : IGameComponent
    {
        public event EventHandler<GameComponentEventArgs<ComponentType>> ComponentAdded;

        public event EventHandler<GameComponentEventArgs<ComponentType>> ComponentRemoved;

        private bool m_IsInitialized;
        private Collection<ComponentType> m_Components;
        private List<ComponentType> m_UninitializedComponents;
        protected List<IUpdateable> m_UpdateableComponents;
        protected List<IDrawable> m_DrawableComponents;
        protected List<Sprite> m_Sprites;
        private SpriteBatch m_SpriteBatch;
        
        protected Vector2 CenterOfViewPort
        {
            get
            {
                return new Vector2((float)Game.GraphicsDevice.Viewport.Width / 2, (float)Game.GraphicsDevice.Viewport.Height / 2);
            }
        }

        protected SpriteBatch SpriteBatch
        {
            get { return m_SpriteBatch; }
            set { m_SpriteBatch = value; }
        }

        public CompositeDrawableComponent(Game i_Game)
            : base(i_Game)
        {
            m_Components = new Collection<ComponentType>();
            m_UninitializedComponents = new List<ComponentType>();
            m_UpdateableComponents = new List<IUpdateable>();
            m_DrawableComponents = new List<IDrawable>();
            m_Sprites = new List<Sprite>();
        }

        protected virtual void OnComponentAdded(GameComponentEventArgs<ComponentType> e)
        {
            IUpdateable asUpdateable;
            IDrawable asDrawable;

            if(m_IsInitialized)
            {
                InitializeComponent(e.GameComponent);
            }
            else
            {
                m_UninitializedComponents.Add(e.GameComponent);
            }

            asUpdateable = e.GameComponent as IUpdateable;
            if(asUpdateable != null)
            {
                insertSorted(asUpdateable);
                asUpdateable.UpdateOrderChanged += new EventHandler<EventArgs>(child_UpdateOrderChanged);
            }

            asDrawable = e.GameComponent as IDrawable;
            if(asDrawable != null)
            {
                insertSorted(asDrawable);
                asDrawable.DrawOrderChanged += new EventHandler<EventArgs>(child_DrawOrderChanged);
            }

            if(ComponentAdded != null)
            {
                ComponentAdded.Invoke(this, e);
            }
        }

        protected virtual void OnComponentRemoved(GameComponentEventArgs<ComponentType> e)
        {
            IUpdateable asUpdateable;
            IDrawable asDrawable;
            Sprite asSprite;

            if(!m_IsInitialized)
            {
                m_UninitializedComponents.Remove(e.GameComponent);
            }

            asUpdateable = e.GameComponent as IUpdateable;
            if(asUpdateable != null)
            {
                m_UpdateableComponents.Remove(asUpdateable);
                asUpdateable.UpdateOrderChanged -= child_UpdateOrderChanged;
            }

            asSprite = e.GameComponent as Sprite;
            if(asSprite != null)
            {
                m_Sprites.Remove(asSprite);
                asSprite.DrawOrderChanged -= child_DrawOrderChanged;
            }
            else
            {
                asDrawable = e.GameComponent as IDrawable;
                if(asDrawable != null)
                {
                    m_DrawableComponents.Remove(asDrawable);
                    asDrawable.DrawOrderChanged -= child_DrawOrderChanged;
                }
            }

            if(ComponentRemoved != null)
            {
                ComponentRemoved.Invoke(this, e);
            }
        }

        private void child_DrawOrderChanged(object sender, EventArgs e)
        {
            IDrawable asDrawable;
            Sprite asSprite;

            asDrawable = sender as IDrawable;
            asSprite = sender as Sprite;
            if(asSprite != null)
            {
                m_Sprites.Remove(asSprite);
            }
            else
            {
                m_DrawableComponents.Remove(asDrawable);
            }

            insertSorted(asDrawable);
        }

        private void child_UpdateOrderChanged(object sender, EventArgs e)
        {
            IUpdateable asUpdateable;

            asUpdateable = sender as IUpdateable;
            m_UpdateableComponents.Remove(asUpdateable);
            insertSorted(asUpdateable);
        }

        private void insertSorted(IUpdateable i_Updateable)
        {
            int idx;

            idx = m_UpdateableComponents.BinarySearch(i_Updateable, UpdateableComparer.Default);
            m_UpdateableComponents.Insert(~idx, i_Updateable);
        }

        private void insertSorted(IDrawable i_Drawable)
        {
            int idx;
            Sprite asSprite;

            asSprite = i_Drawable as Sprite;
            if(asSprite != null)
            {
                idx = m_Sprites.BinarySearch(asSprite, DrawableComparer<Sprite>.Default);
                m_Sprites.Insert(~idx, asSprite);
            }
            else
            {
                idx = m_DrawableComponents.BinarySearch(i_Drawable, DrawableComparer<IDrawable>.Default);
                m_DrawableComponents.Insert(~idx, i_Drawable);
            }
        }

        protected void InitializeComponent(ComponentType i_Component)
        {
            Sprite asSprite;

            asSprite = i_Component as Sprite;
            if(asSprite != null)
            {
                asSprite.SpriteBatch = m_SpriteBatch;
            }

            i_Component.Initialize();
            m_UninitializedComponents.Remove(i_Component);
        }

        public override void Initialize()
        {
            if(!m_IsInitialized)
            {
                while(m_UninitializedComponents.Count > 0)
                {
                    InitializeComponent(m_UninitializedComponents[0]);
                }

                base.Initialize();
                m_IsInitialized = true;
            }
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            m_SpriteBatch = new SpriteBatch(this.GraphicsDevice);
            foreach(Sprite sprite in m_Sprites)
            {
                sprite.SpriteBatch = m_SpriteBatch;
            }
        }

        public override void Update(GameTime i_GameTime)
        {
            IUpdateable updateable;

            for (int i = 0; i < m_UpdateableComponents.Count; i++)
            {
                updateable = m_UpdateableComponents[i];
                if(updateable.Enabled)
                {
                    updateable.Update(i_GameTime);
                }
            }
        }

        public override void Draw(GameTime i_GameTime)
        {
            foreach(IDrawable drawable in m_DrawableComponents)
            {
                if (drawable.Visible)
                {
                    drawable.Draw(i_GameTime);
                }
            }

            this.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
            foreach(Sprite sprite in m_Sprites)
            {
                if (sprite.Visible)
                {
                    sprite.Draw(i_GameTime);
                }
            }

            this.SpriteBatch.End();
        }

        protected override void Dispose(bool i_Disposing)
        {
            IDisposable asDisposable;

            if(i_Disposing)
            {
                for(int i = 0; i < m_Components.Count; i++)
                {
                    asDisposable = m_Components[i] as IDisposable;
                    if(asDisposable != null)
                    {
                        asDisposable.Dispose();
                    }
                }
            }

            base.Dispose(i_Disposing);
        }

        public virtual void Add(ComponentType i_Component)
        {
            this.InsertItem(m_Components.Count, i_Component);
        }

        protected virtual void InsertItem(int i_Idx, ComponentType i_Component)
        {
            if(m_Components.IndexOf(i_Component) != -1)
            {
                throw new ArgumentException("Duplicate components are not allowed in the same GameComponentManager.");
            }

            if(i_Component != null)
            {
                m_Components.Insert(i_Idx, i_Component);
                OnComponentAdded(new GameComponentEventArgs<ComponentType>(i_Component));
            }
        }

        public void Clear()
        {
            for (int i = 0; i < this.Count; i++)
            {
                OnComponentRemoved(new GameComponentEventArgs<ComponentType>(m_Components[i]));
            }

            m_Components.Clear();
        }

        public bool Contains(ComponentType i_Component)
        {
            return m_Components.Contains(i_Component);
        }

        public void CopyTo(ComponentType[] io_ComponentArray, int i_ArrayIndex)
        {
            m_Components.CopyTo(io_ComponentArray, i_ArrayIndex);
        }

        public int Count
        {
            get { return m_Components.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public virtual bool Remove(ComponentType i_Component)
        {
            bool removed;

            removed = m_Components.Remove(i_Component);
            if(i_Component != null && removed)
            {
                OnComponentRemoved(new GameComponentEventArgs<ComponentType>(i_Component));
            }

            return removed;
        }

        public IEnumerator<ComponentType> GetEnumerator()
        {
            return m_Components.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)m_Components).GetEnumerator();
        }
    }
}
