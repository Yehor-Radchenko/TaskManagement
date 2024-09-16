namespace TaskManagement.Common.Dto.Validation;

using System;
using System.ComponentModel.DataAnnotations;

public class ValidEnumValueAttribute : ValidationAttribute
{
    private readonly Type enumType;

    public ValidEnumValueAttribute(Type enumType)
    {
        this.enumType = enumType;
    }

    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null || !Enum.IsDefined(this.enumType, value))
        {
            return new ValidationResult($"Invalid value for {validationContext.DisplayName}. Please use a valid value.");
        }

        return ValidationResult.Success!;
    }
}