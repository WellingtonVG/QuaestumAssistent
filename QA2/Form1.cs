using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Speech.Recognition;
using System.Speech.Synthesis;
using System.Globalization;

namespace QA2
{
    public partial class Form1 : Form
    {
        static CultureInfo ci = new CultureInfo("pt-BR");
        static SpeechRecognitionEngine reconhecedor;
        SpeechSynthesizer resposta = new SpeechSynthesizer();

        public string[] listaPalavras = { "oi", "quem é você", "como você está", "quaestum" };

        public Form1()
        {
            InitializeComponent();

            //this.WindowState = FormWindowState.Minimized;
            //this.ShowInTaskbar = false;

            Init();
        }


        public void Gramatica()
        {
            try
            {
                reconhecedor = new SpeechRecognitionEngine(ci);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao integrar Lingua escolhoida" + ex.Message);
            }

            var gramatica = new Choices();
            gramatica.Add(listaPalavras);

            var gb = new GrammarBuilder();
            gb.Append(gramatica);

            try
            {
                var g = new Grammar(gb);

                try
                {
                    reconhecedor.RequestRecognizerUpdate();
                    reconhecedor.LoadGrammarAsync(g);
                    reconhecedor.SpeechRecognized += Sre_Reconhecimento;
                    reconhecedor.SetInputToDefaultAudioDevice();
                    resposta.SetOutputToDefaultAudioDevice();
                    reconhecedor.RecognizeAsync(RecognizeMode.Multiple);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao criar gramática: " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao criar reconhecedor:  " + ex.Message);
            }
        }
        public void Init()
        {
            resposta.Volume = 75;
            resposta.Rate = 2;

            Gramatica();
        }

        void Sre_Reconhecimento(object sender, SpeechRecognizedEventArgs e)
        {
            string frase = e.Result.Text;

            if (frase.Equals("oi"))
            {
                resposta.SpeakAsync("Oi, como você está ?");
            }
            else if (frase.Equals("quaestum"))
            {
                this.Alert("Quaestum");
            }
        }

        public QuaestumAssistent frm;

        public void Alert(string msg)
        {
            this.frm = new QuaestumAssistent( );
            this.frm.showAlert(msg);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Alert("Quaestum");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                this.frm.comando("painel");
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Erro:  " + ex.Message);
            }
        }
    }
}
