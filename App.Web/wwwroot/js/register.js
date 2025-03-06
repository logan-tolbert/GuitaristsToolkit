function checkPasswordStrength(password) {
    const strengthElement = document.getElementById('password-strength');
    let strength = 'Weak';
    if (password.length >= 8) {
        strength = 'Medium';
    }
    if (password.length >= 12) {
        strength = 'Strong';
    }
    strengthElement.textContent = `Password Strength: ${strength}`;
}

function checkPasswordMatch() {
    const password = document.querySelector('[asp-for="Password"]').value;
    const confirmPassword = document.querySelector('[asp-for="ConfirmPassword"]').value;
    const confirmPasswordValidation = document.getElementById('confirmPassword-validation');
    if (password !== confirmPassword) {
        confirmPasswordValidation.textContent = 'Passwords do not match.';
    } else {
        confirmPasswordValidation.textContent = '';
    }
}