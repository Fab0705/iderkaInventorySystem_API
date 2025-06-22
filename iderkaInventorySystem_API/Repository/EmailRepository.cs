using iderkaInventorySystem_API.Models;
using iderkaInventorySystem_API.Service;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace iderkaInventorySystem_API.Repository
{
    public class EmailRepository : iEmail
    {
        private readonly IConfiguration _config;
        public EmailRepository(IConfiguration config)
        {
            _config = config;
        }
        public async Task SendConditionReport(ConditionReport report)
        {
            var adminEmail = _config["EmailSettings:AdminEmail"];
            var senderEmail = _config["EmailSettings:SenderEmail"];
            var senderPassword = _config["EmailSettings:SenderPassword"];

            var subject = $"🛠️ Reporte de condiciones - Transferencia {report.TransferId}";
            var body = new StringBuilder();
            body.AppendLine($"<h3>Reporte de condiciones de transferencia</h3>");
            body.AppendLine($"<p><strong>Usuario que recibió:</strong> {report.User}</p>");
            body.AppendLine($"<p><strong>ID de Transferencia:</strong> {report.TransferId}</p>");
            body.AppendLine($"<p><strong>Condición reportada:</strong> {report.Condition}</p>");
            body.AppendLine($"<p><strong>Detalles:</strong><br />{report.Details}</p>");
            body.AppendLine("<br/><hr/><p style='font-size: 12px; color: gray;'>Este correo fue generado automáticamente.</p>");

            using (var smtp = new SmtpClient("smtp.gmail.com", 587))
            {
                smtp.Credentials = new NetworkCredential(senderEmail, senderPassword);
                smtp.EnableSsl = true;

                var mail = new MailMessage
                {
                    From = new MailAddress(senderEmail),
                    Subject = subject,
                    Body = body.ToString(),
                    IsBodyHtml = true
                };

                mail.To.Add(adminEmail);

                await smtp.SendMailAsync(mail);
            }
        }
    }
}
