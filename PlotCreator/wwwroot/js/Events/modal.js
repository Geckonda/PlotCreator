import { SearchEntitiesExtend } from "../searchExtend.js"
import { SearchEntities } from "../search.js"

//import { ResetSearchExtend } from "../searchExtend.js"
//import { ResetSearch } from "../search.js"

const searchBar = document.querySelectorAll('.search-bar');
const searchBtn = document.querySelectorAll('.search-btn');
const resetBtn = document.querySelectorAll('.reset-btn');
const groups = document.querySelectorAll('.group-character');
const closeBTN = document.querySelectorAll('.btn-close');

for (var i = 0; i < searchBar.length; i++) {
    SearchEntitiesExtend(groups, searchBar[i], searchBtn[i], resetBtn[i]);
    //ResetSearchExtend(closeBTN, searchBar[i], groups)
}


const searchBarDim = document.querySelectorAll('.search-bar-dim');
const searchBtnDim = document.querySelectorAll('.search-btn-dim');
const resetBtnDim = document.querySelectorAll('.reset-btn-dim');
const groupBTN = document.querySelectorAll('.group-btn');
for (var i = 0; i < searchBarDim.length; i++) {
    SearchEntities(groupBTN, searchBarDim[i], searchBtnDim[i], resetBtnDim[i]);
    //ResetSearch(closeBTN, searchBarDim[i], groupBTN)
}


