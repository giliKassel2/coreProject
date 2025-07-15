async function fetchStudentData() {
    const token = document.cookie.split('; ').find(row => row.startsWith('AuthToken='));
    console.log("token", token);
    
    try {
        const response = await fetch(`/api/Student/me`, {
            method: 'GET',
            credentials: 'include', // חשוב כדי לשלוח את ה-cookie
            headers: {
                'Content-Type': 'application/json'
            }
        });

        if (!response.ok) {
            alert("שגיאה בקבלת נתוני התלמיד");
            return null;
        }

        const student = await response.json();
        return student;

    } catch (error) {
        alert("שגיאה בשרת");
        console.error(error);
        return null;
    }
}
async function main() {
    const student = await fetchStudentData();
    if (!student) return;
    console.log(student.name);
}

main();
console.log(student.name);

//    const teacherNameSpan = document.getElementById("teacherName");
 const studentName = document.getElementById("studentName");

 studentName.textContent = student.name;
