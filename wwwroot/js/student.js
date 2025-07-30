// async function fetchStudentData() {
//     const token = document.cookie.split('; ').find(row => row.startsWith('AuthToken='));
//     console.log("token", token);
    
//     try {
//         const response = await fetch(`/api/Student/me`, {
//             method: 'GET',
//             credentials: 'include', // חשוב כדי לשלוח את ה-cookie
//             headers: {
//                 'Content-Type': 'application/json'
//             }
//         });

//         if (!response.ok) {
//             alert("שגיאה בקבלת נתוני התלמיד");
//             return null;
//         }

//         const student = await response.json();
//         return student;

//     } catch (error) {
//         alert("שגיאה בשרת");
//         console.error(error);
//         return null;
//     }
// }
// async function main() {
//     const student = await fetchStudentData();
//     if (!student) return;
//     console.log(student.name);
// }

// main();
// console.log(student.name);

// //    const teacherNameSpan = document.getElementById("teacherName");
//  const studentName = document.getElementById("studentName");

//  studentName.textContent = student.name;
// פונקציה לקבלת נתוני התלמיד הנוכחי
async function fetchStudentData() {
    const token = document.cookie.split('; ').find(row => row.startsWith('AuthToken='));
    console.log("token", token);
    
    try {
        const response = await fetch(`/api/Student/me`, {
            method: 'GET',
            credentials: 'include',
            headers: {
                'Content-Type': 'application/json'
            }
        });

        if (!response.ok) {
            if (response.status === 401) {
                alert("אין הרשאה - אנא התחבר מחדש");
                window.location.href = '/login.html';
                return null;
            }
            throw new Error(`שגיאה בקבלת נתוני התלמיד: ${response.status}`);
        }

        const student = await response.json();
        return student;

    } catch (error) {
        console.error("Error fetching student data:", error);
        throw error;
    }
}

// פונקציה להצגת פרטי תלמיד
function displayStudentInfo(student) {
    const studentNameElement = document.getElementById("studentName");
    const studentClassElement = document.getElementById("studentClass");
    
    if (studentNameElement && student.name) {
        studentNameElement.textContent = student.name;
    }
    
    if (studentClassElement && student.clas) {
        studentClassElement.textContent = `כיתה: ${student.clas}`;
    }
}

// פונקציה ליצירת סטטיסטיקות מהירות
function createStatsCards(presenceData) {
    const statsContainer = document.getElementById("statsContainer");
    if (!statsContainer) return;

    if (!presenceData || presenceData.length === 0) {
        statsContainer.innerHTML = `
            <div class="stats-card">
                <h4>0</h4>
                <p>אין נתוני נוכחות</p>
            </div>
        `;
        return;
    }

    const totalLessons = presenceData.length;
    const presentLessons = presenceData.filter(p => p.isPresent).length;
    const absentLessons = totalLessons - presentLessons;
    const attendancePercentage = totalLessons > 0 ? Math.round((presentLessons / totalLessons) * 100) : 0;

    statsContainer.innerHTML = `
        <div class="stats-card">
            <h4>${totalLessons}</h4>
            <p>סה"כ שיעורים</p>
        </div>
        <div class="stats-card">
            <h4>${presentLessons}</h4>
            <p>נוכחויות</p>
        </div>
        <div class="stats-card">
            <h4>${absentLessons}</h4>
            <p>היעדרויות</p>
        </div>
        <div class="stats-card">
            <h4>${attendancePercentage}%</h4>
            <p>אחוז נוכחות</p>
        </div>
    `;
}

// פונקציה לעיבוד נתוני הנוכחות וחלוקה לפי מקצועות
function processPresenceData(presenceArray) {
    if (!presenceArray || presenceArray.length === 0) {
        return {};
    }

    const presenceBySubject = {};
    
    presenceArray.forEach(presence => {
        const subject = presence.lesson || "כללי";
        
        if (!presenceBySubject[subject]) {
            presenceBySubject[subject] = [];
        }
        
        presenceBySubject[subject].push({
            date: presence.date,
            lesson: presence.lesson,
            isPresent: presence.isPresent
        });
    });

    // מיון התאריכים בסדר יורד (החדש ביותר קודם)
    Object.keys(presenceBySubject).forEach(subject => {
        presenceBySubject[subject].sort((a, b) => new Date(b.date) - new Date(a.date));
    });

    return presenceBySubject;
}

// פונקציה לעיצוב תאריך בעברית
function formatDate(dateString) {
    const date = new Date(dateString);
    return date.toLocaleDateString('he-IL', {
        year: 'numeric',
        month: '2-digit',
        day: '2-digit'
    });
}

