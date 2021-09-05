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
    }
}
