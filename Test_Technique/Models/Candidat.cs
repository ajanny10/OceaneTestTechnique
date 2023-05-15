using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Test_Technique.Models
{

    [Table("Candidats")]
    public class Candidat
    {

        [Key]
        [Display(Name = "ID")]
        public int CandidatID { get; set; }

        [Required]
        [Display(Name = "Nom Candidat")]
        [Column(TypeName = "varchar(200)")]
        public string? Nom { get; set; }

        [Required]
        [Display(Name = "Prénom Candidat")]
        [Column(TypeName = "varchar(200)")]
        public string? Prenom { get; set; }

        [Required]
        [Display(Name = "Mail Candidat")]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",
            ErrorMessage = "Invalid Email Format")]
        [Column(TypeName = "varchar(200)")]
        public string? Mail { get; set; }

        [Required]
        [Display(Name = "Téléphone")]
        [Column(TypeName = "varchar(200)")]
        public string? Telephone { get; set; }

        [Required]
        [Display(Name = "Niveau d’étude")]
        [Column(TypeName = "varchar(200)")]
        public string? Niveau_Etude { get; set; }

        [Required]
        [Display(Name = "Nombre d’années d’expérience")]
        [Column(TypeName = "varchar(200)")]
        public string? Nombre_Annees_Expérience { get; set; }

        [Required]
        [Display(Name = "Dernier employeur")]
        [Column(TypeName = "varchar(200)")]
        public string? Dernier_Employeur { get; set; }

        [Column(TypeName = "varchar(250)")]
        public string? Path { get; set; }

    }
}
