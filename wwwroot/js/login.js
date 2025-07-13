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
            body: JSON.stringify({ UserId: userId, Password: password })
        });

        if (!response.ok) {
            errorDiv.textContent = "שם משתמש או סיסמה לא נכונים";
            return;
        }

        const data = await response.json();

        if (data.success) {
            // לפי התפקיד redirect לעמוד המתאים
            // כאן נשמור את הטוקן אם תרצי (אבל יש cookie HTTP-only שמנוהל בשרת)

            // לדוגמה: redirect לפי סוג משתמש
            // את זה אפשר לשפר בהמשך כשנשיג את סוג המשתמש
            window.location.href = '/index.html'; // בשלב הזה נשלח לדף הראשי - נשנה בהמשך
        } else {
            errorDiv.textContent = "אירעה שגיאה בהתחברות";
        }
    } catch (error) {
        errorDiv.textContent = "שגיאה בשרת, נסה שוב מאוחר יותר";
    }
});
