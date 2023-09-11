window.addEventListener('load',()=>{
    
    const idInput = document.querySelector('#userId');
    
    const emailInput = document.querySelector('#userEmail');

    const birthdayInput = document.querySelector('#userBirthday');
    
    const roleInput = document.querySelector('#userRole');

    const formActionInput = document.querySelector('#formAction');
    

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
    })();
    
    
    var changerButtons = document.querySelectorAll('.btn-change-user');
    changerButtons.forEach(button => {
        button.addEventListener('click',(e)=>{
            var tr = e.target.parentNode.parentNode.parentNode;
            idInput.value = tr.querySelector('.user-id').innerHTML;
            emailInput.value = tr.querySelector('.user-email').innerHTML;
            birthdayInput.value = tr.querySelector('.user-birthday').innerHTML;
            roleInput.value = tr.querySelector('.user-roleId').innerHTML;
            formActionInput.value = "Update";
        })
    });
});