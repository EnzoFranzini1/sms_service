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

namespace SMS_Sender
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Enviar_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                WebClient client = new System.Net.WebClient();
                Stream S = client.OpenRead(string.Format("https://platform.clickatell.com/messages/http/send?apiKey=4yOpE021Q9OVBESLp77EfA==&to={0}&content={1}",Pais.Text.ToString() + DDD.Text.ToString() + Numero.Text.ToString(), Mensagem.Text.ToString()));
                StreamReader reader = new StreamReader(S);
                string result = reader.ReadToEnd();
                MessageBox.Show("Enviado com sucesso", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButton.OK, MessageBoxImage.Error);
                //throw;
            }
        }

    }


}

