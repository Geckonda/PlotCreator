const registerBTN = document.getElementById('registerBTN');
const agreement = document.getElementById('agreement');

agreement.addEventListener('change', () => {
    if (agreement.checked)
        registerBTN.disabled = false;
    else
        registerBTN.disabled = true;
})
