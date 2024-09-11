namespace TaskManagement.Common.Dto.Validation;

using System.ComponentModel.DataAnnotations;

internal class FutureDateAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is DateTime dueDate)
        {
            if (dueDate >= DateTime.Now)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        return false;
    }
}
