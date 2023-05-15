using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Test_Technique.ModelsWeb
{
    public class RegisterAccountModelWeb
    {

        [Required(ErrorMessage = "E-mail est obligatoire.")]
        [EmailAddress]
        [Remote(action: "IsAlreadyUsed", controller: "Authentification")]
        [Display( Name = "Adresse e-mail :")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Mot de passe est obligatoire.")]
        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe :")]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmez le mot de passe :")]
        [Compare("Password",
            ErrorMessage = "Le mot de passe et le mot de passe de confirmation ne correspondent pas.")]
        public string? ConfirmPassword { get; set; }

    }
}
