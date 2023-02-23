﻿using System.ComponentModel.DataAnnotations;

namespace Utilities.ValidataionAttributes;

public class DateOfBirth : ValidationAttribute
{
    public int MinAge { get; set; }
    public int MaxAge { get; set; }

    public override bool IsValid(object value)
    {
        if (value == null)
            return true;

        var val = (DateTime)value;

        if (val.AddYears(MinAge) > DateTime.Now)
            return false;

        return (val.AddYears(MaxAge) > DateTime.Now);
    }
}