document.addEventListener("DOMContentLoaded", () => {
    const btnStudents = document.getElementById("btnStudents");
    const btnTeachers = document.getElementById("btnTeachers");
    const studentsSection = document.getElementById("studentsSection");
    const teachersSection = document.getElementById("teachersSection");
    const detailsSection = document.getElementById("detailsSection");
    const detailsContent = document.getElementById("detailsContent");
    const closeDetailsBtn = document.getElementById("closeDetailsBtn");
    const logoutBtn = document.getElementById("logoutBtn");

    const studentsTableBody = document.querySelector("#studentsTable tbody");
    const teachersTableBody = document.querySelector("#teachersTable tbody");

    // הצגה/הסתרה של חלקים בדף לפי לחיצה על כפתורים
    btnStudents.addEventListener("click", () => {
        btnStudents.classList.add("active");
        btnTeachers.classList.remove("active");
        studentsSection.style.display = "block";
        teachersSection.style.display = "none";
        detailsSection.style.display = "none";
        loadStudents();
    });

    btnTeachers.addEventListener("click", () => {
        btnTeachers.classList.add("active");
        btnStudents.classList.remove("active");
        teachersSection.style.display = "block";
        studentsSection.style.display = "none";
        detailsSection.style.display = "none";
        loadTeachers();
    });

    closeDetailsBtn.addEventListener("click", () => {
        detailsSection.style.display = "none";
    });

    logoutBtn.addEventListener("click", () => {
        document.cookie = "AuthToken=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";
        window.location.href = "/login.html";
    });

    // טוען את רשימת התלמידים מהשרת
    async function loadStudents() {
        try {
            const response = await fetch("/api/student", {
             credentials: "include",
            method: 'GET',
            headers: {
                'Content-Type': 'application/json'
            } });

            if (!response.ok) {
                alert("שגיאה בטעינת תלמידים");
                return;
            }
            const students = await response.json();
            
            renderStudentsTable(students);
        } catch (err) {
            alert("שגיאה בשרת");
            console.error(err);
        }
    }

    // טוען את רשימת המורים מהשרתV
    async function loadTeachers() {
        try {
            const response = await fetch("/api/Teachers", {
            credentials: "include",
            method: 'GET',
            headers: {
                'Content-Type': 'application/json'
            }});
            if (!response.ok) {
                alert("שגיאה בטעינת מורים");
                return;
            }
            const teachers = await response.json();
            renderTeachersTable(teachers);
        } catch (err) {
            alert("שגיאה בשרת");
            console.error(err);
        }
    }

    // הצגת תלמידים בטבלה
    function renderStudentsTable(students) {
        studentsTableBody.innerHTML = "";
        if (!students.length) {
            studentsTableBody.innerHTML = `<tr><td colspan="4">אין תלמידים במערכת</td></tr>`;
            return;
        }
        students.forEach(student => {
            const tr = document.createElement("tr");

            const nameTd = document.createElement("td");
            nameTd.textContent = student.name;

            const classTd = document.createElement("td");
            classTd.textContent = student.clas || "-";

            const detailsTd = document.createElement("td");
            const detailsBtn = document.createElement("button");
            detailsBtn.textContent = "פרטים";
            detailsBtn.addEventListener("click", () => {
                showDetails("student", student);
            });
            detailsTd.appendChild(detailsBtn);

            const editTd = document.createElement("td");
            const editBtn = document.createElement("button");
            editBtn.textContent = "ערוך";
            editBtn.addEventListener("click", () => {
                showEditForm("student", student);
            });
            editTd.appendChild(editBtn);

            tr.appendChild(nameTd);
            tr.appendChild(classTd);
            tr.appendChild(detailsTd);
            tr.appendChild(editTd);

            studentsTableBody.appendChild(tr);
        });
    }

    // הצגת מורים בטבלהV
    function renderTeachersTable(teachers) {
        teachersTableBody.innerHTML = "";
        if (!teachers.length) {
            teachersTableBody.innerHTML = `<tr><td colspan="5">אין מורים במערכת</td></tr>`;
            return;
        }
        teachers.forEach(teacher => {
            
            const tr = document.createElement("tr");

            const nameTd = document.createElement("td");
            nameTd.innerHTML = teacher.name;

            const subjectTd = document.createElement("td");
            subjectTd.innerHTML = teacher.subject || "-";

            const classesTd = document.createElement("td");
            classesTd.textContent = (teacher.clases && teacher.clases.length > 0) ? teacher.clases.join(", ") : "-";

            const detailsTd = document.createElement("td");
            const detailsBtn = document.createElement("button");
            detailsBtn.textContent = "פרטים";
            detailsBtn.addEventListener("click", () => {
                showDetails("teacher", teacher);
            });
            detailsTd.appendChild(detailsBtn);

            const editTd = document.createElement("td");
            const editBtn = document.createElement("button");
            editBtn.textContent = "ערוך";
            editBtn.addEventListener("click", () => {
                showEditForm("teacher", teacher);
            });
            editTd.appendChild(editBtn);

            tr.appendChild(nameTd);
            tr.appendChild(subjectTd);
            tr.appendChild(classesTd);
            tr.appendChild(detailsTd);
            tr.appendChild(editTd);

            teachersTableBody.appendChild(tr);
        });
    }

    // הצגת פרטים לקריאה בלבד
    function showDetails(type, data) {
        detailsContent.innerHTML = "";

        if (type === "student") {
            detailsContent.innerHTML = `
                <p><strong>מזהה:</strong> ${data.id}</p>
                <p><strong>שם:</strong> ${data.name}</p>
                <p><strong>כיתה:</strong> ${data.clas || "-"}</p>
                <p><strong>נוכחות:</strong> ${data.presence ? data.presence.length : 0}</p>
            `;
        } else if (type === "teacher") {
            detailsContent.innerHTML = `
                <p><strong>מזהה:</strong> ${data.id}</p>
                <p><strong>שם:</strong> ${data.name}</p>
                <p><strong>מקצוע:</strong> ${data.subject || "-"}</p>
                <p><strong>כיתות:</strong> ${(data.clases && data.clases.length > 0) ? data.clases.join(", ") : "-"}</p>
            `;
        }
        detailsSection.style.display = "block";
    }

    // הצגת טופס עריכה בסיסי 
    function showEditForm(type, data) {
        detailsContent.innerHTML = `
            <form id="editForm">
                <label>שם:
                    <input type="text" name="Name" value="${data.name}" required />
                </label><br/>
                ${type === "student" ? `
                    <label>כיתה:
                        <input type="text" name="Clas" value="${data.class || ''}" />
                    </label><br/>
                ` : ''}
                ${type === "teacher" ? `
                    <label>מקצוע:
                        <input type="text" name="Subject" value="${data.subject || ''}" />
                    </label><br/>
                    <label>כיתות (מופרדות בפסיקים):
                        <input type="text" name="Clases" value="${(data.clases && data.clases.join(", ")) || ''}" />
                    </label><br/>
                ` : ''}
                <button type="submit">שמור</button>
            </form>
        `;
        detailsSection.style.display = "block";

        const form = document.getElementById("editForm");
        form.addEventListener("submit", async (e) => {
            e.preventDefault();
            const formData = new FormData(form);
            let updatedData = {};
            formData.forEach((value, key) => {
                updatedData[key] = value;
            });

            // המרה למערך עבור כיתות (למורה)
            if (type === "teacher" && updatedData.Clases) {
                updatedData.Clases = updatedData.Clases.split(",").map(c => c.trim());
            }
            
            // קריאה לעדכון בשרת
            await updateData(type, data.id, updatedData);
        });
    }

    // פונקציה לעדכון פרטי תלמיד/מורה בשרת
    async function updateData(type, id, updatedData) {
        
        const typeLower = type == "student" ? "Student" : "Teachers";
        try {
            const response = await fetch(`/api/${typeLower}/${id}`, {
                method: "PUT",
                credentials: "include",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(updatedData)
            });

            if (!response.ok) {
                alert("שגיאה בעדכון הנתונים");
                return;
            }

            alert("הנתונים עודכנו בהצלחה");
            detailsSection.style.display = "none";

            // ריענון טבלה אחרי עדכון
            if (type === "student") loadStudents();
            else loadTeachers();

        } catch (err) {
            alert("שגיאה בשרת");
            console.error(err);
        }
    }

    // התחלה - נטען את רשימת התלמידים כברירת מחדל
    btnStudents.click();

    students =  loadStudents();
    teachers = loadTeachers();
    
    if (!teacherData){
        console.log("not teacer data!!");
        
         return;
    }
});