// פונקציה ליצירת טבלת נוכחות
function createPresenceTable(presenceBySubject) {
    const tableBody = document.querySelector("#presenceTable tbody");
    if (!tableBody) return;

    // ניקוי הטבלה הקיימת
    tableBody.innerHTML = '';

    if (Object.keys(presenceBySubject).length === 0) {
        const row = tableBody.insertRow();
        const cell = row.insertCell(0);
        cell.colSpan = 3;
        cell.textContent = "אין נתוני נוכחות זמינים";
        cell.style.textAlign = "center";
        cell.style.fontStyle = "italic";
        cell.style.color = "#666";
        cell.style.padding = "20px";
        return;
    }

    Object.keys(presenceBySubject).forEach(subject => {
        // הוספת כותרת מקצוע עם העיצוב החדש
        const headerRow = tableBody.insertRow();
        const headerCell = headerRow.insertCell(0);
        headerCell.colSpan = 3;
        headerCell.textContent = subject;
        headerCell.style.backgroundColor = "#a8c09a";
        headerCell.style.color = "#2d4a28";
        headerCell.style.fontWeight = "600";
        headerCell.style.fontSize = "1.1rem";
        headerCell.style.fontFamily = "Arial, sans-serif";
        headerCell.style.textAlign = "center";
        headerCell.style.padding = "12px";
        headerCell.style.borderBottom = "2px solid #8fb382";

        // הוספת שורות נוכחות לכל מקצוע
        presenceBySubject[subject].forEach(presence => {
            const row = tableBody.insertRow();
            
            // תאריך
            const dateCell = row.insertCell(0);
            dateCell.textContent = formatDate(presence.date);
            dateCell.style.fontFamily = "'Courier New', monospace";
            dateCell.style.fontWeight = "500";
            
            // שיעור
            const lessonCell = row.insertCell(1);
            lessonCell.textContent = presence.lesson || "לא צוין";
            
            // נוכחות
            const presenceCell = row.insertCell(2);
            presenceCell.textContent = presence.isPresent ? "נוכח" : "נעדר";
            presenceCell.style.fontWeight = "bold";
            
            // עיצוב שורה עם הצבעים החדשים
            if (presence.isPresent) {
                presenceCell.style.color = "#4a7c59";
                row.style.backgroundColor = "#f0f8f0";
                row.classList.add("present");
            } else {
                presenceCell.style.color = "#c08552";
                row.style.backgroundColor = "#fdf5f0";
                row.classList.add("absent");
            }
            
            // הוספת אפקט hover
            row.addEventListener('mouseenter', function() {
                const originalBg = this.style.backgroundColor;
                this.style.backgroundColor = "#f5f5f5";
                this.setAttribute('data-original-bg', originalBg);
            });
            
            row.addEventListener('mouseleave', function() {
                const originalBg = this.getAttribute('data-original-bg');
                this.style.backgroundColor = originalBg;
            });
        });
    });
}

// פונקציה להתנתקות
function setupLogout() {
    const logoutBtn = document.getElementById("logoutBtn");
    if (logoutBtn) {
        logoutBtn.addEventListener("click", function() {
            // מחיקת ה-cookie
            document.cookie = "AuthToken=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";
            
            // הפניה לעמוד התחברות
            alert("התנתקת בהצלחה");
            window.location.href = '/login.html';
        });
    }
}

// פונקציה להסתרת הודעת טעינה והצגת התוכן
function hideLoadingAndShowContent() {
    const loadingMessage = document.getElementById("loadingMessage");
    const mainContainer = document.getElementById("mainContainer");
    
    if (loadingMessage) {
        loadingMessage.style.display = "none";
    }
    if (mainContainer) {
        mainContainer.style.display = "block";
    }
}

// פונקציה להצגת שגיאה
function showError(message) {
    const loadingMessage = document.getElementById("loadingMessage");
    const errorContainer = document.getElementById("errorContainer");
    const errorMessage = document.getElementById("errorMessage");
    
    if (loadingMessage) {
        loadingMessage.style.display = "none";
    }
    if (errorContainer) {
        errorContainer.style.display = "block";
    }
    if (errorMessage) {
        errorMessage.textContent = message;
    }
}

// פונקציה ראשית
async function main() {
    try {
        // קבלת נתוני התלמיד
        const student = await fetchStudentData();
        if (!student) {
            showError("לא ניתן לקבל נתוני תלמיד. אנא בדוק את החיבור לשרת.");
            return;
        }

        console.log("Student data:", student);

        // הצגת פרטי התלמיד
        displayStudentInfo(student);

        // יצירת סטטיסטיקות
        createStatsCards(student.presence);

        // עיבוד והצגת נתוני נוכחות
        const presenceBySubject = processPresenceData(student.presence);
        createPresenceTable(presenceBySubject);

        // הגדרת כפתור התנתקות
        setupLogout();

        // הסתרת הודעת הטעינה והצגת התוכן
        hideLoadingAndShowContent();

    } catch (error) {
        console.error("שגיאה בטעינת העמוד:", error);
        showError("שגיאה בטעינת נתוני התלמיד. אנא נסה שוב מאוחר יותר.");
    }
}

// הפעלת הפונקציה הראשית כשהעמוד נטען
document.addEventListener('DOMContentLoaded', main);