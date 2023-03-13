using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using BLL;
using Entities;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConvertController : ControllerBase
    {
        private ConvertBLL _convertBLL = null;


        [HttpGet]
        [Route("GetEncodedStrings")]
        public string GetEncodedStrings()
        {
            _convertBLL = new ConvertBLL();
            string encodedString = _convertBLL.GetEncodedStrings();

            return encodedString;

        }

        [HttpPost]
        [Route("Converter/")]
        public async Task<ResponseData> Converter([FromBody] string text)
        {
            ResponseData responseData = new ResponseData();

            try
            {
                _convertBLL = new ConvertBLL();
                responseData = await _convertBLL.Converter(text);

            }catch (Exception ex)
            {
                responseData.isSuccess = false;
                responseData.Message = ex.Message;
            }

            return responseData;
        }


        [HttpGet]
        [Route("CancelEncode")]
        public IActionResult CancelEncode()
        {
            _convertBLL = new ConvertBLL();
            bool isTrue = _convertBLL.CancelEncode();

            return Ok();
        }


    }
}
