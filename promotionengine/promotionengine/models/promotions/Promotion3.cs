using promotionengine.exceptions;
using promotionengine.interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace promotionengine.models
{
    public class Promotion3 : IPromotion
    {
        public char[] ApplicableSkus { get; } = new char[] { 'C', 'D' };
        public int NumUnitsRequired { get; } = 1;
        public float FixedPrice { get; } = 30.00f;

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
            return ApplyDiscountForMultiSkuPromotion(matchedProductsOnOrder, totalPrice);
        }

        private float ApplyDiscountForMultiSkuPromotion(Dictionary<Product, int> matchedProductsOnOrder, float totalPrice)
        {
            Dictionary<char, bool> skusPresent = new Dictionary<char, bool>();
            CheckPresentSkus(matchedProductsOnOrder, skusPresent);

            if (CheckValidityOfMultiSku(skusPresent))
            {
                int numTimesMatches = 0;
                //how many times do the matched skus appear
                foreach (var sku in ApplicableSkus)
                {
                    List<KeyValuePair<Product, int>> matchedSkuProductsOnOrder = GetNumberOfMatchedSkuItems(matchedProductsOnOrder, sku);

                    int totalNumMatchedSkus = 0;
                    foreach (var order in matchedSkuProductsOnOrder)
                    {
                        totalNumMatchedSkus += order.Value;
                    }

                    if (numTimesMatches == 0 || totalNumMatchedSkus <= numTimesMatches)
                    {
                        numTimesMatches = totalNumMatchedSkus;
                    }
                }

                for (int i = 0; i < numTimesMatches; i++)
                {
                    totalPrice = ApplyDiscountForMultiSku(matchedProductsOnOrder, totalPrice);
                }
            }

            return totalPrice;
        }

        private void CheckPresentSkus(Dictionary<Product, int> matchedProductsOnOrder, Dictionary<char, bool> skusPresent)
        {
            foreach (var applicableSku in ApplicableSkus)
            {
                List<KeyValuePair<Product, int>> matchedSkuProductsOnOrder = GetNumberOfMatchedSkuItems(matchedProductsOnOrder, applicableSku);

                if (matchedSkuProductsOnOrder.Count > 0)
                {
                    skusPresent.Add(applicableSku, true);
                }
            }
        }
       
        private float ApplyDiscountForMultiSku(Dictionary<Product, int> matchedProductsOnOrder, float totalPrice)
        {
            var ordinaryPriceOfAllItems = 0.00f;

            foreach (var skuItem in ApplicableSkus)
            {
                var match = matchedProductsOnOrder.Where(order => order.Key.SkuName == skuItem).FirstOrDefault();

                ordinaryPriceOfAllItems += match.Key.UnitPrice;
            }

            var discount = ordinaryPriceOfAllItems - FixedPrice;
            totalPrice -= discount;
            return totalPrice;
        }

        private bool CheckValidityOfMultiSku(Dictionary<char, bool> skusPresent)
        {
            foreach (var sku in ApplicableSkus)
            {
                if (!skusPresent.ContainsKey(sku))
                {
                    return false;
                }
            }

            return true;
        }

        private static List<KeyValuePair<Product, int>> GetNumberOfMatchedSkuItems(Dictionary<Product, int> matchedProductsOnOrder, char applicableSku)
        {
            return matchedProductsOnOrder.Where(a => a.Key.SkuName == applicableSku && a.Value > 0).ToList();
        }

    }
}
