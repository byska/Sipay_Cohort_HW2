using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sipay_Cohort_HW2.Business.Services.Token;
using Sipay_Cohort_HW2.DataAccess.Response;
using Sipay_Cohort_HW2.Dtos.Token;

namespace Sipay_Cohort_HW2.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ITokenService _service;
        public TokenController(ITokenService service)
        {
            _service = service;
        }
        [HttpPost("Login")]
        public async Task<ApiResponse<TokenResponse>> Post([FromBody] TokenRequest request)
        {
            var response = await _service.Login(request);
            return response;
        }
    }
}
