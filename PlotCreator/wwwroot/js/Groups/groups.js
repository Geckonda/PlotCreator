import {SearchEntities} from  "../search.js"
const searchBar = document.getElementById('search-bar');
const searchBtn = document.getElementById('search-btn');
const resetBtn = document.getElementById('reset-btn');

const groups = document.querySelectorAll('.group-btn');

SearchEntities(groups, searchBar, searchBtn, resetBtn);