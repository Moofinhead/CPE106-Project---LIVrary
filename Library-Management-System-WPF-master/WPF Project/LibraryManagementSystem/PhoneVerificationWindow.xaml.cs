using System;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Media;
using LibraryMSWF.BL;

namespace LibraryManagementSystem
{
    public partial class PhoneVerificationWindow : Window
    {
        private readonly string _email;
        private readonly SmsService _smsService;
        private string _verificationCode;
        private DispatcherTimer _timer;
        private int _timeRemaining;
        public bool IsVerified { get; private set; }

        public PhoneVerificationWindow(string email)
        {
            InitializeComponent();
            _email = email;
            _smsService = new SmsService();
            InitializeTimer();
            SendVerificationCode();
        }

        private void InitializeTimer()
        {
            _timeRemaining = 300; // 5 minutes (300 seconds)
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
            _timer.Start();
            UpdateTimerDisplay();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _timeRemaining--;
            UpdateTimerDisplay();

            if (_timeRemaining <= 0)
            {
                _timer.Stop();
                btnResend.IsEnabled = true;
                timerText.Text = "Code expired";
                debugText.Text = "Code has expired. You can request a new one.";
            }
        }

        private void UpdateTimerDisplay()
        {
            int minutes = _timeRemaining / 60;
            int seconds = _timeRemaining % 60;
            timerText.Text = $"Code expires in: {minutes:D2}:{seconds:D2}";
        }

        private void SendVerificationCode()
        {
            try
            {
                _verificationCode = new Random().Next(100000, 999999).ToString();
                btnResend.IsEnabled = false;
                alertText.Text = string.Empty;

                debugText.Text = $"[DEBUG] Initializing verification process\n";
                debugText.Text += $"Target email: {_email}\n";

                if (string.IsNullOrWhiteSpace(_email))
                {
                    alertText.Text = "Email address cannot be empty.";
                    debugText.Text += "[ERROR] Empty email address";
                    btnResend.IsEnabled = true;
                    return;
                }

                if (_smsService.SendVerificationCode(_email.Trim(), _verificationCode))
                {
                    InitializeTimer();
                    debugText.Text += "[SUCCESS] Verification code sent!\n";
                    debugText.Text += "Please check your email (including spam folder)";
                }
                else
                {
                    alertText.Text = "Failed to send verification code. Please check your email address.";
                    btnResend.IsEnabled = true;
                    debugText.Text += "[ERROR] Failed to send verification code\n";
                    debugText.Text += "Check console for detailed error message";
                }
            }
            catch (Exception ex)
            {
                alertText.Text = "Error sending verification code.";
                btnResend.IsEnabled = true;
                debugText.Text = $"[ERROR] Exception occurred:\n{ex.Message}\n";
                debugText.Text += $"Stack trace: {ex.StackTrace}";
            }
        }

        private void BtnVerify_Click(object sender, RoutedEventArgs e)
        {
            if (_timeRemaining <= 0)
            {
                alertText.Text = "Code has expired. Please request a new code.";
                debugText.Text = "[ERROR] Verification attempt with expired code";
                return;
            }

            debugText.Text = "[DEBUG] Verifying code...\n";
            debugText.Text += $"Entered code: {txtVerificationCode.Text}\n";

            if (txtVerificationCode.Text == _verificationCode)
            {
                debugText.Text += "[SUCCESS] Code verified successfully!";
                IsVerified = true;
                Close();
            }
            else
            {
                alertText.Text = "Invalid verification code. Please try again.";
                txtVerificationCode.Text = string.Empty;
                debugText.Text += "[ERROR] Invalid code entered";
            }
        }

        private void BtnResend_Click(object sender, RoutedEventArgs e)
        {
            debugText.Text = "[DEBUG] Initiating code resend...\n";
            SendVerificationCode();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            IsVerified = false;
            Close();
        }
    }
} 