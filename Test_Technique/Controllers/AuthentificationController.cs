using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Test_Technique.ModelsWeb;

namespace Test_Technique.Controllers
{
    public class AuthentificationController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AuthentificationController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsAlreadyUsed(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"Mail < {email} > est déjà utilisée");
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterAccountModelWeb model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToAction("DisplayCandidats", "Candidat");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }   
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Deconnecter()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("AjoutCandidat", "Candidat");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> login(LoginAccountModelWeb model, string? returnUrl)
        {

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, true);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("AjoutCandidat", "Candidat");
                    }
                }
                ModelState.AddModelError(string.Empty, "Login ou mot de passe incorrect.");
            }

            return View(model);
        }


    }
}
