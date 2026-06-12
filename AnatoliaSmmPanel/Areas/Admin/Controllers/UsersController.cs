using AnatoliaSmmPanel.Areas.Admin.Data;
using AnatoliaSmmPanel.Areas.Admin.ViewModels.Users;
using AnatoliaSmmPanel.Data.Models.Appliciton;
using AnatoliaSmmPanel.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnatoliaSmmPanel.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/users")]
    [Authorize(Roles = "SuperAdmin,Admin")]


    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> _logger;
        private readonly ISmmApiService _smmApiService;
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(ILogger<UsersController> logger, ISmmApiService smmApiService, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _smmApiService = smmApiService;
            _userManager = userManager;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("GetAll")]
        [IgnoreAntiforgeryToken]
        public IActionResult GetAll()
        {
            var users = _userManager.Users
                .OrderByDescending(x => x.UserId)
                .Select(u => new
                {
                    Id = u.UserId,
                    Username = u.Nickname,
                    Email = u.Email,
                    EmailConfirmed = u.EmailConfirmed,
                    Balance = u.Balance,
                    Spent = u.Spent,
                    Status = u.Status,
                    CreatedAt = u.CreatedAt,
                    LastAuthAt = u.LastAuthAt,
                    PhoneNumber = u.PhoneNumber,
                    PhoneNumberConfirmed = u.PhoneNumberConfirmed,
                    TwoFactorEnabled = u.TwoFactorEnabled
                })
                .ToList();

            return Json(new { success = true, total = users.Count, data = users });
        }

        // Edit user modalunu açar
        [HttpGet("EditUser/{id}")]
        public async Task<IActionResult> EditUser(int id)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(x => x.UserId == id);

            if (user == null) return NotFound();

            return PartialView("_EditUserModal", user);
        }
        // Kullanıcı bilgilerini günceller
        [HttpPost("UpdateUser")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserViewModel model)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(x => x.UserId == model.Id);

            if (user.Email != model.Email)
            {
                var existingUser = await _userManager.FindByEmailAsync(model.Email);
                if (existingUser != null && existingUser.UserId != model.Id)
                    return Json(new { success = false, message = "This email is already in use." });
            }

            user.Nickname = model.Nickname;
            user.Email = model.Email;
            user.NormalizedEmail = model.Email.ToUpper();
            user.UserName = model.Email;
            user.NormalizedUserName = model.Email.ToUpper();
            user.PhoneNumber = model.PhoneNumber;
            user.Status = model.Status;
            user.Balance = model.Balance;
            user.Spent = model.Spent;
            user.EmailConfirmed = model.EmailConfirmed;
            user.PhoneNumberConfirmed = model.PhoneNumberConfirmed;
            user.TwoFactorEnabled = model.TwoFactorEnabled;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return Json(new { success = false, message = string.Join(", ", result.Errors.Select(e => e.Description)) });

            return Json(new { success = true });
        }



        [HttpGet("SetPassword/{id}")]
        public async Task<IActionResult> SetPassword(int id)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserId == id);
            if (user == null) return NotFound();
            return PartialView("_SetPasswordModal", new SetPasswordViewModel { UserId = id });
        }

        [HttpPost("SetPassword")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> SetPassword([FromBody] SetPasswordViewModel model)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserId == model.UserId);
            if (user == null)
                return Json(new { success = false, message = "User not found." });

            if (model.NewPassword != model.ConfirmPassword)
                return Json(new { success = false, message = "Passwords do not match." });

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);

            if (!result.Succeeded)
                return Json(new { success = false, message = string.Join(", ", result.Errors.Select(e => e.Description)) });

            return Json(new { success = true });
        }




        [HttpGet("SetDiscount/{id}")]
        public async Task<IActionResult> SetDiscount(int id)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserId == id);
            if (user == null) return NotFound();
            return PartialView("_SetDiscountModal", new SetDiscountViewModel { UserId = id, DiscountPercent = user.DiscountPercent });
        }

        [HttpPost("SetDiscount")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> SetDiscount([FromBody] SetDiscountViewModel model)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserId == model.UserId);
            if (user == null)
                return Json(new { success = false, message = "User not found." });

            user.DiscountPercent = model.DiscountPercent;
            await _userManager.UpdateAsync(user);

            return Json(new { success = true });
        }




        [HttpGet("AddUser")]
        public IActionResult AddUser()
        {
            return PartialView("_AddUserModal", new AddUserViewModel());
        }

        [HttpPost("AddUser")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> AddUser([FromBody] AddUserViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Email))
                return Json(new { success = false, message = "Email is required." });

            var existing = await _userManager.FindByEmailAsync(model.Email);
            if (existing != null)
                return Json(new { success = false, message = "Email already in use." });

            var user = new ApplicationUser
            {
                Nickname = model.Nickname,
                Email = model.Email,
                UserName = model.Email,
                PhoneNumber = model.PhoneNumber,
                Status = model.Status,
                Balance = model.Balance,
                EmailConfirmed = false,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return Json(new { success = false, message = string.Join(", ", result.Errors.Select(e => e.Description)) });

            return Json(new { success = true, id = user.UserId });
        }




        [HttpPost("Suspend/{id}")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Suspend(int id)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserId == id);
            if (user == null)
                return Json(new { success = false, message = "User not found." });

            user.Status = user.Status == "Suspended" ? "Active" : "Suspended";
            await _userManager.UpdateAsync(user);

            return Json(new { success = true, status = user.Status });
        }




        [HttpGet("Export")]
        public IActionResult Export()
        {
            var users = _userManager.Users
                .OrderByDescending(x => x.UserId)
                .Select(u => new {
                    u.UserId,
                    u.Nickname,
                    u.Email,
                    u.Balance,
                    u.Spent,
                    u.Status,
                    u.CreatedAt,
                    u.PhoneNumber
                })
                .ToList();

            var csv = "Id,Username,Email,Balance,Spent,Status,CreatedAt,Phone\n" +
                string.Join("\n", users.Select(u =>
                    $"{u.UserId},{u.Nickname},{u.Email},{u.Balance},{u.Spent},{u.Status},{u.CreatedAt},{u.PhoneNumber}"));

            return File(System.Text.Encoding.UTF8.GetBytes(csv), "text/csv", "users.csv");
        }

    }
}
