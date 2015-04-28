namespace UserInterface
{
    partial class RouteCreationForm
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
            this.labelKey = new System.Windows.Forms.Label();
            this.labelValue = new System.Windows.Forms.Label();
            this.labelResponse = new System.Windows.Forms.Label();
            this.textBoxKey = new System.Windows.Forms.TextBox();
            this.textBoxValue = new System.Windows.Forms.TextBox();
            this.richTextBoxResponse = new System.Windows.Forms.RichTextBox();
            this.buttonAddRoute = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelKey
            // 
            this.labelKey.AutoSize = true;
            this.labelKey.ForeColor = System.Drawing.Color.White;
            this.labelKey.Location = new System.Drawing.Point(13, 50);
            this.labelKey.Name = "labelKey";
            this.labelKey.Size = new System.Drawing.Size(31, 13);
            this.labelKey.TabIndex = 0;
            this.labelKey.Text = "Key :";
            // 
            // labelValue
            // 
            this.labelValue.AutoSize = true;
            this.labelValue.ForeColor = System.Drawing.Color.White;
            this.labelValue.Location = new System.Drawing.Point(12, 85);
            this.labelValue.Name = "labelValue";
            this.labelValue.Size = new System.Drawing.Size(40, 13);
            this.labelValue.TabIndex = 1;
            this.labelValue.Text = "Value :";
            // 
            // labelResponse
            // 
            this.labelResponse.AutoSize = true;
            this.labelResponse.ForeColor = System.Drawing.Color.White;
            this.labelResponse.Location = new System.Drawing.Point(12, 119);
            this.labelResponse.Name = "labelResponse";
            this.labelResponse.Size = new System.Drawing.Size(61, 13);
            this.labelResponse.TabIndex = 2;
            this.labelResponse.Text = "Response :";
            // 
            // textBoxKey
            // 
            this.textBoxKey.Location = new System.Drawing.Point(87, 47);
            this.textBoxKey.Name = "textBoxKey";
            this.textBoxKey.Size = new System.Drawing.Size(100, 20);
            this.textBoxKey.TabIndex = 3;
            // 
            // textBoxValue
            // 
            this.textBoxValue.Location = new System.Drawing.Point(87, 82);
            this.textBoxValue.Name = "textBoxValue";
            this.textBoxValue.Size = new System.Drawing.Size(100, 20);
            this.textBoxValue.TabIndex = 4;
            // 
            // richTextBoxResponse
            // 
            this.richTextBoxResponse.Location = new System.Drawing.Point(87, 119);
            this.richTextBoxResponse.Name = "richTextBoxResponse";
            this.richTextBoxResponse.Size = new System.Drawing.Size(100, 96);
            this.richTextBoxResponse.TabIndex = 5;
            this.richTextBoxResponse.Text = "";
            // 
            // buttonAddRoute
            // 
            this.buttonAddRoute.Location = new System.Drawing.Point(12, 246);
            this.buttonAddRoute.Name = "buttonAddRoute";
            this.buttonAddRoute.Size = new System.Drawing.Size(175, 23);
            this.buttonAddRoute.TabIndex = 6;
            this.buttonAddRoute.Text = "Add Route";
            this.buttonAddRoute.UseVisualStyleBackColor = true;
            this.buttonAddRoute.Click += new System.EventHandler(this.buttonAddRoute_Click);
            // 
            // RouteCreationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.ClientSize = new System.Drawing.Size(205, 301);
            this.Controls.Add(this.buttonAddRoute);
            this.Controls.Add(this.richTextBoxResponse);
            this.Controls.Add(this.textBoxValue);
            this.Controls.Add(this.textBoxKey);
            this.Controls.Add(this.labelResponse);
            this.Controls.Add(this.labelValue);
            this.Controls.Add(this.labelKey);
            this.Name = "RouteCreationForm";
            this.Text = "RouteCreationForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelKey;
        private System.Windows.Forms.Label labelValue;
        private System.Windows.Forms.Label labelResponse;
        private System.Windows.Forms.TextBox textBoxKey;
        private System.Windows.Forms.TextBox textBoxValue;
        private System.Windows.Forms.RichTextBox richTextBoxResponse;
        private System.Windows.Forms.Button buttonAddRoute;
    }
}