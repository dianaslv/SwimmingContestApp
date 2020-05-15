using Core.Models;
using Microsoft.Extensions.Logging;
using Networking.Network;
using Networking.NetworkModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentation
{
    public partial class ContestViewForm : Form
    {
        private User User { get; set; }
        private readonly ILogger<ContestViewForm> m_logger;
        private readonly Client m_client;

        public ContestViewForm(ILogger<ContestViewForm> logger, Client client)
        {
            m_logger = logger;
            m_client = client;
            m_client.Observer =  new Observer {ToInvoke = async () => await FillParticipantsView(participantsTable)};
            InitializeComponent();
        }

        public void SetUser(User loggedUser)
        {
            User = loggedUser;
        }


        private async Task FillParticipantsView(DataGridView dataGridView)
        {
            await m_client.SendMessageAsync(new Request
            {
                RequestType = RequestType.SearchParticipants,
                Body = string.Empty
            });
            var result = await m_client.GetResponse<DataSet>(RequestType.SearchParticipants);
            
            participantsTable.Invoke(new Action(() => {participantsTable.DataSource = result.Tables[0]; }));
        }

        private async Task FillContestTaskView(DataGridView view)
        {
            await m_client.SendMessageAsync(new Request
            {
                RequestType = RequestType.SearchContestTasks,
                Body = string.Empty
            });
            var result = await m_client.GetResponse<DataSet>(RequestType.SearchContestTasks);
            if (result == null)
            {
                m_logger.LogError($"Empty Data on Search");
                return;
            }
            contestTasksTable.Invoke(new Action(() => {contestTasksTable.DataSource = result.Tables[0]; }));
        }


        private async void insertParticipantButton_Click(object sender, System.EventArgs e)
        {
            try
            {
                await InsertParticipantAsync();
            }
            catch (Exception ex)
            {
                m_logger.LogWarning(ex, "Could not insert person");
            }
        }

        private async Task InsertParticipantAsync()
        {
            var idParticipant = System.Guid.NewGuid().ToString();
            await m_client.SendMessageAsync(new Request
            {
                RequestType = RequestType.AddParticipant,
                Body = JsonConvert.SerializeObject(new Dictionary<string, string>
                {
                    {"idParticipant", idParticipant},
                    {"participantName", participantNameField.Text},
                    {"participantAge", ageParticipantField.Text}
                })
            });
            var response = await m_client.GetResponse(RequestType.AddParticipant);

            if (!response.Success)
            {
                m_logger.LogError("Could not insert person");
                return;
            }

            await m_client.SendMessageAsync(new Request
            {
                RequestType = RequestType.AddEntry,
                Body = JsonConvert.SerializeObject(new Dictionary<string, string>
                {
                    {"idParticipant", idParticipant},
                    {"contestTasksIdsParticipant", contestTasksIdsParticipantField.Text}
                })
            });
            response = await m_client.GetResponse(RequestType.AddEntry);

            if (!response.Success)
            {
                m_logger.LogError("Could not insert entry");
                return;
            }

            await FillContestTaskView(contestTasksTable);
            await FillParticipantsView(participantsTable);
        }

        private async void ContestViewForm_Load(object sender, EventArgs e)
        {
            await FillContestTaskView(contestTasksTable);
            await FillParticipantsView(participantsTable);
        }

        private async void searchParticipantByContestTaskIdButton_Click(object sender, EventArgs e)
        {
            await m_client.SendMessageAsync(new Request
            {
                RequestType = RequestType.SearchParticipantsJoin,
                Body = JsonConvert.SerializeObject(new Dictionary<string, string>
                {
                    {"idTask", contestTasksIdsField.Text},
                })
            });
            var result = await m_client.GetResponse<DataSet>(RequestType.SearchParticipantsJoin);
            if (result == null)
            {
                m_logger.LogError($"Empty Data on Search");
                return;
            }
            participantsTable.Invoke(new Action(() => {participantsTable.DataSource = result.Tables[0]; }));
        }

        private void contestTasksIdsField_Click(object sender, EventArgs e)
        {

        }
    }
}