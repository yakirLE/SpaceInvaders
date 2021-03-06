using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace C16_Ex03_Yakir_201049475_Omer_300471430
{
    public sealed class DrawableComparer<TDrawable> : IComparer<TDrawable>
        where TDrawable : class, IDrawable
    {
        private const int k_XBigger = 1;
        private const int k_Equal = 0;
        private const int k_YBigger = -1;
        public static readonly DrawableComparer<TDrawable> Default;

        static DrawableComparer()
        {
            Default = new DrawableComparer<TDrawable>();
        }

        private DrawableComparer()
        {
        }

        public int Compare(TDrawable i_X, TDrawable i_Y)
        {
            int returnedCompareResult;

            returnedCompareResult = k_YBigger;
            if (i_X == null && i_Y == null)
            {
                returnedCompareResult = k_Equal;
            }
            else if (i_X != null)
            {
                if (i_Y == null)
                {
                    returnedCompareResult = k_XBigger;
                }
                else if (i_X.Equals(i_Y))
                {
                    return k_Equal;
                }
                else if (i_X.DrawOrder > i_Y.DrawOrder)
                {
                    return k_XBigger;
                }
            }

            return returnedCompareResult;
        }
    }
}
