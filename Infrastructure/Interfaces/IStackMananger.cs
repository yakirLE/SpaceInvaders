using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C16_Ex03_Yakir_201049475_Omer_300471430.Interfaces
{
    public interface IStackMananger<ItemsType>
    {
        ItemsType ActiveItem { get; }

        void Push(ItemsType i_Item);

        void SetCurrentItem(ItemsType i_NewItem);

        bool Remove(ItemsType i_Item);

        void Add(ItemsType i_Item);
    }
}
