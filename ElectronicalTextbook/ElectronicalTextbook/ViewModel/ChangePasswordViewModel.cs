﻿using ElectronicalTextbook.Model.Supported;
using ElectronicalTextbook.View;
using ElectronicalTextbook;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ElectronicalTextbook.ViewModel
{
    public class ChangePasswordViewModel : CalledViewModel<ChangePasswordWindow>
    {
        public string User { get; set; }
        public ChangePasswordViewModel(Window caller) : base(caller)
        {
            window.newPassword.PasswordChanged += OnNewPasswordChanged;
            window.confirmPassword.PasswordChanged += OnConfirmPasswordChanged;
            window.confirm.Click += OnConfirmButtonClick;
        }
        public override CalledViewModel<ChangePasswordWindow> Init(object value)
        {
            foreach (var item in DataBaseProcessor.GetUserNames())
            {
                window.user.Items.Add(item);
            }
            window.user.SelectedItem = 0;
            window.user.SelectedItem = (string)value;
            return this;
        }
        private void OnNewPasswordChanged(object sender, RoutedEventArgs e)
        {
            CheckPasswords();
        }

        private void OnConfirmPasswordChanged(object sender, RoutedEventArgs e)
        {
            CheckPasswords();

        }

        private void CheckPasswords()
        {
            string newPas = window.newPassword.Password;
            string confirmPas = window.confirmPassword.Password;
            window.confirm.IsEnabled = PasswordChecker
                .IsCorrectNew(newPas, confirmPas, out string success);
            window.error.Text = success;
        }

        private void OnConfirmButtonClick(object sender, RoutedEventArgs e)
        {
            string newPassword = window.newPassword.Password;
            var user = window.user.SelectedItem;
            if(user is null)
            {
                window.error.Text = "Некорректное имя пользователя";
                return;
            }
            string username = user.ToString();
            if (!DataBaseProcessor.IsUsernameAvaliable(username))
            {
                if(PasswordChecker.IsCorrect(newPassword, out var message))
                {

                DataBaseProcessor.ChangePassword(username, newPassword);
                Close();
                }
                else
                {
                    window.error.Text = message;
                }
            }
            else
            {
                window.error.Text = "Пользователь не найден";
            }

        }
    }
}
