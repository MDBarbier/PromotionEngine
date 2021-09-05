using promotionengine.exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace promotionengine.models
{
    public class Promotion
    {
        public bool SingleSku { get; set; }
        public bool CombinedSku { get; set; }
        public char[] ApplicableSkus { get; set; }
        public int NumUnitsRequired { get; set; }
        public float FixedPrice { get; set; }
        public float PercentageDiscount { get; set; }

        internal void Validate()
        {
            StringBuilder stringBuilder = new StringBuilder();
            bool validationFaled = false;

            if (SingleSku == false && CombinedSku == false)
            {
                validationFaled = true;
                stringBuilder.Append(" *Either SingleSku or Combined Sku must be true* ");
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

        internal float CheckAndApplyPromotion(Dictionary<Product, int> matchedProductsOnOrder, float totalPrice)
        {
            if (SingleSku)
            {
                var applicableSku = ApplicableSkus[0];

                var matchedSkuProductsOnOrder = matchedProductsOnOrder.Where(a => a.Key.SkuName == applicableSku).ToList();

                int totalUnitsOfMatchedSku = 0;

                foreach (var orderProduct in matchedSkuProductsOnOrder)
                {
                    totalUnitsOfMatchedSku += orderProduct.Value;
                }

                float numTimesPromotionAchieved = (totalUnitsOfMatchedSku / NumUnitsRequired);
                float normalPrice = (numTimesPromotionAchieved * NumUnitsRequired) * matchedSkuProductsOnOrder.First().Key.UnitPrice;
                float discountedPrice = numTimesPromotionAchieved * FixedPrice;

                totalPrice -= normalPrice; //Subtract full cost
                totalPrice += discountedPrice; //Add discounted price
                return totalPrice;
            }
            else if (CombinedSku)
            {                
                Dictionary<char, bool> skusPresent = new Dictionary<char, bool>();

                foreach (var applicableSku in ApplicableSkus)
                {
                    List<KeyValuePair<Product, int>> matchedSkuProductsOnOrder = GetNumberOfMatchedSkuItems(matchedProductsOnOrder, applicableSku);

                    if (matchedSkuProductsOnOrder.Count > 0)
                    {
                        skusPresent.Add(applicableSku, true);
                    }
                }

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
            }
            else
            {
                throw new PromotionValidationException("Either SingleSku or Combined Sku must be true");
            }

            return totalPrice;
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
