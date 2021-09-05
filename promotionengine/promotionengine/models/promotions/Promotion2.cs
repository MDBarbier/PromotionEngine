using promotionengine.exceptions;
using promotionengine.interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace promotionengine.models
{
    public class Promotion2: IPromotion
    {
        public char[] ApplicableSkus { get; set; } = new char[] { 'B' };
        public int NumUnitsRequired { get; set; } = 2;
        public float FixedPrice { get; set; } = 45.00f;

        void IPromotion.Validate()
        {
            StringBuilder stringBuilder = new StringBuilder();
            bool validationFaled = false;

            if (ApplicableSkus.Length == 0)
            {
                validationFaled = true;
                stringBuilder.Append(" *At least one SKU must be specified* ");
            }

            if (NumUnitsRequired <= 0)
            {
                validationFaled = true;
                stringBuilder.Append(" *NumUnitsRequired must be greater than zero* ");
            }

            if (validationFaled)
            {
                throw new PromotionValidationException(stringBuilder.ToString());
            }
        }

        float IPromotion.CheckAndApplyPromotion(Dictionary<Product, int> matchedProductsOnOrder, float totalPrice)
        {            
            return ApplyDiscountForSingleSkuPromotion(matchedProductsOnOrder, ref totalPrice);            
        }

        private float ApplyDiscountForSingleSkuPromotion(Dictionary<Product, int> matchedProductsOnOrder, ref float totalPrice)
        {
            var matchedSkuProductsOnOrder = matchedProductsOnOrder.Where(a => a.Key.SkuName == ApplicableSkus[0]).ToList();
            int totalUnitsOfMatchedSku = 0;

            foreach (var orderProduct in matchedSkuProductsOnOrder)
            {
                totalUnitsOfMatchedSku += orderProduct.Value;
            }

            float numTimesPromotionAchieved = (totalUnitsOfMatchedSku / NumUnitsRequired);
            float normalPrice = (numTimesPromotionAchieved * NumUnitsRequired) * matchedSkuProductsOnOrder.First().Key.UnitPrice;
            float discountedPrice = numTimesPromotionAchieved * FixedPrice;

            totalPrice -= normalPrice;
            return totalPrice += discountedPrice;
        }
    }
}
