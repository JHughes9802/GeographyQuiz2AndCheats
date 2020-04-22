using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GeographyQuiz2
{
    public partial class CheatForm : Form
    {

        // Not much to comment on here when all I did was add 4 lines of code
        public CheatForm()
        {
            InitializeComponent();
        }

        private void CheatForm_Load(object sender, EventArgs e)
        {
            lblAnswer.Text = Tag.ToString();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnLazy_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
