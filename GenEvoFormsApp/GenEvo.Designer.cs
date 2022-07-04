
namespace GenEvoFormsApp
{
    partial class frmGenEvo
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
            this.grpSimStatus = new System.Windows.Forms.GroupBox();
            this.lblCurrentGenLabel = new System.Windows.Forms.Label();
            this.lblCurrentGen = new System.Windows.Forms.Label();
            this.lblSlash1 = new System.Windows.Forms.Label();
            this.lblGenerationsCount = new System.Windows.Forms.Label();
            this.lblCurrentStepLabel = new System.Windows.Forms.Label();
            this.lblCurrentStep = new System.Windows.Forms.Label();
            this.lblSlash2 = new System.Windows.Forms.Label();
            this.LblStepsPerGen = new System.Windows.Forms.Label();
            this.grpSimStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpSimStatus
            // 
            this.grpSimStatus.Controls.Add(this.LblStepsPerGen);
            this.grpSimStatus.Controls.Add(this.lblGenerationsCount);
            this.grpSimStatus.Controls.Add(this.lblSlash2);
            this.grpSimStatus.Controls.Add(this.lblSlash1);
            this.grpSimStatus.Controls.Add(this.lblCurrentStep);
            this.grpSimStatus.Controls.Add(this.lblCurrentGen);
            this.grpSimStatus.Controls.Add(this.lblCurrentStepLabel);
            this.grpSimStatus.Controls.Add(this.lblCurrentGenLabel);
            this.grpSimStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpSimStatus.Location = new System.Drawing.Point(20, 22);
            this.grpSimStatus.Name = "grpSimStatus";
            this.grpSimStatus.Size = new System.Drawing.Size(309, 586);
            this.grpSimStatus.TabIndex = 0;
            this.grpSimStatus.TabStop = false;
            this.grpSimStatus.Text = "Simulation status";
            // 
            // lblCurrentGenLabel
            // 
            this.lblCurrentGenLabel.AutoSize = true;
            this.lblCurrentGenLabel.Location = new System.Drawing.Point(6, 31);
            this.lblCurrentGenLabel.Name = "lblCurrentGenLabel";
            this.lblCurrentGenLabel.Size = new System.Drawing.Size(142, 20);
            this.lblCurrentGenLabel.TabIndex = 0;
            this.lblCurrentGenLabel.Text = "Current generation";
            // 
            // lblCurrentGen
            // 
            this.lblCurrentGen.AutoSize = true;
            this.lblCurrentGen.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentGen.Location = new System.Drawing.Point(154, 31);
            this.lblCurrentGen.Name = "lblCurrentGen";
            this.lblCurrentGen.Size = new System.Drawing.Size(18, 20);
            this.lblCurrentGen.TabIndex = 0;
            this.lblCurrentGen.Text = "0";
            this.lblCurrentGen.Click += new System.EventHandler(this.label1_Click);
            // 
            // lblSlash1
            // 
            this.lblSlash1.AutoSize = true;
            this.lblSlash1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSlash1.Location = new System.Drawing.Point(195, 31);
            this.lblSlash1.Name = "lblSlash1";
            this.lblSlash1.Size = new System.Drawing.Size(13, 20);
            this.lblSlash1.TabIndex = 0;
            this.lblSlash1.Text = "/";
            this.lblSlash1.Click += new System.EventHandler(this.label1_Click);
            // 
            // lblGenerationsCount
            // 
            this.lblGenerationsCount.AutoSize = true;
            this.lblGenerationsCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGenerationsCount.Location = new System.Drawing.Point(207, 31);
            this.lblGenerationsCount.Name = "lblGenerationsCount";
            this.lblGenerationsCount.Size = new System.Drawing.Size(18, 20);
            this.lblGenerationsCount.TabIndex = 0;
            this.lblGenerationsCount.Text = "0";
            this.lblGenerationsCount.Click += new System.EventHandler(this.label1_Click);
            // 
            // lblCurrentStepLabel
            // 
            this.lblCurrentStepLabel.AutoSize = true;
            this.lblCurrentStepLabel.Location = new System.Drawing.Point(6, 62);
            this.lblCurrentStepLabel.Name = "lblCurrentStepLabel";
            this.lblCurrentStepLabel.Size = new System.Drawing.Size(97, 20);
            this.lblCurrentStepLabel.TabIndex = 0;
            this.lblCurrentStepLabel.Text = "Current step";
            // 
            // lblCurrentStep
            // 
            this.lblCurrentStep.AutoSize = true;
            this.lblCurrentStep.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentStep.Location = new System.Drawing.Point(154, 62);
            this.lblCurrentStep.Name = "lblCurrentStep";
            this.lblCurrentStep.Size = new System.Drawing.Size(18, 20);
            this.lblCurrentStep.TabIndex = 0;
            this.lblCurrentStep.Text = "0";
            this.lblCurrentStep.Click += new System.EventHandler(this.label1_Click);
            // 
            // lblSlash2
            // 
            this.lblSlash2.AutoSize = true;
            this.lblSlash2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSlash2.Location = new System.Drawing.Point(195, 62);
            this.lblSlash2.Name = "lblSlash2";
            this.lblSlash2.Size = new System.Drawing.Size(13, 20);
            this.lblSlash2.TabIndex = 0;
            this.lblSlash2.Text = "/";
            this.lblSlash2.Click += new System.EventHandler(this.label1_Click);
            // 
            // LblStepsPerGen
            // 
            this.LblStepsPerGen.AutoSize = true;
            this.LblStepsPerGen.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblStepsPerGen.Location = new System.Drawing.Point(207, 62);
            this.LblStepsPerGen.Name = "LblStepsPerGen";
            this.LblStepsPerGen.Size = new System.Drawing.Size(18, 20);
            this.LblStepsPerGen.TabIndex = 0;
            this.LblStepsPerGen.Text = "0";
            this.LblStepsPerGen.Click += new System.EventHandler(this.label1_Click);
            // 
            // frmGenEvo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(944, 626);
            this.Controls.Add(this.grpSimStatus);
            this.Name = "frmGenEvo";
            this.Text = "Genetic evolution demo";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.grpSimStatus.ResumeLayout(false);
            this.grpSimStatus.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpSimStatus;
        private System.Windows.Forms.Label lblCurrentGenLabel;
        private System.Windows.Forms.Label lblCurrentGen;
        private System.Windows.Forms.Label LblStepsPerGen;
        private System.Windows.Forms.Label lblGenerationsCount;
        private System.Windows.Forms.Label lblSlash2;
        private System.Windows.Forms.Label lblSlash1;
        private System.Windows.Forms.Label lblCurrentStep;
        private System.Windows.Forms.Label lblCurrentStepLabel;
    }
}

