using System.ComponentModel;
using System.Windows.Forms;

namespace Presentation
{
    partial class ContestViewForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.contestTasksTable = new System.Windows.Forms.DataGridView();
            this.participantsTable = new System.Windows.Forms.DataGridView();
            this.participantNameField = new System.Windows.Forms.TextBox();
            this.ageParticipantField = new System.Windows.Forms.TextBox();
            this.contestTasksIdsParticipantField = new System.Windows.Forms.TextBox();
            this.insertParticipantButton = new System.Windows.Forms.Button();
            this.searchParticipantByContestTaskIdButton = new System.Windows.Forms.Button();
            this.contestTasksIdsField = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize) (this.contestTasksTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.participantsTable)).BeginInit();
            this.SuspendLayout();
            // 
            // contestTasksTable
            // 
            this.contestTasksTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.contestTasksTable.Location = new System.Drawing.Point(32, 28);
            this.contestTasksTable.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.contestTasksTable.Name = "contestTasksTable";
            this.contestTasksTable.RowHeadersWidth = 82;
            this.contestTasksTable.RowTemplate.Height = 33;
            this.contestTasksTable.Size = new System.Drawing.Size(653, 432);
            this.contestTasksTable.TabIndex = 0;
            // 
            // participantsTable
            // 
            this.participantsTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.participantsTable.Location = new System.Drawing.Point(712, 99);
            this.participantsTable.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.participantsTable.Name = "participantsTable";
            this.participantsTable.RowHeadersWidth = 82;
            this.participantsTable.RowTemplate.Height = 33;
            this.participantsTable.Size = new System.Drawing.Size(587, 361);
            this.participantsTable.TabIndex = 1;
            // 
            // participantNameField
            // 
            this.participantNameField.Location = new System.Drawing.Point(176, 512);
            this.participantNameField.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.participantNameField.Name = "participantNameField";
            this.participantNameField.Size = new System.Drawing.Size(267, 31);
            this.participantNameField.TabIndex = 2;
            // 
            // ageParticipantField
            // 
            this.ageParticipantField.Location = new System.Drawing.Point(176, 580);
            this.ageParticipantField.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ageParticipantField.Name = "ageParticipantField";
            this.ageParticipantField.Size = new System.Drawing.Size(267, 31);
            this.ageParticipantField.TabIndex = 3;
            // 
            // contestTasksIdsParticipantField
            // 
            this.contestTasksIdsParticipantField.Location = new System.Drawing.Point(176, 656);
            this.contestTasksIdsParticipantField.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.contestTasksIdsParticipantField.Name = "contestTasksIdsParticipantField";
            this.contestTasksIdsParticipantField.Size = new System.Drawing.Size(267, 31);
            this.contestTasksIdsParticipantField.TabIndex = 4;
            // 
            // insertParticipantButton
            // 
            this.insertParticipantButton.Location = new System.Drawing.Point(222, 721);
            this.insertParticipantButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.insertParticipantButton.Name = "insertParticipantButton";
            this.insertParticipantButton.Size = new System.Drawing.Size(183, 42);
            this.insertParticipantButton.TabIndex = 5;
            this.insertParticipantButton.Text = "adaugaParticipant";
            this.insertParticipantButton.UseVisualStyleBackColor = true;
            this.insertParticipantButton.Click += new System.EventHandler(this.insertParticipantButton_Click);
            // 
            // searchParticipantByContestTaskIdButton
            // 
            this.searchParticipantByContestTaskIdButton.Location = new System.Drawing.Point(1062, 44);
            this.searchParticipantByContestTaskIdButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.searchParticipantByContestTaskIdButton.Name = "searchParticipantByContestTaskIdButton";
            this.searchParticipantByContestTaskIdButton.Size = new System.Drawing.Size(144, 42);
            this.searchParticipantByContestTaskIdButton.TabIndex = 6;
            this.searchParticipantByContestTaskIdButton.Text = "search";
            this.searchParticipantByContestTaskIdButton.UseVisualStyleBackColor = true;
            this.searchParticipantByContestTaskIdButton.Click += new System.EventHandler(this.searchParticipantByContestTaskIdButton_Click);
            // 
            // contestTasksIdsField
            // 
            this.contestTasksIdsField.Location = new System.Drawing.Point(726, 51);
            this.contestTasksIdsField.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.contestTasksIdsField.Name = "contestTasksIdsField";
            this.contestTasksIdsField.Size = new System.Drawing.Size(315, 31);
            this.contestTasksIdsField.TabIndex = 7;
            // 
            // ContestViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1333, 865);
            this.Controls.Add(this.contestTasksIdsField);
            this.Controls.Add(this.searchParticipantByContestTaskIdButton);
            this.Controls.Add(this.insertParticipantButton);
            this.Controls.Add(this.contestTasksIdsParticipantField);
            this.Controls.Add(this.ageParticipantField);
            this.Controls.Add(this.participantNameField);
            this.Controls.Add(this.participantsTable);
            this.Controls.Add(this.contestTasksTable);
            this.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.Name = "ContestViewForm";
            this.Text = "ContestViewForm";
            this.Load += new System.EventHandler(this.ContestViewForm_Load);
            ((System.ComponentModel.ISupportInitialize) (this.contestTasksTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.participantsTable)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.DataGridView contestTasksTable;
        private System.Windows.Forms.DataGridView participantsTable;
        private System.Windows.Forms.TextBox participantNameField;
        private System.Windows.Forms.TextBox ageParticipantField;
        private System.Windows.Forms.TextBox contestTasksIdsParticipantField;
        private System.Windows.Forms.Button insertParticipantButton;
        private System.Windows.Forms.Button searchParticipantByContestTaskIdButton;
        private System.Windows.Forms.TextBox contestTasksIdsField;
    }
}