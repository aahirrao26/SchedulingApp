using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedicalAppointmentScheduler.Core.Data.CustomAttribute
{
    public class ValidateDateRangeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            try
            {
                if (value == null || !(value is DateTime))
                {
                    return ValidationResult.Success;
                }

                DateTime dt = (DateTime)value;
                if (dt.Date >= DateTime.Now.Date)
                {
                    return ValidationResult.Success;
                }

                return new ValidationResult(ErrorMessage ?? "You cannot select a past date");
            }
            catch (Exception)
            {
                return new ValidationResult(ErrorMessage ?? "Invalid date");
            }
        }

    }
}