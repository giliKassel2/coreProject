async function addStudent() {
    const name = document.getElementById('studentName').value;
    const response = await fetch('/api/student', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ name: name })
    });
    
    const result = await response.json();
    document.getElementById('output').innerHTML = `תלמיד חדש נוסף: ${result.name}`;
}

async function addTeacher() {
    const name = document.getElementById('teacherName').value;
    const response = await fetch('/api/teachers', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ name: name })
    });
    
    const result = await response.json();
    document.getElementById('output').innerHTML = `מורה חדש נוסף: ${result.name}`;
}
