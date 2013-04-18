using LifeFall.Logic.Blood;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace LifeFall.Logic
{
    /// <summary>
    /// Dostarczanie obiektów
    /// </summary>
    public interface IObjectsProvider
    {
         //Virus GetVirus();
         //RedBloodCell GetRedBloodCell();
         //CollectiblePoint GetCollectiblePoint();
         T GetObject<T>() where T: MovableObject, new();
    }
}
