namespace MiniBanking.API.Helpers
{
    public static class IbanGenerator
    {
        public static string Generate()
        {
            var random = new Random();

            var number = random.NextInt64(
                1000000000000000,
                9999999999999999);

            return $"TR{number}";
        }
    }
}