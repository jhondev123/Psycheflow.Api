using System.Text.RegularExpressions;

namespace Psycheflow.Api.Entities.ValueObjects
{
    public class Phone
    {
        public string Value { get; set; }
        public Phone(string value)
        {
            ValidatePhone(value);
            Value = value;
        }
        private void ValidatePhone(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value), "Telefone não pode ser nulo ou vazio.");
            }

            var phoneRegex = @"^\(?\d{2}\)?\s?\d{4,5}-?\d{4}$";

            if (!Regex.IsMatch(value, phoneRegex))
            {
                throw new ArgumentException("Telefone em formato inválido.");
            }
        }
    }
}
