using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Autofac;
using Core.Models;
using Microsoft.Extensions.Logging;
using Networking.Network;
using Networking.NetworkModels;
using Newtonsoft.Json;

namespace Presentation
{
    public partial class LoginForm : Form
    {
        private readonly Client m_client;
        private readonly ILogger<LoginForm> m_logger;

        public LoginForm(ILogger<LoginForm> logger, Client client)
        {
            m_logger = logger;
            m_client = client;
            DependencyInjector.ServiceProvider.Resolve<Client>().StartClient();
            InitializeComponent();
        }

        private void ExitButton_Click(object sender, System.EventArgs e)
        {
            Application.Exit();
        }

       

        private void StartArtistViewForm(User loggedUser)
        {
            var artistViewForm = DependencyInjector.ServiceProvider.Resolve<ContestViewForm>();
            artistViewForm.SetUser(loggedUser);
            Hide();
            artistViewForm.Show();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            await m_client.SendMessageAsync(new Request
            {
                RequestType = RequestType.Login,
                Body = JsonConvert.SerializeObject(new Dictionary<string, string>
                {
                    {"username", usernameTextBox.Text},
                    {"password", passwordTextBox.Text}
                })
            });
            var user = await m_client.GetResponse<User>(RequestType.Login);
            if (user == null)
                m_logger.LogWarning("Invalid Username Or Password");
            StartArtistViewForm(user);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}