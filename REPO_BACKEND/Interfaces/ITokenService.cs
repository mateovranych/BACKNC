using backnc.Common.Response;
using backnc.Data.POCOEntities;
using backnc.Service;

namespace backnc.Interfaces
{
	public interface ITokenService 
	{
		public string GenerateToken(User user);
		Task<BaseResponse> ValidateToken(string token);

	}
}
