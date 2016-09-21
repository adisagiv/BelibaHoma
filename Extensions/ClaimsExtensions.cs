using System.Security.Claims;

namespace Extensions
{
    public static class ClaimsExtensions
    {
        public static string getName(this ClaimsIdentity ci)
        {
            foreach (Claim c in ci.Claims)
            {
                if (c.Type == ClaimTypes.Name)
                {
                    return c.Value;
                }
            }
            return string.Empty;
        }         
    }
}