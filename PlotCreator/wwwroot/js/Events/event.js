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