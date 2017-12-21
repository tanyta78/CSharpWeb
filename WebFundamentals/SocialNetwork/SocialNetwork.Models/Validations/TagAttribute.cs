namespace SocialNetwork.Models.Validations
{
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public class TagAttribute:ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var tag = value as string;
            if (tag==null)
            {
                return true;
            }

            return tag.StartsWith("#") && tag.All(s => !char.IsWhiteSpace(s)) && tag.Length <= 20;
        }
    }
}
