using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Product_Management.Context;
using Product_Management.Models;
using Product_Management.Models.UserView;
using Product_Management.Services.IRepository;
using Product_Management.Utility;
using Product_Management.ViewModel;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;

namespace Product_Management.Controllers
{
   // [Route("api/[controller]")]
    [ApiController]

    public class AccountController : ControllerBase
    {
        //public static Users users = new Users();
        private readonly SignInManager<Users> _signInManager;
        public readonly ApplicationDbContext _context;
        public readonly IAccountRepository _accountRepository;
        public IConfiguration _Configuration; 

        public AccountController(ApplicationDbContext context, SignInManager<Users> signInManager, IAccountRepository accountRepository)
        {
            _context = context;
            _signInManager = signInManager;
            _accountRepository = accountRepository;
        }
        [Route("api/account/RegisterSuperAdmin")]
        [HttpPost]
        public async Task<ActionResult> CreateSuperAdmin(Register request)
        {
            Users users = new Users();
            Utils utils = new Utils(_Configuration);
            try
            {
                utils.CreatePasswordHash(request.Password, out byte[] PasswordHash, out byte[] PasswordSalt);
                users.Email = request.Email;
                users.Role = "SuperAdmin";
                users.PasswordSalt = PasswordSalt;
                users.PasswordHash = PasswordHash;

                _accountRepository.Add(users);
                await _context.SaveChangesAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [Route("api/account/RegisterAdmin")]
        [HttpPost]
        public async Task<ActionResult> CreateAdmin(Register request)
        {
            Users users = new Users();
            Utils utils = new Utils(_Configuration);
            try
            {
                utils.CreatePasswordHash(request.Password, out byte[] PasswordHash, out byte[] PasswordSalt);
                users.Email = request.Email;
                users.Role = "Admin";
                users.PasswordSalt = PasswordSalt;
                users.PasswordHash = PasswordHash;

                _accountRepository.Add(users);
                await _context.SaveChangesAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }




        [Route("api/account/Login")]
        [HttpPost]
        public async Task<ActionResult> Login(Login request)
        {
            var users = _accountRepository.GetFirstOrDefault(x => x.Email == request.Email);

            Utils utils = new Utils(_Configuration);
            if (users.Email != request.Email)
            {
                return BadRequest("User not Found");
            };
            if (!utils.VerifyPasswordHash(request.Password, users.PasswordHash, users.PasswordSalt))
            {
                return BadRequest("Wrong password");
            }
            string token = utils.CreateToken(users);
            return Ok(token);

        }

        [Route("api/account/Logout")]
        [HttpPost]
        public async Task<ActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return NoContent();
        }

        [Route("api/account/DeleteAdmin")]
        [HttpPost]
        public ActionResult DeleteAdmins(int id)
        {
             _accountRepository.Delete(id);
            return Ok();
        }

    }
}
