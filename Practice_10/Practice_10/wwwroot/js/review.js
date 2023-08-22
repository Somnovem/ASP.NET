const emailInput = document.getElementById('email');
const reviewInput = document.getElementById('review');
const submitButton = document.getElementById('submitButton');

emailInput.addEventListener('input', validateEmail);
reviewInput.addEventListener('input', toggleSubmitButton);

function validateEmail() {
    const emailValue = emailInput.value.trim();
    const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

    if (emailValue === '' || !emailPattern.test(emailValue)) {
        emailInput.classList.add('invalid');
    } else {
        emailInput.classList.remove('invalid');
    }

    toggleSubmitButton();
}

function toggleSubmitButton() {
    const emailValid = !emailInput.classList.contains('invalid');
    const reviewValid = reviewInput.value.trim() !== '';

    submitButton.disabled = !(emailValid && reviewValid);
}