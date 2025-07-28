// Main script for index page
document.addEventListener('DOMContentLoaded', function() {
    loadTeachers();
    loadStudents();
});

async function loadTeachers() {
    try {
        const response = await fetch('/api/teachers');
        if (response.ok) {
            const teachers = await response.json();
            displayTeachers(teachers);
        } else {
            console.error('Failed to load teachers');
            displayError('teacherList', 'שגיאה בטעינת רשימת המורים');
        }
    } catch (error) {
        console.error('Error loading teachers:', error);
        displayError('teacherList', 'שגיאה בחיבור לשרת');
    }
}

async function loadStudents() {
    try {
        const response = await fetch('/api/students');
        if (response.ok) {
            const students = await response.json();
            displayStudents(students);
        } else {
            console.error('Failed to load students');
            displayError('studentList', 'שגיאה בטעינת רשימת התלמידים');
        }
    } catch (error) {
        console.error('Error loading students:', error);
        displayError('studentList', 'שגיאה בחיבור לשרת');
    }
}

function displayTeachers(teachers) {
    const teacherList = document.getElementById('teacherList');
    teacherList.innerHTML = '';
    
    if (teachers && teachers.length > 0) {
        teachers.forEach(teacher => {
            const li = document.createElement('li');
            li.innerHTML = `
                <strong>${teacher.name || 'ללא שם'}</strong><br>
                <small>מקצוע: ${teacher.subject || 'לא צוין'}</small>
            `;
            teacherList.appendChild(li);
        });
    } else {
        displayError('teacherList', 'אין מורים רשומים במערכת');
    }
}

function displayStudents(students) {
    const studentList = document.getElementById('studentList');
    studentList.innerHTML = '';
    
    if (students && students.length > 0) {
        students.forEach(student => {
            const li = document.createElement('li');
            li.innerHTML = `
                <strong>${student.name || 'ללא שם'}</strong><br>
                <small>כיתה: ${student.className || 'לא צוינה'}</small>
            `;
            studentList.appendChild(li);
        });
    } else {
        displayError('studentList', 'אין תלמידים רשומים במערכת');
    }
}

function displayError(elementId, message) {
    const element = document.getElementById(elementId);
    element.innerHTML = `<li style="color: #e74c3c; text-align: center;">${message}</li>`;
}

// Add loading animation
function showLoading(elementId) {
    const element = document.getElementById(elementId);
    element.innerHTML = '<li style="text-align: center;">טוען...</li>';
}

// Show loading while fetching data
document.addEventListener('DOMContentLoaded', function() {
    showLoading('teacherList');
    showLoading('studentList');
});