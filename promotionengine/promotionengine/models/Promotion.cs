using promotionengine.exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        internal bool CheckValidity(Dictionary<Product, int> matchedProductsOnOrder)
        {
            bool validity;

            if (SingleSku)
            {
                validity = CheckSingleSkuValidity(matchedProductsOnOrder);
            }
            else if (CombinedSku)
            {
                validity = CheckMultiSkuValidity(matchedProductsOnOrder);
            }
            else
            {
                throw new PromotionValidationException("Either SingleSku or Combined Sku must be true");
            }

            return validity;
        }

        private bool CheckMultiSkuValidity(Dictionary<Product, int> matchedProductsOnOrder)
        {
            foreach (var applicableSku in ApplicableSkus)
            {
                var matchedSkuProductsOnOrder = matchedProductsOnOrder.Where(a => a.Key.SkuName == applicableSku).ToList();

                if (matchedSkuProductsOnOrder.Count == 0)
                {
                    return false;
                }
            }

            return true;
        }

        private bool CheckSingleSkuValidity(Dictionary<Product, int> matchedProductsOnOrder)
        {
            var applicableSku = ApplicableSkus[0];

            var matchedSkuProductsOnOrder = matchedProductsOnOrder.Where(a => a.Key.SkuName == applicableSku).ToList();

            int totalUnitsOfMatchedSku = 0;

            foreach (var orderProduct in matchedSkuProductsOnOrder)
            {
                totalUnitsOfMatchedSku += orderProduct.Value;
            }

            return totalUnitsOfMatchedSku >= NumUnitsRequired ? true : false;
        }
    }
}
