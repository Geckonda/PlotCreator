const beginning = document.getElementById('Beginning');
const ending = document.getElementById('Ending');

beginning.addEventListener("change", () => {
    ending.value = beginning.value;
});

ending.addEventListener("focusout", () => {
    if (ending.value < beginning.value)
        ending.value = beginning.value
})