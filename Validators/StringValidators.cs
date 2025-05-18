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



            return Tuple.Create(true, string.Empty);
        }
    }
}

