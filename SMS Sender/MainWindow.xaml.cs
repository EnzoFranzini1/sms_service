using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Security.Cryptography;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Timers;

namespace SMS_Sender
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Variables Var_class = new Variables();

        public MainWindow()
        {
            InitializeComponent();

            //Exibe na tela a ausência de países selecionados
            if (Lista_de_paises.SelectedItem == null)
            {
                Pais_selecionado.Content = "Nenhum país selecionado";
                Pais_selecionado.Foreground = Brushes.Red;
            }
            
            //Altera a cor dos TextBox para simular um place holder
            DDD.Foreground = Brushes.Gray;
            Numero.Foreground = Brushes.Gray;
            Mensagem.Foreground = Brushes.Gray;
        }

        #region DDD

        //Funções destinadas para behaviors do campo DDD

        private void DDD_GotFocus(object sender, RoutedEventArgs e)
        {
            if (DDD.Text == "DDD")
            {
                DDD.Foreground = Brushes.Black;
                DDD.Text = "";
            }

        }

        private void DDD_LostFocus(object sender, RoutedEventArgs e)
        {
            if (DDD.Text == "")
            {
                DDD.Foreground = Brushes.Gray;
                DDD.Text = "DDD";
            }
        }

        private void DDD_TextChanged(object sender, TextChangedEventArgs e)
        {
            Debug.WriteLine(DDD.Text);

            Var_class.DDD = DDD.Text.ToString();
        }

        #endregion

        #region Numero

        //Funções destinadas para behaviors do campo Número

        private void Numero_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Numero.Text == "Número")
            {
                Numero.Foreground = Brushes.Black;
                Numero.Text = "";
            }
        }

        private void Numero_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Numero.Text == "")
            {
                Numero.Foreground = Brushes.Gray;
                Numero.Text = "Número";
            }
        }

        private void Numero_TextChanged(object sender, TextChangedEventArgs e)
        {
            Var_class.Numero = Numero.Text.ToString();
        }

        #endregion

        #region Mensagem

        //Funções destinadas para behaviors do campo de mensagens

        private void Mensagem_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Mensagem.Text == "Mensagem")
            {
                Mensagem.Foreground = Brushes.Black;
                Mensagem.Text = "";
            }
        }

        private void Mensagem_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Mensagem.Text == "")
            {
                Mensagem.Foreground = Brushes.Gray;
                Mensagem.Text = "Mensagem";
            }
        }

        private void Mensagem_TextChanged(object sender, TextChangedEventArgs e)
        {
            Var_class.Mensagem = Mensagem.Text;
            Debug.WriteLine(Var_class.Mensagem);
        }

        #endregion

        private void Enviar_Click(object sender, RoutedEventArgs e)
        {
            //Limpa campo de resultados e verifica as informações inseridas
            Resultado.Content = "";
            Verificar_Dados();
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            //Restaura as informações inseridas pelo usuário, com exceção do país
            DDD.Text = "DDD";
            DDD.Foreground = Brushes.Gray;

            Mensagem.Text = "Mensagem";
            Mensagem.Foreground = Brushes.Gray;

            Numero.Text = "Número";
            Numero.Foreground = Brushes.Gray;

            Resultado.Content = "";
        }

        private void Verificar_Dados()
        {
            //Realiza a verificação dos dados que foram inseridos e as respectivas validades

            #region Check_Pais

            if (Lista_de_paises.SelectedItem == null)
            {
                MessageBox.Show("Selecione um país", "Message", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                Var_class.Cod_pais = "";

                foreach (char c in Lista_de_paises.SelectedItem.ToString())
                {
                    if (char.IsDigit(c) == true)
                    {
                        Var_class.Cod_pais += c;
                    }
                }
            }


            #endregion

            #region Check_DDD

            if (DDD.Text == "DDD" || DDD.Text.Length != 2)
            {
                //DDD não pode ser vazio
                MessageBox.Show("Insira um formato válido de DDD - 0", "Message", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                foreach (char c in DDD.Text.ToString())
                {
                    if (char.IsDigit(c) == false)
                    {
                        MessageBox.Show("Insira um formato válido de DDD - 1", "Message", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
            }

            #endregion

            #region Check_Numero

            if (Numero.Text == "Número")
            {
                //DDD não pode ser vazio
                MessageBox.Show("Insira um formato de número válido - 0", "Message", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                foreach (char c in Numero.Text.ToString())
                {
                    Debug.WriteLine(c);
                    if (char.IsDigit(c) == false)
                    {
                        MessageBox.Show("Insira um formato de número válido - 1", "Message", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
            }

            #endregion

            #region Check_Mensagem

            if (Mensagem.Text == "Mensagem" && (Mensagem.Foreground == Brushes.Gray == true))
            {
                MessageBox.Show("Insira uma mensagem válida - 0", "Message", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                Debug.WriteLine("Enviaria");
                Enviar_Mensagem();
            }

            #endregion

        }

        private void Enviar_Mensagem()
        {
            //Realiza o envio da mensagem para o destinatário inserido, exibe mensagem de feedback e inicia rotina de mensagem de feedback
            try
            {
                WebClient client = new System.Net.WebClient();
                Stream S = client.OpenRead(string.Format("https://platform.clickatell.com/messages/http/send?apiKey=4yOpE021Q9OVBESLp77EfA==&to={0}&content={1}", Var_class.Cod_pais + Var_class.DDD + Var_class.Numero, Var_class.Mensagem));
                StreamReader reader = new StreamReader(S);
                string result = reader.ReadToEnd();

                Resultado.Content = "Mensagem enviada com sucesso para: +" + Var_class.Cod_pais + " (" + Var_class.DDD + ") " + Var_class.Numero;
                Resultado.Foreground = Brushes.Green;

                Iniciar_Timer_Esconder_Mensagem();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Lista_de_paises_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Exibe para o usuário o país que foi escolhido
            int index = Lista_de_paises.SelectedIndex;

            switch (index)
            {
                case 0:
                    Pais_selecionado.Content = "País selecionado: Brasil (+55)";
                    Pais_selecionado.Foreground = Brushes.Green;
                    break;
                case 1:
                    Pais_selecionado.Content = "País selecionado: País selecionado: EUA (+1)";
                    Pais_selecionado.Foreground = Brushes.Green;
                    break;
                case 2:
                    Pais_selecionado.Content = "País selecionado: País selecionado: Reino Unido (+44)";
                    Pais_selecionado.Foreground = Brushes.Green;
                    break;
                default:
                    Pais_selecionado.Content = "Nenhum país selecionado";
                    Pais_selecionado.Foreground = Brushes.Red;
                    break;
            }


        }

        private void Iniciar_Timer_Esconder_Mensagem()
        {
            //Define os atributos e inicia a contagem regressiva do timer
            Var_class.Timer_esconder_resultado.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            Var_class.Timer_esconder_resultado.Interval = 4000;
            Var_class.Timer_esconder_resultado.Enabled = true;
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            //Executa a manipulação do resultado em outra thread
            this.Dispatcher.Invoke(() =>
            {
                Resultado.Content = "";
            });
        }

    }
}

