using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileExplorer
{
    public partial class PropertiesForm : Form
    {
        string path;
        public PropertiesForm(string lisDir, string contain)
        {
            InitializeComponent();
            path = lisDir;
            ShowProperties(contain);
        }


        private void label6_Click(object sender, EventArgs e)
        {

        }
        private void ShowProperties(string contain)
        {
            FileAttributes attr = File.GetAttributes(path);
            
            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(path);
                if (directoryInfo.Parent == null)
                {
                    return;
                }
                nameBox.Text = directoryInfo.Name;
                type.Text = "Folder";
                location.Text = directoryInfo.Parent.FullName;
                size.Text = Form1.GetSizeStr(Form1.getSize(directoryInfo));
                date.Text = directoryInfo.CreationTime.ToString("dd.MM.yyyy HH:mm");
                containtxt.Text = contain;
                readCheckBox.Checked = (directoryInfo.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly;
                hiddenCheckBox.Checked = (directoryInfo.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden;
            }
            else
            {
                FileInfo fileInfo = new FileInfo(path);
                nameBox.Text = fileInfo.Name;
                type.Text = fileInfo.Extension.Substring(1).ToString();
                location.Text = fileInfo.DirectoryName;
                containLB.Visible = false;
                containtxt.Text = "";
                size.Text = Form1.GetSizeStr(fileInfo.Length);
                date.Text = fileInfo.CreationTime.ToString("dd.MM.yyyy HH:mm");
                readCheckBox.Checked = (fileInfo.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly;
                hiddenCheckBox.Checked = (fileInfo.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
