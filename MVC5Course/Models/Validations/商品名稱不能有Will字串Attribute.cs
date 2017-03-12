using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC5Course.Models.Validations
{
    public class 商品名稱不能有Will字串Attribute : DataTypeAttribute
    {
        public 商品名稱不能有Will字串Attribute() : base(DataType.Text)
        {
            this.ErrorMessage = "商品名稱不能有Will字串";
        }

        public override bool IsValid(object value)
        {
            string str = Convert.ToString(value);

            if (str.Contains("Will"))
            {
                return false;
            }

            return true;
        }
    }
}