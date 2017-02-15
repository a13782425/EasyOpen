using EasyOpen.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EasyOpen.Utils
{
    public class FileUtils
    {
        private static FileUtils _instance = null;
        private Encoding _encoding = new UTF8Encoding();
        private FileUtils()
        {
        }
        public static FileUtils Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new FileUtils();
                }
                return _instance;
            }
        }

        public bool CreateFile(string path)
        {
            try
            {
                if (File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + @"app.setting"))
                {
                    File.Delete(System.AppDomain.CurrentDomain.BaseDirectory + @"app.setting");
                }
                FileStream fs = File.Create(System.AppDomain.CurrentDomain.BaseDirectory + @"app.setting");
                fs.Close();
                fs.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("app.setting文件正在被使用！");
                return false;
            }

        }

        public bool WriteFile(string content)
        {
            StreamWriter sw = new StreamWriter(System.AppDomain.CurrentDomain.BaseDirectory + @"app.setting", false, _encoding);
            byte[] bytes = _encoding.GetBytes(content);
            sw.Write(content);
            sw.Close();
            sw.Dispose();
            return true;
        }

        /// <summary>
        /// 读取资源
        /// </summary>
        /// <returns></returns>
        public string ReadFile()
        {
            FileStream fs = File.OpenRead(System.AppDomain.CurrentDomain.BaseDirectory + @"app.setting");
            BinaryReader binReader = new BinaryReader(fs);

            byte[] bytes = new byte[fs.Length];
            binReader.Read(bytes, 0, (int)fs.Length);
            string content = _encoding.GetString(bytes, 0, bytes.Length);
            binReader.Close();
            fs.Close();
            fs.Dispose();
            return content;
        }

        /// <summary>
        /// 读取Lnk文件的路径
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public string GetLnkFile(string filePath)
        {
            IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShellClass();
            IWshRuntimeLibrary.IWshShortcut shortcut = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(filePath);
            //shortcut.TargetPath = "指向地址.exe";
            //shortcut.Arguments = "参数";
            //shortcut.Description = "我是快捷方式名字哦！";
            //shortcut.Hotkey = "CTRL+SHIFT+N";
            //shortcut.IconLocation = "notepad.exe, 0";
            //shortcut.Save();
            return shortcut.TargetPath;
        }

        /// <summary>
        /// 添加文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public bool AddFileModel(string filePath)
        {
            if (IsDir(filePath))
            {
                return false;
            }
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            ModelUtils.Instance.CurrentFileModelList.Add(new FileModel()
            {
                AddTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                Count = 0,
                EndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                FileName = fileName,
                FullPath = filePath,
                ShowName = fileName,
                PinYin = PinYinUtils.Instance.GetPingYin(fileName),
            });
            return true;
        }

        /// <summary>
        /// 判断是不是文件夹
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public bool IsDir(string filepath)
        {
            FileInfo fi = new FileInfo(filepath);
            if ((fi.Attributes & FileAttributes.Directory) != 0)
                return true;
            else
            {
                return false;
            }
        }

    }
}
