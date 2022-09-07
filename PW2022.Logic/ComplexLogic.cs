namespace PW2022.Logic
{
    public class ComplexLogic
    {
        public ComplexLogic(IRestrictedApi RestrictedApi)
        {
            _restrictedApi = RestrictedApi;
        }

        private IRestrictedApi _restrictedApi;

        public Input Input { get; set; }

        public Output Output { get; set; }

        public void Execute()
        {
            string encoded = Input.Data;
            int salt = _restrictedApi.GetSalt();
            for (int i = 0; i < salt; ++i)
            {
                encoded = Base64Encode(encoded);
            }

            Output = new Output()
            {
                Encoded = encoded,
                Times = salt
            };
        }

        private string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
    }

    public interface IRestrictedApi
    {
        int GetSalt();
    }

    public class Input
    {
        public string Data { get; set; }
    }

    public class Output
    {
        public string Encoded { get; set; }
        public int Times { get; set; }
    }
}
