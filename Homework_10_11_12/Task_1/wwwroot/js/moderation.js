window.addEventListener('load',()=>{

    const usernameRegex = /^[a-zA-Z0-9_]{8,16}$/;
    const emailRegex = /^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$/;

    function checkValidInput() {
        if (usernameInputGroup.classList.contains('has-success') &&
            emailInputGroup.classList.contains('has-success') &&
            birthdayInputGroup.classList.contains('has-success') &&
            passwordInputGroup.classList.contains('has-success') &&
            genderInput.value != "disabled") {
                submitBtn.disabled = false;
        }
        else submitBtn.disabled = true;
    }

    function checkInputWithRegex(inputgroup,input, regex) {
        inputgroup.classList.remove('has-success');
        inputgroup.classList.remove('has-error');
        if (regex.test(input.value)) {
            inputgroup.classList.add('has-success');
            return true;
        }
        inputgroup.classList.add('has-error');
        return false;
    }


    const idInput = document.querySelector('#userId');
    
    const usernameInput = document.querySelector('#userUsername');

    const passwordInput = document.querySelector('#userPassword');
    
    const genderInput = document.querySelector('#userGender');

    const emailInput = document.querySelector('#userEmail');

    const birthdayInput = document.querySelector('#userBirthday');

    const submitBtn = document.querySelector('#btnSubmit');

    const usernameErrorSpan = document.querySelector('#userUsernameValidation');

    const passwordErrorSpan = document.querySelector('#userPasswordValidation');

    const emailErrorSpan = document.querySelector('#userEmailValidation');

    const birthdayErrorSpan = document.querySelector('#userBirthdayValidation');

    const usernameInputGroup = document.querySelector('#usernameInputGroup');

    const passwordInputGroup = document.querySelector('#passwordInputGroup');
    
    const emailInputGroup = document.querySelector('#emailInputGroup');

    const birthdayInputGroup = document.querySelector('#birthdayInputGroup');


    usernameInput.addEventListener('input', () => {
        if (checkInputWithRegex(usernameInputGroup,usernameInput, usernameRegex)) {
            usernameErrorSpan.innerHTML = "";
        }
        else usernameErrorSpan.innerHTML = "Username must contain only 8-16 english letters or digits";
    });
    
    emailInput.addEventListener('input', () => {
        if (checkInputWithRegex(emailInputGroup,emailInput, emailRegex)) {
            console.log('valid');
            emailErrorSpan.innerHTML = "";
        }
        else {
            console.log('invalid');
            emailErrorSpan.innerHTML = "Email must be in correct format";
        }
    });

    passwordInput.addEventListener('input', () => {
        passwordInputGroup.classList.remove('has-success');
        passwordInputGroup.classList.remove('has-error');
        if (passwordInput.value.length < 8 || passwordInput.value.length > 16) {
            passwordInputGroup.classList.add('has-error');
            if (!passwordErrorSpan.innerHTML.includes("Length must be from 8 to 16 characters\n")) passwordErrorSpan.innerHTML += "Length must be from 8 to 16 characters\n";
            return;
        }
        var hasUppercase = false;
        var hasLowercase = false;
        var hasNumber = false;
        var hasSpecial = false;
        for (let i = 0; i < passwordInput.value.length; i++) {
            const currentCharacter = passwordInput.value[i];
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
            passwordInputGroup.classList.add('has-success');
            passwordErrorSpan.innerHTML = "";
        }
        else {
            passwordInputGroup.classList.add('has-error');
            if (!passwordErrorSpan.innerHTML.includes("Password must contain an uppercase letter, lowercase letter, a number and a special symbol[+,-,_]\n")) passwordErrorSpan.innerHTML += "Password must contain an uppercase letter, lowercase letter, a number and a special symbol[+,-,_]\n";
        }
    });
    
    birthdayInput.addEventListener('input', () => {
        birthdayInputGroup.classList.remove('has-success');
        birthdayInputGroup.classList.remove('has-error');
        var diffInMilliseconds = new Date() - Date.parse(birthdayInput.value)
        var millisecondsInYear = 1000 * 60 * 60 * 24 * 365;
        var diffInYears = diffInMilliseconds / millisecondsInYear;
        if (diffInYears >= 18) {
            birthdayInputGroup.classList.add('has-success');
            birthdayErrorSpan.innerHTML = "";
        }
        else {
            birthdayInputGroup.classList.add('has-error');
            birthdayErrorSpan.innerHTML = "You must be at least 18 years old";
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
        birthdayInput.value = today;
        birthdayInputGroup.classList.add('has-success');
    })();

    usernameInput.addEventListener('input', checkValidInput);
    passwordInput.addEventListener('input', checkValidInput);
    emailInput.addEventListener('input', checkValidInput);
    genderInput.addEventListener('input', checkValidInput);
    birthdayInput.addEventListener('input', checkValidInput);



    
    var changerButtons = document.querySelectorAll('.btn-change-user');
    changerButtons.forEach(button => {
        button.addEventListener('click',(e)=>{
            var tr = e.target.parentNode.parentNode;
            idInput.value = tr.querySelector('.user-id').innerHTML;
            usernameInput.value = tr.querySelector('.user-username').innerHTML;
            genderInput.value = tr.querySelector('.user-gender').innerHTML;
            emailInput.value = tr.querySelector('.user-email').innerHTML;
            birthdayInput.value = tr.querySelector('.user-birthdate').innerHTML;

            var event = new Event("input");
            emailInput.dispatchEvent(event);

            checkValidInput();
        })
    });
});