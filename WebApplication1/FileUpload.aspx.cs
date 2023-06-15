using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace WebApplication1
{
    public partial class FileUpload : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString);
        SqlCommand cmd = null;
        SqlDataReader dr = null;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {
                string fname = FileUpload1.PostedFile.FileName;
                string extension = Path.GetExtension(fname);
                int fsize = FileUpload1.PostedFile.ContentLength;
                int flag=0;
                switch(extension.ToLower())
                {
                    case ".doc":
                    case ".docx":
                    case ".pdf":
                        flag = 1;
                        break;
                    default:
                        flag = 0;
                        break;
                }
                if(flag==1)
                {
                    FileUpload1.SaveAs(Server.MapPath("~/Files/" + fname));
                    cmd = new SqlCommand("insert into file_details(FileName, FileSize) values('"+fname+"', '"+fsize/1024+"')", con);
                    con.Open();
                    if(cmd.ExecuteNonQuery()!=0)
                    {
                        Label1.Text = "File Uploaded successfully";
                        Label1.ForeColor = System.Drawing.Color.Green;
                        con.Close();
                        GridDisplayFiles();
                    }
                    else
                    {
                        Label1.Text = "File failed to upload";
                    }
                }
                else
                {
                    Label1.Text = "Only files with extendions .doc, .docx and .pdf files are allowed";
                }
            }
            else
            {
                Label1.Text = "Select the file";
            }
        }

        private void GridDisplayFiles()
        {
            con.Open();
            cmd = new SqlCommand("SELECT * FROM file_details", con);
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                GridView1.DataSource = dr;
                GridView1.DataBind();
            }
            else
            {
                Label1.Text = "No files uploaded";
            }
        }
    }
};