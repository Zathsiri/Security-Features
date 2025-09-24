using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using BCrypt.Net;

namespace SafeVault.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly string _connectionString = "Server=localhost;Database=safevault;User=root;Password=yourpassword;";

        // Registration
        [HttpPost("register")]
        public IActionResult Register(string username, string email, string password)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            using var conn = new MySqlConnection(_connectionString);
            conn.Open();
            string query = "INSERT INTO Users (Username, Email, PasswordHash) VALUES (@username, @email, @password)";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@password", hashedPassword);

            try
            {
                cmd.ExecuteNonQuery();
                return Ok("User registered successfully.");
            }
            catch
            {
                return BadRequest("Error: Username or email already exists.");
            }
        }

        // Login (with JWT)
        [HttpPost("login")]
        public IActionResult Login(string username, string password)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();
            string query = "SELECT PasswordHash, Role FROM Users WHERE Username=@username";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@username", username);
            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                string storedHash = reader.GetString("PasswordHash");
                string role = reader.GetString("Role");

                if (BCrypt.Net.BCrypt.Verify(password, storedHash))
                {
                    string token = JwtHelper.GenerateJwtToken(username, role);
                    return Ok(new { Token = token });
                }
            }

            return Unauthorized("Invalid credentials.");
        }

        // Example role-protected route
        [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("admin-dashboard")]
        public IActionResult AdminDashboard()
        {
            return Ok("Welcome to the Admin Dashboard.");
        }
    }
}