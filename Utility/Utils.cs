using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Product_Management.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Product_Management.Utility
{
    public class Utils
    {
        public IConfiguration _Configuration;
        private readonly AppSettings _appSettings; 


        //private readonly string _jwtLifeTime;
        //private readonly string _jwtIssuer;
        //private readonly string _jwtAudience;
        public Utils(IConfiguration Configuration)
        {
            _Configuration = Configuration;  

            //_jwtLifeTime = Configuration["JwtSettings:Lifetime"];
            //_jwtIssuer = Configuration["JwtSettings:Issuer"];
            //_jwtAudience = Configuration["JwtSettings:Audience"];
        }
        public void CreatePasswordHash(string Password, out byte[] PasswordHash, out byte[] PasswordSalt)
        {
            using (var Hmac = new HMACSHA512())
            {
                PasswordSalt = Hmac.Key;
                PasswordHash = Hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(Password));
            }
        }

        public bool VerifyPasswordHash(string password, byte[] PassowrdHash, byte[] PasswordSalt)
        {
            using (var hmac = new HMACSHA512(PasswordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(PassowrdHash);
            }
        }

        //public string CreateToken(Users users)
        //{
        //    List<Claim> claims = new List<Claim>
        //    {
        //        new Claim(ClaimTypes.Email, users.Email)
        //    };

        //    var Key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
        //        _Configuration.GetSection("Setting:Token").Value));

        //    var cred = new SigningCredentials(Key, SecurityAlgorithms.HmacSha512Signature);

        //    var token = new JwtSecurityToken(
        //        claims: claims,
        //        expires: DateTime.Now.AddDays(1),
        //        signingCredentials: cred);

        //    var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        //    return jwt;
        //}

        public string CreateToken(Users model)
        {
            var staffRole = SimplifyUserRole(model.Role);

            //simplify the user role
            var utcNow = DateTime.UtcNow;
            List<Claim> claims = new List<Claim>();
            claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier,model.Email),
                new Claim(ClaimTypes.Name, model.Email),
                new Claim(ClaimTypes.Role, model.Role), 

            }.ToList();


            //create a token for the user
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("09iuhgfdswe456yhnkio");


            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(1000),
                Issuer = "http://localhost/12345",
                Audience = "ProductApi.com",
                SigningCredentials = new SigningCredentials
                                (new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var tokens = tokenHandler.WriteToken(token);

            return tokens;
        }

        private string SimplifyUserRole(string staffRole)
        {
            if (staffRole.Contains("Admin"))
            {
                staffRole = Roles.Admin;
            }
            else if (staffRole.Contains("SuperAdmin"))
            {
                staffRole = Roles.SuperAdmin;
            } 

            return staffRole;
        }

    }

    public class Roles
    {
        public const string SuperAdmin = "SuperAdmin";
        public const string Admin = "Admin";

    }

    public class AppSettings
    {
        public string Secret { get; set; }
    }
}
