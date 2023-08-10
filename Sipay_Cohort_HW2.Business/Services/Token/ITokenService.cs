using Sipay_Cohort_HW2.DataAccess.Response;
using Sipay_Cohort_HW2.Dtos.Token;

namespace Sipay_Cohort_HW2.Business.Services.Token
{
    public interface ITokenService
    {
        Task<ApiResponse<TokenResponse>> Login(TokenRequest request);
    }
}
