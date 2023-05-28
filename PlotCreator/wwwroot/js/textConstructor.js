const responseTextBlock = document.getElementById('response-text-block');
const mainTextBlock = document.getElementById('main-text-block');

MakeParagraphs(responseTextBlock.innerHTML);


function MakeParagraphs(fullText){
    if(fullText.includes("\n")){
        do{
            let indexOfTab = fullText.indexOf('\n');
            const pElement = document.createElement('p');
            pElement.innerHTML = fullText.slice(0, indexOfTab);
            mainTextBlock.appendChild(pElement);
            mainTextBlock.className = 'paragraph fs-4';
            fullText = fullText.slice(indexOfTab+1);
        }while(fullText.includes("\n"));
    }
    const pElement = document.createElement('p');
    pElement.innerHTML = fullText;
    mainTextBlock.appendChild(pElement);
    mainTextBlock.className = 'paragraph fs-4';
}
