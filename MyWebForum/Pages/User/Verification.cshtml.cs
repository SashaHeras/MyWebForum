using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyForum.Extensions;
using MyWebForum.Data;
using MyWebForum.Data.Interfaces;
using MyWebForum.Data.Repository.Repositories;
using System.Net;
using System.Net.Mail;

namespace MyWebForum.Pages.User
{
    public class VerificationModel : PageModel
    {
        private readonly MyForumContext _db;
        private IUserRepository _users;

        public int UserId { get; set; }

        public bool IsAdmin { get; set; }

        public string? SecretPass { get; set; }

        public VerificationModel(MyForumContext db)
        {
            _db = db;
            _users = new UserRepository(db);
        }

        public void OnGet(int id)
        {
            SecretPass = SendSecretPass(id).ToString();
            IsAdmin = _users.GetUserById(id).IsAdmin;
            UserId = id;
        }

        public IActionResult OnPost()
        {
            ModelState.Clear();

            string res = Request.Form["pass"].ToString();

            if (res.ToString() == null || res == string.Empty)
            {
                ModelState.AddModelError("SecretPass", "Secret pass can`t be empty");
            }
            else if(res != SecretPass)
            {
                ModelState.AddModelError("SecretPass", "Uncorrect secret pass!");
            }
            else if(res == SecretPass)
            {
                Models.User u = _users.GetUserById(UserId);
                u.IsVerified = true;

                _db.User.Update(u);
                _db.SaveChanges();

                HttpContext.Session.Set<Models.User>("user", u);

                return RedirectToPage("Index");
            }

            return Page();
        }

        private int SendSecretPass(int id)
        {
            MailMessage msg = new MailMessage();

            msg.From = new MailAddress("acseler16@gmail.com");
            msg.To.Add(_users.GetUserById(id).Email.ToString());
            msg.Subject = "Verification";

            Random rand = new Random();
            int pass = rand.Next(100000, 999999);
            msg.Body = "Secret pass: " + pass.ToString();

            using (SmtpClient client = new SmtpClient())
            {
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("acseler16@gmail.com", "Cthusq2002");
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;

                try
                {
                    client.Send(msg);
                    return pass;
                }
                catch (Exception ex)
                {

                }
            }

            return -1;
        }
    }
}
