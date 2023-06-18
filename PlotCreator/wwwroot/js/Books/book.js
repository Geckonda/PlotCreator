const searchBar = document.getElementById('search-bar');
const searchBtn = document.getElementById('search-btn');
const resetBtn = document.getElementById('reset-btn');

const books = document.querySelectorAll('.book-name');
const characterCards = document.querySelectorAll('.card');

const noBooks = document.getElementById("noBooks");


searchBtn.addEventListener('click', () => {
    var oneIsVisible = true;
    books.forEach(elem => {
        elem.parentElement.parentElement.parentElement.classList.remove('hide');
        let textContent = GetLowerString(elem.innerHTML);
        let request = GetLowerString(searchBar.value);
        if(!textContent.includes(request)){
            elem.parentElement.parentElement.parentElement.classList.add('hide');
        }
        else{
            oneIsVisible = false;
        }
    })
    if(oneIsVisible){
        noBooks.classList.remove('hide')
    }
});
resetBtn.addEventListener('click', () => {
    searchBar.value = "";
    books.forEach(elem => {
        elem.parentElement.parentElement.parentElement.classList.remove('hide');
    })
    noBooks.classList.add('hide')
})

function GetLowerString(str){
    return str.toLowerCase();
}