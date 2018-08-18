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
    public class ScreensMananger : StackMananger<GameScreen>
    {
        public ScreensMananger(Game i_Game)
            : base(i_Game)
        {
            i_Game.Components.Add(this);
        }

        public override void Initialize()
        {
            this.Game.Services.AddService(typeof(IStackMananger<GameScreen>), this);
            base.Initialize();
        }

        protected override void ActivateItem(GameScreen i_Screen)
        {
            i_Screen.Activate();
        }

        protected override void SetManager(GameScreen i_Screen)
        {
            i_Screen.ScreensManager = this;
        }

        protected override void SubscribeToCloseEvent(GameScreen i_Screen)
        {
            i_Screen.Closed += screen_Closed;
        }

        private void screen_Closed(object sender, EventArgs e)
        {
            Item_Closed(sender, e);
        }

        protected override void UnsubscribeToCloseEvent(GameComponentEventArgs<GameScreen> e)
        {
            e.GameComponent.Closed -= screen_Closed;
        }

        protected override void SetPreviousItem(GameScreen i_Screen)
        {
            i_Screen.PreviousScreen = this.ActiveItem;
        }

        protected override void DeactivateCurrentItem()
        {
            this.ActiveItem.Deactivate();
        }

        protected override void ActivateCurrentItem()
        {
            this.ActiveItem.Activate();
        }

        protected override void OnComponentRemoved(GameComponentEventArgs<GameScreen> e)
        {
            base.OnComponentRemoved(e);
            if (ItemsStack.Count == 0)
            {
                this.Game.Exit();
            }
        }
    }
}
