using System.Net;
using System.Data;
using System.Diagnostics;
using LinnenWebApp_v1.Models;
using Microsoft.Data.SqlClient;
using System.Text;

namespace LinnenWebApp_v1.Controllers.utilities;

public class LoginUserCookieChecker
{
    private string connectionString;
    public LoginUserCookieChecker(string connectionString)
    {
        this.connectionString = connectionString;
    }
    public User CheckLoginUserViaCookie(HttpContext httpContext)
    {
        User user = new();
        string encodedUsername;
        string decodedUsername;
        if (httpContext.Request.Cookies.TryGetValue("Username", out encodedUsername))
        {
            decodedUsername = Encoding.UTF8.GetString(Convert.FromBase64String(encodedUsername));

            user = GetUserByUsername(decodedUsername);
        }
        return user;
        // Return this user anyway. If username not found -> user.UserID = -1.
        // Use this to check.
    }

    public User GetUserByUsername(string username)
    {
        User user = new();
        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                if (con.State == ConnectionState.Closed) con.Open();
                string query = "SELECT * FROM dbo.Users WHERE Username=@Username";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            user = new User
                            {
                                UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                                Username = reader.IsDBNull(reader.GetOrdinal("Username")) ? "" : reader.GetString(reader.GetOrdinal("Username")),
                                Password = reader.IsDBNull(reader.GetOrdinal("Password")) ? "" : reader.GetString(reader.GetOrdinal("Password")),
                                Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? "" : reader.GetString(reader.GetOrdinal("Description")),
                                EmployeeID = reader.IsDBNull(reader.GetOrdinal("EmployeeID")) ? -1 : reader.GetInt32(reader.GetOrdinal("EmployeeID"))
                            };

                            Debug.WriteLine($"GetUserByUsername - UserID {user.UserID} - Username - {user.Username}");
                            return user;
                        }
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
        }

        return user;
    }
}