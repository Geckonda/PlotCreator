const searchBar = document.getElementById('search-bar');
const searchBtn = document.getElementById('search-btn');
const resetBtn = document.getElementById('reset-btn');

const Evvent = document.querySelectorAll('.event-name');
const EvventCards = document.querySelectorAll('.eventContainer');

const noEvvent = document.getElementById("noEvent");


searchBtn.addEventListener('click', () => {
    var oneIsVisible = true;
    Evvent.forEach(elem => {
        elem.parentElement.classList.remove('hide');
        let textContent = GetLowerString(elem.innerHTML);
        let request = GetLowerString(searchBar.value);
        if(!textContent.includes(request)){
            elem.parentElement.classList.add('hide');
        }
        else{
            oneIsVisible = false;
        }
    })
    if(oneIsVisible){
        noEvvent.classList.remove('hide')
    }
});
resetBtn.addEventListener('click', () => {
    searchBar.value = "";
    Evvent.forEach(elem => {
        elem.parentElement.classList.remove('hide');
    })
    noEvvent.classList.add('hide')
})

function GetLowerString(str){
    return str.toLowerCase();
}


const reverse = document.getElementById('reverse');
var reverseTrue = true;
const parent = document.getElementById('block_container')
reverse.addEventListener("click", () => {
    reverseTrue = !reverseTrue;
    if (reverseTrue) {
        reverse.innerHTML = "&#8595;"
    }
    else {
        reverse.innerHTML = "&#8593;"
    }
    reverseChildren(parent);
})

function reverseChildren(parent) {
    for (var i = 1; i < parent.childNodes.length; i++) {
        parent.insertBefore(parent.childNodes[i], parent.firstChild);
    }
}