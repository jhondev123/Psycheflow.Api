using System.Text.RegularExpressions;

namespace Psycheflow.Api.Entities.ValueObjects
{
    public class Cpf
    {
        public string Value { get; set; }
        public Cpf(string value)
        {
            ValidateCpf(value);
            Value = value;
        }
        private void ValidateCpf(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value), "CPF não pode ser nulo ou vazio.");
            }

            value = CleanCpf(value);

            if (value.Length != 11)
            {
                throw new ArgumentException("CPF deve ter 11 dígitos.");
            }

            if (IsRepeatedDigits(value))
            {
                throw new ArgumentException("CPF inválido (todos os dígitos iguais).");
            }

            if (!IsValidCpfDigits(value))
            {
                throw new ArgumentException("CPF inválido.");
            }
        }

        private string CleanCpf(string value)
        {
            return Regex.Replace(value ?? "", @"[^0-9]", "");
        }

        private bool IsRepeatedDigits(string value)
        {
            return value.All(c => c == value[0]);
        }

        private bool IsValidCpfDigits(string value)
        {
            int[] numbers = value.Select(c => int.Parse(c.ToString())).ToArray();

            int sum = 0;
            for (int i = 0; i < 9; i++)
                sum += numbers[i] * (10 - i);

            int firstDigit = sum % 11;
            firstDigit = firstDigit < 2 ? 0 : 11 - firstDigit;

            if (numbers[9] != firstDigit)
            {
                return false;
            }

            sum = 0;
            for (int i = 0; i < 10; i++)
            {
                sum += numbers[i] * (11 - i);
            }

            int secondDigit = sum % 11;
            secondDigit = secondDigit < 2 ? 0 : 11 - secondDigit;

            return numbers[10] == secondDigit;
        }
        public override string ToString()
        {
            return Value;
        }
    }
}
