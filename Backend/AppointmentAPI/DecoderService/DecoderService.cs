using LogicLayer.Models;
using System.IdentityModel.Tokens.Jwt;

namespace AppointmentAPI.DecoderService
{
    public class DecoderService : IDecoderService
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        public DecoderService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }


        private void ValidateTime(DateTime dateOfExpiration)
        {

            DateTime currentTime = DateTime.UtcNow.ToLocalTime();
            if (DateTime.Compare(dateOfExpiration, currentTime) < 0)
            {
                throw new UnauthorizedAccessException("Expired token");
            }
        }

        public void Authorize()
        {
            try
            {
                GetCredentials();
            }
            catch (UnauthorizedAccessException uae)
            {
                throw uae;
            }
        }
        public void Authorize(string role)
        {
            try
            {
                DecryptedUser decryptedUser = GetCredentials();
                string userRole = decryptedUser.Role.ToString();
                if (userRole != role)
                {
                    throw new UnauthorizedAccessException("User with this role is not authorized");
                }
            }
            catch (UnauthorizedAccessException uae)
            {
                throw uae;
            }
        }
        public void Authorize(List<string> roles)
        {
            try
            {
                DecryptedUser decryptedUser = GetCredentials();
                string userRole = decryptedUser.Role.ToString();
                bool authorized = false;
                foreach (string role in roles)
                {
                    if (userRole == role)
                    {
                        authorized = true;
                    }
                }

                if (authorized == false)
                {
                    throw new UnauthorizedAccessException("User with this role is not authorized");
                }
            }
            catch (UnauthorizedAccessException uae)
            {
                throw uae;
            }
        }

        public DecryptedUser GetCredentials()
        {
            try
            {
                var result = string.Empty;
                DecryptedUser user = new DecryptedUser();

                if (this.httpContextAccessor.HttpContext != null)
                {
                    result = this.httpContextAccessor.HttpContext.Request.Headers["Authorization"];
                    if (string.IsNullOrEmpty(result))
                    {
                        throw new UnauthorizedAccessException("No token provided");
                    }
                    string bearerToken = result.Substring(7);
                    var handler = new JwtSecurityTokenHandler();
                    var jsonToken = handler.ReadToken(bearerToken);
                    var tokenS = jsonToken as JwtSecurityToken;

                    DateTime dateOfExpiration = tokenS.ValidTo.ToLocalTime();
                    ValidateTime(dateOfExpiration);

                    var id = tokenS.Claims.First(claim => claim.Type == "id").Value;
                    user.Id = int.Parse(id);
                    var email = tokenS.Claims.First(claim => claim.Type == "email").Value;
                    user.Email = email;
                    var role = tokenS.Claims.First(claim => claim.Type == "role").Value;
                    switch (role)
                    {
                        case "Admin": user.Role = Role.Admin; break;
                        case "Recruiter": user.Role = Role.Recruiter; break;
                        case "Candidate": user.Role = Role.Candidate; break;
                    }
                }
                else
                {
                    throw new UnauthorizedAccessException("No token provided");
                }
                return user;
            }
            catch (UnauthorizedAccessException uae)
            {
                throw uae;
            }
        }
    }
}
