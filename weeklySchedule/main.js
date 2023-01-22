const breakTask = document.getElementById('break');
const gymTask = document.getElementById('gym');
const studyTask = document.getElementById('study');
const tvTask = document.getElementById('tv');
const friendsTask = document.getElementById('friends');
const workTask = document.getElementById('work');
const deselectBtn = document.getElementById('deselect');
const taskContainer = document.querySelector('.task__container');
const scheduleContainer = document.querySelector('.schedule__container');
const resetBtn = document.querySelector('.deleteBtn');
const addBtn = document.querySelector('.addBtn');
const popUp = document.querySelector('.pop-up__container');
const addPopUp = document.querySelector('.add-pop-up__container');
const okBtn = document.getElementById('btn__ok')
const noBtn = document.getElementById('btn__no');
const yesBtn = document.getElementById('btn__yes');

let selectedColor, active;

//Event Listeners
taskContainer.addEventListener('click', selectTask);
scheduleContainer.addEventListener('click', setColors);
deselectBtn.addEventListener('click', resetTasks);
resetBtn.addEventListener('click',openPopup);
addBtn.addEventListener('click', openAddPopup)
noBtn.addEventListener('click', closePopup);
yesBtn.addEventListener('click', deleteTasks);
okBtn.addEventListener('click', closeAddPopup);

// Tasks click  (3)
function selectTask (e){
    resetTasks()

    let taskColor = e.target.style.backgroundColor;

    switch(e.target.id){
        case 'break':
            activeTask(breakTask, taskColor);
            icon = '<i class="fas fa-star"></i>';
            break
        case 'gym':
            activeTask(gymTask, taskColor);
            icon = '<i class="fas fa-tree"></i>';
            break
        case 'study':
            activeTask(studyTask, taskColor);
            icon = '<i class="fas fa-sleigh"></i>';
            break
        case 'tv':
            activeTask(tvTask, taskColor);
            icon = '<i class="fas fa-snowflake"></i>';
            break
        case 'friends':
            activeTask(friendsTask, taskColor);
            icon = '<i class="fas fa-socks"></i>';
            break
        case 'work':
            activeTask(workTask, taskColor);
            icon = '<i class="fas fa-gift"></i>';
            break
    }
}

// Set colors for schedule (4)
function setColors (e){
    if(e.target.classList.contains('task') && active === true){
        e.target.style.backgroundColor = selectedColor;
        e.target.innerHTML = icon;
    }else if(e.target.classList.contains('fas') && active === true){
        e.target.parentElement.style.backgroundColor = selectedColor;
        e.target.parentElement.innerHTML = icon;
    }
}

// Active task (1)
function activeTask(task, color){
    task.classList.toggle('selected');

    if(task.classList.contains('selected')){
        active = true;
        selectedColor = color;
        return selectedColor;
    } else {
        active = false;
    }
}

// Reset tasks (2)
function resetTasks(){
    const allTasks = document.querySelectorAll('.task__name');

    allTasks.forEach((item)=>{
        item.className = 'task__name';
    })
}

// Delete tasks
function deleteTasks(){
    const tasks = document.querySelectorAll('.task');

    tasks.forEach((item)=>{
        item.innerHTML = '';
        item.style.backgroundColor = 'white';
    })

    closePopup();
}

// Open Pop-up
function openPopup(){
    popUp.style.display = 'flex';
}

// Open Add Pop-up
function openAddPopup(){
    addPopUp.style.display = 'flex';
}

// Close Pop-up
function closePopup(){
    popUp.style.display = 'none';
}

// Close Add Pop-up
function closeAddPopup(){
    $("#add-form")[0].reset();
    addPopUp.style.display = 'none';
}