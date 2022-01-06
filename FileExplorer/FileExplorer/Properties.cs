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
                pictureBox1.Image = Properties.Resources.folder;
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

                switch (fileInfo.Extension.ToUpper())
                {

                    case ".DOC":
                    case ".DOCX":
                        pictureBox1.Image = Properties.Resources.word;
                        break;

                    case ".EXE":
                        pictureBox1.Image = Properties.Resources.exe;
                        break;

                    case ".PDF":
                        pictureBox1.Image = Properties.Resources.pdf;
                        break;

                    case ".PPTX":
                    case ".PTX":
                        pictureBox1.Image = Properties.Resources.ppt;
                        break;

                    case ".RAR":
                    case ".ZIP":
                        pictureBox1.Image = Properties.Resources.rar;
                        break;

                    case ".SQL":
                        pictureBox1.Image = Properties.Resources.sql;
                        break;

                    case ".XLS":
                    case ".XLXS":
                        pictureBox1.Image = Properties.Resources.excel;
                        break;

                    case ".MP3":
                    case ".MP4":
                    case ".WMV":
                        pictureBox1.Image = Properties.Resources.video;
                        break;

                    case ".PNG":
                    case ".JPG":
                        pictureBox1.Image = Properties.Resources.image;
                        break;
                    case ".TXT":
                        pictureBox1.Image = Properties.Resources.document;
                        break;
                    default:
                        pictureBox1.Image = Properties.Resources.unknown;
                        break;
                }

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void readCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (readCheckBox.Checked == true)
            {
                string path = Path.Combine(location.Text, nameBox.Text);
                if (type.Text.Equals("File"))
                {
                    FileInfo file = new FileInfo(path);
                    file.Attributes = FileAttributes.ReadOnly;
                }
                else
                {
                    DirectoryInfo dir = new DirectoryInfo(path);
                    dir.Attributes = FileAttributes.ReadOnly;
                }
            }
            else
            {
                string path = Path.Combine(location.Text, nameBox.Text);
                if (type.Text.Equals("File"))
                {
                    FileInfo file = new FileInfo(path);
                    file.Attributes &= ~FileAttributes.ReadOnly;
                }
                else
                {
                    DirectoryInfo dir = new DirectoryInfo(path);
                    dir.Attributes &= ~FileAttributes.ReadOnly;
                }
            }
        }

        private void hiddenCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (hiddenCheckBox.Checked == true)
            {
                string path = Path.Combine(location.Text, nameBox.Text);
                if (type.Text.Equals("File"))
                {
                    FileInfo file = new FileInfo(path);
                    file.Attributes = FileAttributes.Hidden;
                }
                else
                {
                    DirectoryInfo dir = new DirectoryInfo(path);
                    dir.Attributes = FileAttributes.Hidden;
                }
            }
            else
            {
                string path = Path.Combine(location.Text, nameBox.Text);
                if (type.Text.Equals("File"))
                {
                    FileInfo file = new FileInfo(path);
                    file.Attributes &= ~FileAttributes.Hidden;
                }
                else
                {
                    DirectoryInfo dir = new DirectoryInfo(path);
                    dir.Attributes &= ~FileAttributes.Hidden;
                }
            }
        }

        private void PropertiesForm_Load(object sender, EventArgs e)
        {

        }

        private void nameBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
