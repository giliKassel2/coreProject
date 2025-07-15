document.getElementById("loginForm").addEventListener("submit", async (e) => {
    e.preventDefault();

    const userId = document.getElementById("userId").value.trim();
    const password = document.getElementById("password").value.trim();
    const errorDiv = document.getElementById("error");
    errorDiv.textContent = "";

    if (!userId || !password) {
        errorDiv.textContent = "נא למלא את כל השדות";
        return;
    }

    try {
        const response = await fetch('/api/login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({userId: userId, password: password })
        });
        if (response.status === 401) {
            errorDiv.textContent = "שם משתמש או סיסמה לא נכונים";
            return;
        }
        if (response.status === 403) {
            errorDiv.textContent = "אין לך הרשאות להתחבר";
            return;
        }
        if (response.status === 500) {
            errorDiv.textContent = "שגיאה בשרת, נסה שוב מאוחר יותר";
            return;
        }
        if (!response.ok) {
            errorDiv.textContent = "שם משתמש או סיסמה לא נכונים";
            return;
        }

        const data = await response.json();

        if (data.success) {
            switch (data.type) {
                case 0:
                    window.location.href ='/principal.html'
                    
                    break;
                case 1:
                    window.location.href ='/teacher.html'
                    
                    break;
                case 2:
                    window.location.href ='/student.html'
                    break;
                default:
                    window.location.href ='login.html'
                    break;
            }

            // לפי התפקיד redirect לעמוד המתאים
            // כאן נשמור את הטוקן אם תרצי (אבל יש cookie HTTP-only שמנוהל בשרת)

            // לדוגמה: redirect לפי סוג משתמש
            // את זה אפשר לשפר בהמשך כשנשיג את סוג המשתמש
    
        } else {
            errorDiv.textContent = "אירעה שגיאה בהתחברות";
        }
    } catch (error) {
        errorDiv.textContent = "שגיאה בשרת, נסה שוב מאוחר יותר";
    }
});
