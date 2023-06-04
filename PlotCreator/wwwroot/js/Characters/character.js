//const GetCharactersButton = document.getElementById("GetCharacters");

//GetCharactersButton.addEventListener("click", getCharacters());

//function getCharacters() {
//	console.log(111);
//	// отправляет запрос и получаем ответ
//	//const response = await fetch("/Characters/GetAllCharacters?userId=2")
//	//	.then(response => response.json())
//	//	.then(data => _displayItems(data));
//	// если запрос прошел нормально
//	//if (response.ok === true) {
//	//	// получаем данные
//	//	const characters = await response.json();
//	//	//const rows = document.querySelector("tbody");
//	//	// добавляем полученные элементы в таблицу  //rows.append(row(characters))
//	//	characters.forEach(character => console.log(Character));
//	//}
//}

const searchBar = document.getElementById('search-bar');
const searchBtn = document.getElementById('search-btn');
const resetBtn = document.getElementById('reset-btn');

const characters = document.querySelectorAll('.character-name');
const characterCards = document.querySelectorAll('.card');


searchBtn.addEventListener('click', () => {
    var oneIsHide = false;
    characters.forEach(elem => {
        elem.parentElement.parentElement.parentElement.classList.remove('hide');
        let textContent = GetLowerString(elem.innerHTML);
        let request = GetLowerString(searchBar.value);
        if(!textContent.includes(request)){
            elem.parentElement.parentElement.parentElement.classList.add('hide');
            oneIsHide = true;
        }
    })
    if(!oneIsHide){
        const message = document.createElement
    }
});
resetBtn.addEventListener('click', () => {
    searchBar.value = "";
    characters.forEach(elem => {
        elem.parentElement.parentElement.parentElement.classList.remove('hide');
    })
})

function GetLowerString(str){
    return str.toLowerCase();
}

