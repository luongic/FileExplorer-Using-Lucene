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
            tabControl1.SelectedTab = tabControl1.TabPages["Computer"];
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
            // them danh icon cho listview
            pathDir.Text = curDir.FullName; //textBox Path
            if (flagLarge == true) //Large Icon View
            {
                ImageList iList = new ImageList();
                iList.ImageSize = new System.Drawing.Size(64, 64);
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
                iList.Images.Add(new Icon("icons/rar.ico"));//10
                iList.Images.Add(new Icon("icons/unknown.ico"));//11
                iList.Images.Add(new Icon("icons/video.ico"));//12
                listDir.LargeImageList = iList;
            }
            else //Small List View
            {
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
                iList.Images.Add(new Icon("icons/rar.ico"));//10
                iList.Images.Add(new Icon("icons/unknown.ico"));//11
                iList.Images.Add(new Icon("icons/video.ico"));//12
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

        private void listDir_ItemActivate(object sender, EventArgs e)
        {
            //try
            //{
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
                    
                    new Process { StartInfo = new ProcessStartInfo(file.FullName) 
                                    { 
                                        UseShellExecute = true 
                                    } 
                    }.Start();
                }
            //}
            //catch
            //{
            //    return;
            //}
        }

        private void button4_Click(object sender, EventArgs e)
        {

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
                int num = 1;
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
                copyTS.Visible = false;
                cutTS.Visible = false;
                deleteTS.Visible = false;
                renameTS.Visible = false;
            }
            else
            {
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
                    if (File.Exists(Path.Combine(curDir.FullName, nameEdit)))
                    {
                        MessageBox.Show("Trùng tên", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        e.CancelEdit = true;
                    }
                    else
                    {
                        FileInfo dirEdit = (FileInfo)lv.Tag;
                        File.Move(dirEdit.FullName, Path.Combine(curDir.FullName, nameEdit));
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
                MessageBox.Show("Chưa có File/ Folder được Copy hoặc Cut!");
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
                                File.Delete(file.FullName);
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
    }
}
