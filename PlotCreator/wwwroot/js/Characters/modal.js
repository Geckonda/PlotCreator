//Модальное окно групп -----------------------------

var modalInfo = document.getElementById('modelDescription')
modalInfo.addEventListener('show.bs.modal', function (event) {
    //Кнопка с группой
    var button = event.relatedTarget

    //Извлечение значений атрибутов кнопки
    var groupId = button.getAttribute('data-bs-groupId')
    var groupName = button.getAttribute('data-bs-name')
    var groupDescription = button.getAttribute('data-bs-description')

    var modalBodyText = modalInfo.querySelector('#descriptionText')
    var modalTitle = modalInfo.querySelector('#nameGroupText')

    modalBodyText.textContent = groupDescription
    modalTitle.textContent = groupName
});

//--------------------------------------------------!