using EasyOpen.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EasyOpen.Utils
{
    public class ModelUtils
    {

        private static ModelUtils _instance = null;
        private ModelUtils()
        {

        }

        public static ModelUtils Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ModelUtils();
                    _instance.LoadData();
                }
                return _instance;
            }
        }

        private JsonModel _currentJsonModel = null;

        public JsonModel CurrentJsonModel
        {
            get
            {
                if (_currentJsonModel == null)
                {
                    _currentJsonModel = new JsonModel();
                }
                return _currentJsonModel;
            }
        }

        public SettingModel CurrentSettingModel
        {
            get
            {
                if (this.CurrentJsonModel == null)
                {
                    return null;
                }
                if (this.CurrentJsonModel.SettingModel == null)
                {
                    this.CurrentJsonModel.SettingModel = new SettingModel(); ;
                }
                return this.CurrentJsonModel.SettingModel;
            }
        }
        public List<SystemModel> CurrentSystemModelList
        {
            get
            {
                if (this.CurrentJsonModel == null)
                {
                    return null;
                }
                if (this.CurrentJsonModel.SystemModelList == null)
                {
                    this.CurrentJsonModel.SystemModelList = new List<SystemModel>();
                }
                return this.CurrentJsonModel.SystemModelList;
            }
        }
        public List<FileModel> CurrentFileModelList
        {
            get
            {
                if (this.CurrentJsonModel == null)
                {
                    return null;
                }
                if (this.CurrentJsonModel.FileModelList == null)
                {
                    this.CurrentJsonModel.FileModelList = new List<FileModel>();
                }
                return this.CurrentJsonModel.FileModelList;
            }
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        private void LoadData()
        {
            if (File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + @"app.setting"))
            {
                ReadData();
                //找到文件读取文件
            }
            else
            {
                //没有找到文件新建文件
                CreateData();
            }

        }

        private void ReadData()
        {

            string json = EncryptUtils.Instance.Decipher(FileUtils.Instance.ReadFile());
            if (string.IsNullOrEmpty(json))
            {
                CreateData();
            }
            else
            {
                this._currentJsonModel = LitJson.JsonMapper.ToObject<JsonModel>(json);
            }
        }

        /// <summary>
        /// 创建数据
        /// </summary>
        private void CreateData()
        {
            FileUtils.Instance.CreateFile(System.AppDomain.CurrentDomain.BaseDirectory + @"app.setting");
            this.CurrentJsonModel.SettingModel = new SettingModel();
            this.CurrentJsonModel.FileModelList = new List<FileModel>();
            this.CurrentJsonModel.SystemModelList = new List<SystemModel>();
            LoadSystemModel();
            Save();

        }

        public bool Remove(string path)
        {
            FileModel fileModel = this.CurrentFileModelList.FirstOrDefault(a => a.FullPath == path);
            if (fileModel != null)
            {
                return this.CurrentFileModelList.Remove(fileModel);
            }
            return false;
        }

        public void Save()
        {
            string content = LitJson.JsonMapper.ToJson(this.CurrentJsonModel);
            content = EncryptUtils.Instance.Encryption(content);
            FileUtils.Instance.WriteFile(content);
        }

        private void LoadSystemModel()
        {
            this.CurrentJsonModel.SystemModelList.Add(new SystemModel() { Instruct = "iexplore", IconName = "ie", ShowName = "IE" });
            this.CurrentJsonModel.SystemModelList.Add(new SystemModel() { Instruct = "mspaint.exe", IconName = "hb", ShowName = "画板" });
            this.CurrentJsonModel.SystemModelList.Add(new SystemModel() { Instruct = "calc", IconName = "jsq", ShowName = "计算器" });
            this.CurrentJsonModel.SystemModelList.Add(new SystemModel() { Instruct = "services.msc", IconName = "fw", ShowName = "服务" });
            this.CurrentJsonModel.SystemModelList.Add(new SystemModel() { Instruct = "mstsc", IconName = "yczm", ShowName = "远程桌面" });
            this.CurrentJsonModel.SystemModelList.Add(new SystemModel() { Instruct = "regedit.exe", IconName = "zcb", ShowName = "注册表" });
            this.CurrentJsonModel.SystemModelList.Add(new SystemModel() { Instruct = "shutdown -s -t", IconName = "jsq", ShowName = "关机", OtherInstruct = true });
        }

    }
}
