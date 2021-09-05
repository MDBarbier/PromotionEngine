using promotionengine.models;
using System.Collections.Generic;

namespace promotionengine.interfaces
{
    public interface IPromotion
    {
        internal void Validate();
        internal float CheckAndApplyPromotion(Dictionary<Product, int> matchedProductsOnOrder, float totalPrice);
    }
}
