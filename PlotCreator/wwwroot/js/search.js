export function SearchEntities(entities, searchBar, searctBtn, resetBtn){
    searctBtn.addEventListener('click', () => {
        entities.forEach(elem => {
            elem.classList.remove('hide');
            let textContent = GetLowerString(elem.innerHTML);
            let request = GetLowerString(searchBar.value);
            if(!textContent.includes(request))
                elem.classList.add('hide');
        })
    });
    resetBtn.addEventListener('click', () => {
        searchBar.value = "";
        entities.forEach(elem => {
            elem.classList.remove('hide');
        })
    })    
}

function GetLowerString(str){
    return str.toLowerCase();
}