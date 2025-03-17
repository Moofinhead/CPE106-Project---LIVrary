using System;
using System.Net.Mail;
using System.Net;
using System.Text.RegularExpressions;

namespace LibraryMSWF.BL
{
    public class SmsService
    {
        // Using Gmail SMTP (you can create a free Gmail account for this)
        private const string SMTP_HOST = "smtp.gmail.com";
        private const int SMTP_PORT = 587;
        private const string SENDER_EMAIL = "LIVRaryproject@gmail.com"; // Replace with your email
        private const string SENDER_PASSWORD = "zgrq oesk aowt dwwt"; // Generate this in Gmail settings

        private bool IsValidEmail(string email)
        {
            try
            {
                Console.WriteLine($"[DEBUG] Validating email: {email}");
                if (string.IsNullOrWhiteSpace(email))
                {
                    Console.WriteLine("[ERROR] Email is null or empty");
                    return false;
                }

                // Basic email format validation
                var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
                if (!regex.IsMatch(email))
                {
                    Console.WriteLine("[ERROR] Email format is invalid");
                    return false;
                }

                // Try creating a MailAddress (this validates the format)
                var addr = new MailAddress(email);
                Console.WriteLine("[DEBUG] Email format is valid");
                return addr.Address == email;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Email validation failed: {ex.Message}");
                return false;
            }
        }

        public bool SendVerificationCode(string email, string code)
        {
            try
            {
                if (!IsValidEmail(email))
                {
                    Console.WriteLine("[ERROR] Invalid email address format");
                    return false;
                }

                Console.WriteLine($"[DEBUG] Initializing email service for: {email}");
                using (var client = new SmtpClient(SMTP_HOST, SMTP_PORT))
                {
                    Console.WriteLine("[DEBUG] Setting up SMTP client...");
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential(SENDER_EMAIL, SENDER_PASSWORD);

                    Console.WriteLine("[DEBUG] Creating email message...");
                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(SENDER_EMAIL, "LIVRary System"),
                        Subject = "Welcome to LIVRary - Verify Your Email",
                        Body = $@"
                            <html>
                            <body style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px; color: #333;'>
                                <div style='background: linear-gradient(135deg, #2A2A2A 0%, #383838 100%); padding: 30px; border-radius: 10px; text-align: center;'>
                                    <h1 style='color: #FFFFFF; margin: 0; font-size: 28px; font-weight: 300;'>Welcome to LIVRary</h1>
                                </div>
                                
                                <div style='background: #FFFFFF; padding: 30px; border-radius: 10px; margin-top: 20px; text-align: center; box-shadow: 0 2px 10px rgba(0,0,0,0.1);'>
                                    <h2 style='color: #2A2A2A; margin-bottom: 30px; font-weight: 400;'>Your Verification Code</h2>
                                    
                                    <div style='background: #F5F5F5; padding: 20px; border-radius: 5px; margin: 20px 0; font-family: monospace;'>
                                        <span style='font-size: 32px; letter-spacing: 5px; color: #2A2A2A; font-weight: bold;'>{code}</span>
                                    </div>
                                    
                                    <p style='color: #666; margin: 20px 0;'>
                                        This code will expire in 5 minutes.<br>
                                        Please do not share this code with anyone.
                                    </p>
                                </div>
                                
                                <div style='text-align: center; margin-top: 20px; color: #666; font-size: 12px;'>
                                    <p>This is an automated message, please do not reply.</p>
                                    <p>If you didn't request this code, please ignore this email.</p>
                                </div>
                            </body>
                            </html>",
                        IsBodyHtml = true
                    };

                    Console.WriteLine("[DEBUG] Adding recipient...");
                    mailMessage.To.Add(new MailAddress(email.Trim()));

                    Console.WriteLine("[DEBUG] Attempting to send email...");
                    client.Send(mailMessage);
                    Console.WriteLine($"[DEBUG] Email sent successfully to: {email}");
                    return true;
                }
            }
            catch (SmtpException ex)
            {
                Console.WriteLine($"[ERROR] SMTP Error: {ex.StatusCode} - {ex.Message}");
                Console.WriteLine($"[ERROR] Failed at: {ex.StackTrace}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] General Error: {ex.Message}");
                Console.WriteLine($"[ERROR] Failed at: {ex.StackTrace}");
                return false;
            }
        }

        public bool VerifyCode(string email, string code)
        {
            return true; // Verification is handled in the window
        }
    }
}