document.addEventListener("DOMContentLoaded", async () => {
    const teacherNameSpan = document.getElementById("teacherName");
    const teacherSubjectSpan = document.getElementById("teacherSubject");
    const classesButtonsDiv = document.getElementById("classesButtons");
    const studentsSection = document.getElementById("studentsSection");
    const selectedClassSpan = document.getElementById("selectedClass");
    const studentsTableBody = document.querySelector("#studentsTable tbody");
    const logoutBtn = document.getElementById("logoutBtn");

    let teacherData = null;
    let currentClass = null;
    let studentsInClass = [];

    // פונקציה לשליפת פרטי המורה כולל כיתות
    async function fetchTeacherData() {
        try {
            const response = await fetch("/api/teachers/me", { credentials: "include" });
            if (!response.ok) {
                alert("שגיאה בקבלת פרטי המורה");
                window.location.href = "/login.html";
                return null;
            }
            return await response.json();
        } catch (err) {
            alert("שגיאה בשרת");
            console.error(err);
            return null;
        }
    }

    // פונקציה לקבלת תלמידים לפי כיתה
    async function fetchStudentsByClass(className) {
        try {
            const response = await fetch(`/api/student?class=${encodeURIComponent(className)}`, { credentials: "include" });
            if (!response.ok) {
                alert("שגיאה בקבלת תלמידים");
                return [];
            }
            return await response.json();
        } catch (err) {
            alert("שגיאה בשרת");
            console.error(err);
            return [];
        }
    }

    // רינדור כפתורי הכיתות
    function renderClassesButtons(classes) {
        classesButtonsDiv.innerHTML = "";
        classes.forEach(cls => {
            const btn = document.createElement("button");
            btn.textContent = cls;
            btn.addEventListener("click", async () => {
                currentClass = cls;
                setActiveClassButton(btn);
                selectedClassSpan.textContent = cls;
                studentsInClass = await fetchStudentsByClass(cls);
                renderStudentsTable(studentsInClass);
                studentsSection.style.display = "block";
            });
            classesButtonsDiv.appendChild(btn);
        });
    }

    // סימון הכיתה הפעילה
    function setActiveClassButton(activeBtn) {
        Array.from(classesButtonsDiv.children).forEach(btn => btn.classList.remove("active"));
        activeBtn.classList.add("active");
    }

    // רינדור טבלת תלמידים
    function renderStudentsTable(students) {
        studentsTableBody.innerHTML = "";
        if (!students.length) {
            studentsTableBody.innerHTML = `<tr><td colspan="3">אין תלמידים בכיתה זו</td></tr>`;
            return;
        }

        students.forEach(student => {
            const tr = document.createElement("tr");

            const nameTd = document.createElement("td");
            nameTd.textContent = student.Name;

            const lastPresence = getLastPresence(student.presence);
            const presenceTd = document.createElement("td");
            presenceTd.textContent = lastPresence ? 
                `${lastPresence.Date.split("T")[0]} - ${lastPresence.IsPresent ? "נכח" : "העדר"}` : "אין נתונים";

            const actionTd = document.createElement("td");
            const presenceBtn = document.createElement("button");
            presenceBtn.textContent = "סמן נוכחות";
            presenceBtn.classList.toggle("absent", false);
            presenceBtn.addEventListener("click", () => {
                togglePresence(student);
            });

            actionTd.appendChild(presenceBtn);

            tr.appendChild(nameTd);
            tr.appendChild(presenceTd);
            tr.appendChild(actionTd);

            studentsTableBody.appendChild(tr);
        });
    }

    // פונקציה לקבלת הנוכחות האחרונה
    function getLastPresence(presences) {
        if (!presences || presences.length === 0) return null;
        // להניח שמסודר לפי תאריך? אם לא, מיון...
        const sorted = presences.slice().sort((a,b) => new Date(b.Date) - new Date(a.Date));
        return sorted[0];
    }

    // פונקציה להוספת/עדכון נוכחות עבור תלמיד
    async function togglePresence(student) {
        // סימולציה של שינוי נוכחות היום
        const today = new Date().toISOString().split("T")[0];
        const lastPresence = getLastPresence(student.presence);

        // נניח שאנחנו מסמנים נוכחות חדשה
        const newPresence = {
            Date: today,
            Lesson: "שיעור רגיל",
            IsPresent: true
        };

        // אם הנוכחות האחרונה היא היום והייתה נוכחות, נשנה להיעדר, ולהפך (דוגמה פשוטה)
        if(lastPresence && lastPresence.Date.startsWith(today) && lastPresence.IsPresent) {
            newPresence.IsPresent = false;
        }

        // כאן יש לקרוא ל-API לעדכון נוכחות - נניח endpoint: /api/student/{id}/presence
        try {
            const response = await fetch(`/api/student/${student.Id}/presence`, {
                method: "POST",
                credentials: "include",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(newPresence)
            });

            if (!response.ok) {
                alert("שגיאה בעדכון נוכחות");
                return;
            }

            // לאחר עדכון, נטען מחדש את רשימת התלמידים עם הנתונים החדשים
            studentsInClass = await fetchStudentsByClass(currentClass);
            renderStudentsTable(studentsInClass);

        } catch (err) {
            alert("שגיאה בשרת");
            console.error(err);
        }
    }

    // Logout
    logoutBtn.addEventListener("click", () => {
        document.cookie = "AuthToken=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";
        window.location.href = "/login.html";
    });

    // הפעלה ראשונית
    teacherData = await fetchTeacherData();
    if (!teacherData) return;

    teacherNameSpan.textContent = teacherData.Name;
    teacherSubjectSpan.textContent = teacherData.Subject;

    renderClassesButtons(teacherData.Clases);
});
