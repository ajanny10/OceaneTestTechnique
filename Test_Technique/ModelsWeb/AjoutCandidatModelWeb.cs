using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Test_Technique.Tools;

namespace Test_Technique.ModelsWeb
{
    public class AjoutCandidatModelWeb
    {

        [Required(ErrorMessage = "Ce champ est obligatoire.")]
        [Display(Name = "Nom :")]
        [Column(TypeName = "varchar(200)")]
        public string? Nom { get; set; }

        [Required(ErrorMessage = "Le champ est obligatoire.")]
        [Display(Name = "Prénom :")]
        [Column(TypeName = "varchar(200)")]
        public string? Prenom { get; set; }

        [Required(ErrorMessage = "Ce champ est obligatoire.")]
        [Display(Name = "E-mail :")]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",
            ErrorMessage = "Format d'email invalide")]
        [Column(TypeName = "varchar(200)")]
        public string? Mail { get; set; }

        [Required(ErrorMessage = "Ce champ est obligatoire.")]
        [Display(Name = "Téléphone :")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Pas un numéro de téléphone valide")]
        [Column(TypeName = "varchar(200)")]
        public string? Telephone { get; set; }

        [Required(ErrorMessage = "Ce champ est obligatoire.")]
        [Display(Name = "Niveau d’étude :")]
        [Column(TypeName = "varchar(200)")]
        public string? Niveau_Etude { get; set; }

        [Required(ErrorMessage = "Ce champ est obligatoire.")]
        [RegularExpression("[0-9]{2}" , ErrorMessage = "Doit être un type numérique Et a deux chiffres ou moins")]
        [Display(Name = "Nombre d’années d’expérience :")]
        [Column(TypeName = "varchar(200)")]
        public string? Nombre_Annees_Expérience { get; set; }

        [Required(ErrorMessage = "Ce champ est obligatoire.")]
        [Display(Name = "Dernier employeur :")]
        [Column(TypeName = "varchar(200)")]
        public string? Dernier_Employeur { get; set; }

        [Required(ErrorMessage = "Doit fournir un fichier.")]
        [AllowedExtensions(new string[] { ".jpg", ".png", ".jpeg", ".pdf" })]
        [Display(Name = "Fichier :")]
        public IFormFile? file { get; set; }

        public  string? Success { get; set; }
    }
}
