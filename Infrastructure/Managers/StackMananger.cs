using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using C16_Ex03_Yakir_201049475_Omer_300471430.Interfaces;
using C16_Ex03_Yakir_201049475_Omer_300471430.CustomEventArgs;

namespace C16_Ex03_Yakir_201049475_Omer_300471430.Managers
{
    public abstract class StackMananger<ItemsType> : CompositeDrawableComponent<ItemsType>, IStackMananger<ItemsType>
        where ItemsType : DrawableGameComponent
    {
        private Stack<ItemsType> m_ItemsStack;

        protected Stack<ItemsType> ItemsStack
        {
            get { return m_ItemsStack; }
            set { m_ItemsStack = value; }
        }

        public ItemsType ActiveItem
        {
            get { return m_ItemsStack.Count > 0 ? m_ItemsStack.Peek() : null; }
        }

        public StackMananger(Game i_Game)
            : base(i_Game)
        {
            m_ItemsStack = new Stack<ItemsType>();
        }

        protected abstract void ActivateItem(ItemsType i_Item);

        protected abstract void SetManager(ItemsType i_Item);

        protected abstract void SubscribeToCloseEvent(ItemsType i_Item);

        protected abstract void UnsubscribeToCloseEvent(GameComponentEventArgs<ItemsType> e);

        protected abstract void SetPreviousItem(ItemsType i_Item);

        protected abstract void DeactivateCurrentItem();

        protected abstract void ActivateCurrentItem();

        public virtual void SetCurrentItem(ItemsType i_Item)
        {
            Push(i_Item);
            ActivateItem(i_Item);
        }

        public virtual void Push(ItemsType i_Item)
        {
            SetManager(i_Item);
            if(!this.Contains(i_Item))
            {
                this.Add(i_Item);
                SubscribeToCloseEvent(i_Item);
            }

            if(this.ActiveItem != i_Item)
            {
                if(this.ActiveItem != null)
                {
                    SetPreviousItem(i_Item);
                    DeactivateCurrentItem();
                }

                m_ItemsStack.Push(i_Item);
            }

            i_Item.DrawOrder = m_ItemsStack.Count;
        }

        protected virtual void Item_Closed(object sender, EventArgs e)
        {
            ItemsType asItem;

            asItem = sender as ItemsType;
            pop(asItem);
            Remove(asItem);
        }

        private void pop(ItemsType i_Item)
        {
            m_ItemsStack.Pop();
            if(m_ItemsStack.Count > 0)
            {
                ActivateCurrentItem();
            }
        }

        private new bool Remove(ItemsType i_Item)
        {
            return base.Remove(i_Item);
        }

        private new void Add(ItemsType i_Component)
        {
            base.Add(i_Component);
        }

        protected override void OnComponentRemoved(GameComponentEventArgs<ItemsType> e)
        {
            base.OnComponentRemoved(e);
            UnsubscribeToCloseEvent(e);
        }
    }
}
