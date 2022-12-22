using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using VLUTUTORS.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VLUTUTORS.Security
{
    public class UserManager
    {
        private CP25Team01Context db = new CP25Team01Context();

        public async void SignIn(HttpContext httpContext, string email, string password, bool loginAdminAccount = false)
        {
            if (!loginAdminAccount)
            {
                Taikhoannguoidung user = db.Taikhoannguoidungs.Where(acc => acc.Email.Equals(email.Trim())).FirstOrDefault();
                if (user != null && user.Id > 0)
                {
                    if (user.MatKhau.Equals(password.Trim()))
                    {
                        ClaimsIdentity identity = new ClaimsIdentity(await this.GetUserClaimsAsync(user), CookieAuthenticationDefaults.AuthenticationScheme);
                        ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                        await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                        return;
                    }
                }
            }

            Taikhoanadmin checkAccount = db.Taikhoanadmins.Where(acc => acc.TaiKhoan.Equals(email.Trim())).FirstOrDefault();
            if (checkAccount != null && checkAccount.Id > 0)
            {
                if (checkAccount.MatKhau.Equals(password.Trim()))
                {
                    ClaimsIdentity identity = new ClaimsIdentity(await this.GetUserClaimsAsync(checkAccount), CookieAuthenticationDefaults.AuthenticationScheme);
                    ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                    await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    return;
                }
            }            
        }

        public async void SignOut(HttpContext httpContext)
        {
            await httpContext.SignOutAsync();
        }

        private async Task<IEnumerable<Claim>> GetUserClaimsAsync(Taikhoannguoidung user)
        {
            List<Claim> claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, user.HoTen));
            claims.Add(new Claim(ClaimTypes.Email, user.Email));
            return claims;
        }

        private async Task<IEnumerable<Claim>> GetUserClaimsAsync(Taikhoanadmin user)
        {
            List<Claim> claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, user.TaiKhoan));
            claims.AddRange(await this.GetUserRoleClaims(user));
            return claims;
        }

        private async Task<IEnumerable<Claim>> GetUserRoleClaims(Taikhoanadmin user)
        {
            List<Claim> claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            var quyen = await db.Quyens.FindAsync(user.IdQuyen);
            if(quyen != null )
                claims.Add(new Claim(ClaimTypes.Role, quyen.TenQuyen.ToString()));
            return claims;
        }
    }
}
