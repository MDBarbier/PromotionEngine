using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace promotionengine.exceptions
{
    [Serializable]
    public class PromotionValidationException : Exception
    {
        public PromotionValidationException() { }

        public PromotionValidationException(string error)
            : base(String.Format("Invalid product values: {0}", error))
        {

        }
    }
}
