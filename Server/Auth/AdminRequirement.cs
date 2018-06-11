using Microsoft.AspNetCore.Authorization;

namespace mvc_auth.Auth
{
    public class AdminRequirement: IAuthorizationRequirement
    {
        public AdminRequirement() {}
    }
}