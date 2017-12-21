namespace SocialNetwork.Models.Validations
{
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public class PasswordAttribute:ValidationAttribute
    {
        private readonly char[] RequeredSymbols = new[] {'!','@','#', '$', '%', '^',  '&', '*', '(', ')', '_', '+', '<', '>', '?'};

        public PasswordAttribute()
        {
            this.ErrorMessage = "Password is not valid.";
        }
        public override bool IsValid(object value)
        {
            var password = value as string;
            if (password==null)
            {
                return true;
            }

            var passwordValid = password.All(s =>
                char.IsLower(s)
               || char.IsUpper(s)
               || char.IsDigit(s)
               || this.RequeredSymbols.Contains(s));

            return passwordValid;
        }

       
    }
}
