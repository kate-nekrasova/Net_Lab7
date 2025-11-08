using System;
using System.IO;
using System.Windows;
using Net_Lab7;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private StudentsList _studentsList;

        public MainWindow()
        {
            InitializeComponent();

            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string filePath = Path.Combine(documentsPath, "students.json");

            _studentsList = new StudentsList(new JsonFileWriter(), new JsonFileReader(), filePath);
            StudentsDataGrid.ItemsSource = _studentsList.Students;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var student = new Student(
                    NameTextBox.Text,
                    SchoolTextBox.Text,
                    int.Parse(YearTextBox.Text)
                );

                _studentsList.AddStudent(student);

                NameTextBox.Clear();
                YearTextBox.Clear();
                SchoolTextBox.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (StudentsDataGrid.SelectedItem is Student selected)
                _studentsList.DeleteStudent(selected);
        }

        private void SortByYearButton_Click(object sender, RoutedEventArgs e)
        {
            _studentsList.SortByYear();
        }

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            string school = FilterSchoolTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(school))
                _studentsList.ResetFilter();
            else
                _studentsList.FilterBySchool(school);
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            await _studentsList.SaveAsync();
            MessageBox.Show("Дані успішно збережено.", "Збереження", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private async void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            await _studentsList.LoadAsync();
            MessageBox.Show("Дані успішно завантажено.", "Завантаження", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
