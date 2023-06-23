const b_box = document.getElementById('birthday_box');
const b_block = document.getElementById('birthday_block');
const b_input = document.getElementById('birthday_input');

const d_box = document.getElementById('deathday_box');
const d_block = document.getElementById('deathday_block');
const d_input = document.getElementById('deathday_input');

const defaultDate = "0001-01-01";

window.onload = () => {
    if (b_box.checked) {
        b_block.classList.remove("hide");
    }
    if (d_box.checked) {
        d_block.classList.remove("hide");
    }
    console.log(b_input.value)
}

b_box.addEventListener("click", () => {
    if (b_box.checked) {
        b_block.classList.remove("hide");
    }
    else {
        b_block.classList.add("hide");
        b_input.value = defaultDate;
    }
})

d_box.addEventListener("click", () => {
    if (d_box.checked) {
        d_block.classList.remove("hide");
        d_input.value = b_input.value;
        console.log(d_input.value)
    }
    else {
        d_block.classList.add("hide");
        d_input.value = defaultDate;
        console.log(d_input.value)
    }
})

b_input.addEventListener("change", () => {
    if (d_box.checked && (d_input.value <= b_input.value)) {
        d_input.value = b_input.value;
    }
})

d_input.addEventListener("focusout", () => {
    if (d_box.checked && (d_input.value <= b_input.value)) {
        d_input.value = b_input.value;
    }
})
