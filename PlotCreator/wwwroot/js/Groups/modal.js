var modalInfo = document.getElementById('modelDescription')
modalInfo.addEventListener('show.bs.modal', function (event) {
    //Кнопка с группой
    var button = event.relatedTarget

    //Извлечение значений атрибутов кнопки
    var groupId = button.getAttribute('data-bs-groupId')
    var groupName = button.getAttribute('data-bs-name')
    var groupDescription = button.getAttribute('data-bs-description')

    var modalBodyText = modalInfo.querySelector('#descriptionText')

    modalBodyText.textContent = groupDescription

    //Кнопка "Редактировать"
    const updateButton = document.querySelector('#updateButton')
    //Создание атрибутов
    const idAttribute = document.createAttribute("data-bs-groupId");
    const nameAttribute = document.createAttribute("data-bs-name");
    const descriptionAttribute = document.createAttribute("data-bs-description");

    //Присвоение атриубтам значений
    idAttribute.value = groupId
    nameAttribute.value = groupName
    descriptionAttribute.value = groupDescription

    //Назначение атрибута кнопке
    updateButton.setAttributeNode(idAttribute)
    updateButton.setAttributeNode(nameAttribute)
    updateButton.setAttributeNode(descriptionAttribute)
    //------------------------------------------------------------------


    //Кнопка "Удалить"
    const deleteButton = document.querySelector('#deleteButton')
    //Получение значения ссылки
    var href = deleteButton.getAttribute('href')
    //Формирование конечной ссылки
    deleteButton.href = href + '&id=' + groupId
    //------------------------------------------------------------------

});

//Обновление группы
const modalUpdate = document.getElementById('modelUpdate');
modalUpdate.addEventListener('show.bs.modal', function (event) {

    const editFormIdField = document.getElementById('editFormIdField');
    const editFormNameField = document.getElementById('editFormNameField');
    const editFormDescriptionField = document.getElementById('editFormDescriptionField');

    var button = event.relatedTarget

    //Извлечение значений атрибутов кнопки
    var groupId = button.getAttribute('data-bs-groupId')
    var groupName = button.getAttribute('data-bs-name')
    var groupDescription = button.getAttribute('data-bs-description')

    editFormIdField.value = groupId
    editFormNameField.value = groupName
    editFormDescriptionField.innerHTML = groupDescription
});