namespace MountainFinance
{
    partial class FormSetSavingGoal
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.labelSetSGoalTitle = new System.Windows.Forms.Label();
            this.buttonSaveGoal = new System.Windows.Forms.Button();
            this.textBoxSavingGoal = new System.Windows.Forms.TextBox();
            this.labelSavingGoalSetSGoal = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelSetSGoalTitle
            // 
            this.labelSetSGoalTitle.AutoSize = true;
            this.labelSetSGoalTitle.Font = new System.Drawing.Font("Century Gothic", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelSetSGoalTitle.ForeColor = System.Drawing.Color.White;
            this.labelSetSGoalTitle.Location = new System.Drawing.Point(11, 1);
            this.labelSetSGoalTitle.Name = "labelSetSGoalTitle";
            this.labelSetSGoalTitle.Size = new System.Drawing.Size(175, 26);
            this.labelSetSGoalTitle.TabIndex = 41;
            this.labelSetSGoalTitle.Text = "Set saving goal";
            // 
            // buttonSaveGoal
            // 
            this.buttonSaveGoal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSaveGoal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.buttonSaveGoal.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.buttonSaveGoal.FlatAppearance.BorderSize = 0;
            this.buttonSaveGoal.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSaveGoal.Font = new System.Drawing.Font("Century Gothic", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.buttonSaveGoal.ForeColor = System.Drawing.Color.White;
            this.buttonSaveGoal.Location = new System.Drawing.Point(-2, 76);
            this.buttonSaveGoal.Name = "buttonSaveGoal";
            this.buttonSaveGoal.Size = new System.Drawing.Size(450, 48);
            this.buttonSaveGoal.TabIndex = 40;
            this.buttonSaveGoal.Text = "Save";
            this.buttonSaveGoal.UseVisualStyleBackColor = false;
            this.buttonSaveGoal.Click += new System.EventHandler(this.buttonSaveGoal_Click);
            // 
            // textBoxSavingGoal
            // 
            this.textBoxSavingGoal.Font = new System.Drawing.Font("Century Gothic", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.textBoxSavingGoal.Location = new System.Drawing.Point(191, 41);
            this.textBoxSavingGoal.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxSavingGoal.Name = "textBoxSavingGoal";
            this.textBoxSavingGoal.Size = new System.Drawing.Size(225, 30);
            this.textBoxSavingGoal.TabIndex = 38;
            // 
            // labelSavingGoalSetSGoal
            // 
            this.labelSavingGoalSetSGoal.AutoSize = true;
            this.labelSavingGoalSetSGoal.Font = new System.Drawing.Font("Century Gothic", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelSavingGoalSetSGoal.ForeColor = System.Drawing.Color.White;
            this.labelSavingGoalSetSGoal.Location = new System.Drawing.Point(50, 44);
            this.labelSavingGoalSetSGoal.Name = "labelSavingGoalSetSGoal";
            this.labelSavingGoalSetSGoal.Size = new System.Drawing.Size(121, 23);
            this.labelSavingGoalSetSGoal.TabIndex = 37;
            this.labelSavingGoalSetSGoal.Text = "Saving goal";
            // 
            // FormSetSavingGoal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(446, 124);
            this.Controls.Add(this.labelSetSGoalTitle);
            this.Controls.Add(this.buttonSaveGoal);
            this.Controls.Add(this.textBoxSavingGoal);
            this.Controls.Add(this.labelSavingGoalSetSGoal);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "FormSetSavingGoal";
            this.Text = "FormSetSavingGoal";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelSetSGoalTitle;
        private System.Windows.Forms.Button buttonSaveGoal;
        private System.Windows.Forms.TextBox textBoxSavingGoal;
        private System.Windows.Forms.Label labelSavingGoalSetSGoal;
    }
}