using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileExplorer
{
    public partial class Form1 : Form
    {
        DirectoryInfo curDir; //currentDirrect
        TreeNode curNode; //currentNode

        bool flag1 = true; //flagCheck
        bool flagLarge = false;

        List<string> ContentFile = new List<string>();
        List<string> ContentName = new List<string>();

        List<DirectoryInfo> dsNext = new List<DirectoryInfo>(); //Stack
        List<DirectoryInfo> dsBack = new List<DirectoryInfo>();

        List<string> list = new List<string>(); //1030
        List<string> FileName = new List<string>(); //1237 1248 1263
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitTreeView();
        }

        private void InitTreeView() // Khởi tạo TreeView
        {
            //Thêm vào danh sách icon
            treeNode.ImageList = new ImageList();
            treeNode.ImageList.Images.Add(new Icon("icons/mycomputer.ico"));
            treeNode.ImageList.Images.Add(new Icon("icons/drive.ico"));
            treeNode.ImageList.Images.Add(new Icon("icons/folder_close.ico"));
            treeNode.ImageList.Images.Add(new Icon("icons/folder_open.ico"));
            treeNode.ImageList.Images.Add(new Icon("icons/document.ico"));

            TreeNode myComputerNode = new TreeNode("This PC");
            myComputerNode.Tag = "This PC";
            myComputerNode.ImageIndex = 0;
            myComputerNode.SelectedImageIndex = 0;
            treeNode.Nodes.Add(myComputerNode);
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                if (drive.IsReady)
                {
                    TreeNode driveNode = new TreeNode(drive.Name);
                    driveNode.Tag = drive.RootDirectory;
                    driveNode.ImageIndex = 1;
                    driveNode.SelectedImageIndex = 1;
                    myComputerNode.Nodes.Add(driveNode);
                }
            }
        }

        private void treeNode_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode selectedNode = treeNode.SelectedNode;
            curNode = selectedNode;
            try
            {
                // Nếu click vào folder trên TreeVew
                if (selectedNode.Tag.GetType() == typeof(DirectoryInfo))
                {
                    selectedNode.Nodes.Clear();
                    DirectoryInfo dir = (DirectoryInfo)selectedNode.Tag;
                    // Lấy danh sách các folder con của folder được click vào
                    foreach (DirectoryInfo subDir in dir.GetDirectories())
                    {
                        // Ẩn file và hiện ra danh sách folder con trên treevew
                        if ((subDir.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden ||
                            (subDir.Attributes & FileAttributes.System) == FileAttributes.System)
                        {
                            continue;
                        }
                        TreeNode dirNode = new TreeNode(subDir.Name);
                        dirNode.Tag = subDir;
                        dirNode.ImageIndex = 2;
                        dirNode.SelectedImageIndex = 3;
                        selectedNode.Nodes.Add(dirNode);
                    }
                    // Hiển thị danh sách folder qua listView
                    curDir = dir;
                    pathDir.Text = curDir.FullName;
                    dsBack.Add(curDir);
                    LoadDirectory(); //567
                }
            }
            catch
            {
                return;
            }
        }

        private void LoadDirectory() //
        {
            // them danh icon cho listview
            pathDir.Text = curDir.FullName; //textBox Path
            if (flagLarge == true) //Large Icon View
            {
                ImageList iList = new ImageList();
                iList.ImageSize = new System.Drawing.Size(64, 64);
                iList.Images.Add(new Icon("icons/folder_open.ico"));
                iList.Images.Add(new Icon("icons/document.ico"));
                iList.Images.Add(new Icon("icons/doc.ico"));//2
                iList.Images.Add(new Icon("icons/exe.ico"));//3
                iList.Images.Add(new Icon("icons/img.ico"));//4
                iList.Images.Add(new Icon("icons/pdf.ico"));//5
                iList.Images.Add(new Icon("icons/pptx.ico"));//6
                iList.Images.Add(new Icon("icons/rar.ico"));//7
                iList.Images.Add(new Icon("icons/sql.ico"));//8
                iList.Images.Add(new Icon("icons/xls.ico"));//9
                iList.Images.Add(new Icon("icons/zip.ico"));//10
                iList.Images.Add(new Icon("icons/unknown.ico"));//11
                iList.Images.Add(new Icon("icons/clip.ico"));//12
                listDir.LargeImageList = iList;
            }
            else //Small List View
            {
                ImageList iList = new ImageList();
                iList.Images.Add(new Icon("icons/folder_open.ico"));
                iList.Images.Add(new Icon("icons/document.ico"));
                iList.Images.Add(new Icon("icons/doc.ico"));//2
                iList.Images.Add(new Icon("icons/exe.ico"));//3
                iList.Images.Add(new Icon("icons/img.ico"));//4
                iList.Images.Add(new Icon("icons/pdf.ico"));//5
                iList.Images.Add(new Icon("icons/pptx.ico"));//6
                iList.Images.Add(new Icon("icons/rar.ico"));//7
                iList.Images.Add(new Icon("icons/sql.ico"));//8
                iList.Images.Add(new Icon("icons/xls.ico"));//9
                iList.Images.Add(new Icon("icons/zip.ico"));//10
                iList.Images.Add(new Icon("icons/unknown.ico"));//11
                iList.Images.Add(new Icon("icons/clip.ico"));//12
                listDir.SmallImageList = iList;
            }
            listDir.Items.Clear();

            foreach (DirectoryInfo dir in curDir.GetDirectories())
            {
                if (((dir.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden) & true == flag1)//flag 1 cho hidden file
                {
                    continue;
                }
                if ((dir.Attributes & FileAttributes.System) == FileAttributes.System)
                {
                    continue;
                }
                ListViewItem lvi = listDir.Items.Add(dir.Name);
                lvi.Tag = dir;
                lvi.ImageIndex = 0;
                lvi.SubItems.Add("");
                lvi.SubItems.Add("Folder");
                lvi.SubItems.Add(dir.LastWriteTime.ToString());
            }
            foreach (FileInfo file in curDir.GetFiles())
            {
                if ((file.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
                {
                    continue;
                }
                if ((file.Attributes & FileAttributes.System) == FileAttributes.System)
                {
                    continue;
                }

                loadType(file);

                int typeFile;

                switch (file.Extension.ToUpper())
                {

                    case ".DOC":
                    case ".DOCX":
                        typeFile = 2;
                        break;

                    case ".EXE":
                        typeFile = 3;
                        break;

                    case ".PDF":
                        typeFile = 5;
                        break;

                    case ".PPTX":
                    case ".PTX":
                        typeFile = 6;
                        break;

                    case ".RAR":
                        typeFile = 7;
                        break;

                    case ".SQL":
                        typeFile = 8;
                        break;

                    case ".XLS":
                        typeFile = 9;
                        break;

                    case ".ZIP":
                        typeFile = 10;
                        break;

                    case ".MP3":
                    case ".MP4":
                    case ".WMV":
                        typeFile = 12;
                        break;

                    case ".PNG":
                    case ".JPG":
                        typeFile = 4;
                        break;
                    default:
                        typeFile = 11;
                        break;
                }
                ListViewItem listItem = listDir.Items.Add(file.Name);
                listItem.Tag = file;
                listItem.ImageIndex = typeFile;
                listItem.SubItems.Add(GetSizeStr(file.Length));
                listItem.SubItems.Add(file.Extension.Substring(1));
                listItem.SubItems.Add(file.LastWriteTime.ToString());
            }
        }

        public void loadType(FileInfo file) //Lấy type của file
        {
            string Type = file.Extension.Substring(1);
            if (list.Contains(Type) == false)
            {
                list.Add(Type);
                //type.Items.Add(Type);
            }
        }

        // Format size theo B KB MB GB
        public static string GetSizeStr(float fileSize)
        {
            string fileSizeStr = string.Empty;

            if (fileSize < 1024 * 1024)
                fileSizeStr = Math.Round(fileSize * 1.0 / 1024, 2) + " KB";

            else if (fileSize >= 1024 * 1024 && fileSize < 1024 * 1024 * 1024)
                fileSizeStr = Math.Round(fileSize * 1.0 / (1024 * 1024), 2) + " MB";

            else if (fileSize >= 1024 * 1024 * 1024)
                fileSizeStr = Math.Round(fileSize * 1.0 / (1024 * 1024 * 1024), 2) + " GB";

            return fileSizeStr;
        }
        private TreeNode findCurrentNode(string textNode)
        {
            int foundIndex = 0;
            int indexCount = 0;
            foreach (TreeNode node in curNode.Nodes)
            {
                if (node.Text.Equals(textNode))
                {
                    foundIndex = indexCount;
                }
                indexCount++;
            }
            return curNode.Nodes[foundIndex];
        }

        private void listDir_ItemActivate(object sender, EventArgs e)
        {
            try
            {
                if (listDir.SelectedItems[0].Tag.GetType() == typeof(DirectoryInfo))
                {
                    curDir = (DirectoryInfo)listDir.SelectedItems[0].Tag;
                    LoadDirectory();
                    curNode = findCurrentNode(curDir.Name);
                    treeNode.SelectedNode = curNode;
                    pathDir.Text = curDir.FullName;
                }
                else
                {
                    FileInfo file = (FileInfo)listDir.SelectedItems[0].Tag;
                    Process.Start(file.FullName);
                }
            }
            catch
            {
                return;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }
    }
}
