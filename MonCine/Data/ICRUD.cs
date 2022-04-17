using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.CompilerServices;

namespace MonCine.Data
{
    public interface ICRUD<T>
    {
        string CollectionName { get; set; }


        List<T> ReadItems();

        bool AddItem(T pObj);
        bool UpdateItem(T pObj);
        bool DeleteItem(T pObj);

    }
}
