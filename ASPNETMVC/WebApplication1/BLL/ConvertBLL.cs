using Entities;
using System.Text;

namespace BLL
{
    public class ConvertBLL
    {
        private static string encodedStrings = "";

        private static bool isProcessing = false;
        private static CancellationTokenSource _cancellationTokenSource;
        private readonly Random _random = new Random();

        private static readonly Random random = new Random();

        public string GetEncodedStrings()
        {
            return encodedStrings;

        }

        public async Task<ResponseData> Converter(string text)
        {

            ResponseData responseData = new ResponseData();


            try
            {
                if (isProcessing == false)
                {
                    if (string.IsNullOrWhiteSpace(text.Trim()))
                    {
                        responseData.isSuccess = false;
                        responseData.Message = "Input Text cannot be Empty!";
                    }
                    else {
                        encodedStrings = "";
                        isProcessing = true;
                        var encodedText = Convert.ToBase64String(Encoding.UTF8.GetBytes(text));
                        var characters = encodedText.ToCharArray();

                        var random = new Random();

                        _cancellationTokenSource = new CancellationTokenSource();

                        foreach (var character in characters)
                        {
                            if (_cancellationTokenSource != null)
                            {
                                if (_cancellationTokenSource.IsCancellationRequested)
                                {
                                    _cancellationTokenSource = null;
                                    encodedStrings = "";
                                    isProcessing = false;
                                    responseData.isSuccess = true;
                                    responseData.Message = "Successfully Canceled the Conversion!";
                                    return responseData;
                                }
                            }

                            encodedStrings = encodedStrings + character;
                            await Task.Delay(random.Next(1000, 5000));
                        }

                        responseData.isSuccess = true;
                        responseData.Message = "Successfully Converted the String!";
                    }
                }
                else
                {
                    responseData.isSuccess = false;
                    responseData.Message = "Conversion is still in progress!";
                }
            }
            catch (Exception ex)
            {
                responseData.isSuccess = false;
                responseData.Message = ex.Message;
            }


            isProcessing = false;

            return responseData;
        }

        public bool CancelEncode()
        {
            if (_cancellationTokenSource != null && !_cancellationTokenSource.IsCancellationRequested)
            {
                _cancellationTokenSource.Cancel();
            }

            return true;
        }
    }
}