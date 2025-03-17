private void BtnRegister_Click(object sender, RoutedEventArgs e)
{
    try
    {
        // Basic validation
        if (string.IsNullOrEmpty(txtPhoneNumber.Text))
        {
            MessageBox.Show("Please enter a phone number.");
            return;
        }

        // Show verification window
        var verificationWindow = new PhoneVerificationWindow(txtPhoneNumber.Text);
        verificationWindow.ShowDialog();

        if (verificationWindow.IsVerified)
        {
            // Continue with registration
            User newUser = new User
            {
                UserName = txtUsername.Text,
                UserEmail = txtEmail.Text,
                UserPassword = txtPassword.Text,
                PhoneNumber = txtPhoneNumber.Text,
                IsVerified = true
            };

            // Add user to database
            if (userBL.AddUserBL(newUser))
            {
                MessageBox.Show("Registration successful!");
                this.Close();
            }
            else
            {
                MessageBox.Show("Registration failed. Please try again.");
            }
        }
        else
        {
            MessageBox.Show("Phone verification failed. Please try again.");
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show($"Error during registration: {ex.Message}");
    }
} 