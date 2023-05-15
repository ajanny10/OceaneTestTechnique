using Microsoft.AspNetCore.Mvc;
using Test_Technique.Models;
using Test_Technique.ModelsWeb;
using Test_Technique.TT_Data;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.IO;
using Microsoft.AspNetCore.StaticFiles;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using MailKit.Security;
using MimeKit;

//using System.Net.Mail;
using System.Net;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit.Text;
using Microsoft.AspNetCore.Authorization;

namespace Test_Technique.Controllers
{
    public class CandidatController : Controller
    {
        private readonly AppDbContext _appDbContext;
        public CandidatController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public ActionResult DisplayCandidats()
        {

            return View();

        }

        public ActionResult GetCandidatAsJson()
        {
            try
            {

                var candidat = _appDbContext.candidats.AsEnumerable().Select( o => new
                {
                        id          = o.CandidatID,
                        nom         = o.Nom,
                        prenom      = o.Prenom,
                        email       = o.Mail,
                        tel         = o.Telephone,
                        niveuEtude  = o.Niveau_Etude,
                        anneeExpert = o.Nombre_Annees_Expérience,
                        dernierEmpl = o.Dernier_Employeur,
                        path        = o.Path 
                }).ToList();

                return Json(new { success = true , data = candidat });

            }
            catch (Exception)
            {
                return Json(new { success = false , Message = "Quelque chose s'est mal passé essaie encore" });
                throw;
            }
        }

        [HttpPost]
        public ActionResult DeleteCandidat(int candidatid)
        {

            try
            {

                var candidat = _appDbContext.candidats.FirstOrDefault(o => o.CandidatID == candidatid);

                if(candidat != null && candidat.Path != null)
                {
                    string[] paths = candidat.Path.Split('/');
                    var filePathDossier = Path.Combine("wwwroot", "Fichier", paths[0]);
                    _appDbContext.candidats.Remove(candidat);
                    _appDbContext.SaveChanges();
                    Directory.Delete(filePathDossier, true);
                }

                return Json(new { success = true, message = "Suppression réussie" });

            }       
            catch (Exception ex)
            {

                return Json(new { success = true, message = ex.ToString() });
                throw;

            }

        }

        [HttpPost]
        public JsonResult GetPath(string path)
        {

            string[] paths = path.Split('/');

            var filePathCV = Path.Combine("wwwroot", "Fichier", paths[0] , paths[1]);

            var provider = new FileExtensionContentTypeProvider();
            provider.TryGetContentType(filePathCV, out string contentType);

            return Json( new { success = true, path = filePathCV, contentType = contentType });

        }

        public FileResult ShowFile(string path, string contentType)
        {

            var fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Read);

            return File(fileStream, contentType);
        }

        [HttpGet]
        public ActionResult GetCandidatAsJsonKOESearch(string Search)
        {

            try
            {

                var candidat = (dynamic)null;

                if (Search == null)
                {
                     candidat = _appDbContext.candidats.AsEnumerable().Select(o => new
                    {
                        id = o.CandidatID,
                        nom = o.Nom,
                        prenom = o.Prenom,
                        email = o.Mail,
                        tel = o.Telephone,
                        niveuEtude = o.Niveau_Etude,
                        anneeExpert = o.Nombre_Annees_Expérience,
                        dernierEmpl = o.Dernier_Employeur,
                    }).ToList();

                }
                else
                {

                    candidat = _appDbContext.candidats.AsEnumerable()   
                        .Where( o => (string.IsNullOrEmpty(o.Nom) == false && o.Nom.ToUpper().Contains(Search.ToUpper(), StringComparison.OrdinalIgnoreCase))
                        || (string.IsNullOrEmpty(o.Prenom) == false && o.Prenom.ToUpper().Contains(Search.ToUpper(), StringComparison.OrdinalIgnoreCase))
                        || (string.IsNullOrEmpty(o.Mail) == false && o.Mail.ToUpper().Contains(Search.ToUpper(), StringComparison.OrdinalIgnoreCase))
                        || (string.IsNullOrEmpty(o.Telephone) == false && o.Telephone.ToUpper().Contains(Search.ToUpper(), StringComparison.OrdinalIgnoreCase))
                        ).Select(o => new
                    {
                        id = o.CandidatID,
                        nom = o.Nom,
                        prenom = o.Prenom,
                        email = o.Mail,
                        tel = o.Telephone,
                        niveuEtude = o.Niveau_Etude,
                        anneeExpert = o.Nombre_Annees_Expérience,
                        dernierEmpl = o.Dernier_Employeur,
                    }).ToList();

                }

                return Json(new { success = true, data = candidat });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, Message = ex.ToString() });
            }

        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult AjoutCandidat()
        {
                return View();
        }
    
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> AjoutCandidat(AjoutCandidatModelWeb model)
        {
            if (ModelState.IsValid)
            {

                Candidat candidat = new Candidat()
                {
                    Nom = model.Nom,
                    Prenom = model.Prenom,
                    Mail = model.Mail,
                    Telephone = model.Telephone,
                    Nombre_Annees_Expérience = model.Nombre_Annees_Expérience,
                    Niveau_Etude = model.Niveau_Etude,
                    Dernier_Employeur = model.Dernier_Employeur,
                    Path = ProcessUploadedFile(model).ToString()
               };

                _appDbContext.candidats.Add(candidat);
                _appDbContext.SaveChanges();

                string fullname = model.Nom + " " + model.Prenom;
                
                if( model.Mail != null )
                {
                    await SendEmailAsync(model.Mail.Trim(), fullname);
                }

                ModelState.Clear();
                return View(new AjoutCandidatModelWeb() { Success = "true" });
            }
            ModelState.Remove("Success");
            model.Success = "false";
            return View(model);
        }
            
        private string ProcessUploadedFile(AjoutCandidatModelWeb model)
        {

            string uniqueFileName = String.Empty;

            if (model.file != null && model.Prenom != null && model.Nom != null )
            {

                string CandidatFullName = string.Format("{0}{1}", model.Nom.ToUpper().Trim(), model.Prenom.ToUpper().Trim());

                string FolderName = CheckNameExisting(CandidatFullName);

                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Fichier", FolderName);

                Directory.CreateDirectory(folderPath);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.file.FileName);

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Fichier", FolderName, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    model.file.CopyTo(stream);
                }

                uniqueFileName = FolderName + "/" + fileName;
            }
            
            return uniqueFileName;
        }

        private string CheckNameExisting(string namefolder)
        {
            var localpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Fichier");
            var fullpath = Path.Combine(localpath, namefolder);
            int number = 1;
            string NewNameFolder = string.Empty;
            while (Directory.Exists(fullpath))
            {
                NewNameFolder = $"{namefolder}({number++})";
                fullpath = Path.Combine(localpath, NewNameFolder);
            }

            return string.IsNullOrEmpty(NewNameFolder) == true ? namefolder : NewNameFolder;

        }

        public async Task SendEmailAsync(string email, string name)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Test Technique", "Test.Technique10@gmail.com"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = "Message de confirmation";
            emailMessage.Body = new TextPart("html") { Text = $"Bonjour {name} , vous avez postulé avec succès pour l'offre Développeur Back end Dot Net Core" };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync("Test.Technique10@gmail.com", "bcgkrkcoyjrvohsh");
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }

    }
}
