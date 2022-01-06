using GemBox.Document;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

        private int fileCount = 0;
        private int directorieCount = 0;

        ArrayList arrCopy = new ArrayList();
        ArrayList arrCut = new ArrayList();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitTreeView();
            tabControl1.SelectedTab = tabControl1.TabPages["File"];
            dateTimePicker1.Visible = false;
            chooseDateTXT.Visible = false;
            searchContentBox.Enabled = false;
        }

        private void InitTreeView() // Khởi tạo TreeView
        {
            //Thêm vào danh sách icon
            treeNode.ImageList = new ImageList();
            treeNode.ImageList.Images.Add(new Icon("icons/thisPC.ico"));
            treeNode.ImageList.Images.Add(new Icon("icons/drisk.ico"));
            treeNode.ImageList.Images.Add(new Icon("icons/folders.ico"));
            treeNode.ImageList.Images.Add(new Icon("icons/opened-folder.ico"));
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
                    diskInfo.Text = directorieCount.ToString() + " Folder(s)  " + fileCount.ToString() + " File(s)";
                }
            }
            catch
            {
                return;
            }
        }

        private void LoadDirectory() //
        {
            directorieCount = 0;
            fileCount = 0;
            ImageList iList = new ImageList();
           
            iList.Images.Add(new Icon("icons/folders.ico"));
            iList.Images.Add(new Icon("icons/document.ico"));
            iList.Images.Add(new Icon("icons/doc.ico"));//2
            iList.Images.Add(new Icon("icons/exe.ico"));//3
            iList.Images.Add(new Icon("icons/img.ico"));//4
            iList.Images.Add(new Icon("icons/pdf.ico"));//5
            iList.Images.Add(new Icon("icons/pptx.ico"));//6
            iList.Images.Add(new Icon("icons/rar.ico"));//7
            iList.Images.Add(new Icon("icons/sql.ico"));//8
            iList.Images.Add(new Icon("icons/xlxs.ico"));//9
            iList.Images.Add(new Icon("icons/unknown.ico"));//10
            iList.Images.Add(new Icon("icons/video.ico"));//11
            // them danh icon cho listview
            pathDir.Text = curDir.FullName; //textBox Path
            if (flagLarge == true) //Large Icon View
            {
                iList.ImageSize = new System.Drawing.Size(64, 64);
                listDir.LargeImageList = iList;
            }
            else //Small List View
            {
                
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
                directorieCount++;
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
                    case ".ZIP":
                        typeFile = 7;
                        break;

                    case ".SQL":
                        typeFile = 8;
                        break;

                    case ".XLS":
                    case ".XLXS":
                        typeFile = 9;
                        break;

                    case ".MP3":
                    case ".MP4":
                    case ".WMV":
                        typeFile = 11;
                        break;

                    case ".PNG":
                    case ".JPG":
                        typeFile = 4;
                        break;
                    case ".TXT":
                        typeFile = 1;
                        break;
                    default:
                        typeFile = 10;
                        break;
                }
                ListViewItem listItem = listDir.Items.Add(file.Name);
                listItem.Tag = file;
                listItem.ImageIndex = typeFile;
                listItem.SubItems.Add(GetSizeStr(file.Length));
                listItem.SubItems.Add(file.Extension.Substring(1));
                listItem.SubItems.Add(file.LastWriteTime.ToString());
                fileCount++;
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

        private void OpenFile()
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
                pathDir.Text = file.FullName;

                new Process
                {
                    StartInfo = new ProcessStartInfo(file.FullName)
                    {
                        UseShellExecute = true
                    }
                }.Start();
            }
        }

        private void listDir_ItemActivate(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void left_Click(object sender, EventArgs e)
        {
            if (dsBack.Count > 1)
            {
                curDir = dsBack[dsBack.Count - 2];
                dsNext.Add(dsBack[dsBack.Count - 1]);
                dsBack.Remove(dsBack[dsBack.Count - 1]);
                LoadDirectory();
            }
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void refresh_Click(object sender, EventArgs e)
        {
            reFresh();
        }
        private void reFresh()
        {
            LoadDirectory();
            UpdateTreeView();
        }
        private void UpdateTreeView()
        {
            try
            {
                curNode.Nodes.Clear();
                DirectoryInfo directoryInfo = curDir;
                foreach (DirectoryInfo dir in directoryInfo.GetDirectories())
                {
                    if (((dir.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden) & true == flag1)
                    {
                        continue;
                    }
                    if ((dir.Attributes & FileAttributes.System) == FileAttributes.System)
                    {
                        continue;
                    }
                    TreeNode dirNode = new TreeNode(dir.Name);
                    dirNode.Tag = dir;
                    dirNode.ImageIndex = 2;
                    dirNode.SelectedImageIndex = 3;
                    curNode.Nodes.Add(dirNode);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine(ex.StackTrace);
            }
        }

        private void right_Click(object sender, EventArgs e)
        {
            if (dsNext.Count > 0)
            {
                curDir = dsNext[dsNext.Count - 1];
                dsBack.Add(dsNext[dsNext.Count - 1]);
                dsNext.Remove(dsNext[dsNext.Count - 1]);
                LoadDirectory();
            }
        }

        private void up_Click(object sender, EventArgs e)
        {
            if (curDir.Parent != null)
            {
                curDir = curDir.Parent;
                curNode = curNode.Parent;
                reFresh();
            }
            else
            {
                return;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void searchTypeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string typeSearch = this.searchTypeBox.SelectedItem.ToString();
            if (typeSearch == "Date")
            {
                dateTimePicker1.Visible = true;
                chooseDateTXT.Visible = true;
                searchContentBox.Enabled = false;
            }
            if (typeSearch == "Type")
            {
                dateTimePicker1.Visible = false;
                chooseDateTXT.Visible = false;
                searchContentBox.Enabled = true;
            }
            if (typeSearch == "Name")
            {
                dateTimePicker1.Visible = false;
                chooseDateTXT.Visible = false;
                searchContentBox.Enabled = true;
            }
            if (typeSearch == "Content")
            {
                dateTimePicker1.Visible = false;
                chooseDateTXT.Visible = false;
                searchContentBox.Enabled = true;
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            listDir.View = System.Windows.Forms.View.List;
            reFresh();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            listDir.View = System.Windows.Forms.View.Details;
            reFresh();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            listDir.View = System.Windows.Forms.View.Tile;

            flagLarge = true;
            reFresh();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            listDir.View = System.Windows.Forms.View.LargeIcon;
            flagLarge = true;
            reFresh();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            //foreach (ListViewItem item in listDir.Items)
            //{
               
            //}
            //for (int i = 0; i < listDir.Items.Count; i++)
            //{
            //    listDir.Items[i]. = true;
            //}
        }

        private void button1_Click(object sender, EventArgs e) //Copy Path
        {
            if (pathDir.Text == "")
            {
                MessageBox.Show("Không có đường dẫn");
            }
            else
            {
                Clipboard.SetText(pathDir.Text);
                MessageBox.Show("Đã Sao Chép đường đẫn");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            newFolder();
        }
        private void newFolder()
        {
            if (curDir == null)
            {
                MessageBox.Show("Chưa chọn nơi tạo");
            }
            else
            {
                // Tạo ra Folder với tên mặc định là New Folder
                DirectoryInfo curDirectory = curDir;
                string path = Path.Combine(curDirectory.FullName, "New Folder");
                string newFolderPath = path;
                int num = 2;
                while (System.IO.Directory.Exists(newFolderPath))
                {
                    newFolderPath = path + " (" + num + ")";
                    num++;
                }
                DirectoryInfo dirInfo = System.IO.Directory.CreateDirectory(newFolderPath);
                reFresh();
            }
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            System.Drawing.Point curPoint = listDir.PointToClient(Cursor.Position);
            ListViewItem item = listDir.GetItemAt(curPoint.X, curPoint.Y);

            if (item == null) // hiển thị tool trip khi không click vào item
            {
                openTS.Visible = false;
                copyTS.Visible = false;
                cutTS.Visible = false;
                deleteTS.Visible = false;
                renameTS.Visible = false;
            }
            else
            {
                openTS.Visible = true;
                copyTS.Visible = true;
                cutTS.Visible = true;
                deleteTS.Visible = true;
                renameTS.Visible = true;
            }
        }

        private void folderTS_Click(object sender, EventArgs e)
        {
            newFolder();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            if (listDir.SelectedItems.Count > 0)
            {
                listDir.SelectedItems[0].BeginEdit();
            }
        }
        public static bool checkFileName(string fileName)
        {
            const string errChar = "\\/:*?\"<>|";

            foreach (char ch in errChar)
            {
                if (fileName.Contains(ch.ToString()))
                    return false;
            }
            return true;
        }

        private void listDir_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            string nameEdit = e.Label;
            ListViewItem lv = listDir.SelectedItems[0];
            // Nếu không thay đổi tên
            if (string.IsNullOrEmpty(e.Label) || nameEdit.Equals(lv.Text))
            {
                e.CancelEdit = true;
                return;
            }
            // Kiểm tra tên có chứa các kí tự đặc biệt
            else if (!checkFileName(nameEdit))
            {
                MessageBox.Show("Tên không được chứa kí tự đặc biệt",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                // Nếu thay đổi tên folder 
                if (lv.Tag.GetType() == typeof(DirectoryInfo))
                {
                    // Bắt trùng tên folder
                    if (System.IO.Directory.Exists(Path.Combine(curDir.FullName, nameEdit)))
                    {
                        MessageBox.Show("Trùng tên", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        e.CancelEdit = true;
                    }
                    else
                    {
                        DirectoryInfo dirEdit = (DirectoryInfo)lv.Tag;
                        System.IO.Directory.Move(dirEdit.FullName, Path.Combine(curDir.FullName, nameEdit));
                        DirectoryInfo dirtmp = new DirectoryInfo(Path.Combine(curDir.FullName, nameEdit));
                        lv.Tag = dirtmp;
                    }
                }
                else //Nếu thay đổi tên file
                {
                    // Bắt trùng tên file
                    if (System.IO.File.Exists(Path.Combine(curDir.FullName, nameEdit)))
                    {
                        MessageBox.Show("Trùng tên", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        e.CancelEdit = true;
                    }
                    else
                    {
                        FileInfo dirEdit = (FileInfo)lv.Tag;
                        System.IO.File.Move(dirEdit.FullName, Path.Combine(curDir.FullName, nameEdit));
                        FileInfo dirtmp = new FileInfo(Path.Combine(curDir.FullName, nameEdit));
                        lv.Tag = dirtmp;
                    }
                }
            }
            reFresh();
        }

        private void Copy()
        {
            // Nếu chưa chọn folder hoặc file để copy
            if (listDir.SelectedItems.Count < -1)
            {
                MessageBox.Show("Cần chọn File/ Folder để copy!");
            }
            else
            {
                try
                {
                    arrCopy.Clear();
                    arrCut.Clear();
                    // Thêm file or folder cần copy vào danh sách tạm thời
                    foreach (ListViewItem item in listDir.SelectedItems)
                    {
                        arrCopy.Add(item.Tag);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Console.WriteLine(ex.StackTrace);
                }
            }
        }

        private void copyTS_Click(object sender, EventArgs e)
        {
            Copy();
        }

        private void Cut()
        {
            // Nếu chưa chọn folder hoặc file để cut
            if (listDir.SelectedItems.Count < -1)
            {
                MessageBox.Show("Cần chọn File/ Folder để Cut!");
            }
            else
            {
                try
                {
                    arrCut.Clear();
                    arrCopy.Clear();
                    // Thêm file or folder cần di chuyển vào danh sách tạm thời
                    foreach (ListViewItem item in listDir.SelectedItems)
                    {
                        arrCut.Add(item.Tag);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Console.WriteLine(ex.StackTrace);
                }
            }
        }

        private void cutTS_Click(object sender, EventArgs e)
        {
            Cut();
        }

        private void renameTS_Click(object sender, EventArgs e)
        {
            if (listDir.SelectedItems.Count > 0)
            {
                listDir.SelectedItems[0].BeginEdit();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Paste();
        }

        private void pasteTS_Click(object sender, EventArgs e)
        {
            Paste();
        }
        
        private void Paste()
        {
            // Nếu danh sách đang chứa các Folder or file copy
            if (arrCopy.Count > 0)
            {
                //Lấy danh sách các folder/ file đang copy
                foreach (var tmp in arrCopy)
                {
                    if (tmp.GetType() == typeof(DirectoryInfo))  //Copy folder
                    {
                        DirectoryInfo folderpaste = (DirectoryInfo)tmp;

                        CopyorCutFolder(folderpaste.FullName, true); //Dùng hàm CopyorCutFolder() để paste ra folder copy
                    }
                    else //Copy file
                    {
                        FileInfo file = (FileInfo)tmp;
                        // Dùng hàm CopyorCutFile() để paste ra folder copy
                        CopyorCutFile(file.FullName, true);
                    }
                }
            }
            // Nếu danh sách đang chứa các Folder or file Cut
            else if (arrCut.Count > 0)
            {
                // Lấy danh sách các folder/ file đang di chuyển
                foreach (var tmp in arrCut)
                {
                    if (tmp.GetType() == typeof(DirectoryInfo)) //Cut folder
                    {
                        DirectoryInfo folderpaste = (DirectoryInfo)tmp; //Tạo DirectoryInfo tạm để lưu thông tin folder
                        CopyorCutFolder(folderpaste.FullName, false); // Dùng hàm CopyorCutFolder() để paste ra folder
                    }
                    else  //Cut File
                    {
                        FileInfo file = (FileInfo)tmp; // tạo fileInfo tạm để lưu thông tin file
                        CopyorCutFile(file.FullName, false); // Dùng hàm CopyorCutFile() để paste ra folder
                    }
                }
            }
            else
            {
                return;
            }
            reFresh();
        }

        private void CopyorCutFile(string path, bool flag)
        {
            try
            {
                FileInfo file = new FileInfo(path);
                string destPath = Path.Combine(curDir.FullName, file.Name);
                if (listDir.SelectedItems.Count > 0)
                {
                    ListViewItem item = listDir.SelectedItems[0];
                    if (item.Tag.GetType() == typeof(DirectoryInfo))
                    {
                        DirectoryInfo directoryInfo = (DirectoryInfo)item.Tag;
                        destPath = Path.Combine(directoryInfo.FullName, file.Name);
                    }
                }
                if (flag)
                {
                    file.CopyTo(destPath);
                }
                else
                {
                    file.MoveTo(destPath);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CopyorCutFolder(string path, bool flag)
        {
            try
            {
                DirectoryInfo sourcefoulder = new DirectoryInfo(path);
                string destPath = Path.Combine(curDir.FullName, sourcefoulder.Name);
                if (listDir.SelectedItems.Count > 0)
                {
                    ListViewItem item = listDir.SelectedItems[0];
                    if (item.Tag.GetType() == typeof(DirectoryInfo))
                    {
                        DirectoryInfo directoryInfo = (DirectoryInfo)item.Tag;
                        destPath = Path.Combine(directoryInfo.FullName, sourcefoulder.Name);
                    }
                }
                DirectoryInfo destfolder = new DirectoryInfo(destPath);
                if (flag)
                {
                    CopyFolder(sourcefoulder, destfolder);
                }
                else
                {
                    sourcefoulder.MoveTo(destPath);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CopyFolder(DirectoryInfo source, DirectoryInfo dest)
        {
            System.IO.Directory.CreateDirectory(dest.FullName);
            foreach (FileInfo file in source.GetFiles())
            {
                file.CopyTo(Path.Combine(dest.FullName, file.Name));
            }

            foreach (DirectoryInfo sourceSubDir in source.GetDirectories())
            {
                DirectoryInfo destPath = new DirectoryInfo(Path.Combine(dest.FullName, sourceSubDir.Name));
                CopyFolder(sourceSubDir, destPath);
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Cut();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            Copy();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Delete();
        }

        private void Delete()
        {
            // Nếu chọn vị trí listview cần xóa
            if (listDir.SelectedItems.Count > 0)
            {
                DialogResult dialogResult = MessageBox.Show("Bạn có muốn xóa?", "Deletion", MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation);
                //Dialog Confirm
                if (dialogResult == DialogResult.No)
                    return;
                else
                {
                    try
                    {
                        // Lấy item chọn
                        foreach (ListViewItem item in listDir.SelectedItems)
                        {
                            // Nếu là Folder
                            if (item.Tag.GetType() == typeof(DirectoryInfo))
                            {
                                DirectoryInfo folderdelete = (DirectoryInfo)item.Tag;
                                System.IO.Directory.Delete(folderdelete.FullName, true);
                                listDir.Refresh();
                            }
                            // Nếu là File
                            else
                            {
                                FileInfo file = (FileInfo)item.Tag;
                                System.IO.File.Delete(file.FullName);
                            }
                            // Xóa Item ra khỏi listview
                            listDir.Items.Remove(item);
                        }
                        UpdateTreeView();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Console.WriteLine(ex.StackTrace);
                    }
                }
            }
        }

        private void deleteTS_Click(object sender, EventArgs e)
        {
            Delete();
        }

        private void refreshTS_Click(object sender, EventArgs e)
        {
            Refresh();
        }

        private void openTS_Click(object sender, EventArgs e)
        {
            OpenFile();
        }


        //PlaceHoder for searchContentBox-------------------------------
        private void searchContentBox_Enter(object sender, EventArgs e)
        {
            if(searchContentBox.Text == " Search content...")
            {
                searchContentBox.Text = "";
                searchContentBox.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void searchContentBox_Leave(object sender, EventArgs e)
        {
            if (searchContentBox.Text == "")
            {
                searchContentBox.Text = " Search content...";
                searchContentBox.ForeColor = System.Drawing.Color.Silver;
            }
        }
        //------------------------------------------------------------
        private void splitter1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void searchContentBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void textFileTS_Click(object sender, EventArgs e)
        {
            newFileMS("New Text Document.txt","txt");
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string typeSearch = searchTypeBox.Text;
            if (typeSearch == "")
            {
                MessageBox.Show("Chưa chọn kiểu tìm kiếm", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if(typeSearch == "Type")
            {
                if (searchContentBox.Text == "")
                {
                    MessageBox.Show("Chưa nhập định dạng", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    findByType();
                }
            }
            else if (typeSearch == "Date")
            {
                findByDay();
            }
            else if (typeSearch == "Name")
            {
                if (searchContentBox.Text == "")
                {
                    MessageBox.Show("Chưa nhập tên", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    findByName();
                    FileName.Clear();
                }

            }
            else if (typeSearch == "Content")
            {
                if (searchContentBox.Text == "")
                {
                    MessageBox.Show("Chưa nhập nội dung", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    findByContent();
                    ContentFile.Clear();
                    ContentName.Clear();
                }
            }
        }

        public void findByType()
        {
            //if (searchContentBox.Text == "All")
            //{
            //    LoadDirectory();
            //}
            if (curDir == null)
            {
                return;
            }
            else
            {
                listDir.Items.Clear();
                string findType = searchContentBox.Text;
                if (findType.Equals("Folder"))
                {
                    foreach (DirectoryInfo subDir in curDir.GetDirectories())
                    {
                        ListViewItem lvi = listDir.Items.Add(subDir.Name);
                        lvi.Tag = subDir;
                        lvi.ImageIndex = 0;
                        lvi.SubItems.Add("");
                        lvi.SubItems.Add("Folder");
                        lvi.SubItems.Add(subDir.LastWriteTime.ToString());
                    }
                }
                else if (findType.Equals("All"))
                {
                    reFresh();
                }
                else
                {
                    foreach (FileInfo file in curDir.GetFiles())
                    {
                        string Type = file.Extension.Substring(1);
                        if (Type == findType)
                        {
                            ListViewItem lvi = listDir.Items.Add(file.Name);
                            lvi.Tag = file;
                            int count;
                            switch (file.Extension.ToUpper())
                            {

                                case ".DOC":
                                case ".DOCX":
                                    count = 2;
                                    break;
                                case ".EXE":
                                    count = 3;
                                    break;
                                case ".PDF":
                                    count = 5;
                                    break;
                                case ".PPTX":
                                case ".PTX":
                                    count = 6;
                                    break;
                                case ".RAR":
                                case ".ZIP":
                                    count = 7;
                                    break;
                                case ".SQL":
                                    count = 8;
                                    break;
                                case ".XLS":
                                case ".XLXS":
                                    count = 9;
                                    break;
                                case ".MP3":
                                case ".MP4":
                                case ".WMV":
                                    count = 11;
                                    break;
                                case ".PNG":
                                case ".JPG":
                                    count = 4;
                                    break;
                                default:
                                    count = 10;
                                    break;
                            }
                            lvi.ImageIndex = count;
                            lvi.SubItems.Add(GetSizeStr(file.Length));
                            lvi.SubItems.Add(file.Extension.Substring(1));
                            lvi.SubItems.Add(file.LastWriteTime.ToString());
                        }

                    }

                }
            }
        }


        public void findByDay()
        {
            listDir.Items.Clear();
            // Lấy date input
            string findDay = dateTimePicker1.Text;
            if(curDir == null)
            {
                return;
            }
            // Lọc danh sách folder hiện tại
            foreach (DirectoryInfo subDir in curDir.GetDirectories())
            {
                int day = subDir.LastWriteTime.Day;
                int month = subDir.LastWriteTime.Month;
                int year = subDir.LastWriteTime.Year;
                // Lấy date theo MM/DD/YY 
                string txtDate = (month.ToString() + "/" + day.ToString() + "/" + year.ToString());
                // Nếu ngày tìm kiếm giống nhau thì add vào listview
                if (txtDate == findDay)
                {
                    ListViewItem lvi = listDir.Items.Add(subDir.Name);
                    lvi.Tag = subDir;
                    lvi.ImageIndex = 0;
                    lvi.SubItems.Add("");
                    lvi.SubItems.Add("Folder");
                    lvi.SubItems.Add(subDir.LastWriteTime.ToString());
                }

            }
            // Tương tự đối với file
            foreach (FileInfo file in curDir.GetFiles())
            {
                int day = file.LastWriteTime.Day;
                int month = file.LastWriteTime.Month;
                int year = file.LastWriteTime.Year;
                string txtDate = (month.ToString() + "/" + day.ToString() + "/" + year.ToString());
                if (txtDate == findDay)
                {
                    ListViewItem lvi = listDir.Items.Add(file.Name);
                    lvi.Tag = file;
                    int count;
                    switch (file.Extension.ToUpper())
                    {

                        case ".DOC":
                        case ".DOCX":
                            count = 2;
                            break;
                        case ".EXE":
                            count = 3;
                            break;
                        case ".PDF":
                            count = 5;
                            break;
                        case ".PPTX":
                        case "PTX":
                            count = 6;
                            break;
                        case ".RAR":
                        case ".ZIP":
                            count = 7;
                            break;
                        case ".SQL":
                            count = 8;
                            break;
                        case ".XLS":
                            count = 9;
                            break;
                       
                        case ".MP3":
                        case ".MP4":
                        case ".WMV":
                            count = 12;
                            break;
                        case ".PNG":
                        case "JPG":
                            count = 4;
                            break;
                        default:
                            count = 11;
                            break;
                    }
                    lvi.ImageIndex = count;
                    lvi.SubItems.Add(GetSizeStr(file.Length));
                    lvi.SubItems.Add(file.Extension.Substring(1));
                    lvi.SubItems.Add(file.LastWriteTime.ToString());
                }

            }
        }


        public void loadName(string file)
        {
            string Namefile = searchContentBox.Text;
            var F = new Lucene.Net.Documents.Document();
            F.Add(new Lucene.Net.Documents.Field("File", file, Lucene.Net.Documents.Field.Store.YES, Lucene.Net.Documents.Field.Index.ANALYZED));

            Lucene.Net.Store.Directory directory = FSDirectory.Open(new System.IO.DirectoryInfo(Environment.CurrentDirectory + "\\LuceneIndex"));
            Analyzer analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29);
            var writer = new IndexWriter(directory, analyzer, true, IndexWriter.MaxFieldLength.LIMITED);
            writer.AddDocument(F);
            writer.Optimize();
            writer.Dispose();

            IndexReader reader = IndexReader.Open(directory, true);
            Searcher search = new IndexSearcher(reader);
            var queryParser = new QueryParser(Lucene.Net.Util.Version.LUCENE_29, "File", analyzer);
            var query = queryParser.Parse(Namefile);

            TopDocs result = search.Search(query, reader.MaxDoc);
            var hits = result.ScoreDocs;
            foreach (var hit in hits)
            {
                var documentFS = search.Doc(hit.Doc);
                FileName.Add(documentFS.Get("File"));

            }
        }


        public void findByName()
        {
            listDir.Items.Clear();
            if (curDir == null)
            {
                return;
            }
            foreach (DirectoryInfo subDir in curDir.GetDirectories())
            {
                loadName(subDir.Name);
                if (FileName.Contains(subDir.Name))
                {
                    loadName(subDir.Name);
                    ListViewItem lvi = listDir.Items.Add(subDir.Name);
                    lvi.Tag = subDir;
                    lvi.ImageIndex = 0;
                    lvi.SubItems.Add("");
                    lvi.SubItems.Add("Folder");
                    lvi.SubItems.Add(subDir.LastWriteTime.ToString());
                }
            }
            foreach (FileInfo file in curDir.GetFiles())
            {
                int index = file.Name.LastIndexOf(".");
                loadName(file.Name.Remove(index));
                if (FileName.Contains(file.Name.Remove(index)) == true)
                {
                    loadName(file.Name.Remove(index));
                    ListViewItem lvi = listDir.Items.Add(file.Name);
                    lvi.Tag = file;
                    int count;
                    switch (file.Extension.ToUpper())
                    {

                        case ".DOC":
                        case ".DOCX":
                            count = 2;
                            break;
                        case ".EXE":
                            count = 3;
                            break;
                        case ".PDF":
                            count = 5;
                            break;
                        case ".PPTX":
                        case "PTX":
                            count = 6;
                            break;
                        case ".RAR":
                        case ".ZIP":
                            count = 7;
                            break;
                        case ".SQL":
                            count = 8;
                            break;
                        case ".XLS":
                            count = 9;
                            break;
                        case ".MP3":
                        case ".MP4":
                        case ".WMV":
                            count = 12;
                            break;
                        case ".PNG":
                        case "JPG":
                            count = 4;
                            break;
                        default:
                            count = 11;
                            break;
                    }
                    lvi.ImageIndex = count;
                    lvi.SubItems.Add(GetSizeStr(file.Length));
                    lvi.SubItems.Add(file.Extension.Substring(1));
                    lvi.SubItems.Add(file.LastWriteTime.ToString());
                }

            }
        }

        public void loadByContent(string content, string fileName)
        {
            string FileContent = searchContentBox.Text;
            var F = new Lucene.Net.Documents.Document();
            F.Add(new Lucene.Net.Documents.Field("FileContent", content, Lucene.Net.Documents.Field.Store.YES, Lucene.Net.Documents.Field.Index.ANALYZED));

            Lucene.Net.Store.Directory directory = FSDirectory.Open(new DirectoryInfo(Environment.CurrentDirectory + "\\LuceneIndex"));
            Analyzer analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29);
            var writer = new IndexWriter(directory, analyzer, true, IndexWriter.MaxFieldLength.LIMITED);
            writer.AddDocument(F);
            writer.Optimize();
            writer.Dispose();

            IndexReader reader = IndexReader.Open(directory, true);
            Searcher search = new IndexSearcher(reader);
            var queryParser = new QueryParser(Lucene.Net.Util.Version.LUCENE_29, "FileContent", analyzer);

            var query = queryParser.Parse(FileContent);
            TopDocs result = search.Search(query, reader.MaxDoc);

            var hits = result.ScoreDocs;
            foreach (var hit in hits)
            {
                var documentFS = search.Doc(hit.Doc);
                ContentFile.Add(documentFS.Get("FileContent"));
                ContentName.Add(fileName);

            }

        }

        public void findByContent()
        {
            listDir.Items.Clear();
            if (curDir == null)
            {
                return;
            }
            foreach (FileInfo file in curDir.GetFiles())
            {
                int count;
                switch (file.Extension.ToUpper())
                {

                    case ".DOC":
                    case ".DOCX":
                        count = 2;
                        break;
                    case ".EXE":
                        count = 3;
                        break;
                    case ".PDF":
                        count = 5;
                        break;
                    case ".PPTX":
                    case "PTX":
                        count = 6;
                        break;
                    case ".RAR":
                    case ".ZIP":
                        count = 7;
                        break;
                    case ".SQL":
                        count = 8;
                        break;
                    case ".XLS":
                        count = 9;
                        break;

                    case ".MP3":
                    case ".MP4":
                    case ".WMV":
                        count = 12;
                        break;
                    case ".PNG":
                    case "JPG":
                        count = 4;
                        break;
                    default:
                        count = 11;
                        break;
                }

                if (count == 2)//đoc file Word
                {
                    // If using Professional version, put your serial key below.
                    ComponentInfo.SetLicense("FREE-LIMITED-KEY");

                    // Load Word document from file's path.
                    var document = DocumentModel.Load(@file.FullName.ToString());

                    // Get Word document's plain text.
                    string text = document.Content.ToString();

                    // Get Word document's count statistics.
                    int charactersCount = text.Replace(Environment.NewLine, string.Empty).Length;
                    int wordsCount = Regex.Matches(text, @"[\S]+").Count;
                    int paragraphsCount = document.GetChildElements(true, ElementType.Paragraph).Count();
                    int pageCount = document.GetPaginator().Pages.Count;


                    loadByContent(text.ToLower(), file.Name);
                }

                //Đọc file pdf
                if (count == 5)
                {
                    StringBuilder text = new StringBuilder();
                    using (iTextSharp.text.pdf.PdfReader reader = new iTextSharp.text.pdf.PdfReader(file.FullName))
                    {
                        for (int i = 1; i <= reader.NumberOfPages; i++)
                        {
                            text.Append(iTextSharp.text.pdf.parser.PdfTextExtractor.GetTextFromPage(reader, i));
                        }
                    }
                    loadByContent(text.ToString().ToLower(), file.Name);
                }

                //đọc file Text
                if (file.Extension.ToUpper() == ".TXT")
                {
                    string testData = System.IO.File.ReadAllText(file.FullName);
                    loadByContent(testData.ToString().ToLower(), file.Name);
                }


                if (ContentName.IndexOf(file.Name) != -1)
                {
                    ListViewItem lvi = listDir.Items.Add(file.Name);
                    lvi.Tag = file;
                    lvi.ImageIndex = count;
                    lvi.SubItems.Add(file.Length.ToString() + " KB");
                    lvi.SubItems.Add(file.Extension.Substring(1));
                    lvi.SubItems.Add(file.LastWriteTime.ToString());
                }
            }

        }

        public static float getSize(DirectoryInfo dir)
        {
            float size = 0.0f;
            try
            {
                FileInfo[] fileInfos = dir.GetFiles();

                if (fileInfos.Length > 0)
                {
                    foreach (FileInfo file in fileInfos)
                    {
                        try
                        {
                            size += file.Length;
                        }
                        catch
                        {
                            continue;
                        }
                    }
                }
                DirectoryInfo[] directoryInfos = dir.GetDirectories();
                foreach (DirectoryInfo dirInfo in directoryInfos)
                {
                    try
                    {
                        size += getSize(dirInfo);
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                return size;
            }
            return size;
        }

        private void button15_Click(object sender, EventArgs e)
        {
            Properties();
        }

        private void Properties()
        {
            string contain = "";
            if (curDir == null)
            {
                return;
            }
            else
            {
                if (listDir.SelectedItems.Count > 0)
                {
                    ListViewItem item = listDir.SelectedItems[0];
                    if (item.Tag.GetType() == typeof(DirectoryInfo))
                    {
                        DirectoryInfo directoryInfo = (DirectoryInfo)item.Tag;
                        PropertiesForm prop = new PropertiesForm(directoryInfo.FullName, diskInfo.Text);
                        prop.ShowDialog();
                    }
                    else
                    {
                        FileInfo fileInfo = (FileInfo)item.Tag;
                        PropertiesForm prop = new PropertiesForm(fileInfo.FullName, contain);
                        prop.ShowDialog();
                    }
                }
                else
                {
                    PropertiesForm prop = new PropertiesForm(curDir.FullName, contain);
                    prop.ShowDialog();
                }
            }
        }

        private void propertiesTS_Click(object sender, EventArgs e)
        {
            Properties();
        }


        private void newFileMS(string name, string type)
        {
            int fileTypeLengh = type.Length + 1;
            if (curDir == null)
            {
                MessageBox.Show("Không thể tạo File ở đây");
            }
            else
            {
                string currentPath = curDir.FullName;
                string fileName = name;
                string path = Path.Combine(currentPath, fileName);
                string newFilePath = path;
                int num = 2;
                while (System.IO.File.Exists(newFilePath))
                {
                    newFilePath = path.Remove(path.Length - fileTypeLengh) + "(" + num + ")" + "." + type;
                    num++;
                }
                System.IO.File.Create(newFilePath).Close();
                reFresh();
            }
        }

        private void wordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newFileMS("New Word Document.doc","doc");
        }

        private void powerpointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newFileMS("New Word Document.pptx", "pptx");
        }

        private void excelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newFileMS("New Word Document.xlxs", "xlxs");
        }

        private void accessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newFileMS("New Microsoft Access Database.accdb", "accdb");
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void chooseDateTXT_Click(object sender, EventArgs e)
        {

        }

        private void pathDir_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
