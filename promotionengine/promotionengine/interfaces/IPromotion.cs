using promotionengine.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace promotionengine.interfaces
{
    public interface IPromotion
    {
        internal void Validate();
        internal float CheckAndApplyPromotion(Dictionary<Product, int> matchedProductsOnOrder, float totalPrice);
    }
}
