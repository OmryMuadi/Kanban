﻿using Frontend.Model;
using System;

namespace Frontend.ViewModel
{
    public class MainViewModel : NotifiableObject
    {
        public BackendController Controller { get; private set; }

        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                this._username = value;
                RaisePropertyChanged("Username");
            }
        }
        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                this._password = value;
                RaisePropertyChanged("Password");
            }
        }
        private string _message;
        public string Message
        {
            get => _message;
            set
            {
                this._message = value;
                RaisePropertyChanged("Message");
            }
        }
        /// <summary>
        /// login function
        /// </summary>
        /// <returns></returns>
        public UserModel Login()
        {
            Message = "";
            try
            {
                return Controller.Login(Username, Password);
            }
            catch (Exception e)
            {
                Message = e.Message;
                return null;
            }
        }
        /// <summary>
        /// register function
        /// </summary>
        /// <returns></returns>
        public UserModel Register()
        {
            Message = "";
            try
            {
                Message = "Registered successfully";
                return Controller.Register(Username, Password);
            }
            catch (Exception e)
            {
                Message = e.Message;
                return null;
            }
        }

        /// <summary>
        /// this is responsible for showing datas on the window
        /// </summary>
        public MainViewModel()
        {
            this.Controller = new BackendController();
            this.Username = "";
            this.Password = "";
        }
    }
}
