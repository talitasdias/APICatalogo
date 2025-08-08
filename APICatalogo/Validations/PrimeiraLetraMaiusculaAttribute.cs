using System.ComponentModel.DataAnnotations;

namespace APICatalogo.Validations;
public class PrimeiraLetraMaiusculaAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var nome = value?.ToString();
        if (nome == null || string.IsNullOrEmpty(nome))
            return ValidationResult.Success;

        var primeiraLetra = nome[0].ToString();
        if (primeiraLetra != primeiraLetra.ToUpper())
            return new ValidationResult("A primeira letra do produto deve ser maiúscula.");
        
        return ValidationResult.Success;
    }
}