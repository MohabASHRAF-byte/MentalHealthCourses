using Org.BouncyCastle.Crypto;
using iText.Bouncycastleconnector;
using iText.Kernel.Crypto.Securityhandler;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Repositories;
using MentalHealthcare.Domain.Repositories.EmailSender;
using MentalHealthcare.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;
using Org.BouncyCastle.Asn1.X509;
using iText.IO.Font;

namespace MentalHealthcare.Infrastructure.Repositories
{
    public class CertificateRepository(
        MentalHealthDbContext dbContext,
        IEmailSender emailSender,
        UserManager<User> userManager,
        IEmailSenderWithAttachment emailSenderWith,
        ILogger<CertificateRepository> logger
    ) : ICertificateRepository
    {
        public async Task<Certificate> GenerateCertificateAsync(string userName, string courseName, string userEmail , int adminId)
        {
            try
            {
                // Path to the certificate template
                string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "templates", "WhatsApp Image 2025-04-05 at 6.28.17 PM-Edites.pdf");

                // Generate unique file name for the certificate
                string outputFileName = $"{Guid.NewGuid()}.pdf";
                string outputPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "generated_certificates", outputFileName);
                Directory.CreateDirectory(Path.GetDirectoryName(outputPath)!);

                // Read the template PDF
                using var reader = new PdfReader(templatePath);
                using var writer = new PdfWriter(outputPath);
                using var pdfDoc = new PdfDocument(reader, writer);
                var document = new Document(pdfDoc);
                var page = pdfDoc.GetFirstPage();
                var canvas = new PdfCanvas(page);

                // Load Arabic font (make sure to use a font that supports Arabic)
                string arabicFontPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "fonts", "Cairo-Regular.ttf");
                if (!File.Exists(arabicFontPath))
                {
                    throw new FileNotFoundException($"Arabic font not found at: {arabicFontPath}");
                }

                // Correct font loading with PDF encoding
                PdfFont arabicFont = PdfFontFactory.CreateFont(arabicFontPath, PdfEncodings.IDENTITY_H, PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED);

                // Ensure that the user name and course name are not null or empty
                if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(courseName))
                {
                    throw new ArgumentException("User name or course name cannot be null or empty.");
                }

                // Add user name to the certificate in Arabic
                canvas.BeginText()
                    .SetFontAndSize(arabicFont, 18)
                    .SetColor(ColorConstants.BLACK, true)
                    .MoveText(200, 300) // Adjust the position on the certificate
                    .ShowText(userName) // Display the user name in Arabic
                    .EndText();

                // Add course name to the certificate in Arabic
                canvas.BeginText()
                    .SetFontAndSize(arabicFont, 16)
                    .SetColor(ColorConstants.BLACK, true)
                    .MoveText(200, 270) // Adjust position for course name
                    .ShowText(courseName) // Display course name in Arabic
                    .EndText();

                // Close the document after updating the PDF
                document.Close();

                // Generate dynamic URL
                string baseUrl = "https://mentalhealthcareapi20250307003056.azurewebsites.net"; // Update with dynamic URL if needed
                string certificateUrl = $"{baseUrl}/generated_certificates/{outputFileName}";

                // Create a new certificate entity to save in the database
                var certificate = new Certificate
                {
                    UserName = userName,
                    UserEmail = userEmail,
                    CourseName = courseName,
                    CertificatePath = certificateUrl, // Use dynamic URL
                    DateGenerated = DateTime.UtcNow,
                    AdminId = adminId
                };

                // Save the certificate information to the database
                dbContext.Certificates.Add(certificate);
                await dbContext.SaveChangesAsync();

                return certificate;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error generating certificate for {UserEmail}", userEmail);
                throw;
            }
        }

        public async Task SendCertificateEmailAsync(string userEmail, string pdfFilePath)
        {
            try
            {
                string subject = "Your Certificate is Ready!";
                string htmlMessage = $@"
    <h1>Congratulations!</h1>
    <p>Your certificate for completing the course has been generated. Please find it attached below.</p>
    <p>Or download it here: <a href='{pdfFilePath}'>View Certificate</a></p>";

                await emailSenderWith.SendEmailWithAttachmentAsync(userEmail, subject, htmlMessage, pdfFilePath);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error sending certificate email to {UserEmail}", userEmail);
                throw;
            }
        }
    }
}
