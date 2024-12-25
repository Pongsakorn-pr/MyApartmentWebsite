using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Text.Json.Nodes;
using webapi.Model;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post([FromBody] accountLogin account)
        {
            try
            {
                if (account.username == "admin" && account.password == "admin") // Replace with secure password verification
                {
                    //testttt
                    return Ok(new { message = "Login successful" });
                }
                else
                {
                    return Ok(new { message = "Invalid credentials." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
