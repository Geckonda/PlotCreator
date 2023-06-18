const searchBar = document.getElementById('search-bar');
const searchBtn = document.getElementById('search-btn');
const resetBtn = document.getElementById('reset-btn');

const Idea = document.querySelectorAll('.idea-name');
const IdeaCards = document.querySelectorAll('.ideaContainer');

const noIdea = document.getElementById("noIdea");


searchBtn.addEventListener('click', () => {
    var oneIsVisible = true;
    Idea.forEach(elem => {
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
        noIdea.classList.remove('hide')
    }
});
resetBtn.addEventListener('click', () => {
    searchBar.value = "";
    Idea.forEach(elem => {
        elem.parentElement.classList.remove('hide');
    })
    noIdea.classList.add('hide')
})

function GetLowerString(str){
    return str.toLowerCase();
}