using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace promotionengine.exceptions
{
    [Serializable]
    public class ProductValidationException :Exception
    {
        public ProductValidationException() { }

        public ProductValidationException(string error)
            : base(String.Format("Invalid product values: {0}", error))
        {

        }
    }
}
