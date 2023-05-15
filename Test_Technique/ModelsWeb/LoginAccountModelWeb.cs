using Microsoft.AspNetCore.Authentication;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Test_Technique.ModelsWeb
{
    public class LoginAccountModelWeb
    {

        [Required(ErrorMessage = "Veuillez saisir votre login.")]
        [EmailAddress]
        [Display(Name = "Adresse e-mail :")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Veuillez saisir votre Mot de passe.")]
        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe :")]
        public string? Password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }

    }
}
