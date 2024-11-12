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

namespace Alekseev_Autoservice
{
    /// <summary>
    /// Логика взаимодействия для SignUpPage.xaml
    /// </summary>
    public partial class SignUpPage : Page
    {
        private Service _currentService = new Service();

        public SignUpPage(Service SelectedService)
        {
            InitializeComponent();
            if (SelectedService != null)
                this._currentService = SelectedService;

            DataContext = _currentService;

            var _currentClient = AlekseevAutoserviceEntities.GetContext().Client.ToList();
            ComboClient.ItemsSource = _currentClient;
        }

        private ClientService _currentClientService = new ClientService();
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string s = TBStart.Text;
            StringBuilder errors = new StringBuilder();

            if (ComboClient.SelectedItem == null)
                errors.AppendLine("Укажите ФИО клиента");
            if (StartDate.Text == "")
                errors.AppendLine("Укажите дату услуги");
            if (TBStart.Text == "")
                errors.AppendLine("Укажите время начала услуги");

            // Проверка на правильный формат времени
            if (s.Length < 4 || s.Length > 5 || !s.Contains(':'))
            {
                errors.AppendLine("Указан неверный формат записи (должно быть HH:MM)");
            }
            else
            {
                string[] start = s.Split(new char[] { ':' });

                // Проверка на количество частей после разделения
                if (start.Length != 2)
                {
                    errors.AppendLine("Указан неверный формат записи (должно быть HH:MM)");
                }
                else
                {
                    // Попытка преобразования строк в числа
                    if (!int.TryParse(start[0], out int StartHour) || !int.TryParse(start[1], out int startMin))
                    {
                        errors.AppendLine("Часы и минуты должны быть целыми числами");
                    }
                    else
                    {
                        // Проверка корректности значений часов и минут
                        if (StartHour < 0 || StartHour >= 24 || startMin < 0 || startMin >= 60)
                        {
                            errors.AppendLine("Время указано неверно");
                        }
                    }
                }
            }

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            _currentClientService.ClientID = ComboClient.SelectedIndex + 1;
            _currentClientService.ServiceID = _currentService.ID;
            _currentClientService.StartTime = Convert.ToDateTime(StartDate.Text + " " + TBStart.Text);

            if (_currentClientService.ID == 0)
                AlekseevAutoserviceEntities.GetContext().ClientService.Add(_currentClientService);

            try
            {
                AlekseevAutoserviceEntities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена");
                Manager.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void TBStart_TextChanged(object sender, TextChangedEventArgs e)
        {
            string s = TBStart.Text;

            if (s.Length < 5 || !s.Contains(':'))
                TBEnd.Text = "";
            else
            {

                string startTimeInput = TBStart.Text;

                DateTime startTime;


                if (DateTime.TryParse(startTimeInput, out startTime))
                {
                    string[] start = s.Split(new char[] { ':' });
                    int startHour = Convert.ToInt32(start[0].ToString()) * 60;
                    int startMin = Convert.ToInt32(start[1].ToString());

                    int sum = startHour + startMin + _currentService.Duration;

                    int endHour = (sum / 60) % 24;
                    int endMin = sum % 60;
                    s = endHour.ToString() + ":" + endMin.ToString();
                    TBEnd.Text = s;

                }
                else
                {
                    MessageBox.Show("Некорректное время");
                }
            }
        }
    }
}
