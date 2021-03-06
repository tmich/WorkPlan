﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WorkPlan
{
    public partial class DlgVoceBusta : Form
    {
        public DlgVoceBusta()
        {
            InitializeComponent();
        }

        public DlgVoceBusta(VocePaga vp)
            : this()
        {
            Descrizione = vp.Descrizione;
            Importo = vp.Importo;
        }

        public string Descrizione
        {
            get
            {
                return txtDescrizione.Text.Trim();
            }

            set
            {
                txtDescrizione.Text = value;
            }
        }

        public double Importo
        {
            get
            {
                return Double.Parse(txtImporto.Text);
            }

            set
            {
                txtImporto.Text = value.ToString();
            }
        }

        private void DlgVoceBusta_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.OK)
            {
                // Controllo Descrizione
                if(Descrizione == String.Empty)
                {
                    GuiUtils.Warning("Inserire una descrizione", this);
                    txtDescrizione.Focus();
                    e.Cancel = true;
                    return;
                }

                // Controllo Importo
                try
                {
                    double okimp = Importo;
                }
                catch (FormatException)
                {
                    GuiUtils.Warning("Controllare l'importo", this);
                    txtImporto.Focus();
                    e.Cancel = true;
                    return;
                }
            }
        }

        private void txtImporto_KeyDown(object sender, KeyEventArgs e)
        {
            // Initialize the flag to false.
            bool nonNumberEntered = false;

            // Determine whether the keystroke is a number from the top of the keyboard.
            if (e.KeyCode < Keys.D0 || e.KeyCode > Keys.D9)
            {
                // Determine whether the keystroke is a number from the keypad.
                if (e.KeyCode < Keys.NumPad0 || e.KeyCode > Keys.NumPad9)
                {
                    // Determine whether the keystroke is a backspace.
                    if (e.KeyCode != Keys.Back)
                    {
                        // A non-numerical keystroke was pressed.
                        // Set the flag to true and evaluate in KeyPress event.
                        nonNumberEntered = true;
                    }
                }
            }

            //If shift key was pressed, it's not a number.
            if (Control.ModifierKeys == Keys.Shift)
            {
                nonNumberEntered = true;
            }

            // la virgola è ammessa
            if (e.KeyCode == Keys.Oemcomma)
                nonNumberEntered = false;

            // se non è numerico, sopprimo il carattere
            if (nonNumberEntered)
                e.SuppressKeyPress = true;
        }
    }
}
