﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Xamarin.Forms;
using Newtonsoft.Json;

namespace COVID19_Part2_Mobile
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        private static RSAParameters publicKey;
        public MainPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private async void btnUnlock_Clicked(object sender, EventArgs e)
        {
            var password = await DisplayPromptAsync("Master Login", "Please key in master password");
            using (var webClient = new WebClient())
            {
                webClient.Headers.Add("Content-Type", "application/json");
                var response = await webClient.UploadDataTaskAsync($"http://10.0.2.2:51908/LoginUsers/GetPublicKey?password={password}", "POST", Encoding.UTF8.GetBytes(""));
                if (Encoding.Default.GetString(response) == "\"Password incorrect! Application will now close!\"")
                {
                    await DisplayAlert("Master Login", "Password incorrect! Application will now close!", "Ok");
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                }
                else
                {
                    publicKey = JsonConvert.DeserializeObject<RSAParameters>(Encoding.Default.GetString(response));
                    await DisplayAlert("Master Login", "Deserialised completed", "Ok");
                    GridFields.IsVisible = true;
                    btnUnlock.IsVisible = false;
                }

            }
        }
    }
}
