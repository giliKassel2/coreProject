async function fetchStudentData() {
    try {
        const response = await fetch(`/api/student/me`, {
            credentials: 'include'  // שולח קוקיז (כולל HttpOnly)
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
