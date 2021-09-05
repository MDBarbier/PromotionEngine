using promotionengine.exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace promotionengine.models
{
    public class Product
    {
        public char SkuName { get; set; }
        public float UnitPrice { get; set; }

        internal void Validate()
        {
            StringBuilder stringBuilder = new StringBuilder();
            bool validationFaled = false;

            if (char.IsWhiteSpace(SkuName))            
            {
                validationFaled = true;
                stringBuilder.Append(" *SkuName cannot be empty* ");
            }

            if (UnitPrice == 0.00f || UnitPrice < 0.00f)
            {
                validationFaled = true;
                stringBuilder.Append(" *Invalid unit price* ");                
            }

            if (validationFaled)
            {
                throw new ProductValidationException(stringBuilder.ToString()); 
            }
        }
    }
}
