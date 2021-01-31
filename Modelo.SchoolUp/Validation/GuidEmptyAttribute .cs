using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Modelo.SchoolUp.Validation
{
    public class IsGuidEmptyAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            Guid valorCampo;

            if (!Guid.TryParse(value.ToString(), out valorCampo))
            {
                return false;
            }
            else
            {
                if (valorCampo == Guid.Empty)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
