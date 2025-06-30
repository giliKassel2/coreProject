async function fetchStudents() {
    const response = await fetch('api/students');
    const students = await response.json();
    const studentList = document.getElementById('studentList');
    
    students.forEach(student => {
        const li = document.createElement('li');
        li.textContent = student.name; // Assuming 'name' is a property of Student
        studentList.appendChild(li);
    });
}

document.addEventListener('DOMContentLoaded', fetchStudents);
