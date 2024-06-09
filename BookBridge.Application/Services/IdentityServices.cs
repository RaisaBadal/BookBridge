using BookBridge.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using BookBridge.Application.Interfaces;
using BookBridge.Application.Models.Request;
using BookBridge.Application.StaticFiles;
using BookBridge.Persistance.SMTPService;
using Microsoft.EntityFrameworkCore;


namespace BookBridge.Application.Services
{
    public sealed class IdentityServices(
        UserManager<User> userManager,
        SignInManager<User> signIn,
        RoleManager<IdentityRole> roleManager,
        IMapper mapper,
        SmtpService smtpService):IIdentityService
    {

        #region Registration

        public async Task<IdentityResult> Registration(UserModel userModel)
        {

            ArgumentNullException.ThrowIfNull(userModel, nameof(userModel));
            if (userManager == null)
            {
                throw new InvalidOperationException("UserManager is not initialized.");
            }
            if (mapper == null)
            {
                throw new InvalidOperationException("Mapper is not initialized.");
            }
            if (smtpService == null)
            {
                throw new InvalidOperationException("SmtpService is not initialized.");
            }

            var user = await userManager.FindByEmailAsync(userModel.Email);
            if (user != null) throw new ArgumentException(ErrorKeys.UnSuccessFullInsert);

            var mapped = mapper.Map<User>(userModel) ?? throw new ArgumentException(ErrorKeys.Mapped);
            var result = await userManager.CreateAsync(mapped, userModel.Password);

            if (!result.Succeeded)
            {
                throw new ArgumentException("Something went wrong during user creation.");
            }

            var body = @"
                    <!DOCTYPE html>
                    <html lang='en'>
                    <head>
    <meta charset='UTF-8'>
    <meta http-equiv='X-UA-Compatible' content='IE=edge'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Welcome to It Step Georgia!</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            margin: 0;
            padding: 0;
            background-color: #f8f9fa;
        }
        .container {
            width: 80%;
            margin: auto;
            margin-left:30%;
            padding: 20px;
        }
        .header {
            text-align: center;
            color: #007bff;
            font-size: 24px;
            margin-bottom: 20px;
        }
        .content {
            font-size: 16px;
            color: #333;
            margin-bottom: 15px;
        }
        .list-item {
            font-size: 16px;
            color: #333;
            margin-left: 20px;
        }
        .security-notice {
            background-color: #f8f9fa;
            padding: 10px;
            border-radius: 5px;
            margin-top: 20px;
            text-align: center;
            color: #555;
            font-size: 14px;
        }
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            Welcome to BookBridge library!
        </div>
        <div class='content'>
            <p>მოგესალმებით,</p>
            <p>თქვენ წარმატებით დაარეგისტრირეთ ანგარიში.</p>
            <p>კეთილი იყოს თქვენი მობრძანება!</p>
            <p>აქ  არის რეკომენდაციები თქვენთვის:</p>
            <ul>
                <li class='list-item'>დაათვალიერეთ ჩვენი სერვისები.</li>
                <li class='list-item'>შეცვალეთ თქვენი აქაუნთის დეტალები.</li>
                <li class='list-item'>დაგვიკავშირდით დამატებითი კითხვების შემთხვევაში.</li>
            </ul>
            <p>მადლობა რომ აგვირჩიეთ!</p>
        </div>
        <div class='security-notice'>
            გთხოვთ არ გაუზიაროთ ეს მეილი მესამე პირს.
        </div>
    </div>
</body>
</html>
";
            if (string.IsNullOrEmpty(userModel.Email))
            {
                throw new ArgumentException("User email is not valid.");
            }

            try
            {
                smtpService.SendMessage(userModel.Email, $"BookBridge Library new account {DateTime.Now.ToShortTimeString()}", body);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to send email.", ex);
            }

            return result;
        }
        #endregion

        #region SignIn

