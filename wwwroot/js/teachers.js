async function fetchTeachers() {
    const response = await fetch('api/teachers');
    const teachers = await response.json();
    const teacherList = document.getElementById('teacherList');
    
    teachers.forEach(teacher => {
        const li = document.createElement('li');
        li.textContent = teacher.name; // Assuming 'name' is a property of Teacher
        teacherList.appendChild(li);
    });
}

document.addEventListener('DOMContentLoaded', fetchTeachers);
