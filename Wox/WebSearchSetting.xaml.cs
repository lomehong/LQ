﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Wox.Helper;
using Wox.Infrastructure;
using Wox.Infrastructure.UserSettings;
using MessageBox = System.Windows.MessageBox;

namespace Wox
{
    public partial class WebSearchSetting : Window
    {
        private SettingWidow settingWidow;
        private bool update;
        private WebSearch updateWebSearch;

        public WebSearchSetting(SettingWidow settingWidow)
        {
            this.settingWidow = settingWidow;
            InitializeComponent();
        }

        public void UpdateItem(WebSearch webSearch)
        {
            updateWebSearch = CommonStorage.Instance.UserSetting.WebSearches.FirstOrDefault(o => o == webSearch);
            if (updateWebSearch == null || string.IsNullOrEmpty(updateWebSearch.Url))
            {
                MessageBox.Show("无效的网络搜索");
                Close();
                return;
            }

            update = true;
            lblAdd.Text = "更新";
            tbIconPath.Text = webSearch.IconPath;
            ShowIcon(webSearch.IconPath);
            cbEnable.IsChecked = webSearch.Enabled;
            tbTitle.Text = webSearch.Title;
            tbUrl.Text = webSearch.Url;
            tbActionword.Text = webSearch.ActionWord;
        }

        private void ShowIcon(string path)
        {
            try
            {
                imgIcon.Source = new BitmapImage(new Uri(path));
            }
            catch (Exception)
            {
            }
        }

        private void BtnCancel_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnAdd_OnClick(object sender, RoutedEventArgs e)
        {
            string title = tbTitle.Text;
            if (string.IsNullOrEmpty(title))
            {
                MessageBox.Show("请输入标题字段");
                return;
            }

            string url = tbUrl.Text;
            if (string.IsNullOrEmpty(url))
            {
                MessageBox.Show("请输入URL字段");
                return;
            }

            string action = tbActionword.Text;
            if (string.IsNullOrEmpty(action))
            {
                MessageBox.Show("请输入ActionWord字段");
                return;
            }


            if (!update)
            {
                if (CommonStorage.Instance.UserSetting.WebSearches.Exists(o => o.ActionWord == action))
                {
                    MessageBox.Show("ActionWord已经存在，请输入一个新的。");
                    return;
                }
                CommonStorage.Instance.UserSetting.WebSearches.Add(new WebSearch()
                {
                    ActionWord = action,
                    Enabled = cbEnable.IsChecked ?? false,
                    IconPath = tbIconPath.Text,
                    Url = url,
                    Title = title
                });
                MessageBox.Show(string.Format("成功添加了新的Web搜索 {0} !", title));
            }
            else
            {
                updateWebSearch.ActionWord = action;
                updateWebSearch.IconPath = tbIconPath.Text;
                updateWebSearch.Enabled = cbEnable.IsChecked ?? false;
                updateWebSearch.Url = url;
                updateWebSearch.Title= title;
                MessageBox.Show(string.Format("更新Web搜索 {0} 成功!", title));
            }
            CommonStorage.Instance.Save();
            settingWidow.ReloadWebSearchView();
            Close();
        }

        private void BtnSelectIcon_OnClick(object sender, RoutedEventArgs e)
        {
            var dlg = new Microsoft.Win32.OpenFileDialog
            {
                DefaultExt = ".png",
                Filter =
                    "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif"
            };

            bool? result = dlg.ShowDialog();
            if (result == true)
            {
                string filename = dlg.FileName;
                tbIconPath.Text = filename;
                ShowIcon(filename);
            }
        }
    }
}