        public async Task<SignInResult> SignIn(SignInModel signInModel)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(signInModel);
                var user = await userManager.FindByNameAsync(signInModel.UserName);
                if (user == null) throw new UnauthorizedAccessException(ErrorKeys.NotFound);
                var signInResult =
                    await signIn.PasswordSignInAsync(signInModel.UserName, signInModel.Password, true, false);
                var recipientName = user.Name + ' ' + user.Surname;
                var emailContent = $@"
                      <html>
                     <body style='font-family: Arial, sans-serif;'>
                     <p>ძვირფასო <span style='color: #3366cc;'>{recipientName}</span>,</p>
                     <p>ჩვენ შევნიშნეთ ახალი მოწყობილება თქვენს BookBridge account-ზე. თუ ეს თქვენ იყავით, არ არის საჭირო შემდგომი ქმედება. 
                      თუმცა, თუ თქვენ არ შესულხართ, გთხოვთ, დაუყოვნებლივ დაგვიკავშირდეთ და ჩვენ დაგეხმარებით თქვენი ანგარიშის დაცვაში!</p>
                     <p>მადლობა ყურადღებისთვის.</p>
                     <p style='color: #ff6600;'>პატივისცემით,<br/>BookBridge team</p>
                     </body>
                     </html>";

                smtpService.SendMessage(user.Email,
                    $"ახალი მოწყობილობა დაფიქსირდა თქვენს ანგარიშზე {DateTime.Now.ToShortTimeString()}",
                    emailContent);
                return signInResult;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        #endregion

        #region SignOut

        public async Task<bool> SignOut()
        {
            try
            {
                await signIn.SignOutAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
        #endregion

        #region CreateRole

        public async Task<bool> CreateRole(string roleName)
        {
            ArgumentNullException.ThrowIfNull(roleName);
            if (await roleManager.RoleExistsAsync(roleName))
            {
                throw new ArgumentException("Such role is exist already");
            }
            var res = await roleManager.CreateAsync(new IdentityRole(roleName));
            return res == IdentityResult.Success;
        }
        #endregion

        #region DeleteRole

        public async Task<bool> DeleteRole(string roleName)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(roleName);
                if (await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.DeleteAsync(new IdentityRole(roleName));
                    return true;
                }

                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        #endregion

        #region GetAllRole

        public async Task<IEnumerable<string>> GetAllRole()
        {
            try
            {
                return await roleManager.Roles.Select(i => i.Name).ToListAsync();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        #endregion

        #region AssignRoleToUser

        public async Task<bool> AssignRoleToUser(string roleName, string userId)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(roleName);
                ArgumentNullException.ThrowIfNull(userId);
                if (!await roleManager.RoleExistsAsync(roleName)) throw new ArgumentException(ErrorKeys.NotFound);
                var user =await  userManager.FindByIdAsync(userId);
                if (user is null) throw new ArgumentException(ErrorKeys.NotFound);
                var res = await userManager.AddToRoleAsync(user,roleName);
                return res == IdentityResult.Success;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        #endregion

        #region GetAllUser
        public async Task<IEnumerable<UserModel>> GetAllUser()
        {
            try
            {
                var ser=await userManager.Users.ToListAsync();
                var mapped = mapper.Map<IEnumerable<UserModel>>(ser);
                return mapped;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        #endregion

        #region GetUserById
        public async Task<UserModel> GetUserById(string userId)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(userId);
                var res = await userManager.FindByIdAsync(userId)
                          ?? throw new ArgumentException(ErrorKeys.NotFound);
                var mapped = mapper.Map<UserModel>(res)
                             ?? throw new ArgumentException(ErrorKeys.Mapped);
                return mapped;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        #endregion

        #region DeleteUser

        public async Task<IdentityResult> DeleteUser(string id)
        {
            var res = userManager.Users.FirstOrDefault(io => io.Id == id);
            if (res is null) return new IdentityResult();

            var rek = await userManager.DeleteAsync(res);
            return rek;
        }

        #endregion

        #region Info

        public async Task<UserModel> Info(string Username)
        {
            var res = await userManager.Users.FirstOrDefaultAsync(io => io.UserName == Username);
            if (res is null) return null;
            var mapped = mapper.Map<UserModel>(res);
            return mapped;
        }

        #endregion

    }
}
