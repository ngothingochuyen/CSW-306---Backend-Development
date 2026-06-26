using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.IO;

namespace ProjectBackendFoodie.User
{
    public partial class Registration : System.Web.UI.Page
    {
        MySqlConnection con;
        MySqlCommand cmd;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                con = new MySqlConnection(
                    ConfigurationManager.ConnectionStrings["cs"].ConnectionString
                );

                con.Open();

                // CHECK DUPLICATE
                string checkQuery = @"SELECT COUNT(*) FROM Users
                              WHERE Username=@Username
                              OR Email=@Email
                              OR Mobile=@Mobile";

                cmd = new MySqlCommand(checkQuery, con);

                cmd.Parameters.AddWithValue("@Username",
                    txtUsername.Text.Trim());

                cmd.Parameters.AddWithValue("@Email",
                    txtEmail.Text.Trim());

                cmd.Parameters.AddWithValue("@Mobile",
                    txtMobile.Text.Trim());

                int userExists =
                    Convert.ToInt32(cmd.ExecuteScalar());

                if (userExists > 0)
                {
                    lblMsg.Visible = true;

                    lblMsg.Text =
                        "<div class='alert alert-danger'>" +
                        "Username, Email or Mobile already exists!" +
                        "</div>";

                    return;
                }

                // IMAGE
                string imagePath = "";

                if (fuUserImage.HasFile)
                {
                    Guid obj = Guid.NewGuid();

                    string extension =
                        Path.GetExtension(fuUserImage.FileName);

                    imagePath =
                        "Images/User/" +
                        obj.ToString() +
                        extension;

                    string folderPath =
                        Server.MapPath("~/Images/User/");

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    string fullPath =
                        folderPath +
                        obj.ToString() +
                        extension;

                    fuUserImage.SaveAs(fullPath);
                }

                // INSERT USER
                string insertQuery = @"INSERT INTO Users
        (
            Name,
            Username,
            Mobile,
            Email,
            Address,
            PostCode,
            Password,
            ImageUrl,
            CreatedDate
        )

        VALUES
        (
            @Name,
            @Username,
            @Mobile,
            @Email,
            @Address,
            @PostCode,
            @Password,
            @ImageUrl,
            NOW()
        )";

                cmd = new MySqlCommand(insertQuery, con);

                cmd.Parameters.AddWithValue("@Name",
                    txtName.Text.Trim());

                cmd.Parameters.AddWithValue("@Username",
                    txtUsername.Text.Trim());

                cmd.Parameters.AddWithValue("@Mobile",
                    txtMobile.Text.Trim());

                cmd.Parameters.AddWithValue("@Email",
                    txtEmail.Text.Trim());

                cmd.Parameters.AddWithValue("@Address",
                    txtAddress.Text.Trim());

                cmd.Parameters.AddWithValue("@PostCode",
                    txtPostCode.Text.Trim());

                cmd.Parameters.AddWithValue("@Password",
                    txtPassword.Text.Trim());

                cmd.Parameters.AddWithValue("@ImageUrl",
                    imagePath);

                int result = cmd.ExecuteNonQuery();

                if (result > 0)
                {
                    lblMsg.Visible = true;

                    lblMsg.Text =
                        "<div class='alert alert-success'>" +

                        "<strong>" + txtUsername.Text +
                        "</strong> registration is successful!" +

                        "</div>";

                    lblMsg.CssClass = "alert alert-success";

                    // CLEAR
                    txtName.Text = "";
                    txtUsername.Text = "";
                    txtMobile.Text = "";
                    txtEmail.Text = "";
                    txtAddress.Text = "";
                    txtPostCode.Text = "";
                    txtPassword.Text = "";
                }
            }
            catch (Exception ex)
            {
                lblMsg.Visible = true;

                lblMsg.Text =
                    "<div class='alert alert-danger'>" +
                    ex.ToString() +
                    "</div>";
            }
            finally
            {
                con.Close();
            }
        }
    }
}