export function SearchEntitiesExtend(entities, searchBar, searctBtn, resetBtn){
    searctBtn.addEventListener('click', () => {
        entities.forEach(elem => {
            elem.parentNode.classList.remove('hide'); 
            let textContent = GetLowerString(elem.innerHTML);
            let request = GetLowerString(searchBar.value);
            if(!textContent.includes(request) && !elem.previousElementSibling.checked)
                elem.parentNode.classList.add('hide');
        })
    });
    resetBtn.addEventListener('click', () => {
        searchBar.value = "";
        entities.forEach(elem => {
            elem.parentNode.classList.remove('hide');
        })
    })    
}

function GetLowerString(str){
    return str.toLowerCase();
}