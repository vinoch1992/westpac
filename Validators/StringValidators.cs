using System.Text.RegularExpressions;

namespace westpac.Validators
{
    public static class StringValidators
    {
        public static Tuple<bool, string> IsValidName(string name)
        {
            if (name.Length < 3)
                return Tuple.Create(false, "Provided name having less than 3 characters. It should be more than 3 characters.");

            if (name.Length > 40)
                return Tuple.Create(false, "Provided name having greater than 40 characters. It should be less than 40 characters");

            // validating name starts with alphanumeric.
            if (!char.IsLetterOrDigit(name[0]))
            {
                return Tuple.Create(false, "Name must start with either a Letter or number");
            }

            // validating name has any special character
            // except for the expected ones.
            if (!Regex.IsMatch(name, @"^[a-zA-Z0-9.\-_\s]*$"))
            {
                return Tuple.Create(false, "Name can only contain dots (.), dashes (-), underscores (_) and spaces");
            }

            return Tuple.Create(true, string.Empty);
        }

        public static Tuple<bool, string> IsValidEmail(string email)
        {
            // validating the given email address.
            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                return Tuple.Create(false, "Email address format is invalid");
            }

            return Tuple.Create(true, string.Empty);
        }

        public static Tuple<bool, string> IsValidPassword(string password)
        {
            // validating the given password.
            if (password.Length < 8)
            {
                return Tuple.Create(false, "Password must have atleast 8 characters");
            }

            return Tuple.Create(true, string.Empty);
        }
    }
}

