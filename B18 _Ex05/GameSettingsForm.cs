using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace B18__Ex05
{
    public partial class GameSettingsForm : Form
    {
        private int m_BoardSize;

        public GameSettingsForm()
        {
            InitializeComponent();
        }

        private void checkBoxPlayer2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxPlayer2.Checked)
            {
                textBoxPlayer2.Enabled = true;
            }
            else
            {
                textBoxPlayer2.Enabled = false;
            }
        }

        private void buttonDone_Click(object sender, EventArgs e)
        {
            if (checkIfPlayerNameIsValid(textBoxPlayer1.Text) && checkIfPlayerNameIsValid(textBoxPlayer2.Text))
            {
                updateBoardSize();
                this.Close();
            }
            else
            {
                DialogResult result;
                result = MessageBox.Show(
@"Please enter Valid Names.
At least one character and not over 20 characters",
                    "ERROR",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void updateBoardSize()
        {
            if (radioButton6x6.Checked)
            {
                m_BoardSize = 6;
            }
            else if (radioButton8x8.Checked)
            {
                m_BoardSize = 8;
            }
            else
            {
                m_BoardSize = 10;
            }
        }

        public string NameOfPlayer1
        {
            get
            {
                return this.textBoxPlayer1.Text;
            }
        }

        public string NameOfPlayer2
        {
            get
            {
                return this.textBoxPlayer2.Text;
            }
        }

        public bool ComputerPlay
        {
            get
            {
                return !this.checkBoxPlayer2.Checked;
            }
        }

        private bool checkIfPlayerNameIsValid(string i_NameOfPlayer)
        {
            bool isNameOk = true;

            if (i_NameOfPlayer.Contains(" ") || i_NameOfPlayer.Length > 20 || i_NameOfPlayer == string.Empty)
            {
                isNameOk = false;
            }

            return isNameOk;
        }

        public int SizeOfBoard
        {
            get
            {
                return m_BoardSize;
            }
        }
    }
}