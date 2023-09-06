const wrapper = document.querySelector('.wrapper');
const signUpLink = document.querySelector('.signUp-link');
const signInLink = document.querySelector('.signIn-link');

const regPass = document.getElementById('registerPassword')
const regUsername = document.getElementById('registerUsername')
const regEmail = document.getElementById('registerEmail')
const regGender = document.getElementById('registerGender')
const regBirthday = document.getElementById('registerBirthday')
const regSubmit = document.getElementById('btnRegisterSubmit')

const usernameRegex = /^[a-zA-Z0-9_]{8,16}$/;
const emailRegex = /^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$/;

signUpLink.addEventListener('click', () => {
    wrapper.classList.add('animate-signIn');
    wrapper.classList.remove('animate-signUp');
});

signInLink.addEventListener('click', () => {
    wrapper.classList.add('animate-signUp');
    wrapper.classList.remove('animate-signIn');
});

function checkInputWithRegex(input, regex) {
    input.classList.remove('valid');
    input.classList.remove('invalid');
    if (regex.test(input.value)) {
        input.classList.add('valid');
        return true;
    }
    input.classList.add('invalid');
    return false;
}

function checkValidInput() {
    if (regUsername.classList.contains('valid') &&
        regPass.classList.contains('valid') &&
        regEmail.classList.contains('valid') &&
        regBirthday.classList.contains('valid') &&
        regGender.value != "disabled") {
        regSubmit.disabled = false;
    }
    else regSubmit.disabled = true;
}

regUsername.addEventListener('input', () => {
    if (checkInputWithRegex(regUsername, usernameRegex)) {
        regUsername.title = "";
    }
    else regUsername.title = "Username must contain only 8-16 english letters or digits";
});

regEmail.addEventListener('input', () => {
    if (checkInputWithRegex(regEmail, emailRegex)) {
        regEmail.title = "";
    }
    else regEmail.title = "Email must be in correct format";
});

regPass.addEventListener('input', () => {
    regPass.classList.remove('valid');
    regPass.classList.remove('invalid');
    if (regPass.value.length < 8 || regPass.value.length > 16) {
        regPass.classList.add('invalid');
        if (!regPass.title.includes("Length must be from 8 to 16 characters\n")) regPass.title += "Length must be from 8 to 16 characters\n";
        return;
    }
    var hasUppercase = false;
    var hasLowercase = false;
    var hasNumber = false;
    var hasSpecial = false;
    for (let i = 0; i < regPass.value.length; i++) {
        const currentCharacter = regPass.value[i];
        if (/[A-Z]/.test(currentCharacter)) {
            hasUppercase = true;
        } else if (/[a-z]/.test(currentCharacter)) {
            hasLowercase = true;
        } else if (/\d/.test(currentCharacter)) {
            hasNumber = true;
        } else if (/[+-_]/.test(currentCharacter)) {
            hasSpecial = true;
        }
    }
    if (hasUppercase && hasLowercase && hasNumber && hasSpecial) {
        regPass.classList.add('valid');
        regPass.title = "";
    }
    else {
        regPass.classList.add('invalid');
        if (!regPass.title.includes("Password must contain an uppercase letter, lowercase letter, a number and a special symbol[+,-,_]\n")) regPass.title += "Password must contain an uppercase letter, lowercase letter, a number and a special symbol[+,-,_]\n";
    }
});

regBirthday.addEventListener('input', () => {
    regBirthday.classList.remove('valid');
    regBirthday.classList.remove('invalid');
    var diffInMilliseconds = new Date() - Date.parse(regBirthday.value)
    var millisecondsInYear = 1000 * 60 * 60 * 24 * 365;
    var diffInYears = diffInMilliseconds / millisecondsInYear;
    if (diffInYears >= 18) {
        regBirthday.classList.add('valid');
        regBirthday.title = "";
    }
    else {
        regBirthday.classList.add('invalid');
        regBirthday.title = "You must be at least 18 years old";
    }
});

(()=>{
    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth()+1;
    var yyyy = today.getFullYear() - 18;
    if(dd<10) {
        dd = '0'+dd
    } 
    if(mm<10) {
        mm = '0'+mm
    } 
    today = yyyy + '-' + mm + '-' + dd;
    regBirthday.value = today;
    regBirthday.classList.add('valid');
})();

regUsername.addEventListener('change', checkValidInput);
regPass.addEventListener('change', checkValidInput);
regEmail.addEventListener('change', checkValidInput);
regGender.addEventListener('change', checkValidInput);
regBirthday.addEventListener('change', checkValidInput);