# Security-Features

## ✅ Features Implemented
- Input validation (username, email, password rules).
- SQL Injection protection (parameterized queries).
- XSS protection (HTML encoding).
- Authentication with hashed passwords (BCrypt).
- Authorization with role-based access (RBAC).
- JWT tokens for session management.
- Security-focused unit tests.

## 🛠 Vulnerabilities Found and Fixed
- **SQL Injection** → Fixed by replacing concatenated queries with parameterized queries.
- **XSS** → Fixed by sanitizing user input/output with HtmlEncode.
- **Weak Authentication** → Fixed by hashing passwords with BCrypt + role-based access.
- **Brute-Force Risk** → Mitigated with secure login flow and token expiry.

## 🤖 Role of Copilot
Microsoft Copilot assisted in:
- Generating secure C# code snippets (parameterized queries, JWT).
- Suggesting best practices for password hashing and RBAC.
- Creating test cases to simulate SQL injection and XSS attacks.

## 📂 Project Structure