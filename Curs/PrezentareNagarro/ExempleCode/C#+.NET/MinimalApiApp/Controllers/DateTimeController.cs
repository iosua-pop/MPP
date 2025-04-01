using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;

namespace MinimalApiApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DateTimeController : ControllerBase
    {
        // Endpoint to validate a date-time string
        [HttpGet("validate")]
        public IActionResult ValidateDateTime(string dateTime, string format)
        {
            if (DateTime.TryParseExact(dateTime, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
            {
                return Ok(new { isValid = true, parsedDate });
            }
            else
            {
                return BadRequest(new { isValid = false, message = "Invalid date-time format." });
            }
        }

        // Endpoint to get the current date-time in a specified format
        [HttpGet("current")]
        public IActionResult GetCurrentDateTime(string format)
        {
            try
            {
                string formattedDateTime = DateTime.Now.ToString(format, CultureInfo.InvariantCulture);
                return Ok(new { currentDateTime = formattedDateTime });
            }
            catch (FormatException)
            {
                return BadRequest(new { message = "Invalid date-time format." });
            }
        }

        // Endpoint to convert a date-time string from one format to another
        [HttpGet("convert")]
        public IActionResult ConvertDateTime(string dateTime, string fromFormat, string toFormat)
        {
            if (DateTime.TryParseExact(dateTime, fromFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
            {
                try
                {
                    string convertedDateTime = parsedDate.ToString(toFormat, CultureInfo.InvariantCulture);
                    return Ok(new { originalDateTime = dateTime, convertedDateTime });
                }
                catch (FormatException)
                {
                    return BadRequest(new { message = "Invalid target format." });
                }
            }
            else
            {
                return BadRequest(new { message = "Invalid date-time format." });
            }
        }
    }
}
