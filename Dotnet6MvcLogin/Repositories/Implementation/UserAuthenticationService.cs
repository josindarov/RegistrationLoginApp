using Dotnet6MvcLogin.Models;
using Dotnet6MvcLogin.Models.Domain;
using Dotnet6MvcLogin.Models.DTO;
using Dotnet6MvcLogin.Repositories.Abstract;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Text;

namespace Dotnet6MvcLogin.Repositories.Implementation
{
    public class UserAuthenticationService: IUserAuthenticationService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        public UserAuthenticationService(UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager; 

        }

        public async Task<Status> RegisterAsync(RegistrationModel model)
        {
            var status = new Status();
            var userExists = await userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
            {
                status.StatusCode = 0;
                status.Message = "User already exist";
                return status;
            }
            
            var user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                FirstName = model.FirstName,
                LastName=model.LastName,
                EmailConfirmed=true,
                PhoneNumberConfirmed=true,
                UserName = model.Email,
                RegistrationTime = DateTime.Now,
                LoginTime = DateTime.Now,
                UserStatus = "Active"
            };

            var result = await userManager.CreateAsync(user, model.Password);
            
            if (!result.Succeeded)
            {
                var errorMessage = string.Empty;

                foreach (var error in result.Errors)
                {
                    errorMessage += error.Description + "\n";
                }

                status.StatusCode = 0;
                status.Message = errorMessage;
                return status;
            }

            if (!await roleManager.RoleExistsAsync(model.Role))
                await roleManager.CreateAsync(new IdentityRole(model.Role));
            

            if (await roleManager.RoleExistsAsync(model.Role))
            {
                await userManager.AddToRoleAsync(user, model.Role);
            }

            status.StatusCode = 1;
            status.Message = "You have registered successfully";
            return status;
        }


        public async Task<Status> LoginAsync(LoginModel model)
        {
            var status = new Status();
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                status.StatusCode = 0;
                status.Message = "Invalid username";
                return status;
            }

            if (!await userManager.CheckPasswordAsync(user, model.Password))
            {
                status.StatusCode = 0;
                status.Message = "Invalid Password";
                return status;
            }

            var signInResult = await signInManager.PasswordSignInAsync(user, model.Password, false, true);
            if (signInResult.Succeeded)
            {
                var userRoles = await userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }
                status.StatusCode = 1;
                status.Message = "Logged in successfully";
                var loginUser = new ApplicationUser()
                {
                    LoginTime = DateTime.Now
                };

            }
            else if (signInResult.IsLockedOut)
            {
                status.StatusCode = 0;
                status.Message = "User is locked out";
            }
            else
            {
                status.StatusCode = 0;
                status.Message = "Error on logging in";
            }
           
            return status;
        }

        public async Task LogoutAsync()
        {
           await signInManager.SignOutAsync();
           
        }

        public async Task<Status> ChangePasswordAsync(ChangePasswordModel model,string email)
        {
            var status = new Status();
            
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                status.Message = "User does not exist";
                status.StatusCode = 0;
                return status;
            }
            var result = await userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (result.Succeeded)
            {
                status.Message = "Password has updated successfully";
                status.StatusCode = 1;
            }
            else
            {
                status.Message = "Some error occcured";
                status.StatusCode = 0;
            }
            return status;

        }
    }
}
