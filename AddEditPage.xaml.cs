﻿using System;
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
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {
        private Service _currentService = new Service();
        public AddEditPage(Service SelectedService)
        {
            InitializeComponent();

            if (SelectedService != null)
            {
                _currentService = SelectedService;
            }

            DataContext = _currentService;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_currentService.Title))
            {
                errors.AppendLine("Укажите название услуги");
            }

            if (_currentService.Cost <= 0 || _currentService.Cost == null)
            {
                errors.AppendLine("Укажите стоимость услуги");
            }

            if (_currentService.Discount == null || _currentService.Discount < 0 || _currentService.Discount > 100)
            {
                errors.AppendLine("Укажите скидку");
            }

            if (string.IsNullOrWhiteSpace(_currentService.Duration))
            {
                errors.AppendLine("Укажите длительность услуги");
            }

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (_currentService.ID == 0)
            {
                AlekseevAutoserviceEntities.GetContext().Service.Add(_currentService);
            }

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
    }
}
